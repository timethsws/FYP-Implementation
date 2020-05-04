using System;
using SinSense.Core.Entities;
using SinSense.Core.Models;

namespace SinSense.Core.Interfaces
{
    public interface ISenseDisambiguationService
    {
        /// <summary>
        /// Language supported by the Service
        /// </summary>
        Language Language { get;}

        /// <summary>
        /// Disambiguates words in a text.
        /// </summary>
        /// <param name="text">the text to disambiguate</param>
        /// <returns> The disambiguation response</returns>
        DisambiguationResponse Disambiguate(string text);
    }
}
