using System;
using System.Collections.Generic;

namespace SinSense.Core.Entities
{
    public class Word
    {
        /// <summary>
        /// Identification
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Text representation of the word
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Language the word belonging to 
        /// </summary>
        public Language Language { get; set; }

        /// <summary>
        /// Relationships
        /// </summary>
        public virtual List<WordRelation> Relations { get; set; }
    }

    public enum Language
    {
        None = 0,
        Sinhala = 1,
        English = 2
    }
}
