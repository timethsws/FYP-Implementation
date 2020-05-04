using System;
using SinSense.Core.Entities;
using SinSense.Core.Interfaces;
using SinSense.Core.Models;
using SinSense.Infastructure.Services.External;

namespace SinSense.Infastructure.Services.NLP.English
{
    public class EnglishDisambiguatorService : ISenseDisambiguationService
    {
        public Language Language => Language.English;

        private readonly BabelNetService babelNetService;
        private readonly ITokenizerService tokenizer;


        public EnglishDisambiguatorService( BabelNetService babelNetService, SimpleTokenizer tokenizer)
        {
            this.babelNetService = babelNetService;
            this.tokenizer = tokenizer;
        }


        public DisambiguationResponse Disambiguate(string text) => throw new NotImplementedException();
    }
}
