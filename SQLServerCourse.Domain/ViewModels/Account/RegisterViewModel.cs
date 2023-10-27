using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace SQLServerCourse.Domain.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(4, ErrorMessage = "Длина логина должна быть больше четырёх символов")]
        [MaxLength(12, ErrorMessage = "Длина логина должна быть меньше 12 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2, ErrorMessage = "Длина имени должна быть больше двух символов")]
        [MaxLength(25, ErrorMessage = "Длина имени должна быть меньше 25 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [MinLength(2, ErrorMessage = "Длина фамилии должна быть больше двух символов")]
        [MaxLength(25, ErrorMessage = "Длина фамилии должна быть меньше 25 символов")]
        public string Surname { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(5, ErrorMessage = "Длина пароля должна быть больше пяти символов")]
        [MaxLength(15, ErrorMessage = "Длина фамилии должна быть меньше 15 символов")]
        public string Password { get; set; }
    }
}