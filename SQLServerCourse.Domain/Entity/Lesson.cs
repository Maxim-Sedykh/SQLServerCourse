using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations.Schema;
using SQLServerCourse.Domain.Enum;

namespace SQLServerCourse.Domain.Entity
{
    public class Lesson
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? LectureMarkup { get; set; }

        public List<Question> Questions { get; set; }

        public List<PracticalTask> Evaluations { get; set; }

        public List<LessonRecord> Registers { get; set; }
    }
}
