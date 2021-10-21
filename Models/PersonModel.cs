using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class PersonModel
    {
        public int Id { get; set; }

        [Display(Name = "Логин")]
        public string LoginName { get; set; }

        [Display(Name = "Почтовый адрес")]
        public string Email { get; set; }

        [Display(Name = "Пароль")]
        public string Password { get; set; }

    }
}
