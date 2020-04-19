using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using SinSenseCore.Entities;

namespace SinSenseInfastructure.Services
{
    public class WordManagerService
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<WordManagerService> logger;

        public WordManagerService(AppDbContext dbContext, ILogger<WordManagerService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public Word AddWord(Word word, bool saveChanges = true)
        {
            // TODO Validation
            logger.LogDebug($"Adding word {word.Text}");
            // var wordDb = dbContext.Words.FirstOrDefault(w => w.Text.Equals(word.Text) && w.Language == word.Language);
            // if (wordDb == null)
            var exists = dbContext.Words.Any(w => w.Text.Equals(word.Text) && w.Language == word.Language);
            if (!exists)
            {

                dbContext.Words.Add(word);
                if (saveChanges)
                {
                    dbContext.SaveChanges();
                }
                return word;
            }
            logger.LogDebug($"Word {word.Text} is already in the database");
            return dbContext.Words.FirstOrDefault(w => w.Text.Equals(word.Text) && w.Language == word.Language);
            // return null;
        }
    }
}
