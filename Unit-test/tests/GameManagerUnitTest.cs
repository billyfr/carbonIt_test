using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleManager;
using GameConsole.Interfaces;
using GameConsole.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using static GameConsole.Services.GameManagerErrors;

namespace unit_test
{
    public class GameManagerUnitTest
    {
        GameManagerService _gameManager;

        Mock<ILogger<GameManagerService>> log = new Mock<ILogger<GameManagerService>>();
        Mock<IConfiguration> config = new Mock<IConfiguration>();
        Mock<IGameManagerErrors> gameManagerErrors = new Mock<IGameManagerErrors>();
        Mock<IFileManager> fileManager = new Mock<IFileManager>();

        [SetUp]
        public void Setup()
        {
            _gameManager = new GameManagerService(log.Object, config.Object, gameManagerErrors.Object, fileManager.Object);
        }

        [Test]
        public void Mountain_Out_Of_Range()
        {
            // arrange
            var data = "M 4 4";
            Game game = new Game() { positions = new List<Position>(), categories = new List<Category>() { new Category() { type = 'C', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });
            // assert
            StringAssert.Contains("out of range", ex.Message.ToString());
        }

        [Test]
        public void Mountain_Already_Exist()
        {
            // arrange
            var data = "M 1 1";
            Game game = new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };
            List<string> datas = new List<string>() { "M", "1", "1" };
            var category = _gameManager.CreateMapOrMountain(datas);
            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });

            // assert
            StringAssert.Contains($"in this position x: {category.positionX} and y: {category.positionY} already exist", ex.Message.ToString());
        }

        [Test]
        public void Map_Already_Exist()
        {
            // arrange
            var data = "C 1 1";
            Game game = new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });
            // assert
            StringAssert.Contains("Map already init", ex.Message.ToString());
        }

        [Test]
        public void Treasure_Out_Of_Range()
        {
            // arrange
            var data = "T 4 4 1";
            Game game = new Game() { positions = new List<Position>(), categories = new List<Category>() { new Category() { type = 'C', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });
            // assert
            StringAssert.Contains("out of range", ex.Message.ToString());
        }

        [Test]
        public void Treasure_Already_Exist()
        {
            // arrange
            var data = "T 1 1 1";
            Game game = new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };
            List<string> datas = new List<string>() { "T", "1", "1", "1" };
            var treasure = _gameManager.CreateTreasure(datas);
            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });

            // assert
            StringAssert.Contains($"in this position x: {treasure.positionX} and y: {treasure.positionY} already exist", ex.Message.ToString());
        }

        [Test]
        public void Adventurer_Out_Of_Range()
        {
            // arrange
            var data = "A Lara 4 4 S AADADAGGA";
            Game game = new Game() { positions = new List<Position>(), categories = new List<Category>() { new Category() { type = 'C', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });
            // assert
            StringAssert.Contains("out of range", ex.Message.ToString());
        }

        [Test]
        public void Adventurer_Already_Exist()
        {
            // arrange
            var data = "A Lara 1 1 S AADADAGGA";
            Game game = new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };
            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });

            // assert
            StringAssert.Contains($"in this position x: 1 and y: 1 already exist", ex.Message.ToString());
        }

        [Test]
        public void Type_Doesnt_Exist()
        {
            // arrange
            var data = "P Lara 1 1 S AADADAGGA";
            Game game = new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };
            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManager.GenerateGame(data, game);
            });

            // assert
            StringAssert.Contains($"type P undefined", ex.Message.ToString());
        }

        [Test]
        public void Should_Generate_Map()
        {
            // arrange
            var data = "C 3 4";
            Game game = new Game() { positions = new List<Position>(), categories = new List<Category>() { }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };
            // act
            var result = _gameManager.GenerateGame(data, game);

            // assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Game() { positions = new List<Position>(), categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() }), JsonConvert.SerializeObject(result));
        }

        [Test]
        public void Should_Generate_Mountain()
        {
            // arrange
            var data = "M 1 1";
            Game game = new Game() { positions = new List<Position>(), categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };
            // act
            var result = _gameManager.GenerateGame(data, game);

            // assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = 1, positionY = 1 } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() }), JsonConvert.SerializeObject(result));
        }

        [Test]
        public void Should_Generate_Treasure()
        {
            // arrange
            var data = "T 2 1 1";
            Game game = new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = '1', positionY = '1' } }, treasures = new List<Treasure>(), adventurers = new List<Adventurer>() };
            // act
            var result = _gameManager.GenerateGame(data, game);

            // assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 1 }, new Position() { positionX = 2, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = '1', positionY = '1' } }, treasures = new List<Treasure>() { new Treasure() { type = 'T', positionX = 2, positionY = 1, nbTreasures = 1 } }, adventurers = new List<Adventurer>() }), JsonConvert.SerializeObject(result));
        }

        [Test]
        public void Should_Generate_Adventurer()
        {
            // arrange
            var data = "A Lara 1 1 S AADADAGGA";
            Game game = new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 0 }, new Position() { positionX = 0, positionY = 3 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = '1', positionY = '0' } }, treasures = new List<Treasure>() { new Treasure() { type = 'T', positionX = 0, positionY = 3, nbTreasures = 1 } }, adventurers = new List<Adventurer>() };
            // act
            var result = _gameManager.GenerateGame(data, game);

            // assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Game() { positions = new List<Position>() { new Position() { positionX = 1, positionY = 0 }, new Position() { positionX = 0, positionY = 3 }, new Position() { positionX = 1, positionY = 1 } }, categories = new List<Category>() { new Category() { type = 'C', positionX = 3, positionY = 4 }, new Category() { type = 'M', positionX = '1', positionY = '0' } }, treasures = new List<Treasure>() { new Treasure() { type = 'T', positionX = 0, positionY = 3, nbTreasures = 1 } }, adventurers = new List<Adventurer>() { new Adventurer() { type = 'A', name = "Lara", positionX = 1, positionY = 1, orientation = 'S', path = "AADADAGGA" } } }), JsonConvert.SerializeObject(result));
        }


        [Test]
        public void Should_Create_Map()
        {
            // arrange
            List<string> datas = new List<string>() { "C", "3", "4" };
            // act
            var result = _gameManager.CreateMapOrMountain(datas);
            // assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Category() { type = 'C', positionX = 3, positionY = 4 }), JsonConvert.SerializeObject(result));
        }

        [Test]
        public void Should_Create_Mountain()
        {
            // arrange
            List<string> datas = new List<string>() { "M", "1", "1" };
            // act
            var result = _gameManager.CreateMapOrMountain(datas);

            // assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Category() { type = 'M', positionX = 1, positionY = 1 }), JsonConvert.SerializeObject(result));
        }

        [Test]
        public void Should_Create_Treasure()
        {
            // arrange
            List<string> datas = new List<string>() { "T", "2", "1", "1" };
            // act
            var result = _gameManager.CreateTreasure(datas);

            // assert
            Assert.AreEqual(JsonConvert.SerializeObject(new Treasure() { type = 'T', positionX = 2, positionY = 1, nbTreasures = 1 }), JsonConvert.SerializeObject(result));
        }

        [Test]
        public void Should_Create_Adventurer()
        {
            // arrange
            List<string> datas = new List<string>() { "A", "Lara", "1", "1", "S", "AADADAGGA" };
            // act
            var result = _gameManager.CreateAdventurer(datas);
            // assert
            Assert.AreEqual(JsonConvert.SerializeObject( new Adventurer() { type = 'A', name = "Lara", positionX = 1, positionY = 1, orientation = 'S', path = "AADADAGGA" }), JsonConvert.SerializeObject(result));
        }


    }
}