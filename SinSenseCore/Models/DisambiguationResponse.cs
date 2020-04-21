using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SinSense.Core.Models
{
    public class DisambiguationResponse
    {
        public string Sentence { get; set; }

        public string Translation { get; set; }

        List<WordToken> wordSenses { get; set; }

    }

    public class WordToken
    {
        static readonly Regex HasSymbol = new Regex("[^\\p{L}\\p{M}\\u200d]");

        public string Content { get; set; }

        public bool IsWord { get => !HasSymbol.IsMatch(Content); }

        public WordSense Sense { get; set; }
      
    }

    public class WordSense
    {
        public string EnglishWord { get; set; }

        public string Gloss { get; set; }

        public List<string> DictionaryEntries { get; set; }
    }
}
