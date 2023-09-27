using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Extensions;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Domain.ViewModels.Teaching;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLServerCourse.Service.Implementations
{
    public class TeachingService: ITeachingService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;
        private readonly IBaseRepository<Lesson> _lessonRepository;
        private readonly IBaseRepository<Question> _questionRepository;
        private readonly IBaseRepository<TestVariant> _testVariantRepository;

        public TeachingService(IBaseRepository<User> userRepository, IBaseRepository<LessonRecord> lessonRecordRepository, 
            IBaseRepository<Lesson> lessonRepository, 
            IBaseRepository<Question> questionRepository, IBaseRepository<TestVariant> pageAnswerRepository)
        {
            _userRepository = userRepository;
            _lessonRecordRepository = lessonRecordRepository;
            _lessonRepository = lessonRepository;
            _questionRepository = questionRepository;
            _testVariantRepository = pageAnswerRepository;
        }

        public IBaseResponse<LessonLectureViewModel> GetLecture(int lessonId)
        {
            try
            {
                var lesson = _lessonRepository.GetAll().FirstOrDefault(x => x.Id == lessonId);
                if (lesson is null)
                {
                    return new BaseResponse<LessonLectureViewModel>()
                    {
                        StatusCode = StatusCode.LessonNotFound,
                        Description = "Урок не найден"
                    };
                }

                return new BaseResponse<LessonLectureViewModel>()
                {
                    Data = new LessonLectureViewModel()
                    {
                        Id = lessonId,
                        LessonName = lesson.Name,
                        LessonMarkup = new HtmlString($"{lesson.LectureMaterial}")
            },
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LessonLectureViewModel>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<LessonPassViewModel>> GetQuestions(int lessonId)
        {
            try
            {
                var response = new LessonPassViewModel
                {
                    LessonId = lessonId,
                    TestQuestions = await (from question in _questionRepository.GetAll()
                                           where question.LessonId == lessonId
                                           where question.Type == TaskType.Test
                                           orderby question.Number
                                           select new TestQuestionViewModel
                                           {
                                               Number = question.Number,
                                               DisplayQuestion = question.DisplayQuestion,
                                               VariantsOfAnswer = (from testVariant in _testVariantRepository.GetAll()
                                                                   where testVariant.QuestionId == question.Id
                                                                   select testVariant).ToList(),
                                               InnerAnswer = question.Answer,
                                               RightPageAnswer = (from testVariant in _testVariantRepository.GetAll()
                                                                  where testVariant.QuestionId == question.Id && testVariant.IsRight
                                                                 select testVariant.Content).FirstOrDefault(),
                                           }).ToListAsync(),
                    OpenQuestions = QuestionExstension.CreateDifferentValuesList(_questionRepository.GetAll().ToList())
                };
                if (response is null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.TasksDataNotFound,
                        Description = "Данные заданий не найдены"
                    };
                }
                return new BaseResponse<LessonPassViewModel>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<LessonPassViewModel>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        //public async Task<IBaseResponse<ResultViewModel>> PassExam(LessonPassViewModel model, string userName) // прохождение экзамена
        //{
        //    try
        //    {
        //        var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == userName);
        //        var lessonAnswers = await _taskAnswerRepository.GetAll().Where(x => x.LessonId == model.LessonId).ToListAsync();
        //        if (user is null)
        //        {
        //            return new BaseResponse<ResultViewModel>()
        //            {
        //                StatusCode = StatusCode.UserNotFound,
        //                Description = "Пользователь не найден"
        //            };
        //        }

        //        Tuple<float, List<bool>> tasksEvaluations = await CheckCommonTasks(model.Answers, model.LessonId);

        //        float grade = tasksEvaluations.Item1;
        //        List<bool> lessonsTasksCorrectness = tasksEvaluations.Item2;

        //        user.FinalGrade = +grade;
        //        await _userRepository.Update(user);

        //        string analysText = CreateAnalys(user);

        //        await _lessonRecordRepository.Create(new LessonRecord
        //        {
        //            LessonId = model.LessonId,
        //            UserId = user.Id,
        //            Mark = grade
        //        });

        //        return new BaseResponse<ResultViewModel>()
        //        {
        //            Data = new ResultViewModel()
        //            {
        //                Login = user.Login,
        //                Name = user.Name,
        //                Surname = user.Surname,
        //                FinalGrade = user.FinalGrade,
        //                UserAnalys = analysText,
        //                TasksCorrectness = lessonsTasksCorrectness
        //            },
        //            StatusCode = StatusCode.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<ResultViewModel>()
        //        {
        //            Description = $"Внутренняя ошибка: {ex.Message}",
        //            StatusCode = StatusCode.InternalServerError,
        //        };
        //    }

        //    string CreateAnalys(User user) // Создание анализа прохождения курса
        //    {
        //        string generalGrade = user.FinalGrade switch
        //        {
        //            > 60 and <= 75 => "Данный курс вы прошли удовлетворительно. ",
        //            > 75 and <= 90 => "Вы очень хорошо прошли курс! ",
        //            > 90 => "У вас отличный результат! Так держать!!! ",
        //            _ => "Вы прошли этот тест неудовлетворительно. Но не сдавайтесь! " +
        //                    "Вы всегда рано или поздно достигните успеха, если будете стараться!!! "
        //        };

        //        float[] markSumOfTheme = new float[3];
        //        for (int i = 0; i < 3; i++)
        //        {
        //            markSumOfTheme[i] = _lessonRecordRepository.GetAll()
        //                .Include(x => x.Lesson).Where(x => x.UserId == user.Id && x.LessonId == i + 1)
        //                .ToList().Sum(x => x.Mark);
        //        }

        //        float pracTasksMarkSum = _lessonRecordRepository.GetAll()
        //            .Where(x => x.UserId == user.Id && x.LessonId == 2 && x.LessonId == 3)
        //            .ToList().Sum(x => x.Mark);

        //        return generalGrade + $"Всего по первой теме - \"Введение в MS SQL Server и T-SQL\" (2 урока) " +
        //            $"Вы получили {markSumOfTheme[0]} баллов из 20, по второй теме - \"Основы T-SQL\" (5 уроков) " +
        //            $"Вы получили {markSumOfTheme[1]} баллов из 42,5, и по третьей теме - \"Хранимые процедуры\" (3 урока) " +
        //            $"Вы получили {markSumOfTheme[2]} баллов из 22,5. Всего за урок без практических занятий можно было получить: 7,5 баллов," +
        //            $"с практическими 12,5. 8 уроков были без практических занятий. Ваша средняя оценка занятий по первой теме - {markSumOfTheme[0] / 2} " +
        //            $"баллов из 10. Ваша средняя оценка занятий по второй теме - {markSumOfTheme[1] / 5} баллов из 9,17. " +
        //            $"Ваша средняя оценка занятий по третьей теме - {markSumOfTheme[2] / 3} баллов из 11. Ваш средний балл по занятиям " +
        //            $"с практикой: {pracTasksMarkSum / 2} баллов из 12,5. По экзамену вы получили {user.FinalGrade} баллов из 15.";
        //    }
        //}

        //public async Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel model, string userName) // Прохождение практической части уроков
        //{
        //    try
        //    {
        //        var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == userName);
        //        var lesson = await _lessonRepository.GetAll().FirstOrDefaultAsync(x => x.Id == model.LessonId);
        //        if (user is null)
        //        {
        //            return new BaseResponse<LessonPassViewModel>()
        //            {
        //                StatusCode = StatusCode.UserNotFound,
        //                Description = "Пользователь не найден"
        //            };
        //        }
        //        if (lesson is null)
        //        {
        //            return new BaseResponse<LessonPassViewModel>()
        //            {
        //                StatusCode = StatusCode.LessonNotFound,
        //                Description = "Урок не найден"
        //            };
        //        }

        //        Tuple<float, List<bool>> tasksEvaluations;

        //        //if (lesson.LessonTasksType == LessonTasksType.CommonTasks)
        //        //{
        //        //    tasksEvaluations = await CheckCommonTasks(model.Answers, model.LessonId);
        //        //}
        //        //else
        //        //{
        //        //    tasksEvaluations = await CheckCommonWithPractical(model.Answers, model.LessonId);
        //        //}

        //        float grade = 324;
        //        List<bool> lessonsTasksCorrectness = null;

        //        user.FinalGrade = +grade;
        //        await _userRepository.Update(user);

        //        await _lessonRecordRepository.Create(new LessonRecord
        //        {
        //            LessonId = model.LessonId,
        //            UserId = user.Id,
        //            Mark = grade
        //        });

        //        model.TasksCorrectness = lessonsTasksCorrectness;

        //        return new BaseResponse<LessonPassViewModel>()
        //        {
        //            Data = model,
        //            StatusCode = StatusCode.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<LessonPassViewModel>()
        //        {
        //            Description = $"Внутренняя ошибка: {ex.Message}",
        //            StatusCode = StatusCode.InternalServerError,
        //        };
        //    }
        //}

        //private async Task<Tuple<float, List<bool>>> CheckCommonTasks(List<string> commonTasksAnswers, 
        //    int lessonId) // Проверка заданий тестового и открытого типа
        //{  
        //    float grade = 0;
        //    List<bool> tasksCorrectness = new List<bool>();

        //    var lessonAnswers = await _taskAnswerRepository.GetAll().Where(x => x.LessonId == lessonId).ToListAsync();
        //    int testsboundaryIndex;

        //    if (commonTasksAnswers.Count == 6)
        //    {
        //        testsboundaryIndex = 2;
        //        tasksCorrectness = new List<bool>(6);
        //    }
        //    else
        //    {
        //        testsboundaryIndex = 5;
        //        tasksCorrectness = new List<bool>(12);
        //    }
                
        //    for (int i = 0; i < commonTasksAnswers.Count; i++)
        //    {
        //        if (i > testsboundaryIndex)
        //        {
        //            var openTaskPossibleAnswers = await _taskAnswerRepository.GetAll(). // Находим список возможных ответов на открытые вопросы экзамена
        //                Where(x => x.LessonId == lessonId && x.TaskNumber == i + 1).ToListAsync(); 

        //            for (int j = 0; j < openTaskPossibleAnswers.Count; j++)
        //            {
        //                if (commonTasksAnswers[i] == openTaskPossibleAnswers[j].Answer)
        //                {
        //                    grade += 1.5f;
        //                    tasksCorrectness[i] = true;
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (commonTasksAnswers[i] == lessonAnswers[i + 1].Answer)
        //            {
        //                grade++;
        //                tasksCorrectness[i] = true;
        //            }
        //        }
        //    }

        //    return new Tuple<float, List<bool>>(grade, tasksCorrectness);
        //}

        //private async Task<Tuple<float, List<bool>>> CheckCommonWithPractical(List<string> allTypesTasksAnswers, 
        //    int lessonId) // Проверка заданий всех типов
        //{
        //    var lessonAnswers = await _taskAnswerRepository.GetAll().Where(x => x.LessonId == lessonId).ToListAsync();
        //    List<string> commonTasksAnswers = allTypesTasksAnswers.GetRange(0, 6);
        //    List<string> practicalTasksAnswers = allTypesTasksAnswers.GetRange(6, 2);

        //    Tuple<float, List<bool>> tasksEvaluations = await CheckCommonTasks(commonTasksAnswers, lessonId);

        //    float grade = tasksEvaluations.Item1;
        //    List<bool> lessonsTasksCorrectness = tasksEvaluations.Item2;

        //    //обработка практических заданий
        //    //допишешь после моря вася


        //    return new Tuple<float, List<bool>>(grade, lessonsTasksCorrectness);
        //}
    }
}
