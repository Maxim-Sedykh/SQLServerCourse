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
using System.Threading.Tasks;
using System.Xml.Linq;

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
                    OpenQuestions = QuestionExstension.CreateDifferentValuesList((from question in _questionRepository.GetAll()
                                                                                  where question.Type == TaskType.Open && question.LessonId == lessonId
                                                                                  select question).ToList())
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

        public async Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel model, string userName) // Прохождение практической части уроков
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == userName);

                if (user is null)
                {
                    return new BaseResponse<LessonPassViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                Tuple<float, List<bool>> tasksEvaluations = ITask.CheckTasks(model); //Проверка ответов пользователя

                user.FinalGrade = +tasksEvaluations.Item1;
                
                await _userRepository.Update(user);
                await _lessonRecordRepository.Create(new LessonRecord
                {
                    LessonId = model.LessonId,
                    UserId = user.Id,
                    Mark = tasksEvaluations.Item1
                });

                model.TasksCorrectness = tasksEvaluations.Item2;

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

        
    }
}
