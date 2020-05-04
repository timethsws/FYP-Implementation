using System;
using System.Collections.Generic;
using SinSense.Core.Entities;

namespace SinSense.Core.Interfaces
{
    public interface IDictionaryService
    {
        /// <summary>
        /// Source language
        /// </summary>
        Language SourceLanguage { get; }

        /// <summary>
        /// Target Language
        /// </summary>
        Language[] TargetLanguages { get; }

        /// <summary>
        /// Get dictionary entries
        /// </summary>
        /// <param name="word"></param>
        /// <param name="TargetLanguageCode"></param>
        /// <returns></returns>
        List<string> GetWords(string word, Language TargetLanguage);

    }
}
