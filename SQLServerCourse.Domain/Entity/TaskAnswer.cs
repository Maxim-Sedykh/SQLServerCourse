using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class TaskAnswer
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public Lesson Lesson { get; set; }

        public int TaskNumber { get; set; }

        public TaskType TaskType { get; set; }

        public string Answer { get; set; }
    }
}
