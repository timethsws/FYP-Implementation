using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SinSense.Core.Models;

namespace SinSense.Infastructure.Services
{
    public class SinhalaDisambiguatorService
    {
        private GoogleTranslatorService translatorService;
        private SinhalaDictionaryService SinhalaDictionary;
        private SinhalaMorphologyService SinhalaMorphology;
        private EnglishMorphologyService EnglishMorphology;
        private BabelNetService BabelNetService;

        public SinhalaDisambiguatorService(
            GoogleTranslatorService translatorService,
            SinhalaDictionaryService SinhalaDictionary,
            SinhalaMorphologyService SinhalaMorphology,
            EnglishMorphologyService EnglishMorphology,
            BabelNetService BabelNetService
            )
        {
            this.translatorService = translatorService;
            this.SinhalaDictionary = SinhalaDictionary;
            this.SinhalaMorphology = SinhalaMorphology;
            this.EnglishMorphology = EnglishMorphology;
            this.BabelNetService = BabelNetService;
        }

        public DisambiguationResponse Disambiguate(string sentence)
        {
            var tokenizeRegex = new Regex("[\\p{L}\\p{M}\\u200d]+|[^\\p{L}\\p{M}]+");

            var sinhalaTokens = new List<WordToken>();
            foreach (Match match in tokenizeRegex.Matches(sentence))
            {
                sinhalaTokens.Add(new WordToken { Content = match.Value });
            }

            var englishTranslation = translatorService.Translate(sentence);

            foreach (var token in sinhalaTokens.Where(t => t.IsWord))
            {

            }
        }
    }
}
