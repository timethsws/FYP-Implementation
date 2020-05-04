using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using SinSense.Core.Entities;
using SinSense.Core.Interfaces;

namespace SinSense.Infastructure.Services.NLP.Sinhala
{
    public class MaduraDictionaryService : IDictionaryService
    {

        public Language SourceLanguage => Language.Sinhala;

        public Language[] TargetLanguages { get; } = { Language.English };

        private static string MaduraUrl = "https://www.maduraonline.com/?find=";
        private readonly HttpClient client;

        public MaduraDictionaryService()
        {
            this.client = new HttpClient();
        }

        public List<string> GetWords(string word, Language TargetLanguage)
        {
            if (!TargetLanguages.Any(l => l == TargetLanguage))
            {
                throw new ApplicationException("Target Language is not supported");
            }

            var uri = $"{MaduraUrl}{word}";
            var maduraResponse = client.GetAsync(uri).Result;

            if (maduraResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Request to \"{uri}\" failed with status code {System.Net.HttpStatusCode.OK}");
            }
            var responseContent = maduraResponse.Content.ReadAsStringAsync().Result;

            Regex findWords = new Regex("<td\\sclass=\"td\">([a-z ]+)");

            var matches = new List<string>();

            foreach (Match match in findWords.Matches(responseContent))
            {
                matches.Add(match.Groups[1].Value);
            }

            return matches;
        }
    }
}
