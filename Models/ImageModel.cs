using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class ImageModel 
    {
        public int Id { get; set; }

        public string ImageSource { get; set; }

        public List<PersonModel> People { get; set; } = new List<PersonModel>();

    }
}
