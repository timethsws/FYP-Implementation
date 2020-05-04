using System;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.Extensions.Configuration;

namespace SinSense.Infastructure.Services.External
{
    /// <summary>
    /// Google Translate API Wrapper
    /// </summary>
    public class GoogleTranslateService
    {
        private readonly TranslationServiceClient translationServiceClient;
        private readonly string ProjectId;

        public GoogleTranslateService(IConfiguration configuration)
        {
            var clientBuilder = new TranslationServiceClientBuilder()
            {
                CredentialsPath = configuration.GetValue<string>("ServiceConfiguration:GoogleTranslateService:CredentialsPath")
            };
            this.translationServiceClient = clientBuilder.Build();
            this.ProjectId = configuration.GetValue<string>("ServiceConfiguration:GoogleTranslateService:ProjectId");

        }

        /// <summary>
        /// Translates a given to text to target language (Should be supported by google translate
        /// </summary>
        /// <param name="text">Text to translate</param>
        /// <param name="targetLanguage"> Language to translate to</param>
        /// <returns>Translated Text</returns>
        public string Translate(string text, string targetLanguage = "en")
        {
            TranslateTextRequest request = new TranslateTextRequest
            {
                Contents = { text },
                TargetLanguageCode = targetLanguage,
                ParentAsLocationName = new LocationName(ProjectId, "global"),
            };
            TranslateTextResponse response = translationServiceClient.TranslateText(request);

            return response.Translations[0].TranslatedText;
        }
    }
}
