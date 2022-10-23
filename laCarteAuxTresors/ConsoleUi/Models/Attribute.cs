namespace ConsoleUi.Models
{
    public class Attribute
    {
        public char type {get;set;}
    }

    public class Carte :  Attribute
    {
        public int largeur {get;set;}
        public int longeur {get;set;}
    }

    public class Montagne :  Attribute
    {
        public int horizontal {get;set;}
        public int vertical {get;set;}
    }

    public class Tresor :  Attribute
    {
        public int horizontal {get;set;}
        public int vertical {get;set;}
        public int nbTresors {get;set;}
    }


}