using System;
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

        public 
    }
}
