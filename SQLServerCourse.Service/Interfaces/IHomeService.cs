﻿using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces
{
    public interface IHomeService
    {
        BaseResponse<IEnumerable<string>> GetCoursePlan();
    }
}
