using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SinSense.Core.Entities;

namespace SinSense.Infastructure.Services
{
    /// <summary>
    /// Service to manage dictionary information
    /// </summary>
    public class WordRelationManagerService
    {
        private readonly AppDbContext dbContext;
        private readonly ILogger<WordRelationManagerService> logger;

        public WordRelationManagerService(AppDbContext dbContext, ILogger<WordRelationManagerService> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public void AddRecords(List<WordRelation> wordRelations)
        {
            foreach (var wordRelation in wordRelations)
            {
                AddWordRelationRecord(wordRelation);
                dbContext.SaveChanges();
            }
        }

        public WordRelation AddRecord(WordRelation wordRelation)
        {
            var word = AddWordRelationRecord(wordRelation);
            dbContext.SaveChanges();
            return word;
        }

        protected WordRelation AddWordRelationRecord(WordRelation wordRelation)
        {
            // Check if there is an exisiting relationship
            var exisits = dbContext.WordRelations.Any(wr => wr.FromWordId == wordRelation.FromWordId && wr.ToWordId == wordRelation.ToWordId && wr.Type == wordRelation.Type);

            try
            {
                if (!exisits)
                {
                    dbContext.WordRelations.Add(wordRelation);
                    return wordRelation;
                }
                else
                {
                    // logger.LogInformation($"Dictionry relation \"{wordRelation.FromWord.Text} -> {wordRelation.ToWord.Text}\" already exists");
                    //return dbContext.WordRelations.FirstOrDefault(wr => wr.FromWordId == wordRelation.FromWordId && wr.ToWordId == wordRelation.ToWordId && wr.Type == wordRelation.Type);
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
        }
    }
}
