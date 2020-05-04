using System;
using System.Collections.Generic;
using SinSense.Core.Entities;
using SinSense.Core.Models;

namespace SinSense.Core.Interfaces
{
    public interface ITokenizerService
    {
        Language Language { get;}

        /// <summary>
        /// Tokenize a sentence.
        /// </summary>
        /// <param name="sentence">The sentence to tokenize</param>
        /// <returns> Tokens </returns>
        List<SentenceToken> TokenizeSentence(string sentence);
    }
}
