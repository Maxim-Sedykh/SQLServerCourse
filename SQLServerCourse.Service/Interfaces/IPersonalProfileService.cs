using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces
{
    public interface IPersonalProfileService
    {
        Task<IBaseResponse<UserInfoViewModel>> GetPersonalProfile(string userName);

        Task<IBaseResponse<List<LessonRecordViewModel>>> GetLessonRecords(string userName);
    }
}
