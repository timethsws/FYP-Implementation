using System;
using System.Diagnostics;
using System.IO;
using Google.Apis.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SinSenseInfastructure.Services
{
    public class EnglishMorphologyService
    {
        private readonly string PythonExecutablePath;
        private readonly string LemmaScriptPath;
        private readonly string StemScriptPath;
        private readonly ILogger<EnglishMorphologyService> logger;

        public EnglishMorphologyService(IConfiguration configuration,ILogger<EnglishMorphologyService> logger)
        {
            this.logger = logger;

            PythonExecutablePath = configuration.GetValue<string>("ServiceConfiguration:EnglishMorphologyService:PythonExecutable");
            LemmaScriptPath = Path.GetFullPath(configuration.GetValue<string>("ServiceConfiguration:EnglishMorphologyService:LemmaScript"));
            StemScriptPath = Path.GetFullPath(configuration.GetValue<string>("ServiceConfiguration:EnglishMorphologyService:StemScript"));

        }

        public string GetLemma (string word)
        {
            try
            {
                var arguments = $"\"{word}\"";
                var res = ExecutePythonScript(LemmaScriptPath, arguments);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
        }

        public string GetStem(string word)
        {
            try
            {
                var arguments = $"\"{word}\"";
                var res = ExecutePythonScript(StemScriptPath, arguments);
                return res;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
                return null;
            }
        }


        private string ExecutePythonScript(string filePath, string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    Arguments = arguments,
                    FileName = PythonExecutablePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                process.Start();
                process.WaitForExit();
                if(process.ExitCode != 0)
                {
                    throw new ApplicationException($"Executing command \"{PythonExecutablePath} {filePath} {arguments}\" failed with exit code : {process.ExitCode} ");
                }

                return process.StandardOutput.ReadToEnd();
            }
        }
    }
}
