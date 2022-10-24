using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ConsoleUi.Interfaces;
using ConsoleUi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleUi.Services
{
    public class FileManagerService : IFileManager
    {
        private readonly ILogger<FileManagerService> _log;
        private readonly IConfiguration _config;
        public FileManagerService(ILogger<FileManagerService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public List<string> ReadFile(string arg)
        {
            return File.ReadLines(arg)
                        .Select(x => Regex.Replace(x, " -", string.Empty))
                        .ToList();
        }

        public void WriteFile(Game game)
        {
            var objectCarte = game.categories.Where(x => x.type == 'C').First();
            var objectMontagne = game.categories.Where(x => x.type == 'M').ToList();
            var objectTresors = game.treasures;
            var objectAventurier = game.adventurers;
            string montagnesString = "";
            string tresorsString = "";
            string aventuriersString = "";
            for (int i = 0; i < objectMontagne.Count(); i++)
            {
                if (i == objectMontagne.Count() - 1)
                    montagnesString += $"{objectMontagne[i].type.ToString()} - {objectMontagne[i].positionX.ToString()} - {objectMontagne[i].positionY.ToString()}";
                else
                    montagnesString += $"{objectMontagne[i].type.ToString()} - {objectMontagne[i].positionX.ToString()} - {objectMontagne[i].positionY.ToString()}\n";
            }
            for (int i = 0; i < objectTresors.Count(); i++)
            {
                if (objectTresors[i].nbTreasures != 0)
                {
                    if (i == objectTresors.Where(x => x.nbTreasures != 0).Count() - 1)
                        tresorsString += $"{objectTresors[i].type.ToString()} - {objectTresors[i].positionX.ToString()} - {objectTresors[i].positionY.ToString()} - {objectTresors[i].nbTreasures.ToString()}";
                    else
                        tresorsString += $"{objectTresors[i].type.ToString()} - {objectTresors[i].positionX.ToString()} - {objectTresors[i].positionY.ToString()} - {objectTresors[i].nbTreasures.ToString()}\n";
                }
            }
            for (int i = 0; i < objectAventurier.Count(); i++)
            {
                if (i == objectAventurier.Count() - 1)
                    aventuriersString += $"{objectAventurier[i].type.ToString()} - {objectAventurier[i].name.ToString()} - {objectAventurier[i].positionX.ToString()} - {objectAventurier[i].positionY.ToString()} - {objectAventurier[i].orientation.ToString()} - {objectAventurier[i].nbTreasures.ToString()}";
                else
                    aventuriersString += $"{objectAventurier[i].type.ToString()} - {objectAventurier[i].name.ToString()} - {objectAventurier[i].positionX.ToString()} - {objectAventurier[i].positionY.ToString()} - {objectAventurier[i].orientation.ToString()} - {objectAventurier[i].nbTreasures.ToString()}\n";
            }
            string response = $@"{objectCarte.type} - {objectCarte.positionX} - {objectCarte.positionY} 
{montagnesString}
{tresorsString}
{aventuriersString}";

            File.WriteAllText("WriteLines.txt", response);
        }
    }
}