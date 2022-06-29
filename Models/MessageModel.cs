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
        public string Sender { get; set; }

        public DateTime Date { get; set; }
        public GameModel Game { get; set; }

    }
}
