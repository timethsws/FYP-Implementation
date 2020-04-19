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
    public class MorphDataUpdater
    {
        private readonly WordManagerService wordManager;
        private readonly WordRelationManagerService wordRelationManager;
        private readonly ILogger<MorphDataUpdater> logger;
        private readonly AppDbContext dbContext;

        public MorphDataUpdater(AppDbContext dbContext,
            WordManagerService wordManager,
            WordRelationManagerService wordRelationManager,
            ILogger<MorphDataUpdater> logger)
        {
            this.wordManager = wordManager;
            this.wordRelationManager = wordRelationManager;
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public void UpdateFromFile(string filepath)
        {
            string line = string.Empty;
            logger.LogInformation("Start updating Morph data");

            // Validate if the file exists
            var fullPath = Path.GetFullPath(filepath);
            if (!File.Exists(fullPath))
            {
                throw new ApplicationException($"File not found {fullPath}");
            }

            // For Information purposes
            var lineCount = File.ReadLines(fullPath).Count();
            var count = 0;

            var startTime = DateTime.UtcNow;

            List<Word> toAdd = new List<Word>();
            using (var file = new StreamReader(fullPath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    count++;
                    logger.LogDebug($"Processing Line \n{line}");
                    // 22	ෆැරැන්සියම්	ෆැරැන්සියම්	ෆැරැන්සියම්	ෆැරැන්සියම්	ෆැරැන්සියම්ුත්	ෆැරැන්සියම්ුත්	ෆැරැන්සියම්ුයි	ෆැරැන්සියම්ුයි	ෆැරැන්සියම්වලින්	...
                    var splitted = line.Split("\t", 4);
                    var str_lemma = splitted[1];
                    var str_stem = splitted[2];

                    var lemma = new Word
                    {
                        Language = Language.Sinhala,
                        Text = str_lemma,
                    };

                    var stem = new Word
                    {
                        Language = Language.Sinhala,
                        Text = str_stem,
                    };

                    toAdd = AddWordToWordsList(toAdd, lemma);
                    toAdd = AddWordToWordsList(toAdd, stem);
                    // wordManager.AddWord(stem, false);

                    var str_morph_list = splitted[3].Split("\t");
                    foreach (var str_word in str_morph_list)
                    {
                        //word = self.add_new_word(word = word_str, language = "si")
                        var word = new Word { Language = Language.Sinhala, Text = str_word };
                        // wordManager.AddWord(word, false);
                        toAdd = AddWordToWordsList(toAdd, word);
                    }

                    if (count % 10 == 0)
                    {
                        var duration = DateTime.UtcNow - startTime;
                        startTime = DateTime.UtcNow;
                        if (toAdd.Any())
                        {
                            dbContext.Words.AddRange(toAdd);
                            dbContext.SaveChanges();
                        }
                        logger.LogInformation($"{count}/{lineCount} : {count} word records imported in {duration.ToReadableString()}");
                    }
                }
            }
            var finalduration = DateTime.UtcNow - startTime;
            startTime = DateTime.UtcNow;
            if (toAdd.Any())
            {
                dbContext.Words.AddRange(toAdd);
                dbContext.SaveChanges();
            }
            logger.LogInformation($"{count}/{lineCount} : {count} word records imported in {finalduration.ToReadableString()}");

            using (var file = new StreamReader(fullPath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    count++;
                    logger.LogDebug($"Processing Line \n{line}");
                    // 22	ෆැරැන්සියම්	ෆැරැන්සියම්	ෆැරැන්සියම්	ෆැරැන්සියම්	ෆැරැන්සියම්ුත්	ෆැරැන්සියම්ුත්	ෆැරැන්සියම්ුයි	ෆැරැන්සියම්ුයි	ෆැරැන්සියම්වලින්	...
                    var splitted = line.Split("\t", 4);
                    var str_lemma = splitted[1];
                    var str_stem = splitted[2];

                    var lemma = new Word
                    {
                        Language = Language.Sinhala,
                        Text = str_lemma,
                    };

                    var stem = new Word
                    {
                        Language = Language.Sinhala,
                        Text = str_stem,
                    };

                    lemma = wordManager.GetWord(lemma);
                    stem = wordManager.GetWord(stem);

                    List<WordRelation> relations = new List<WordRelation>
                    {
                        new WordRelation
                        {
                            FromWordId = lemma.Id,
                            ToWordId = stem.Id,
                            Type = RelationType.Stem
                        },
                        new WordRelation
                        {
                            FromWordId = lemma.Id,
                            ToWordId = lemma.Id,
                            Type = RelationType.Lemma
                        },
                        new WordRelation
                        {
                            FromWordId = stem.Id,
                            ToWordId = stem.Id,
                            Type = RelationType.Stem
                        },
                        new WordRelation
                        {
                            FromWordId = stem.Id,
                            ToWordId = lemma.Id,
                            Type = RelationType.Lemma
                        },
                    };

                    wordRelationManager.AddRecords(relations);

                    var str_morph_list = splitted[3].Split("\t");
                    foreach (var str_word in str_morph_list)
                    {
                        //word = self.add_new_word(word = word_str, language = "si")
                        var word = new Word { Language = Language.Sinhala, Text = str_word };
                        word = wordManager.GetWord(word);
                        List<WordRelation> new_relations = new List<WordRelation>
                        {
                            new WordRelation
                            {
                                FromWordId = word.Id,
                                ToWordId = lemma.Id,
                                Type = RelationType.Lemma
                            },
                            new WordRelation
                            {
                                FromWordId = word.Id,
                                ToWordId = stem.Id,
                                Type = RelationType.Stem
                            }
                        };

                        wordRelationManager.AddRecords(new_relations);
                    }

                    if (count % 10 == 0)
                    {
                        var duration = DateTime.UtcNow - startTime;
                        startTime = DateTime.UtcNow;
                        dbContext.SaveChanges();
                        logger.LogInformation($"{count}/{lineCount} : {count} relationship records imported in {duration.ToReadableString()}");
                    }
                }
            }

            finalduration = DateTime.UtcNow - startTime;
            startTime = DateTime.UtcNow;
            dbContext.SaveChanges();
            logger.LogInformation($"{count}/{lineCount} : {count} word records imported in {finalduration.ToReadableString()}");
        }

        public List<Word> AddWordToWordsList(List<Word> list, Word word)
        {
            if (!wordManager.Exists(word) && !list.Any(w => w.Text.Equals(word.Text) && w.Language == word.Language))
            {
                list.Add(word);
            }
            return list;
        }
    }
}
