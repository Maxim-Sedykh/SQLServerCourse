using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLServerCourse.Domain.ViewModels.PersonalProfile
{
    public class UserProfileViewModel
    {
        public long Id { get; set; } 

        public string Login { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2, ErrorMessage = "Длина имени должна быть больше двух символов")]
        [MaxLength(25, ErrorMessage = "Длина имени должна быть меньше 25 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [MinLength(2, ErrorMessage = "Длина фамилии должна быть больше двух символов")]
        [MaxLength(25, ErrorMessage = "Длина фамилии должна быть меньше 25 символов")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Укажите возраст")]
        [Range(0, 130, ErrorMessage = "Диапазон возраста должен быть от 0 до 130")]
        public byte Age { get; set; }

        public float CurrentGrade { get; set; }

        public bool IsExamCompleted { get; set;}

        public byte LessonsCompleted { get; set; }

        public bool IsEditAble { get; set; }
    }
}
