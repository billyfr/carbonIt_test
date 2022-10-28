using System;

namespace GameConsole.Common
{
    public class Utils
    {
        public static void checkPositionIsInt(string posX, string posY){
            int positionX;
            int positionY;
            try
            {
                positionX = Int32.Parse(posX);
                positionY = Int32.Parse(posY);
            }
            catch (System.Exception)
            {
                throw new Exception($"Position X or Y must be numeric {posX} {posY}");
            }
        }
        
    }
}