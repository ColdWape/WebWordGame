using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class GameInfoModel
    {
        public int Id { get; set; }
        public int QuantityOfGames { get; set; }
        public int QuantityOfPlayers { get; set; }
        public int QuantityOfUsedWords { get; set; }
        public int CurrentlyOnline { get; set; }
        public int PeopleInQueue { get; set; }

    }

    
}
