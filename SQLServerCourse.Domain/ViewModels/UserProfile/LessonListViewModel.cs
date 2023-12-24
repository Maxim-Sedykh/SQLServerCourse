using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.UserProfile
{
    public class LessonListViewModel
    {
        public byte LessonsCompleted { get; set; }

        public List<string> LessonNames { get; set; }
    }
}
