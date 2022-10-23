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
            var lines = File.ReadLines(arg[0])
                        .Select(x => Regex.Replace(x, " -", string.Empty))
                        .ToList();
            Game map = new Game(){Categories = new List<Categorie>()};




            // Use a tab to indent each line of the file.
            lines.RemoveAt(0);
            foreach (var item in lines)
            {
                map = GenerateMap(item, map);
            }
            Console.WriteLine(JsonConvert.SerializeObject(map));



            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");

            // System.Console.ReadKey();
        }

        public Game GenerateMap(string data, Game map)
        {
            List<string> datas = data.Split(" ").ToList();
            Console.WriteLine(JsonConvert.SerializeObject(datas[0]));
            var type = datas[0].ToCharArray()[0];
            Game games = new Game();
            Categorie gameCarte = new Categorie();
            if(type != 'C')
                gameCarte = map.Categories.Find(x => x.type == 'C');
            switch (type)
            {
                case 'C':
                    Categorie carte = new Categorie();
                    carte.type = type;
                    int number;
                    bool isParsableCarte = Int32.TryParse(datas[1], out number);

                    if (isParsableCarte)
                        carte.positionX = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableCarte = Int32.TryParse(datas[2], out number);
                    if (isParsableCarte)
                        carte.positionY = number;
                    else
                        Console.WriteLine("Could not be parsed.");

                    // for (int i = 0; i < carte.positionX; i++)
                    // {
                    //     List<string> largeur = new List<string>();
                    //     for (int p = 0; p < carte.positionX; p++)
                    //     {
                    //         largeur.Add(".");
                    //     }
                    //     tempMap.Add(largeur);

                    // }
                    map.Categories.Add(carte);
                    break;
                case 'M':
                    Categorie montagne = new Categorie();
                    montagne.type = type;
                    bool isParsableMontagne = Int32.TryParse(datas[1], out number);
                    if (isParsableMontagne)
                        montagne.positionX = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableMontagne = Int32.TryParse(datas[2], out number);
                    if (isParsableMontagne)
                        montagne.positionY = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    if(montagne.positionX > gameCarte.positionX && montagne.positionY > gameCarte.positionY)
                        throw new Exception("out of range");
                    map.Categories.Add(montagne);
                    break;
                case 'T':
                    Tresor tresor = new Tresor();
                    tresor.type = type;
                    bool isParsableTresor = Int32.TryParse(datas[1], out number);

                    if (isParsableTresor)
                        tresor.positionX = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableTresor = Int32.TryParse(datas[2], out number);
                    if (isParsableTresor)
                        tresor.positionY = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableTresor = Int32.TryParse(datas[3], out number);
                    if (isParsableTresor)
                        tresor.nbTresors = number;
                    else
                        Console.WriteLine("Could not be parsed.");

                    if(tresor.positionX > gameCarte.positionX && tresor.positionY > gameCarte.positionY)
                        throw new Exception("out of range");
                    map.Categories.Add(tresor);
                    break;
                case 'A':
                    Aventurier aventurier = new Aventurier();
                    aventurier.type = type;
                    aventurier.nbTresors = 0;
                    bool isParsableAventurier = Int32.TryParse(datas[2], out number);

                    if (isParsableAventurier)
                        aventurier.positionX = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    isParsableAventurier = Int32.TryParse(datas[3], out number);
                    if (isParsableAventurier)
                        aventurier.positionY = number;
                    else
                        Console.WriteLine("Could not be parsed.");
                    aventurier.nom = datas[1];
                    aventurier.oriantation = datas[4].ToCharArray()[0];
                    aventurier.seqMouvement = datas[5];
                    var seqMouvementSplitToChar = aventurier.seqMouvement.ToCharArray();
                    foreach (var mouvement in seqMouvementSplitToChar)
                    {
                        switch (mouvement)
                        {
                            case 'A':
                                try
                                {
                                    switch (aventurier.oriantation)
                                    {
                                        case 'S':
                                            if (!map.Categories.Exists(x => x.positionY == aventurier.positionY + 1 && x.positionX == aventurier.positionX && x.type != 'T')){
                                                aventurier.positionY += 1;
                                                if (!map.Categories.Exists(x => x.type == 'T' ))
                                                    aventurier.nbTresors += 1;
                                            }
                                            break;
                                        case 'N':
                                            if (!map.Categories.Exists(x => x.positionY == aventurier.positionY -1  && x.positionX == aventurier.positionX))
                                                aventurier.positionY -= 1;
                                            break;
                                        case 'E':
                                            if (!map.Categories.Exists(x => x.positionY == aventurier.positionY -1  && x.positionX == aventurier.positionX +1 ))
                                                aventurier.positionX += 1;
                                            break;
                                        case 'O':
                                            if (!map.Categories.Exists(x => x.positionY == aventurier.positionY -1  && x.positionX == aventurier.positionX -1 ))
                                                aventurier.positionX -= 1;
                                            break;
                                        default:
                                            break;
                                    }
                                    
                                }
                                catch (System.Exception)
                                {
                                    Console.WriteLine("cogner");
                                };
                                break;
                            case 'G':
                                switch (aventurier.oriantation)
                                {
                                    case 'S':
                                        aventurier.oriantation = 'E';
                                        break;
                                    case 'O':
                                        aventurier.oriantation = 'S';
                                        break;
                                    case 'N':
                                        aventurier.oriantation = 'O';
                                        break;
                                    case 'E':
                                        aventurier.oriantation = 'N';
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case 'D':
                                switch (aventurier.oriantation)
                                {
                                    case 'S':
                                        aventurier.oriantation = 'O';
                                        break;
                                    case 'O':
                                        aventurier.oriantation = 'S';
                                        break;
                                    case 'N':
                                        aventurier.oriantation = 'E';
                                        break;
                                    case 'E':
                                        aventurier.oriantation = 'N';
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    Console.WriteLine(JsonConvert.SerializeObject(aventurier));
                    break;

                default:
                    Console.WriteLine("Sunday");
                    break;
            }
            return map;
            // throw new NotImplementedException();
        }
    }
}