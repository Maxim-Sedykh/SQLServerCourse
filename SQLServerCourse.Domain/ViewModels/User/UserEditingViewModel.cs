using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.User
{
    public class UserEditingViewModel
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Укажите логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Укажите роль")]
        [Display(Name = "Роль")]
        public Role Role { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Может редактировать свои данные")]
        public bool IsEditAble { get; set; }

        [Display(Name = "Прошёл экзамен")]
        public bool IsExamCompleted { get; set; }

        [Display(Name = "Текущая оценка")]
        [Range(0, 100, ErrorMessage = "Диапазон оценки должен быть от 0 до 100")]
        public float CurrentGrade { get; set; }

        [Display(Name = "Прошёл уроков")]
        [Range(0, 130, ErrorMessage = "Диапазон пройденных уроков должен быть от 0 до 5")]
        public byte LessonsCompleted { get; set; }
    }
}
