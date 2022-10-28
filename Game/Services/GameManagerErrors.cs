using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using GameConsole.Common;
using GameConsole.Models;
using Newtonsoft.Json;

namespace GameConsole.Services
{
    public class GameManagerErrors
    {
        public class GameManagerErrorsService : IGameManagerErrors
        {
            public void CheckAdventurerEntries(List<string> options)
            {
                if (options.Count > 6)
                    throw new Exception($"to much arguments {JsonConvert.SerializeObject(options)}");
                if (options.Count < 6)
                    throw new Exception($"missing arguments {JsonConvert.SerializeObject(options)}");
                Utils.checkPositionIsInt(options[2], options[3]);
                if (!System.Enum.IsDefined(typeof(Orientation), options[4]))
                    throw new Exception($"Orientation must be one of this value N, E, W, S {JsonConvert.SerializeObject(options[4])}");
                if (!Regex.IsMatch(options[5], "^[AGD]+$"))
                    throw new Exception($"Path must only contain A, G or D {JsonConvert.SerializeObject(options[5])}");
            }

            public void CheckTreasureEntries(List<string> options)
            {
                if (options.Count > 4)
                    throw new Exception($"to much arguments {JsonConvert.SerializeObject(options)}");
                if (options.Count < 4)
                    throw new Exception($"missing arguments {JsonConvert.SerializeObject(options)}");
                Utils.checkPositionIsInt(options[1], options[2]);
                int nbTreasure;
                try
                {
                    nbTreasure = Int32.Parse(options[3]);
                }
                catch (System.Exception)
                {
                    throw new Exception($"Nb Treasure must be numeric {options[3]}");
                }
            }
            public void CheckMapOrMountainEntries(List<string> options)
            {
                if (options.Count > 3)
                    throw new Exception($"to much arguments {JsonConvert.SerializeObject(options)}");
                if (options.Count < 3)
                    throw new Exception($"missing arguments {JsonConvert.SerializeObject(options)}");
                Utils.checkPositionIsInt(options[1], options[2]);
            }

            public void CheckErrorEntry(string[] arg)
            {
                if (arg.Length == 0)
                    throw new Exception("missing file in arg");

                if (arg.Length > 1)
                    throw new Exception($"there are too many arguments {string.Join(",", arg)}");

                FileInfo fi = new FileInfo(arg[0]);
                if (fi.Extension != ".txt")
                    throw new Exception("the file extension is not in the correct format, we only accept .txt files");

                if (fi.Exists == false)
                    throw new Exception($"the file {fi.Name} doesn't not exist in the directory {fi.DirectoryName}");
            }
        }
    }
}