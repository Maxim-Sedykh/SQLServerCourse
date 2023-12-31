﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLServerCourse.Domain.Enum
{
    public enum StatusCode
    {
        UserNotFound = 0,
        UserAlreadyExists = 1,
        UserAnalysNotFound = 2,
        UserInteractionError = 3,

        LessonNotFound = 10,

        LessonRecordsNotFound = 20,

        TasksDataNotFound = 30,

        ReviewNotFound = 40,

        OK = 200,
        InternalServerError = 500
    }
}
