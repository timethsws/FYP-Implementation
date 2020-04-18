using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using SinSenseCore.Entities;
using SinSenseInfastructure.Services;

namespace SinSenseCli
{
    public class MorphDataUpdater
    {
        private readonly WordManagerService wordManager;
        private readonly WordRelationManagerService wordRelationManager;
        private readonly ILogger<MorphDataUpdater> logger;

        public MorphDataUpdater(WordManagerService wordManager,
            WordRelationManagerService wordRelationManager,
            ILogger<MorphDataUpdater> logger)
        {
            this.wordManager = wordManager;
            this.wordRelationManager = wordRelationManager;
            this.logger = logger;
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
            var lines_per_percentage_point = lineCount / 1000;
            var old_percentage = 0.0;
            var next_limit = lines_per_percentage_point;
            var count = 0;

            using (var file = new StreamReader(fullPath))
            {
                while ((line = file.ReadLine()) != null)
                {
                    logger.LogDebug($"Processing Line \n{line.Substring(0,20)}");
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

                    wordManager.AddWord(lemma, saveChanges: false);
                    wordManager.AddWord(stem, saveChanges: false);

                    List<WordRelation> relations = new List<WordRelation>
                    {
                        new WordRelation
                        {
                            FromWord = lemma,
                            ToWord = stem,
                            Type = RelationType.Stem
                        },
                        new WordRelation
                        {
                            FromWord = lemma,
                            ToWord = lemma,
                            Type = RelationType.Lemma
                        },
                        new WordRelation
                        {
                            FromWord = stem,
                            ToWord = stem,
                            Type = RelationType.Stem
                        },
                        new WordRelation
                        {
                            FromWord = stem,
                            ToWord = lemma,
                            Type = RelationType.Lemma
                        },
                    };

                    wordRelationManager.AddRecords(relations, saveChanges: true);

                    var str_morph_list = splitted[3].Split("\t");
                    foreach (var str_word in str_morph_list)
                    {
                        //word = self.add_new_word(word = word_str, language = "si")
                        var word = wordManager.AddWord(new Word { Language = Language.Sinhala, Text = str_word }, saveChanges: false);
                        List<WordRelation> new_relations = new List<WordRelation>
                        {
                            new WordRelation
                            {
                                FromWord = word,
                                ToWord = lemma,
                                Type = RelationType.Lemma
                            },
                            new WordRelation
                            {
                                FromWord = word,
                                ToWord = stem,
                                Type = RelationType.Stem
                            }
                        };

                        wordRelationManager.AddRecords(new_relations, saveChanges: true);
                    }

                    if (count > next_limit)
                    {
                        next_limit += lines_per_percentage_point;
                        old_percentage+=0.1;
                        logger.LogInformation($"{old_percentage}% Completed {count}/{lineCount}");
                    }
                }
            }
        }
    }
}
