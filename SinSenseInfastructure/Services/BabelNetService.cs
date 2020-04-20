using System;
using System.Collections.Generic;
using Google.Apis.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SinSenseInfastructure.Services
{
    public class BabelNetService
    {
        private readonly ILogger<BabelNetService> logger;

        private readonly string Key;
        private readonly string DisambiguationBase;
        private readonly string SynsetBase;

        public BabelNetService(IConfiguration configuration, ILogger<BabelNetService> logger)
        {
            this.Key = configuration.GetValue<string>("ServiceConfiguration:BabelNetService:Key");
            this.DisambiguationBase = configuration.GetValue<string>("ServiceConfiguration:BabelNetService:DisambiguationBaseUrl");
            this.SynsetBase = configuration.GetValue<string>("ServiceConfiguration:BabelNetService:SynsetBaseUrl");
        }
    }

    public 

    namespace BabelNet
    {
        class SenseResponseItem
        {
            public Fragment TokenFragment { get; set; }
            public Fragment CharFragment { get; set; }
            public string BabelSynsetID { get; set; }
            public string DBpediaURL { get; set; }
            public string BabelNetURL { get; set; }
            public string Source { get; set; }
            public double Score { get; set; }
            public double CoherenceScore { get; set; }
            public double GlobalScore { get; set; }
        }

        class Fragment
        {
            public int Start { get; set; }

            public int End { get; set; }
        }
    }
}
