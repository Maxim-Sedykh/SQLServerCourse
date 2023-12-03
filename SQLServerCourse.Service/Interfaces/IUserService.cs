using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Review;
using SQLServerCourse.Domain.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces
{
    public interface IUserService
    {
        Task<IBaseResponse<List<UserViewModel>>> GetUsers();

        Task<IBaseResponse<bool>> DeleteUser(long id);

        Task<IBaseResponse<UserEditingViewModel>> GetUser(long id);

        BaseResponse<Dictionary<int, string>> GetRoles();

        Task<IBaseResponse<UserProfile>> UpdateUserData(UserEditingViewModel model);
    }
}
