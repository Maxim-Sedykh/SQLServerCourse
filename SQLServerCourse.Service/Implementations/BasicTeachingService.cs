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
using SQLServerCourse.Service.Interfaces.TeachingInterfaces;
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
    public class BasicTeachingService: IBasicTeachingService, ITask
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

        public async Task<IBaseResponse<LessonPassViewModel>> GetQuestions(int lessonId)
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

                var response = ITask.GetQuestions(currentLesson, _questionRepository.GetAll().ToList(), _testVariantRepository.GetAll().ToList());

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

                var generalModel = ITask.GetQuestions(currentLesson, _questionRepository.GetAll().ToList(), _testVariantRepository.GetAll().ToList());

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

                Tuple<float, List<bool>> tasksEvaluations = ITask.CheckTasks(generalModel); //Проверка ответов пользователя
                if (generalModel.LessonId > user.LessonsCompleted)
                {
                    await CommitPassageChanges(user, tasksEvaluations, generalModel.LessonId);
                }

                for (int i = 0; i < generalModel.Questions.Count; i++)
                {
                    generalModel.Questions[i].AnswerCorrectness = tasksEvaluations.Item2[i];
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

            async Task CommitPassageChanges(User user, Tuple<float, List<bool>> tasksEvaluations, int lessonId)
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
            }
        }
    }
}
