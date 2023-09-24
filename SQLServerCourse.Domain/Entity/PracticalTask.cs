using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class PracticalTask
    {
        public int Id { get; set; }

        public int LessonId { get; set; }

        public Lesson Lesson { get; set; }

        public int FrequentRemarkId { get; set; }

        public FrequentRemark FrequentRemark { get; set; }

        public int PracticalTaskNumber { get; set; }
    }
}
