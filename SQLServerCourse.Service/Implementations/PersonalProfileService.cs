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
    public class PersonalProfileService: IPersonalProfileService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<LessonRecord> _lessonRecordRepository;
        private readonly IBaseRepository<Lesson> _lessonRepository;

        public PersonalProfileService(IBaseRepository<User> userRepository, IBaseRepository<LessonRecord> lessonRecordRepository,
            IBaseRepository<Lesson> lessonRepository)
        {
            _userRepository = userRepository;
            _lessonRecordRepository = lessonRecordRepository;
            _lessonRepository = lessonRepository;
        }

        //private readonly ILogger<PersonalProfileService> _logger;

        public async Task<IBaseResponse<UserInfoViewModel>> GetPersonalProfile(string userName)
        {
            try
            {
                var result = await _userRepository.GetAll()
                    .Select(x => new UserInfoViewModel()
                    {
                        Login = x.Login,
                        Name = x.Name,
                        Surname = x.Surname,
                        FinalGrade = x.FinalGrade,
                        IsExamCompleted = x.IsExamCompleted,
                        LessonsCompleted = x.LessonsCompleted,
                        Analys = x.Analys,
                        LessonNames = _lessonRepository.GetAll().Select(x => x.Name).ToList()
                    })
                    .FirstOrDefaultAsync(x => x.Login == userName);

                if (result is null)
                {
                    return new BaseResponse<UserInfoViewModel>()
                    {
                        Description = "Ваш профиль не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                return new BaseResponse<UserInfoViewModel>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserInfoViewModel>()
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
                    .Include(lr => lr.Lesson)
                    .Where(lr => lr.UserId == user.Id)
                    .Select(lrvm => new LessonRecordViewModel
                    {
                        LessonName = lrvm.Lesson.Name,
                        LessonMark = lrvm.Mark,
                        DateOfReceiving = lrvm.DateOfReceiving,
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
    }
}
