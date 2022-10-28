using System.Collections.Generic;

namespace GameConsole.Models
{
    public class Game
    {
        public List<Category> categories { get; set; }
        public List<Treasure> treasures { get; set; }
        public List<Adventurer> adventurers { get; set; }
        public List<Position> positions{get;set;}


    }
}