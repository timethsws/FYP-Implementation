using System;
using System.Collections.Generic;
using SinSense.Core.Entities;

namespace SinSense.Core.Interfaces
{
    public interface IStemmerService
    {
        Language Language { get; }

        /// <summary>
        /// Stems the text (If multiple words are there it'll be splitted using spaces
        /// </summary>
        /// <param name="text"> the text to stem</param>
        /// <returns></returns>
        List<string> Stem(string text);

        /// <summary>
        /// Stems 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        string StemWord(string text);
    }
}
