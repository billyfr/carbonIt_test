using System;
using System.Collections.Generic;

public interface IConsoleManager
{
    void Run(string[] arg);
    List<List<string>> GenerateMap(string data,List<List<string>> map);
}