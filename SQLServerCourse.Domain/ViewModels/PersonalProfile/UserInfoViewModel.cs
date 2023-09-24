using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLServerCourse.Domain.ViewModels.PersonalProfile
{
    public class UserInfoViewModel
    {
        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public float FinalGrade { get; set; }

        public bool IsExamCompleted { get; set;}

        public int LessonsCompleted { get; set; }

        public string? Analys { get; set; }

        public List<string> LessonNames { get; set; }
    }
}
