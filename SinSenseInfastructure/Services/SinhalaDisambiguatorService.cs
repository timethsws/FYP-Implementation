using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SinSense.Core.Models;

namespace SinSense.Infastructure.Services
{
    public class SinhalaDisambiguatorService
    {
        private readonly GoogleTranslatorService translatorService;
        private readonly SinhalaDictionaryService sinhalaDictionaryService;
        private readonly SinhalaMorphologyService sinhalaMorphologyService;
        private readonly EnglishMorphologyService englishMorphologyService;
        private readonly BabelNetService BabelNetService;

        public SinhalaDisambiguatorService(
            GoogleTranslatorService translatorService,
            SinhalaDictionaryService sinhalaDictionaryService,
            SinhalaMorphologyService sinhalaMorphologyService,
            EnglishMorphologyService englishMorphologyService,
            BabelNetService BabelNetService
            )
        {
            this.translatorService = translatorService;
            this.sinhalaDictionaryService = sinhalaDictionaryService;
            this.sinhalaMorphologyService = sinhalaMorphologyService;
            this.englishMorphologyService = englishMorphologyService;
            this.BabelNetService = BabelNetService;
        }

        public DisambiguationResponse Disambiguate(string sentence)
        {
            var tokenizeRegex = new Regex("[\\p{L}\\p{M}\\u200d]+|[^\\p{L}\\p{M}\\u200d]+");
            var HasSymbol = new Regex("[^\\p{L}\\p{M}\\u200d]");

            var sinhalaTokens = new List<WordToken>();
            

            foreach (Match match in tokenizeRegex.Matches(sentence))
            {
                sinhalaTokens.Add(new WordToken { Content = match.Value });
            }
            var translation = translatorService.Translate(sentence);
            var englishTranslationReplacedPunctioation = Regex.Replace(translation, "[^\\p{L}\\p{M}\\u200d]+", " ").ToLower();
            var englishLemma = englishMorphologyService.GetLemma(englishTranslationReplacedPunctioation);

            DisambiguationResponse response = new DisambiguationResponse
            {
                Sentence = sentence,
                Translation = translation,
                Tokens = sinhalaTokens
            };

            List<string> englishTokens = new List<string>();
            foreach (Match match in tokenizeRegex.Matches(englishTranslationReplacedPunctioation))
            {
                if(!HasSymbol.IsMatch(match.Value))
                {
                    englishTokens.Add(match.Value);
                }
            }

            foreach (Match match in tokenizeRegex.Matches(englishLemma))
            {
                if (!HasSymbol.IsMatch(match.Value))
                {
                    englishTokens.Add(match.Value);
                }
            }

            englishTokens = englishTokens.Distinct().ToList();
            List<WordToken> mappedTokens = new List<WordToken>();

            foreach (var sinhalaToken in sinhalaTokens.Where(t => t.IsWord))
            {
                var lemma = sinhalaMorphologyService.GetLemma(sinhalaToken.Content);
                var stem = sinhalaMorphologyService.GetStem(sinhalaToken.Content);

                var dictionaryEntries = sinhalaDictionaryService.GetWords(sinhalaToken.Content) ?? new List<string>();
                dictionaryEntries.AddRange(sinhalaDictionaryService.GetWords(lemma) ?? new List<string>());
                if(!dictionaryEntries.Any()) dictionaryEntries.AddRange(sinhalaDictionaryService.GetWords(stem) ?? new List<string>());

                dictionaryEntries = dictionaryEntries.Distinct().ToList();
                if(!dictionaryEntries.Any())
                {
                    continue;
                }

                sinhalaToken.Sense = new WordSense
                {
                    DictionaryEntries = dictionaryEntries
                };

                if(!(dictionaryEntries.Count(c => englishTokens.Any(t => c.Equals(t))) == 1))
                {
                    continue;
                }

                sinhalaToken.Sense.EnglishWord = dictionaryEntries.FirstOrDefault(c => englishTokens.Any(c.Equals));
                mappedTokens.Add(sinhalaToken);
            }

            if(!mappedTokens.Any())
            {
                return response;
            }

            var mappedSentence = string.Join(" ", mappedTokens.Select(t => t.Sense.EnglishWord));
            var senses = BabelNetService.GetSenses(mappedSentence);

            for (int i = 0; i < senses.Count(); i++)
            {
                var mappedToken = mappedTokens[i];
                var sense = senses[i];
                if(!string.IsNullOrWhiteSpace(sense.Value))
                {
                    mappedToken.Sense.Gloss = sense.Value;
                }
            }

            return response;
        }
    }
}
