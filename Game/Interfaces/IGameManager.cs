using System.Collections.Generic;
using GameConsole.Models;

public interface IGameManager
{
    void Run(string[] arg);
    Game GenerateGame(string data, Game game);
    Game PlayGame(Game game);
    void ActionAdventurer(char action, Adventurer aventurier, Game game);
    Adventurer CreateAdventurer(List<string> options);
    Treasure CreateTreasure(List<string> options);
    Category CreateMapOrMountain(List<string> options);
}