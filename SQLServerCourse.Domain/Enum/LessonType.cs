using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Enum
{
    public enum LessonType
    {
        WithPractical = 0,
        Common = 1,
        Exam = 2, //Экзамен содержит практические
    }
}
