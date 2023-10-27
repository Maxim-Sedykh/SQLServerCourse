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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SQLServerCourse.Service.Implementations
{
    public class BasicTeachingService: IBasicTeachingService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;
        private readonly IBaseRepository<Lesson> _lessonRepository;
        private readonly IBaseRepository<Question> _questionRepository;
        private readonly IBaseRepository<TestVariant> _testVariantRepository;

        public BasicTeachingService(IBaseRepository<User> userRepository, IBaseRepository<LessonRecord> lessonRecordRepository, 
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

        public IBaseResponse<LessonPassViewModel> GetQuestions(int lessonId)
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

        public async Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel  userAnswersModel, string userName) // Прохождение практической части уроков
        {
            try
            {
                var currentLesson = _lessonRepository.GetAll().FirstOrDefault(x => x.Id == userAnswersModel.LessonId);
                if (currentLesson is null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.LessonNotFound,
                        Description = "Урок не найден"
                    };
                }

                var generalModel = GetLessonQuestions(currentLesson);
                for (int i = 0; i < generalModel.Questions.Count; i++)
                {
                    if (userAnswersModel.Questions[i].UserAnswer is not null)
                    {
                        generalModel.Questions[i].UserAnswer = Regex.Replace(userAnswersModel.Questions[i].UserAnswer, @"\s+", " ")?.ToLower()?.Trim();
                    } 
                }
                    
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == userName);
                if (user is null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                Tuple<float, List<bool>> tasksEvaluations = CheckTasks(generalModel); //Проверка ответов пользователя
                if (generalModel.LessonId > user.LessonsCompleted)
                {
                    await CommitPassageChanges(generalModel, user, tasksEvaluations, generalModel.LessonId);
                }

                return new BaseResponse<LessonPassViewModel>()
                {
                    Data = generalModel,
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

            async Task CommitPassageChanges(LessonPassViewModel generalModel, User user, Tuple<float, List<bool>> tasksEvaluations, int lessonId)
            {
                user.FinalGrade =+ tasksEvaluations.Item1;
                user.LessonsCompleted++;

                await _userRepository.Update(user);
                await _lessonRecordRepository.Create(new LessonRecord
                {
                    LessonId = lessonId,
                    UserId = user.Id,
                    Mark = tasksEvaluations.Item1
                });

                for (int i = 0; i < generalModel.Questions.Count; i++)
                {
                    generalModel.Questions[i].AnswerCorrectness = tasksEvaluations.Item2[i];
                }
            }
        }   

        private LessonPassViewModel GetLessonQuestions(Lesson currentLesson)
        {
            List<Question> lessonQuestions =  (from question in _questionRepository.GetAll().ToList()
                                              where question.LessonId == currentLesson.Id
                                              select question).ToList();
            List<TestVariant> lessonTestVariants = (from question in _questionRepository.GetAll().ToList()
                                                    where question.LessonId == currentLesson.Id && question.Type == TaskType.Test
                                                    join testVariant in _testVariantRepository.GetAll().ToList() on question.Id equals testVariant.QuestionId
                                                    select testVariant).ToList();
            List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();
            for (int i = 0, j = 0; i < lessonQuestions.Count; i++)
            {
                if (i > 0)
                {
                    if (lessonQuestions[i - 1].Number == lessonQuestions[i].Number)
                    {
                        questionViewModels[j - 1].InnerAnswers.Add(lessonQuestions[i].Answer);

                        continue;
                    }
                    goto CreateViewModel;
                }

            CreateViewModel:
                questionViewModels.Add(new QuestionViewModel
                {
                    Number = lessonQuestions[i].Number,
                    DisplayQuestion = lessonQuestions[i].DisplayQuestion,
                    QuestionType = lessonQuestions[i].Type,
                    VariantsOfAnswer = lessonQuestions[i].Type == TaskType.Test ? (from testVariant in lessonTestVariants
                                                                                   where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                   select testVariant).ToList() : null,
                    InnerAnswers = new List<string> { lessonQuestions[i].Answer },
                    RightPageAnswer = lessonQuestions[i].Type == TaskType.Open ? lessonQuestions[i].Answer : (from testVariant in lessonTestVariants
                                                                                                              where testVariant.QuestionId == lessonQuestions[i].Id
                                                                                                              where testVariant.IsRight
                                                                                                              select testVariant.Content).First(),
                });
                j++;
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

            for (int i = 0; i < model.Questions.Count; i++) // Проверка открытых вопросов
            {
                for (int j = 0; j < model.Questions[i].InnerAnswers.Count; j++)
                {
                    if (model.Questions[i].InnerAnswers[j] == model.Questions[i].UserAnswer)
                    {
                        tasksCorrectness.Add(true);
                        if (model.Questions[i].QuestionType == TaskType.Test)
                        {
                            grade += 1.5f;
                        }
                        else
                        {
                            grade += 3.5f;
                        }
                        goto LoopEnd;
                    }
                }
                tasksCorrectness.Add(false);
            LoopEnd: continue;
            }
            return new Tuple<float, List<bool>>(grade, tasksCorrectness);
        }
    }
}