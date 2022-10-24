using System;
using System.Collections.Generic;
using ConsoleUi.Models;

public interface IConsoleManager
{
    void Run(string[] arg);
    Game GenerateGame(string data,Game game);
    Game PlayGame(Game game);
    void ActionAdventurer(char action, Adventurer aventurier, Game game);
}