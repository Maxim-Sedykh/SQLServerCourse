using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(5, ErrorMessage = "Длина пароля должна быть больше 5 символов")]
        [MaxLength(15, ErrorMessage = "Длина пароля должна быть меньше 15 символов")]
        public string NewPassword { get; set; }
    }
}
