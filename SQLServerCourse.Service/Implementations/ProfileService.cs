using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Service.Interfaces;
using System.Security.Claims;

namespace SQLServerCourse.Service.Implementations
{
    public class ProfileService: IProfileService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;
        private readonly IBaseRepository<Lesson> _lessonRepository;

        public ProfileService(IBaseRepository<User> userRepository, IBaseRepository<LessonRecord> lessonRecordRepository,
            IBaseRepository<Lesson> lessonRepository)
        {
            _userRepository = userRepository;
            _lessonRecordRepository = lessonRecordRepository;
            _lessonRepository = lessonRepository;
        }

        //private readonly ILogger<PersonalProfileService> _logger;

        public async Task<IBaseResponse<ProfileViewModel>> GetProfile(string userName)
        {
            try
            {
                var result = await _userRepository.GetAll()
                    .Select(x => new ProfileViewModel()
                    {
                        Id = x.Id,
                        Login = x.Login,
                        Name = x.Name,
                        Surname = x.Surname,
                        FinalGrade = x.FinalGrade,
                        IsExamCompleted = x.IsExamCompleted,
                        LessonsCompleted = x.LessonsCompleted,
                        //LessonNames = _lessonRepository.GetAll().Select(x => x.Name).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.Login == userName);

                if (result is null)
                {
                    return new BaseResponse<ProfileViewModel>()
                    {
                        Description = "Ваш профиль не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                return new BaseResponse<ProfileViewModel>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProfileViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<User>> UpdateInfo(ProfileViewModel model)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                if (user == null)
                {
                    return new BaseResponse<User>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                user.Name = model.Name;
                user.Surname = model.Surname;

                await _userRepository.Update(user);

                return new BaseResponse<User>()
                {
                    Data = user,
                    Description = "Данные обновлены",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<User>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<LessonRecordViewModel>>> GetLessonRecords(string userName)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == userName);

                if (user == null)
                {
                    return new BaseResponse<List<LessonRecordViewModel>>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var response = await _lessonRecordRepository.GetAll()
                    .Include(lessonrec => lessonrec.Lesson)
                    .Where(lessonrec => lessonrec.UserId == user.Id)
                    .Select(lessonRecordViewModel => new LessonRecordViewModel
                    {
                        LessonName = lessonRecordViewModel.Lesson.Name,
                        LessonMark = lessonRecordViewModel.Mark,
                        DateOfReceiving = lessonRecordViewModel.DateOfReceiving,
                    }).ToListAsync();

                return new BaseResponse<List<LessonRecordViewModel>>()
                {
                    Data = response,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<LessonRecordViewModel>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        public IBaseResponse<List<string>> GetLessonList()
        {
            try
            {
                var lessons = _lessonRepository.GetAll().Select(x => x.Name).ToList();
                if (!lessons.Any())
                {
                    return new BaseResponse<List<string>>()
                    {
                        Description = "Найдено 0 элементов",
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<string>>()
                {
                    Data = lessons,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
