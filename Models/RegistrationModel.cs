using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class RegistrationModel
    {
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Не указан логин")]
        [MaxLength(10, ErrorMessage = "Максимальная длина логина 10 символов")]
        [MinLength(3, ErrorMessage = "Минимальная дляна логина 3 символа")]
        [RegularExpression("[a-zA-Z-_0-9]*$", ErrorMessage = "Логин может содержать только латниские буквы, цифры, нижнее подчеркивание и дефис")]
        public string LoginName { get; set; }

        [Display(Name = "Почтовый адрес")]
        [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты!")]
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Минимальная длина пароля 8 символов")]
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }
    }
}
