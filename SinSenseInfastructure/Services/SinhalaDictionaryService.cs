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

        public List<string> GetWords (string strWord)
        {
            var wordDb = dbContext.Words
                .Include(w => w.Relations).ThenInclude(wr => wr.ToWord)
                .FirstOrDefault(w => w.Text.Equals(strWord));
            if(wordDb == null)
            {
                throw new ApplicationException($"Word {strWord} Not Found");
            }

            var dictionaryEntiries = wordDb.Relations?.Where(r => r.Type == RelationType.Dictionary) ?? new List<WordRelation>();

            if(!dictionaryEntiries.Any())
            {
                throw new ApplicationException($"No dictionary records :)");
            }

            return dictionaryEntiries.Select(d => d.ToWord.Text).ToList();
        }
    }
}
