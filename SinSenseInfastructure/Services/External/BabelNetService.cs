using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SinSense.Infastructure.Services.External.BabelNet;

namespace SinSense.Infastructure.Services.External
{
    /// <summary>
    /// Service to interact with BabelNet
    /// </summary>
    public class BabelNetService
    {
        private readonly ILogger<BabelNetService> logger;
        private readonly HttpClient client;

        private readonly string Key;
        private readonly string DisambiguationBase;
        private readonly string SynsetBase;

        public BabelNetService(IConfiguration configuration, ILogger<BabelNetService> logger)
        {
            this.logger = logger;
            this.client = new HttpClient();

            this.Key = configuration.GetValue<string>("ServiceConfiguration:BabelNetService:Key");
            this.DisambiguationBase = configuration.GetValue<string>("ServiceConfiguration:BabelNetService:DisambiguationBaseUrl");
            this.SynsetBase = configuration.GetValue<string>("ServiceConfiguration:BabelNetService:SynsetBaseUrl");
        }

        public List<KeyValuePair<string, string>> GetSenses(string sentence)
        {
            Console.WriteLine($"Sentence : {sentence}");
            var tokens = sentence.Split(' ');
            var babelifySenses = GetBabelifyResponse(sentence);

            List<KeyValuePair<string, string>> res = new List<KeyValuePair<string, string>>();

            for (int i = 0; i < tokens.Length; i++)
            {
                var sense = new KeyValuePair<string, string>(tokens[i], null);
                var senseItem = babelifySenses.FirstOrDefault(s => s.TokenFragment.Start == i && s.TokenFragment.Start == i);
                if (senseItem != null)
                {
                    sense = GetSenseDetails(senseItem.BabelSynsetID);
                }
                res.Add(sense);
            }
            return res;
        }

        public List<SenseResponseItem> GetBabelifyResponse(string sentence)
        {
            var uri = $"{DisambiguationBase}?text={sentence}&key={Key}&lang=en";
            var babelifyResponse = client.GetAsync(uri).Result;

            if (babelifyResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Request to \"{uri}\" failed with status code {System.Net.HttpStatusCode.OK}");
            }
            var responseContent = babelifyResponse.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<SenseResponseItem>>(responseContent);
        }

        public KeyValuePair<string, string> GetSenseDetails(string synsetId)
        {
            var uri = $"{SynsetBase}?id={synsetId}&key={Key}";
            var babelifyResponse = client.GetAsync(uri).Result;

            if (babelifyResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Request to \"{uri}\" failed with status code {System.Net.HttpStatusCode.OK}");
            }

            var responseContent = babelifyResponse.Content.ReadAsStringAsync().Result;
            var responseObj = JsonConvert.DeserializeObject<JObject>(responseContent);

            var mainSense = responseObj["mainSense"].Value<string>().Split('#')[0];
            var gloss = responseObj["glosses"][0]["gloss"].Value<string>();

            return new KeyValuePair<string, string>(mainSense, gloss);
        }
    }

    namespace BabelNet
    {
        public class SenseResponseItem
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

        public class Fragment
        {
            public int Start { get; set; }
            public int End { get; set; }
        }

        public class Synset
        {

        }
    }
}
