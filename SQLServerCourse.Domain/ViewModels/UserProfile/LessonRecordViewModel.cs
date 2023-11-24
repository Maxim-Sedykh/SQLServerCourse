using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.PersonalProfile
{
    public class LessonRecordViewModel
    {
        public string LessonName { get; set; }

        public float LessonMark { get; set; }

        public DateTime DateOfReceiving { get; set; }
    }
}
