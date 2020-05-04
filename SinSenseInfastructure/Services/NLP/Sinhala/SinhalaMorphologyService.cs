using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SinSense.Core.Entities;
using SinSense.Core.Interfaces;

namespace SinSense.Infastructure.Services.Sinhala
{
    public class SinhalaMorphologyService : ILemmetizerService, IStemmerService
    {
        /// <summary>
        /// Language Supported
        /// </summary>
        public Language Language => Language.Sinhala;

        /// <summary>
        /// Data Context
        /// </summary>
        private readonly AppDbContext dbContext;

        public SinhalaMorphologyService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<string> Lemmetize(string text)
        {
            List<string> results = new List<string>();

            var words = text.Split(' ');
            foreach (var word in words)
            {
                // Check of the word exists in the database
                if (!dbContext.Words.Any(w => w.Text.Equals(text)))
                {
                    results.Add(word);
                }

                // Get the word
                var wordId = dbContext.Words.Where(w => w.Text.Equals(word)).Select(w => w.Id).FirstOrDefault();

                // Check if there is a relation ship entry
                if (!dbContext.WordRelations.Any(wr => wr.FromWordId == wordId && wr.Type == RelationType.Lemma))
                {
                    results.Add(word);
                }

                // return the lemma
                results.Add(dbContext.WordRelations
                    .Include(wr => wr.ToWord)
                    .Where(wr => wr.FromWordId == wordId && wr.Type == RelationType.Lemma)
                    .Select(wr => wr.ToWord.Text)
                    .FirstOrDefault());
            }

            return results;
        }

        public string LemmetizeWord(string text)
        {
            var word = text.Split(' ')[0];
            // Check of the word exists in the database
            if (!dbContext.Words.Any(w => w.Text.Equals(text)))
            {
                return word;
            }

            // Get the word
            var wordId = dbContext.Words.Where(w => w.Text.Equals(word)).Select(w => w.Id).FirstOrDefault();

            // Check if there is a relation ship entry
            if (!dbContext.WordRelations.Any(wr => wr.FromWordId == wordId && wr.Type == RelationType.Lemma))
            {
                return word;
            }

            // return the lemma
            return dbContext.WordRelations
                .Include(wr => wr.ToWord)
                .Where(wr => wr.FromWordId == wordId && wr.Type == RelationType.Lemma)
                .Select(wr => wr.ToWord.Text)
                .FirstOrDefault();
        }

        public List<string> Stem(string text)
        {
            List<string> results = new List<string>();

            var words = text.Split(' ');
            foreach (var word in words)
            {
                // Check of the word exists in the database
                if (!dbContext.Words.Any(w => w.Text.Equals(word)))
                {
                    results.Add(word);
                }

                // Get the word
                var wordId = dbContext.Words.Where(w => w.Text.Equals(word)).Select(w => w.Id).FirstOrDefault();

                // Check if there is a relation ship entry
                if (!dbContext.WordRelations.Any(wr => wr.FromWordId == wordId && wr.Type == RelationType.Stem))
                {
                    results.Add(word);
                }

                // return the lemma
                results.Add(dbContext.WordRelations
                    .Include(wr => wr.ToWord)
                    .Where(wr => wr.FromWordId == wordId && wr.Type == RelationType.Stem)
                    .Select(wr => wr.ToWord.Text)
                    .FirstOrDefault());
            }
            return results;
        }

        public string StemWord(string text)
        {

            var word = text.Split(' ')[0];

            // Check of the word exists in the database
            if (!dbContext.Words.Any(w => w.Text.Equals(word)))
            {
                return word;
            }

            // Get the word
            var wordId = dbContext.Words.Where(w => w.Text.Equals(word)).Select(w => w.Id).FirstOrDefault();

            // Check if there is a relation ship entry
            if (!dbContext.WordRelations.Any(wr => wr.FromWordId == wordId && wr.Type == RelationType.Stem))
            {
                return word;
            }

            // return the stem
            return dbContext.WordRelations
                .Include(wr => wr.ToWord)
                .Where(wr => wr.FromWordId == wordId && wr.Type == RelationType.Stem)
                .Select(wr => wr.ToWord.Text)
                .FirstOrDefault();
        }

    }
}
