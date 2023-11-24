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
    public interface IProfileService
    {
        Task<IBaseResponse<UserProfileViewModel>> GetProfile(string userName);

        Task<IBaseResponse<List<LessonRecordViewModel>>> GetLessonRecords(string userName);

        Task<IBaseResponse<User>> UpdateInfo(UserProfileViewModel model);

        IBaseResponse<List<string>> GetLessonList();
    }
}
