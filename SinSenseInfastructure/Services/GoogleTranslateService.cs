using System;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Translate.V3;
using Microsoft.Extensions.Configuration;

namespace SinSense.Infastructure.Services
{
    public class GoogleTranslatorService
    {
        private readonly TranslationServiceClient translationServiceClient;
        private readonly string ProjectId;

        public GoogleTranslatorService(IConfiguration configuration)
        {
            var clientBuilder = new TranslationServiceClientBuilder()
            {
                CredentialsPath = configuration.GetValue<string>("ServiceConfiguration:GoogleTranslateService:CredentialsPath")
            };
            this.translationServiceClient = clientBuilder.Build();
            this.ProjectId = configuration.GetValue<string>("ServiceConfiguration:GoogleTranslateService:ProjectId");

        }

        public string Translate (string text, string targetLanguage = "en")
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
