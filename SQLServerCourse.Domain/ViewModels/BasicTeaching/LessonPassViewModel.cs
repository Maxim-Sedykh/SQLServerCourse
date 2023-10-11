using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Helpers;
using SQLServerCourse.Domain.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Lesson
{
    public class LessonPassViewModel
    {
        public int LessonId { get; set; }

        public LessonType? LessonType { get; set; }

        public List<QuestionViewModel> Questions { get; set; }
    }
}

