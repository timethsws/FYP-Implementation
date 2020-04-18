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
            var lines_per_percentage_point = lineCount / 1000;
            var old_percentage = 0.00;
            var next_limit = lines_per_percentage_point;
            var count = 0;

            logger.LogInformation($"{old_percentage}% Completed {count}/{lineCount}");
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

                    wordManager.AddWord(sinhalaWord);

                    foreach (var str_en in englishWordStrs)
                    {
                        var word = new Word
                        {
                            Language = Language.English,
                            Text = str_en
                        };

                        wordManager.AddWord(word);

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

                        wordRelationManager.AddRecords(relations, saveChanges: true);
                    }

                    if (count > next_limit)
                    {
                        next_limit += lines_per_percentage_point;
                        old_percentage += 0.1;
                        logger.LogInformation($"{old_percentage}% Completed {count}/{lineCount}");
                    }
                }
            }

        }
    }
}
