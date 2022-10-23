using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleUi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ConsoleManager
{
    public class ConsoleManagerService : IConsoleManager
    {
        private readonly ILogger<ConsoleManagerService> _log;
        private readonly IConfiguration _config;
        private readonly IConsoleManagerErrors _consoleManagerErrors;
        public ConsoleManagerService(ILogger<ConsoleManagerService> log, IConfiguration config, IConsoleManagerErrors consoleManagerErrors)
        {
            _log = log;
            _config = config;
            _consoleManagerErrors = consoleManagerErrors;
        }
        public void Run(string[] arg)
        {
            // check error in arg
            _consoleManagerErrors.checkErrorEntry(arg);

            // string[] lines = System.IO.File.ReadAllLines(arg[0]);

            // Display the file contents by using a foreach loop.
            Console.WriteLine("Contents of WriteLines2.txt = ");
            var lines = File.ReadLines(arg[0])
                        .Select(x => Regex.Replace(x, " -", string.Empty))
                        .ToList();
            List<List<string>> map = new List<List<string>>();




            // Use a tab to indent each line of the file.
            foreach (var item in lines)
            {
                map = GenerateMap(item, map);
            }
            Console.WriteLine(JsonConvert.SerializeObject(map));



            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");

            System.Console.ReadKey();
        }

        public List<List<string>> GenerateMap(string data, List<List<string>> map)
        {
            List<string> datas = data.Split(" ").ToList();
            List<List<string>> tempMap = new List<List<string>>();
            Console.WriteLine(JsonConvert.SerializeObject(datas[0]));
            var type = datas[0].ToCharArray()[0];
            switch (type)
            {
                case 'C':
                    Carte carte = new Carte();
                    carte.type = type;
                    int number;
                    bool isParsableCarte = Int32.TryParse(datas[1], out number);

                    if (isParsableCarte)
                        carte.largeur = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableCarte = Int32.TryParse(datas[2], out number);
                    if (isParsableCarte)
                        carte.longeur = number;
                    else
                        Console.WriteLine("Could not be parsed.");

                    for (int i = 0; i < carte.longeur; i++)
                    {
                        List<string> largeur = new List<string>();
                        for (int p = 0; p < carte.largeur; p++)
                        {
                            largeur.Add(".");
                        }
                        tempMap.Add(largeur);

                    }
                    break;
                case 'M':
                    Montagne montagne = new Montagne();
                    montagne.type = type;
                    bool isParsableMontagne = Int32.TryParse(datas[1], out number);
                    
                    if (isParsableMontagne)
                        montagne.horizontal = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableMontagne = Int32.TryParse(datas[2], out number);
                    if (isParsableMontagne)
                        montagne.vertical = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    tempMap = map;
                    try
                    {
                        tempMap[montagne.vertical][montagne.horizontal] = type.ToString();
                    }
                    catch (System.Exception)
                    {

                        throw new Exception("out of range");
                    }

                    break;
                case 'T':
                    Console.WriteLine(JsonConvert.SerializeObject(datas));
                    Tresor tresor = new Tresor();
                    tresor.type = type;
                    bool isParsableTresor = Int32.TryParse(datas[1], out number);
                    
                    if (isParsableTresor)
                        tresor.horizontal = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableTresor = Int32.TryParse(datas[2], out number);
                    if (isParsableTresor)
                        tresor.vertical = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    if (isParsableTresor)
                        tresor.nbTresors = number;
                    else
                        Console.WriteLine("Could not be parsed.");

                    tempMap = map;
                    try
                    {
                        tempMap[tresor.vertical][tresor.horizontal] = $"T({tresor.nbTresors})";
                    }
                    catch (System.Exception)
                    {

                        throw new Exception("out of range");
                    }

                    break;
                default:
                    Console.WriteLine("Sunday");
                    tempMap = map;
                    break;
            }
            return tempMap;
            // throw new NotImplementedException();
        }
    }
}