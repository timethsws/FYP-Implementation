using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SinSenseCore.Entities;

namespace SinSenseInfastructure.Services
{
    public class SinhalaDictionaryService
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<SinhalaDictionaryService> logger;

        public SinhalaDictionaryService(AppDbContext dbContext, ILogger<SinhalaDictionaryService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public List<string> GetWords (string word)
        {
            // Check of the word exists in the database
            if (!dbContext.Words.Any(w => w.Text.Equals(word)))
            {
                return null;
            }

            // Get the word
            var wordId = dbContext.Words.Where(w => w.Text.Equals(word)).Select(w => w.Id).FirstOrDefault();

            // Check if there is relation ships
            if (!dbContext.WordRelations.Any(wr => wr.FromWordId == wordId && wr.Type == RelationType.Dictionary))
            {
                return null;
            }

            // return the words
            return dbContext.WordRelations
                .Include(wr => wr.ToWord)
                .Where(wr => wr.FromWordId == wordId && wr.Type == RelationType.Dictionary)
                .Select(wr => wr.ToWord.Text)
                .ToList();
        }
    }
}
