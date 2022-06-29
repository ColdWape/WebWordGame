using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class WordModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfUses { get; set; }

        //public int Cost { get; set; }


    }
}
