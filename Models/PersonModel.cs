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

        
        public string LoginName { get; set; }

        
        public string Email { get; set; }

        
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Минимальная длина пароля 8 символов")]
        
        public string Password { get; set; }

        //public int ProfileImageId { get; set; }
        //public ImageModel ProfileImageId { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public ImageModel ProfileImageId { get; set; }

    }


    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PersonModel> Users { get; set; }
        public Role()
        {
            Users = new List<PersonModel>();
        }
    }
}
