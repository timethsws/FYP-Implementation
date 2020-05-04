using System;
using System.Collections.Generic;
using SinSense.Core.Entities;

namespace SinSense.Core.Interfaces
{
    public interface ITranslatorService
    {
        /// <summary>
        /// Source language
        /// </summary>
        Language SourceLanguage { get;}

        /// <summary>
        /// Target Language
        /// </summary>
        Language[] TargetLanguages { get;}

        /// <summary>
        /// Translate a text from target language to source language
        /// </summary>
        /// <param name="text"></param>
        /// <param name="TargetLanguage"> Language vode of the target language</param>
        /// <returns></returns>
        string Translate(string text, Language TargetLanguage);
    }
}
