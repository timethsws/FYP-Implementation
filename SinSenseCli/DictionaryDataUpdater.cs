using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using SinSenseCore.Entities;
using SinSenseInfastructure;
using SinSenseInfastructure.Services;

namespace SinSenseCli
{
    public class DictionaryDataUpdater
    {
        private readonly WordRelationManagerService wordRelationManager;
        private readonly WordManagerService wordManager;
        private readonly AppDbContext dbContext;
        private readonly ILogger<DictionaryDataUpdater> logger;

        public DictionaryDataUpdater(AppDbContext dbContext,
            WordRelationManagerService wordRelationManager,
            WordManagerService wordManager,
            ILogger<DictionaryDataUpdater> logger)
        {
            this.wordRelationManager = wordRelationManager;
            this.wordManager = wordManager;
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public void UpdateFromFile(string filepath)
        {
            string line = string.Empty;
            logger.LogInformation("Start updating Dictionary");

            // Validate if the file exists
            var fullPath = Path.GetFullPath(filepath);
            if (!File.Exists(fullPath))
            {
                throw new ApplicationException($"File not found {fullPath}");
            }
            var lineCount = File.ReadLines(fullPath).Count();
            var count = 0;
            var startTime = DateTime.UtcNow;
            using (var file = new StreamReader(fullPath))
            {

                while ((line = file.ReadLine()) != null)
                {
                    count++;
                    logger.LogDebug($"Processing Line \n{line}");
                    // Sample Line
                    // කර්තෘ: agent | author | composer | doer | maker | redactor
                    var sinhalaWordStr = "";
                    var englishWordStrs = new string[0];
                    if (line.Split("=").Count() < 2)
                    {
                        sinhalaWordStr = line.Split(":")[0].Trim();
                        englishWordStrs = line.Split(":")[1].Trim().Split(" | ");
                    }
                    else
                    {
                        sinhalaWordStr = line.Split("=")[0].Trim();
                        englishWordStrs = line.Split("=")[1].Trim().Split(" | ");
                        sinhalaWordStr = sinhalaWordStr.Split("\t")[1].Trim();
                    }

                    var sinhalaWord = new Word
                    {
                        Language = Language.Sinhala,
                        Text = sinhalaWordStr
                    };

                    sinhalaWord = wordManager.AddWord(sinhalaWord, false);

                    foreach (var str_en in englishWordStrs)
                    {
                        var word = new Word
                        {
                            Language = Language.English,
                            Text = str_en
                        };

                        word = wordManager.AddWord(word, false);

                        var relations = new List<WordRelation>
                        {
                            new WordRelation
                            {
                                FromWord = sinhalaWord,
                                FromWordId = sinhalaWord.Id,
                                ToWord = word,
                                ToWordId = word.Id,
                                Type = RelationType.Dictionary
                            }, new WordRelation
                            {
                                FromWord = word,
                                FromWordId = word.Id,
                                ToWord = sinhalaWord,
                                ToWordId = sinhalaWord.Id,
                                Type = RelationType.Dictionary
                            }
                        };

                        wordRelationManager.AddRecords(relations, saveChanges: false);
                    }

                    if (count % 1 == 0)
                    {
                        var duration = DateTime.UtcNow - startTime;
                        startTime = DateTime.UtcNow;
                        dbContext.SaveChanges();
                        logger.LogInformation($"{count}/{lineCount} : {count} recored imported in {duration.ToReadableString()}");
                    }
                }
            }


        }
    }
}
