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
    public interface IPersonalProfileService
    {
        Task<IBaseResponse<ProfileViewModel>> GetPersonalProfile(string userName);

        Task<IBaseResponse<List<LessonRecordViewModel>>> GetLessonRecords(string userName);

        Task<IBaseResponse<User>> UpdateInfo(ProfileViewModel model);

        IBaseResponse<List<string>> GetLessonList();
    }
}
