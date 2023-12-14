using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
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

        IBaseResponse<List<string>> GetLessonList();
    }
}
