using System;
using System.Collections.Generic;
using GameConsole.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using static GameConsole.Services.GameManagerErrors;

namespace unit_test
{
    public class GameManagerErrorsUnitTest
    {
        GameManagerErrorsService _gameManagerErrors;
        [SetUp]
        public void Setup()
        {
            _gameManagerErrors = new GameManagerErrorsService();
        }

        [Test]

        public void Missing_Arg_File()
        {
            // arrange
            var arg = new string[] { };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckErrorEntry(arg);
            });
            // assert
            StringAssert.Contains("missing file in arg", ex.Message.ToString());
        }

        [Test]

        public void File_Not_Good_Extention()
        {
            // arrange
            var arg = new string[] { "recruteMoi.pdf" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckErrorEntry(arg);
            });
            // assert
            StringAssert.Contains("the file extension is not in the correct format, we only accept .txt files", ex.Message.ToString());
        }

        [Test]

        public void To_Many_Arg()
        {
            // arrange
            var arg = new string[] { "recruteMoi.pdf", "vous allez pas le regretter" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckErrorEntry(arg);
            });
            // assert
            StringAssert.Contains("there are too many arguments recruteMoi.pdf,vous allez pas le regretter", ex.Message.ToString());
        }

        [Test]

        public void File_Doesnt_Exist()
        {
            // arrange
            var arg = new string[] { "./jespere/avoir/reussi/le/test/rdvaubar.txt" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckErrorEntry(arg);
            });
            // assert
            StringAssert.Contains($"the file rdvaubar.txt doesn't not exist in the directory {Environment.CurrentDirectory}/jespere/avoir/reussi/le/test", ex.Message.ToString());
        }

        [Test]

        public void To_Much_Attribut_For_Adventurer()
        {
            // arrange
            var adventurer = (new List<string>() { "A", "Lara", "1", "1", "S", "AADADAGGA", "recrutez moi" });

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckAdventurerEntries(adventurer);
            });
            // assert
            StringAssert.Contains($"to much arguments {JsonConvert.SerializeObject(adventurer)}", ex.Message.ToString());
        }

        [Test]

        public void Missing_Attribut_For_Adventurer()
        {
            // arrange
            var adventurer = new List<string>() { "A", "Lara", "1", "1", "S" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckAdventurerEntries(adventurer);
            });
            // assert
            StringAssert.Contains($"missing arguments {JsonConvert.SerializeObject(adventurer)}", ex.Message.ToString());
        }

        [Test]

        public void Wrong_Position_Type_For_Adventurer()
        {
            // arrange
            var adventurer = new List<string>() { "A", "Lara", "vous me", "voulez dans votre équipe", "S", "AADADAGGA" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckAdventurerEntries(adventurer);
            });
            // assert
            StringAssert.Contains($"Position X or Y must be numeric {adventurer[2]} {adventurer[3]}", ex.Message.ToString());
        }

        [Test]

        public void Wrong_Orientation_For_Adventurer()
        {
            // arrange
            var adventurer = new List<string>() { "A", "Lara", "1", "1", "T", "AADADAGGA" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckAdventurerEntries(adventurer);
            });
            // assert
            StringAssert.Contains($"Orientation must be one of this value N, E, W, S {JsonConvert.SerializeObject(adventurer[4])}", ex.Message.ToString());
        }
        [Test]

        public void Wrong_Path_For_Adventurer()
        {
            // arrange
            var adventurer = new List<string>() { "A", "Lara", "1", "1", "S", "MAADADAGGAZ" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckAdventurerEntries(adventurer);
            });
            // assert
            StringAssert.Contains($"Path must only contain A, G or D {JsonConvert.SerializeObject(adventurer[5])}", ex.Message.ToString());
        }

        [Test]

        public void To_Much_Attribut_For_Treasure()
        {
            // arrange
            var treasure = (new List<string>() { "T", "1", "1", "1", "S", "AADADAGGA", "recrutez moi" });

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckTreasureEntries(treasure);
            });
            // assert
            StringAssert.Contains($"to much arguments {JsonConvert.SerializeObject(treasure)}", ex.Message.ToString());
        }

        [Test]

        public void Missing_Attribut_For_Treasure()
        {
            // arrange
            var treasure = new List<string>() { "T", "1" };

            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckTreasureEntries(treasure);
            });
            // assert
            StringAssert.Contains($"missing arguments {JsonConvert.SerializeObject(treasure)}", ex.Message.ToString());
        }

        [Test]

        public void Wrong_Position_Type_For_Treasure()
        {
            // arrange
            var treasure = new List<string>() { "T", "1", "toto", "1" };
            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckTreasureEntries(treasure);
            });
            // assert
            StringAssert.Contains($"Position X or Y must be numeric {treasure[1]} {treasure[2]}", ex.Message.ToString());
        }

        [Test]

        public void Wrong_Nb_Treasure_Type_For_Treasure()
        {
            // arrange
            var treasure = new List<string>() { "T", "1", "1", "AA" };
            // act
            var ex = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckTreasureEntries(treasure);
            });
            // assert
            StringAssert.Contains($"Nb Treasure must be numeric {treasure[3]}", ex.Message.ToString());
        }

        [Test]

        public void To_Much_Attribut_For_Map_Or_Mountain()
        {
            // arrange
            var mountain = (new List<string>() { "M", "1", "2", "4" });
            var map = (new List<string>() { "C", "1", "2", "4" });
            // act
            var exMountain = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckMapOrMountainEntries(mountain);
            });
            var exMap = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckMapOrMountainEntries(map);
            });
            // assert
            StringAssert.Contains($"to much arguments {JsonConvert.SerializeObject(mountain)}", exMountain.Message.ToString());
            StringAssert.Contains($"to much arguments {JsonConvert.SerializeObject(map)}", exMap.Message.ToString());

        }

        [Test]

        public void Missing_Attribut_For_Map_Or_Mountain()
        {
            // arrange
            var mountain = (new List<string>() { "M", "1" });
            var map = (new List<string>() { "C", "1" });

            // act
            var exMountain = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckMapOrMountainEntries(mountain);
            });
            var exMap = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckMapOrMountainEntries(map);
            });
            // assert
            StringAssert.Contains($"missing arguments {JsonConvert.SerializeObject(mountain)}", exMountain.Message.ToString());
            StringAssert.Contains($"missing arguments {JsonConvert.SerializeObject(map)}", exMap.Message.ToString());
        }

        [Test]

        public void Wrong_Position_Type_For_Map_Or_Mountain()
        {
            // arrange
            var mountain = new List<string>() { "M", "1", "toto" };
            var map = new List<string>() { "C", "1", "toto" };

            // act
            var exMountain = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckMapOrMountainEntries(mountain);
            });
            var exMap = Assert.Throws<Exception>(() =>
            {
                _gameManagerErrors.CheckMapOrMountainEntries(map);
            });
            // assert
            StringAssert.Contains($"Position X or Y must be numeric {mountain[1]} {mountain[2]}", exMountain.Message.ToString());
            StringAssert.Contains($"Position X or Y must be numeric {map[1]} {map[2]}", exMap.Message.ToString());

        }
    }
}