using System;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SinSenseInfastructure;
using SinSenseInfastructure.Services;

namespace SinSenseCli
{
    class Program
    {
        private IConfiguration appConfiguration; //basicEntityCheckSettings:DataPath
        private ILogger logger;
        private IServiceProvider serviceProvider;

        [Argument(0, Description = "Provider to execute (asic|acra|test)")]
        public static string Provider { get; set; }

        [Argument(1, Description = "Action to perform (update|import)")]
        public static string Action { get; set; }

        [Argument(2, Description = "Paramater (if any)")]
        public static string Parameter { get; set; }

        [Argument(3, Description = "Second Parameter (if any)")]
        public static string Parameter2 { get; set; }

        /// <summary>
        /// Main entry point. Configure command line utilities and delegate execution
        /// </summary>
        static void Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private void Startup()
        {
            serviceProvider = new ServiceCollection()
            .AddLogging(opt => {
                opt.AddConfiguration(appConfiguration.GetSection("Logging"));
                opt.AddConsole(); }
            )
            // Configure DB contextx
            .AddDbContext<AppDbContext>(options => appConfiguration.ConfigureDbContext(options, "DefaultDb"))
            .AddScoped<WordRelationManagerService>()
            .AddScoped<WordManagerService>()
            .AddScoped<DictionaryDataUpdater>()
            // Builds the service provider
            .BuildServiceProvider();

            // Initialize logger instance
            logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogDebug("Starting application");
        }

        /// <summary>
        /// Main program flow executer
        /// </summary>
        public void OnExecute()
        {
            try
            {
                // load configuration settings
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                appConfiguration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    //.AddJsonFile($"appsettings.{environmentName}.json", true, true)
                    .Build();

                // DEBUG: Following section is only for development testing. Please comment
                //if (environmentName == "Development")
                //{
                Provider = "D";
                Action = "U";
                Parameter = "../../../../Data/Dictionary/2020-01-25 09:32:43.244059_found-words.txt";
                //Parameter2 = "ACT";
                // Provider = "data";
                // Action = "migrate";
                // Parameter = "SGRegistrationDbContext";
                //}

                if (string.IsNullOrEmpty(Provider) || string.IsNullOrEmpty(Action))
                {
                    // TODO : Show Useage
                }
                else
                {
                    Startup();
                    switch(Provider)
                    {
                        case "D":
                            var dictionaryService = serviceProvider.GetService<DictionaryDataUpdater>();
                            switch (Action)
                            {
                                case "U":
                                    dictionaryService.UpdateFromFile(Parameter);
                                    break;
                                default:
                                    logger.LogError($"Invalid action : {Action}");
                                    break;
                            }
                            break;
                        default:
                            logger.LogError($"Invalid provider : {Provider}");
                            break;
                            
                    }

                    // TODO :  Process Command
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, $"ERROR: {e.Message}");
            }
        }
    }
}
