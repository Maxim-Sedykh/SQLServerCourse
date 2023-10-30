using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLServerCourse.Domain.ViewModels.PersonalProfile
{
    public class ProfileViewModel
    {
        public int Id { get; set; } 

        public string Login { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2, ErrorMessage = "Длина имени должна быть больше двух символов")]
        [MaxLength(25, ErrorMessage = "Длина имени должна быть меньше 25 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [MinLength(2, ErrorMessage = "Длина фамилии должна быть больше двух символов")]
        [MaxLength(25, ErrorMessage = "Длина фамилии должна быть меньше 25 символов")]
        public string Surname { get; set; }

        public float FinalGrade { get; set; }

        public bool IsExamCompleted { get; set;}

        public int LessonsCompleted { get; set; }
    }
}
