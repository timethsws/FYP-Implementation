using System;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.Extensions.Configuration;

namespace SinSenseInfastructure.Services
{
    public class SiEnTranslaterService
    {
        private readonly TranslationServiceClient translationServiceClient;
        private readonly string ProjectId;

        public SiEnTranslaterService(IConfiguration configuration)
        {
            var clientBuilder = new TranslationServiceClientBuilder()
            {
                CredentialsPath = configuration.GetValue<string>("GCP:CredentialsPath")
            };
            this.translationServiceClient = clientBuilder.Build();
            this.ProjectId = configuration.GetValue<string>("GCP:ProjectId");

        }

        public string Translate (string text, string targetLanguage = "en")
        {
            TranslateTextRequest request = new TranslateTextRequest
            {
                Contents =
                {
                    text,
                },
                TargetLanguageCode = targetLanguage,
                ParentAsLocationName = new LocationName(ProjectId, "global"),
            };
            TranslateTextResponse response = translationServiceClient.TranslateText(request);
            // Display the translation for each input text provided
            
            foreach (Translation translation in response.Translations)
            {
                Console.WriteLine($"Translated text: {translation.TranslatedText}");
            }

            return response.Translations[0].TranslatedText;
        }
    }
}
