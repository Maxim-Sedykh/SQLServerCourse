using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.DAL.SqlHelper;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Domain.ViewModels.Teaching;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SQLServerCourse.Service.Implementations
{
    public class LessonService: ILessonService
    {
        private readonly IBaseRepository<UserProfile> _userProfileRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;
        private readonly IBaseRepository<Lesson> _lessonRepository;
        private readonly IBaseRepository<Question> _questionRepository;
        private readonly IBaseRepository<TestVariant> _testVariantRepository;

        public LessonService(IBaseRepository<LessonRecord> lessonRecordRepository, 
            IBaseRepository<Lesson> lessonRepository, IBaseRepository<UserProfile> userProfileRepository,
            IBaseRepository<Question> questionRepository, IBaseRepository<TestVariant> pageAnswerRepository)
        {
            _lessonRecordRepository = lessonRecordRepository;
            _lessonRepository = lessonRepository;
            _userProfileRepository = userProfileRepository;
            _questionRepository = questionRepository;
            _testVariantRepository = pageAnswerRepository;
        }

        public IBaseResponse<LessonLectureViewModel> GetLecture(byte lessonId)
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
                        LessonMarkup = new HtmlString($"{lesson.LectureMarkup}")
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

        public IBaseResponse<LessonPassViewModel> GetQuestions(byte lessonId)
        {
            try
            {
                var currentLesson = _lessonRepository.GetAll().FirstOrDefault(x => x.Id == lessonId);
                if (currentLesson is null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.LessonNotFound,
                        Description = "Урок не найден"
                    };
                }

                var response = GetLessonQuestions(currentLesson);

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

        public async Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel model, string userLogin) // Прохождение практической части уроков
        {
            try
            {
                var profile = await _userProfileRepository.GetAll().FirstOrDefaultAsync(x => x.User.Login == userLogin);
                if (profile == null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                Tuple<float, List<bool>> tasksEvaluations = CheckTasks(model); //Проверка ответов пользователя
                if (model.LessonId > profile.LessonsCompleted)
                {
                    profile.CurrentGrade = +tasksEvaluations.Item1;
                    profile.LessonsCompleted++;

                    await _userProfileRepository.Update(profile);
                    await _lessonRecordRepository.Create(new LessonRecord
                    {
                        LessonId = model.LessonId,
                        UserId = profile.Id,
                        Mark = tasksEvaluations.Item1
                    });
                }

                for (int i = 0; i < model.Questions.Count; i++)
                {
                    model.Questions[i].AnswerCorrectness = tasksEvaluations.Item2[i];
                }

                return new BaseResponse<LessonPassViewModel>()
                {
                    Data = model,
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

        private LessonPassViewModel GetLessonQuestions(Lesson currentLesson)
        {
            var lessonQuestions = _questionRepository.GetAll().Where(question => question.LessonId == currentLesson.Id).ToList();
            var lessonTestVariants = _questionRepository.GetAll().Where(question => question.LessonId == currentLesson.Id && question.Type == TaskType.Test)
                                                    .Join(_testVariantRepository.GetAll(), question => question.Id, testVariant => testVariant.QuestionId, (question, testVariant) => testVariant)
                                                    .ToList();
            var questionViewModels = new List<QuestionViewModel>();
            for (int i = 0; i < lessonQuestions.Count; i++)
            {
                if (i > 0 && lessonQuestions[i - 1].Number == lessonQuestions[i].Number)
                {     
                    questionViewModels.Last().InnerAnswers.Add(lessonQuestions[i].Answer);
                    continue;
                }
                else
                {
                    questionViewModels.Add(new QuestionViewModel
                    {
                        Number = lessonQuestions[i].Number,
                        DisplayQuestion = lessonQuestions[i].DisplayQuestion,
                        QuestionType = lessonQuestions[i].Type,
                        VariantsOfAnswer = lessonQuestions[i].Type == TaskType.Test ? (from testVariant in lessonTestVariants
                                                                                       where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                       select testVariant).ToList() : null,
                        InnerAnswers = new List<string> { lessonQuestions[i].Answer },
                        RightPageAnswer = (lessonQuestions[i].Type == TaskType.Open) || (lessonQuestions[i].Type == TaskType.Practical) ? lessonQuestions[i].Answer : (from testVariant in lessonTestVariants
                                                                                                                  where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                                                  where testVariant.IsRight
                                                                                                                  select testVariant.Content).First(),
                    });
                }
            }
            return new LessonPassViewModel
            {
                LessonId = currentLesson.Id,
                Questions = questionViewModels,
                LessonType = currentLesson.LessonType
            };
        }

        private Tuple<float, List<bool>> CheckTasks(LessonPassViewModel model) // Проверка заданий тестового и открытого типа
        {
            float grade = 0;
            List<bool> tasksCorrectness = new List<bool>();

            foreach (var question in model.Questions)
            {
                bool isAnswerCorrect = false;
                foreach (var answer in question.InnerAnswers)
                {
                    if (question.QuestionType == TaskType.Practical)
                    {
                        try
                        {
                            var userResult = SqlHelper.ExecuteQuery(question.UserAnswer);
                            var rightResult = SqlHelper.ExecuteQuery(question.InnerAnswers.First());

                            DataTableComparer comparer = new DataTableComparer();
                            int result = comparer.Compare(userResult, rightResult);

                            if (result == 0)
                            {
                                grade = +5.75f;
                                tasksCorrectness.Add(true);
                            }
                            else 
                            {
                                tasksCorrectness.Add(false);
                            }
                            question.QueryResult = userResult;
                        }
                        catch (Exception)
                        {
                            tasksCorrectness.Add(false);
                        }
                    }
                    else
                    {
                        if (answer == question.UserAnswer)
                        {
                            isAnswerCorrect = true;
                            tasksCorrectness.Add(true);
                            if (question.QuestionType == TaskType.Test)
                            {
                                grade += 1f;
                            }
                            else
                            {
                                grade += 2f;
                            }
                            break;
                        }
                    }
                }
                if (!isAnswerCorrect)
                {
                    tasksCorrectness.Add(false);
                }
            }
            return new Tuple<float, List<bool>>(grade, tasksCorrectness);
        }

        public async Task<IBaseResponse<LessonLectureViewModel>> SaveLectureMarkup(LessonContentViewModel model)
        {
            try
            {
                var currentLesson = _lessonRepository.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (currentLesson == null)
                {
                    return new BaseResponse<LessonLectureViewModel>()
                    {
                        StatusCode = StatusCode.LessonNotFound,
                        Description = "Урок не найден"
                    };
                }

                currentLesson.LectureMarkup = model.LessonMarkup;

                await _lessonRepository.Update(currentLesson);

                return new BaseResponse<LessonLectureViewModel>()
                {
                    Description = "Изменения сохранены",
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
    }
}