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

        [Required(ErrorMessage = "Укажите роль")]
        [Display(Name = "Роль")]
        public Role Role { get; set; }

        [Display(Name = "Имя")]
        public string? Name { get; set; }

        [Display(Name = "Фамилия")]
        public string? Surname { get; set; }

        [Display(Name = "Может редактировать свои данные")]
        public bool IsEditAble { get; set; }

        public Dictionary<int, string>? UserRoles { get; set; }
    }
}
