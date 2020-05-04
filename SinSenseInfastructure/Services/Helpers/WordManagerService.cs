using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Logging;
using SinSense.Core.Entities;

namespace SinSense.Infastructure.Services
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

        public void AddWord(Word word, bool saveChanges = true)
        {
            // TODO Validation
            logger.LogDebug($"Adding word {word.Text}");
            // var wordDb = dbContext.Words.FirstOrDefault(w => w.Text.Equals(word.Text) && w.Language == word.Language);
            // if (wordDb == null)
            var exists = dbContext.Words.Any(w => w.Text.Equals(word.Text) && w.Language == word.Language);
            if (!exists)
            {

                dbContext.Words.Add(word);
                dbContext.SaveChanges();
                logger.LogDebug($"Word {word.Text} addd");
                return;
            }
            logger.LogDebug($"Word {word.Text} is already in the database");
        }

        public bool Exists(Word word)
        {
            return dbContext.Words.Any(w => w.Text.Equals(word.Text) && w.Language == word.Language);
        }

        public Word GetWord(Word word)
        {
            var wordDb = dbContext.Words.FirstOrDefault(w => w.Text.Equals(word.Text) && w.Language == word.Language);
            if (wordDb == null || wordDb.Id == Guid.Empty)
            {
                throw new ApplicationException("Word Not found");
            }
            return wordDb;
        }
    }
}
