using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using SQLServerCourse.Domain.Enum;

namespace SQLServerCourse.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }

        public string Login { get; set; }
                                                                                                                                                                                                    
        public string Name { get; set; }

        public string Surname { get; set; }
            
        public string Password { get; set; }

        public Role Role { get; set; }

        public float FinalGrade { get; set; }

        public int LessonsCompleted { get; set; } = 0;

        public List<Review> Review { get; set; }

        public List<LessonRecord> Registers { get; set; }

        public bool IsExamCompleted { get; set; }

        public string? Analys { get; set; }

        public bool IsReviewed { get; set; }
    }
}
