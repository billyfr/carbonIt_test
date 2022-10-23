using System.Collections.Generic;

namespace ConsoleUi.Models
{
    public class Game {
        public List<Categorie> Categories {get;set;}
    }

    public class Categorie
    {
        public char type {get;set;}
        public int positionX {get;set;}
        public int positionY {get;set;}
    }

    public class Tresor :  Categorie
    {
        public int nbTresors {get;set;}
    }

    public class Aventurier :  Tresor
    {
        public string nom {get;set;}
        public char oriantation {get;set;}
        public string seqMouvement {get;set;}
    }

}