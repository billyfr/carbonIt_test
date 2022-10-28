using System.Collections.Generic;
using GameConsole.Models;

namespace GameConsole.Interfaces
{
    public interface IFileManager
    {
        public List<string> ReadFile(string arg);
        public void WriteFile(Game game);
    }
}