using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebWordGame.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PersonModel> People { get; set; } = new List<PersonModel>();
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
        public List<RoomGamer> roomGamers { get; set; } = new List<RoomGamer>();


        public string GameCreated = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();

        public DateTime StartTime { get; set; }
        public string GameEnded { get; set; }
        public string GameLifeTime { get; set; }
        public string GameStatus { get; set; }
        public int MaximumNumbersOfGamers { get; set; }
        public string Creator { get; set; }

    }

    public class RoomGamer
    {
        public int GameId { get; set; }
        public GameModel Game { get; set; }

        public int PersonId { get; set; }
        public PersonModel Person { get; set; }

        public int Position { get; set; }
        public bool IsActive { get; set; }
        public int TimeToMove { get; set; }
        public string ConnectId { get; set; }
        public int OrderOfTheMove { get; set; }
        public int Score { get; set; }
        public bool ConnectedToTheGame { get; set; }

    }

}
