using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class Question
    {
        public byte Id { get; set; }

        public byte LessonId { get; set; }

        public Lesson Lesson { get; set; }

        public byte Number { get; set; }

        public TaskType Type { get; set; }

        public string DisplayQuestion { get; set; }

        public string Answer { get; set; }

        public List<TestVariant> TestVariants { get; set; }
    }
}
