using System;
using System.Collections.Generic;
using ConsoleUi.Models;

public interface IConsoleManager
{
    void Run(string[] arg);
    Game GenerateMap(string data,Game map);
}