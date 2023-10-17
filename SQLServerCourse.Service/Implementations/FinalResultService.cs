using Microsoft.EntityFrameworkCore;
using ServiceStack;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.FinalResult;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ServiceStack.Svg;

namespace SQLServerCourse.Service.Implementations
{
    public class FinalResultService : IFinalResultService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;

        public FinalResultService(IBaseRepository<User> userRepository, IBaseRepository<LessonRecord> lessonRecordRepository)
        {
            _userRepository = userRepository;
            _lessonRecordRepository = lessonRecordRepository;
        }

        public async Task<IBaseResponse<ResultViewModel>> GetResultModel(string userName)
        {
            try
            {
                var currentUser = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == userName);
                if (currentUser is null)
                {
                    return new BaseResponse<ResultViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                currentUser.Analys = await CreateAnalys(currentUser);
                currentUser.IsExamCompleted = true;
                await _userRepository.Update(currentUser);

                var response =  new ResultViewModel()
                                {
                                    Login = currentUser.Login,
                                    Name = currentUser.Name,
                                    Surname = currentUser.Surname,
                                    FinalGrade = currentUser.FinalGrade,
                                    UserAnalys = currentUser.Analys
                };

                if (response is null)
                {
                    return new BaseResponse<ResultViewModel>()
                    {
                        Description = "Уроки не найдены",
                        StatusCode = StatusCode.LessonNotFound
                    };
                }

                return new BaseResponse<ResultViewModel>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ResultViewModel>()
                {
                    Description = $"Внутренняя ошибка : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }


        }

        private async Task<string> CreateAnalys(User user) // Создание анализа прохождения курса
        {
            string firstPartOfAnalys = user.FinalGrade switch
            {
                > 60 and <= 75 => "Данный курс вы прошли удовлетворительно. ",
                > 75 and <= 90 => "Вы очень хорошо прошли курс! ",
                > 90 => "У вас отличный результат! Так держать!!! ",
                _ => "Вы прошли этот тест неудовлетворительно. Но не сдавайтесь! " +
                        "Вы всегда рано или поздно достигните успеха, если будете стараться!!! "
            };

            int supposedLessonRecordsCount = 5;
            if (_lessonRecordRepository.GetAll().Where(x => x.UserId == user.Id).Count() != supposedLessonRecordsCount)
            {
                return "Извините, данные вашего анализа были утеряны.";
            }
            var usersLessonRecords = await _lessonRecordRepository.GetAll().Include(x => x.Lesson).Where(x => x.UserId == user.Id).ToListAsync();
            var examLessonRecord = usersLessonRecords.Where(x => x.Lesson.Name == "Экзамен");
            var commonLessonRecords = usersLessonRecords.Except(examLessonRecord);

            var maxMarkLessonRecord = commonLessonRecords.OrderByDescending(x => x.Mark).First();
            var minMarkLessonRecord = commonLessonRecords.OrderByDescending(x => x.Mark).Last();

            string secondPartOfAnalys = $"Ваша средняя оценка по обычным занятиям {commonLessonRecords.Sum(x => x.Mark) / commonLessonRecords.Count()} " +
                 $"из 15 возможных. Вы набрали максимальное количество баллов по уроку {maxMarkLessonRecord.Lesson.Name} - {maxMarkLessonRecord.Mark} баллов." +
                 $" Минимальное по уроку {minMarkLessonRecord.Lesson.Name} - {minMarkLessonRecord.Mark} баллов," +
                 $" вы набрали {examLessonRecord.First().Mark} баллов по экзамену из 40б. " +
                 $"За одно тестовое задание можно было получить 1.5 балла, за открытое 3.5.";

            return firstPartOfAnalys + secondPartOfAnalys;
        }

        public IBaseResponse<AnalysViewModel> GetUserAnalys(string userName)
        {
            try
            {
                var currentUser = _userRepository.GetAll().FirstOrDefault(x => x.Login == userName);
                if (currentUser is null)
                {
                    return new BaseResponse<AnalysViewModel>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }
                if (currentUser.Analys is null)
                {
                    return new BaseResponse<AnalysViewModel>()
                    {
                        Description = "Извините, мы не сумели обнаружить ваш анализ",
                        StatusCode = StatusCode.UserAnalysNotFound
                    };
                }
                return new BaseResponse<AnalysViewModel>()
                {
                    Data = new AnalysViewModel { Analys = currentUser.Analys },
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AnalysViewModel>()
                {
                    Description = $"Внутренняя ошибка : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
