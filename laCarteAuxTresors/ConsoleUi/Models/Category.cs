using System.Collections.Generic;

namespace ConsoleUi.Models
{
    public class Position {
        public int positionX {get;set;}
        public int positionY {get;set;}
    }
    public class Category : Position
    {
        public char type {get;set;}

    }

    public class Treasure :  Category
    {
        public int nbTreasures {get;set;}
    }

    public class Adventurer :  Treasure
    {
        public string name {get;set;}
        public char orientation {get;set;}
        public char lastOrientation {get;set;}
        public string seqMouvement {get;set;}
    }

}