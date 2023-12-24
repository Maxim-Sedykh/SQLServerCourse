using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Domain.ViewModels.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces
{
    public interface IUserProfileService
    {
        Task<IBaseResponse<UserProfileViewModel>> GetUserProfile(string userLogin);

        Task<IBaseResponse<List<LessonRecordViewModel>>> GetLessonRecords(long id);

        Task<IBaseResponse<UserProfile>> UpdateInfo(UserProfileViewModel model);

        Task<IBaseResponse<LessonListViewModel>> GetLessonList(string userLogin);
    }
}
