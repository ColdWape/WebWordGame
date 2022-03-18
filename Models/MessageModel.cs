using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string TextMeassage { get; set; }

        public string Date = DateTime.Now.ToShortTimeString();

        public GameModel Game { get; set; }
    }
}
