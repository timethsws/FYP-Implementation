using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SinSense.Core.Entities;
using SinSense.Core.Interfaces;
using SinSense.Core.Models;
using SinSense.Infastructure.Services.NLP;
using SinSense.Infastructure.Services.NLP.Sinhala;

namespace SinSense.Infastructure.Services.Sinhala
{
    public class SinhalaDisambiguatorService :ISenseDisambiguationService
    {
        public Language Language { get => Language.Sinhala; }

        private readonly External.GoogleTranslateService translatorService;
        private readonly IDictionaryService sinhalaDictionaryService;
        private readonly SinhalaMorphologyService sinhalaMorphologyService;
        private readonly EnglishMorphologyService englishMorphologyService;
        private readonly External.BabelNetService BabelNetService;
        private readonly SimpleTokenizer tokenizer;

        public SinhalaDisambiguatorService(
            External.GoogleTranslateService translatorService,
            MaduraDictionaryService sinhalaDictionaryService,
            SinhalaMorphologyService sinhalaMorphologyService,
            EnglishMorphologyService englishMorphologyService,
            External.BabelNetService BabelNetService,
            SimpleTokenizer tokenizer
            )
        {
            this.translatorService = translatorService;
            this.sinhalaDictionaryService = sinhalaDictionaryService;
            this.sinhalaMorphologyService = sinhalaMorphologyService;
            this.englishMorphologyService = englishMorphologyService;
            this.BabelNetService = BabelNetService;
            this.tokenizer = tokenizer;
        }

        public DisambiguationResponse Disambiguate(string sentence)
        {
            var tokenizeRegex = new Regex("[\\p{L}\\p{M}\\u200d]+|[^\\p{L}\\p{M}\\u200d]+");
            var HasSymbol = new Regex("[^\\p{L}\\p{M}\\u200d]");

            var sinhalaTokens = tokenizer.TokenizeSentence(sentence);

            var translation = translatorService.Translate(sentence);
            var translationReplacedPunctuation = Regex.Replace(translation, "[^\\p{L}\\p{M}\\u200d]+", " ").ToLower();
            var englishLemma = englishMorphologyService.GetLemma(translationReplacedPunctuation);

            DisambiguationResponse response = new DisambiguationResponse
            {
                Sentence = sentence,
                Translation = translation,
                Tokens = sinhalaTokens
            };

            List<WordSense> englishTokens = new List<WordSense>();
            foreach (var token in BabelNetService.GetSenses(translationReplacedPunctuation))
            {
                englishTokens.Add(new WordSense
                {
                    EnglishWord = token.Key,
                    Gloss = token.Value
                });
            }

            foreach (var token in BabelNetService.GetSenses(englishLemma))
            {
                
                englishTokens.Add(new WordSense
                {
                    EnglishWord = token.Key,
                    Gloss = token.Value
                });
            }

            englishTokens = englishTokens.Distinct().ToList();
            List<SentenceToken> mappedTokens = new List<SentenceToken>();

            foreach (var sinhalaToken in sinhalaTokens.Where(t => t.IsWord))
            {
                var lemma = sinhalaMorphologyService.Lemmetize(sinhalaToken.Content);
                var stem = sinhalaMorphologyService.Stem(sinhalaToken.Content);

                var dictionaryEntries = sinhalaDictionaryService.GetWords(sinhalaToken.Content,Language.English) ?? new List<string>();
                dictionaryEntries.AddRange(sinhalaDictionaryService.GetWords(lemma[0],Language.English) ?? new List<string>());
                if (!dictionaryEntries.Any()) dictionaryEntries.AddRange(sinhalaDictionaryService.GetWords(stem[0], Language.English) ?? new List<string>());

                dictionaryEntries = dictionaryEntries.Distinct().ToList();
                if (!dictionaryEntries.Any())
                {
                    continue;
                }

                sinhalaToken.Sense = new WordSense
                {
                    DictionaryEntries = dictionaryEntries
                };

                if (!(dictionaryEntries.Count(c => englishTokens.Any(t => c.Equals(t.EnglishWord))) == 1))
                {
                    continue;
                }
                sinhalaToken.Sense = englishTokens.FirstOrDefault(t => dictionaryEntries.Any(c => c.Equals(t.EnglishWord)));
                sinhalaToken.Sense.DictionaryEntries = dictionaryEntries;
                mappedTokens.Add(sinhalaToken);
            }

            return response;
        }
    }
}
