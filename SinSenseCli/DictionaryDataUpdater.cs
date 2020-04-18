using System;
using System.IO;
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

            logger.LogInformation("Start updating Dictionary");
            string line = string.Empty;
            var fullPath = Path.GetFullPath(filepath);
            using (var file = new StreamReader(fullPath))
            {

                while ((line = file.ReadLine()) != null)
                {
                    logger.LogInformation($"Processing Line \n{line}");
                    // Sample Line
                    // කර්තෘ: agent | author | composer | doer | maker | redactor
                    var sinhalaWordStr = line.Split(":")[0].Trim();
                    var englishWordStrs = line.Split(":")[1].Trim().Split(" | ");

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

                        var relationOne = new WordRelation
                        {
                            FromWord = sinhalaWord,
                            FromWordId = sinhalaWord.Id,
                            ToWord = word,
                            ToWordId = word.Id,
                            Type = RelationType.Dictionary
                        };

                        var relationTwo = new WordRelation
                        {
                            FromWord = word,
                            FromWordId = word.Id,
                            ToWord = sinhalaWord,
                            ToWordId = sinhalaWord.Id,
                            Type = RelationType.Dictionary
                        };
                    }

                }
            }

        }
    }
}
