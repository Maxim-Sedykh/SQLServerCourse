﻿using SQLServerCourse.Domain.Responce;
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
        Task<IBaseResponse<ResultViewModel>> GetResultModel(string userLogin);

        Task<IBaseResponse<UserAnalysViewModel>> GetUserAnalys(long id);
    }
}
