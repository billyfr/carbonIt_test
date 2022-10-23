using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ConsoleUi.Services
{
    public class ConsoleManagerErrors
    {
        public class ConsoleManagerErrorsService : IConsoleManagerErrors
        {
            private readonly ILogger<ConsoleManagerErrorsService> _log;
            private readonly IConfiguration _config;

            public ConsoleManagerErrorsService(ILogger<ConsoleManagerErrorsService> log, IConfiguration config)
            {
                _log = log;
                _config = config;
            }

            public void checkErrorEntry(string[] arg)
            {
                if(arg.Length == 0){
                _log.LogError("il manque le fichier dans la commande");
                throw new Exception("il manque le fichier dans la commande");
            };

            if(arg.Length > 1){
                var err = $"il y'a trop argument {string.Join(",", arg)}";
                _log.LogError(err);
                throw new Exception(err);
            };

            FileInfo fi = new FileInfo(arg[0]);
            if(fi.Extension != ".txt"){
                var err = "l'extention du fichier n'est pas au bon format, nous acceptons que les fichiers .txt";
                _log.LogError(err);
                throw new Exception(err);
            };
            
            if(fi.Exists == false){
                var err = $"le fichier {fi.Name} n'existe pas dans le dossier {fi.DirectoryName}";
                _log.LogError(err);
                throw new Exception(err);
            };
            }
        }
    }
}