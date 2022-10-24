using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUi.Interfaces;
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
        private readonly IFileManager _fileManager;
        public ConsoleManagerService(ILogger<ConsoleManagerService> log, IConfiguration config, IConsoleManagerErrors consoleManagerErrors, IFileManager fileManager)
        {
            _log = log;
            _config = config;
            _consoleManagerErrors = consoleManagerErrors;
            _fileManager = fileManager;
        }
        public void Run(string[] arg)
        {
            // check error in arg
            _consoleManagerErrors.checkErrorEntry(arg);

            // Display the file contents by using a foreach loop.
            var lines = _fileManager.ReadFile(arg[0]);

            Game game = new Game() { positions = new List<Position>(), categories = new List<Category>(), treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };

            // Use a tab to indent each line of the file.
            // _consoleManagerErrors.checkErrorFile(lines);

            foreach (var item in lines)
            {
                if (!item.StartsWith('#'))
                    game = GenerateGame(item, game);
            }

            game = PlayGame(game);
            _fileManager.WriteFile(game);

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");

            // System.Console.ReadKey();
        }

        public Game GenerateGame(string data, Game game)
        {
            List<string> datas = data.Split(" ").ToList();
            var type = datas[0].ToCharArray()[0];
            Game games = new Game();
            Category gameCarte = new Category();

            if (type != 'C')
                gameCarte = game.categories.Find(x => x.type == 'C');
            int positionX;
            int positionY;
            int nbTreasures;
            bool isParsablePositionX = false;
            bool isParsablePositionY = false;
            bool isParsablenbTreasures = false;
            Position position = new Position();
            switch (type)
            {
                case var typeCategory when (typeCategory == 'C' || typeCategory == 'M'):
                    if (datas.Count > 3)
                        throw new Exception($"to much arguments {JsonConvert.SerializeObject(datas)}");
                    if (datas.Count < 3)
                        throw new Exception($"missing arguments {JsonConvert.SerializeObject(datas)}");
                    if (!game.categories.Exists(x => type == 'C' && x.type == 'C'))
                    {
                        isParsablePositionX = false;
                        isParsablePositionY = false;
                        Category category = new Category();
                        category.type = type;
                        isParsablePositionX = Int32.TryParse(datas[1], out positionX);
                        isParsablePositionY = Int32.TryParse(datas[2], out positionY);

                        if (isParsablePositionX && isParsablePositionY)
                        {
                            if (!game.positions.Exists(x => x.positionX == positionX && x.positionY == positionY))
                            {
                                if (type != 'C' && (positionX > gameCarte.positionX || positionY > gameCarte.positionY || positionX < 0 || positionY < 0))
                                    throw new Exception("out of range");
                                category.positionX = positionX;
                                category.positionY = positionY;
                                position.positionX = positionX;
                                position.positionY = positionY;
                            }
                            else
                                throw new Exception($"in this position x: {positionX} and y: {positionY} already exist");

                        }

                        else
                            throw new Exception($"Some arguments is not valid {JsonConvert.SerializeObject(datas)}");
                        game.categories.Add(category);
                        game.positions.Add(position);
                    }
                    else
                        throw new Exception("Map already init");
                    break;
                case 'T':
                    if (datas.Count > 4)
                        throw new Exception($"to much arguments {JsonConvert.SerializeObject(datas)}");
                    if (datas.Count < 4)
                        throw new Exception($"missing arguments {JsonConvert.SerializeObject(datas)}");
                    isParsablePositionX = false;
                    isParsablePositionY = false;
                    isParsablenbTreasures = false;
                    Treasure treasure = new Treasure();
                    treasure.type = type;
                    isParsablePositionX = Int32.TryParse(datas[1], out positionX);
                    isParsablePositionY = Int32.TryParse(datas[2], out positionY);
                    isParsablenbTreasures = Int32.TryParse(datas[3], out nbTreasures);
                    if (isParsablePositionX && isParsablePositionY && isParsablenbTreasures)
                    {
                        if (positionX > gameCarte.positionX || positionY > gameCarte.positionY || positionX < 0 || positionY < 0)
                            throw new Exception("out of range");
                        if (!game.positions.Exists(x => x.positionX == positionX && x.positionY == positionY))
                        {
                            treasure.positionX = positionX;
                            treasure.positionY = positionY;
                            treasure.nbTreasures = nbTreasures;
                            position.positionX = positionX;
                            position.positionY = positionY;
                        }
                        else
                            throw new Exception($"in this position x: {positionX} and y: {positionY} already exist");
                    }
                    else
                        throw new Exception($"Some arguments is not valid {JsonConvert.SerializeObject(datas)}");
                    game.treasures.Add(treasure);
                    game.positions.Add(position);
                    break;
                case 'A':
                    if (datas.Count > 6)
                        throw new Exception($"to much arguments {JsonConvert.SerializeObject(datas)}");
                    if (datas.Count < 6)
                        throw new Exception($"missing arguments {JsonConvert.SerializeObject(datas)}");
                    Adventurer adventurer = new Adventurer();
                    adventurer.type = type;
                    adventurer.nbTreasures = 0;
                    isParsablePositionX = false;
                    isParsablePositionY = false;
                    isParsablePositionX = Int32.TryParse(datas[2], out positionX);
                    isParsablePositionY = Int32.TryParse(datas[3], out positionY);
                    Position positionAventurier = new Position();
                    if (isParsablePositionX && isParsablePositionY)
                    {
                        if (positionX > gameCarte.positionX || positionY > gameCarte.positionY || positionX < 0 || positionY < 0)
                            throw new Exception("out of range");
                        if (!game.positions.Exists(x => x.positionX == positionX && x.positionY == positionY))
                        {
                            adventurer.positionX = positionX;
                            adventurer.positionY = positionY;
                            position.positionX = positionX;
                            position.positionY = positionY;
                        }
                        else
                            throw new Exception($"in this position x: {positionX} and y: {positionY} already exist");
                    }
                    else
                        throw new Exception($"Some arguments is not valid {JsonConvert.SerializeObject(datas)}");
                    if (adventurer.positionX > gameCarte.positionX && adventurer.positionY > gameCarte.positionY)
                        throw new Exception("out of range");
                    adventurer.name = datas[1];
                    adventurer.orientation = datas[4].ToCharArray()[0];
                    adventurer.lastOrientation = datas[4].ToCharArray()[0];
                    adventurer.seqMouvement = datas[5];
                    game.adventurers.Add(adventurer);
                    game.positions.Add(position);
                    break;
                case '#':
                    break;
                default:
                    throw new Exception($"type {type} undefined");
            }
            return game;
        }
        public Game PlayGame(Game game)
        {
            var adventurers = game.adventurers;
            foreach (var adventurer in adventurers)
            {
                var seqMouvementSplitToChar = adventurer.seqMouvement.ToCharArray();
                foreach (var mouvement in seqMouvementSplitToChar)
                {
                    ActionAdventurer(mouvement, adventurer, game);
                }
            }
            return game;
        }

        public void ActionAdventurer(char action, Adventurer aventurier, Game game)
        {
            var carte = game.categories.Find(x => x.type == 'C');
            var lastPositionX = aventurier.positionX;
            var lastPositionY = aventurier.positionY;
            switch (action)
            {
                case 'A':
                    try
                    {
                        if ((aventurier.lastOrientation == aventurier.orientation) &&(carte.positionX -1 == aventurier.positionX || carte.positionY-1 == aventurier.positionY))
                                    break;
                        switch (aventurier.orientation)
                        {
                            case 'S':
                                    if (!game.categories.Exists(x => x.positionY == aventurier.positionY + 1 && x.positionX == aventurier.positionX) && !game.adventurers.Exists(x => x.type != 'A'))
                                        aventurier.positionY += 1;
                                break;
                            case 'N':
                                if (!game.categories.Exists(x => x.positionY == aventurier.positionY - 1 && x.positionX == aventurier.positionX) && !game.adventurers.Exists(x => x.type != 'A'))
                                    aventurier.positionY -= 1;
                                break;
                            case 'E':
                                if (!game.categories.Exists(x => x.positionY == aventurier.positionY - 1 && x.positionX == aventurier.positionX + 1) && !game.adventurers.Exists(x => x.type != 'A'))
                                    aventurier.positionX += 1;
                                break;
                            case 'O':
                                if (!game.categories.Exists(x => x.positionY == aventurier.positionY - 1 && x.positionX == aventurier.positionX - 1) && !game.adventurers.Exists(x => x.type != 'A'))
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

                    if (game.treasures.Exists(x => x.type == 'T' && x.positionX == aventurier.positionX && x.positionY == aventurier.positionY) && (lastPositionX !=  aventurier.positionX || lastPositionY !=  aventurier.positionY))
                    {
                        game.treasures.Where(x => x.positionX == aventurier.positionX && x.positionY == aventurier.positionY).First().nbTreasures -= 1;
                        aventurier.nbTreasures += 1;
                    }

                    break;
                case 'G':
                    aventurier.lastOrientation = aventurier.orientation;
                    switch (aventurier.orientation)
                    {
                        case 'S':
                            aventurier.orientation = 'E';
                            break;
                        case 'O':
                            aventurier.orientation = 'S';
                            break;
                        case 'N':
                            aventurier.orientation = 'O';
                            break;
                        case 'E':
                            aventurier.orientation = 'N';
                            break;
                        default:
                            break;
                    }
                    break;
                case 'D':
                    aventurier.lastOrientation = aventurier.orientation;
                    switch (aventurier.orientation)
                    {
                        case 'S':
                            aventurier.orientation = 'O';
                            break;
                        case 'O':
                            aventurier.orientation = 'N';
                            break;
                        case 'N':
                            aventurier.orientation = 'E';
                            break;
                        case 'E':
                            aventurier.orientation = 'S';
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}