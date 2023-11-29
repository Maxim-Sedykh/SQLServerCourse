using Microsoft.EntityFrameworkCore;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Extensions;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Domain.ViewModels.Review;
using SQLServerCourse.Domain.ViewModels.User;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<UserProfile> _userProfileRepository;
        private readonly IBaseRepository<User> _userRepository;


        public UserService(IBaseRepository<UserProfile> userProfileRepository, IBaseRepository<User> userRepository)
        {
            _userProfileRepository = userProfileRepository;
            _userRepository = userRepository;
        }

        public async Task<IBaseResponse<bool>> DeleteUser(long id)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (user == null)
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }
                if (user.Role == Role.Admin && user.Login == "Admin123")
                {
                    return new BaseResponse<bool>()
                    {
                        StatusCode = StatusCode.UserInteractionError,
                        Description = "Главный админ не может быть удалён"
                    };
                }

                await _userRepository.Delete(user);

                return new BaseResponse<bool>()
                {
                    Data = true,
                    Description = "Пользователь удалён!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public Task<IBaseResponse<Review>> EditUser(long id)
        {
            throw new NotImplementedException();
        }

        public BaseResponse<Dictionary<int, string>> GetRoles()
        {
            try
            {
                var roles = ((Role[])Enum.GetValues(typeof(Role)))
                    .ToDictionary(k => (int)k, v => v.GetDisplayName());

                return new BaseResponse<Dictionary<int, string>>()
                {
                    Data = roles,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Dictionary<int, string>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<UserViewModel>>> GetUsers()
        {
            try
            {
                var result = await _userProfileRepository.GetAll()
                    .Select(x => new UserViewModel()
                    {
                        Id = x.Id,
                        Login = x.User.Login,
                        Role = x.User.Role,
                        IsExamCompleted = x.IsExamCompleted,
                        IsReviewLeft = x.IsReviewLeft
                    })
                    .ToListAsync();

                if (result is null)
                {
                    return new BaseResponse<List<UserViewModel>>()
                    {
                        Description = "Ваш профиль не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                return new BaseResponse<List<UserViewModel>>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<UserViewModel>>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
    }
}
