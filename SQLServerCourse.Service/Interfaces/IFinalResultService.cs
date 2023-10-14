using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.FinalResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces
{
    public interface IFinalResultService
    {
        Task<BaseResponse<ResultViewModel>> GetResultModel(string userName);

        Task<BaseResponse<string>> GetUserAnalys(string userName);
    }
}
