using System.Collections.Generic;
using ConsoleUi.Models;

namespace ConsoleUi.Interfaces
{
    public interface IFileManager
    {
        public List<string> ReadFile(string arg);
        public void WriteFile(Game game);
    }
}