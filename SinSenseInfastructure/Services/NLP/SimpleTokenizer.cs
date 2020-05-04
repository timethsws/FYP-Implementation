using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SinSense.Core.Entities;
using SinSense.Core.Interfaces;
using SinSense.Core.Models;

namespace SinSense.Infastructure.Services.NLP
{
    /// <summary>
    /// A simple word tokenizer built using regex
    /// </summary>
    public class SimpleTokenizer : ITokenizerService
    {
        /// <summary>
        /// Regex used to tokenize
        /// </summary>
        private static readonly Regex tokenizeRegex = new Regex("[\\p{L}\\p{M}\\u200d]+|[^\\p{L}\\p{M}\\u200d]+");

        /// <summary>
        /// Supported language
        /// </summary>
        public Language Language => Language.None;

        /// <summary>
        /// Tokenizes a Sentence
        /// </summary>
        /// <param name="sentence"> The sentence</param>
        /// <returns>Tokens</returns>
        public List<SentenceToken> TokenizeSentence(string sentence)
        {
            var tokens = new List<SentenceToken>();
            foreach (Match match in tokenizeRegex.Matches(sentence))
            {
                tokens.Add(new SentenceToken { Content = match.Value });
            }

            return tokens;
        }
    }
}
