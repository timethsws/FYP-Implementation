using System;
namespace SinSense.Core.Entities
{
    public class Translation
    {
        public string SourceText { get; set; }
        public Language SourceLanguage { get; set; }

        public string TranslatedText { get; set; }
        public Language TargetLanguage { get; set; }

    }
}
