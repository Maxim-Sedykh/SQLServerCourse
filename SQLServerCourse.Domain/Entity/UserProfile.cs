using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class UserProfile
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public bool IsExamCompleted { get; set; }

        public float CurrentGrade { get; set; }

        public int LessonsCompleted { get; set; }

        public string? Analys { get; set; }

        public bool IsEditAble { get; set; }

        public bool IsReviewLeft { get; set; }

        public long UserId { get; set; }

        public User User { get; set; }
    }
}
