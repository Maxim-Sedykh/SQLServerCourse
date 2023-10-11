using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Enum
{
    public enum LessonType
    {
        CommonWithPractical = 0,
        CommonWithoutPractical = 1,
        Exam = 2, //Экзамен содержит практические
    }
}
