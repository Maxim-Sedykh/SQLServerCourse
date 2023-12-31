﻿using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLServerCourse.Domain.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [MinLength(4, ErrorMessage = "Длина логина должна быть больше четырёх символов")]
        [MaxLength(12, ErrorMessage = "Длина логина должна быть меньше 12 символов")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(5, ErrorMessage = "Длина пароля должна быть больше пяти символов")]
        [MaxLength(20, ErrorMessage = "Длина пароля должна быть меньше двадцати символов")]
        public string Password { get; set; }
    }
}
