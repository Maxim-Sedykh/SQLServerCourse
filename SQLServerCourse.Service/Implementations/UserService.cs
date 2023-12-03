﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IBaseResponse<UserEditingViewModel>> GetUser(long id)
        {
            try
            {
                var roles = ((Role[])Enum.GetValues(typeof(Role)))
                    .ToDictionary(k => (int)k, v => v.GetDisplayName());
                var result = await _userProfileRepository.GetAll()
                    .Select(x => new UserEditingViewModel()
                    {
                        Id = x.User.Id,
                        Login = x.User.Login,
                        Role = x.User.Role,
                        Name = x.Name,
                        Surname = x.Surname,
                        IsEditAble = x.IsEditAble,
                        UserRoles = roles
                    })
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (result is null)
                {
                    return new BaseResponse<UserEditingViewModel>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }
                return new BaseResponse<UserEditingViewModel>()
                {
                    Data = result,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserEditingViewModel>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
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

        public async Task<IBaseResponse<UserProfile>> UpdateUserData(UserEditingViewModel model)
        {
            try
            {
                var profile = await _userProfileRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.User.Id == model.Id);
                var currentUser = await _userRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == model.Id);
                if (profile == null || currentUser == null)
                {
                    return new BaseResponse<UserProfile>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Пользователь не найден"
                    };
                }

                var otherUsers = _userRepository.GetAll().Where(x => x.Id != model.Id);
                foreach (var user in otherUsers)
                {
                    if (user.Login == model.Login) 
                    {
                        return new BaseResponse<UserProfile>()
                        {
                            Description = "Уже есть пользователь с таким логином"
                        };
                    }
                }

                currentUser.Login = model.Login;
                currentUser.Role = model.Role;
                profile.Name = model.Name;
                profile.Surname = model.Surname;
                profile.IsEditAble = model.IsEditAble;

                await _userProfileRepository.Update(profile);
                await _userRepository.Update(currentUser);

                return new BaseResponse<UserProfile>()
                {
                    Description = "Данные обновлены!",
                    Data = profile,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<UserProfile>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }
    }
}