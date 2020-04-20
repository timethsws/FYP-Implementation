using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SinSenseCore.Entities;

namespace SinSenseInfastructure.Services
{
    public class SinhalaMorphologyService
    {
        private AppDbContext dbContext;

        public SinhalaMorphologyService(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GetLemma (string word)
        {
            // Check of the word exists in the database
            if(!dbContext.Words.Any(w => w.Text.Equals(word)))
            {
                return word;
            }

            // Get the word
            var wordId = dbContext.Words.Where(w => w.Text.Equals(word)).Select(w => w.Id).FirstOrDefault();

            // Check if there is a relation ship entry
            if(!dbContext.WordRelations.Any(wr => wr.FromWordId == wordId && wr.Type == RelationType.Lemma))
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

        public string GetStem (string word)
        {
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

            // return the lemma
            return dbContext.WordRelations
                .Include(wr => wr.ToWord)
                .Where(wr => wr.FromWordId == wordId && wr.Type == RelationType.Stem)
                .Select(wr => wr.ToWord.Text)
                .FirstOrDefault();
        }
    }
}
