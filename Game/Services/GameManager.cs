using System;
using System.Collections.Generic;
using System.Linq;
using GameConsole.Interfaces;
using GameConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleManager
{
    public class GameManagerService : IGameManager
    {
        private readonly ILogger<GameManagerService> _log;
        private readonly IConfiguration _config;
        private readonly IGameManagerErrors _gameManagerErrors;
        private readonly IFileManager _fileManager;
        public GameManagerService(ILogger<GameManagerService> log, IConfiguration config, IGameManagerErrors consoleManagerErrors, IFileManager fileManager)
        {
            _log = log;
            _config = config;
            _gameManagerErrors = consoleManagerErrors;
            _fileManager = fileManager;
        }
        public void Run(string[] arg)
        {
            // check error in arg
            _gameManagerErrors.CheckErrorEntry(arg);

            // Display the file contents by using a foreach loop.

            var lines = _fileManager.ReadFile(arg[0]);

            Game game = new Game() { positions = new List<Position>(), categories = new List<Category>(), treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };

            // Use a tab to indent each line of the file.
            foreach (var item in lines)
            {
                if (!item.StartsWith('#'))
                    game = GenerateGame(item, game);
            }

            game = PlayGame(game);
            _fileManager.WriteFile(game);
        }

        public Game GenerateGame(string data, Game game)
        {
            List<string> datas = data.Split(" ").ToList();
            var type = datas[0][0];
            Game games = new Game();
            Category gameCarte = new Category();

            if (type != 'C')
                gameCarte = game.categories.Find(x => x.type == 'C');
            Position position = new Position();
            switch (type)
            {
                case var typeCategory when (typeCategory == 'C' || typeCategory == 'M'):
                    if (!game.categories.Exists(x => type == 'C' && x.type == 'C'))
                    {
                        Category category = CreateMapOrMountain(datas);
                        if (category.type != 'C')
                        {
                            if (!game.positions.Exists(x => x.positionX == category.positionX && x.positionY == category.positionY))
                            {
                                if (type != 'C' && (category.positionX > gameCarte.positionX || category.positionY > gameCarte.positionY || category.positionX < 0 || category.positionY < 0))
                                    throw new Exception("out of range");
                                position.positionX = category.positionX;
                                position.positionY = category.positionY;
                            }
                            else
                                throw new Exception($"in this position x: {category.positionX} and y: {category.positionY} already exist");
                            game.positions.Add(position);
                        }
                        game.categories.Add(category);
                            
                    }
                    else
                        throw new Exception("Map already init");
                    break;
                case 'T':
                    Treasure treasure = CreateTreasure(datas);
                    if (treasure.positionX > gameCarte.positionX || treasure.positionY > gameCarte.positionY || treasure.positionX < 0 || treasure.positionY < 0)
                        throw new Exception("out of range");
                    if (!game.positions.Exists(x => x.positionX == treasure.positionX && x.positionY == treasure.positionY))
                    {

                        position.positionX = treasure.positionX;
                        position.positionY = treasure.positionY;
                    }
                    else
                        throw new Exception($"in this position x: {treasure.positionX} and y: {treasure.positionY} already exist");
                    game.treasures.Add(treasure);
                    game.positions.Add(position);
                    break;
                case 'A':
                    Adventurer adventurer = CreateAdventurer(datas);
                    Position positionAventurier = new Position();
                    if (adventurer.positionX > gameCarte.positionX || adventurer.positionY > gameCarte.positionY || adventurer.positionX < 0 || adventurer.positionY < 0)
                        throw new Exception("out of range");
                    if (!game.positions.Exists(x => x.positionX == adventurer.positionX && x.positionY == adventurer.positionY))
                    {
                        position.positionX = adventurer.positionX;
                        position.positionY = adventurer.positionY;
                    }
                    else
                        throw new Exception($"in this position x: {adventurer.positionX} and y: {adventurer.positionY} already exist");
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
                var seqMouvementSplitToChar = adventurer.path.ToCharArray();
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
                    if ((aventurier.lastOrientation == aventurier.orientation) && (carte.positionX - 1 == aventurier.positionX || carte.positionY - 1 == aventurier.positionY))
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

                    if (game.treasures.Exists(x => x.type == 'T' && x.positionX == aventurier.positionX && x.positionY == aventurier.positionY) && (lastPositionX != aventurier.positionX || lastPositionY != aventurier.positionY))
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

        public Adventurer CreateAdventurer(List<string> options)
        {
            _gameManagerErrors.CheckAdventurerEntries(options);
            return new Adventurer()
            {
                type = options[0][0],
                name = options[1],
                positionX = Int32.Parse(options[2]),
                positionY = Int32.Parse(options[3]),
                orientation = options[4][0],
                path = options[5],
                nbTreasures = 0
            };
        }
        public Treasure CreateTreasure(List<string> options)
        {
            _gameManagerErrors.CheckTreasureEntries(options);
            return new Treasure()
            {
                type = options[0][0],
                positionX = Int32.Parse(options[1]),
                positionY = Int32.Parse(options[2]),
                nbTreasures = Int32.Parse(options[3])
            };
        }
        public Category CreateMapOrMountain(List<string> options)
        {
            _gameManagerErrors.CheckMapOrMountainEntries(options);
            return new Category()
            {
                type = options[0][0],
                positionX = Int32.Parse(options[1]),
                positionY = Int32.Parse(options[2])
            };
        }
    }
}