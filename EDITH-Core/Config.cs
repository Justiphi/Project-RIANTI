using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EDITH_Core.Models;
using Microsoft.CognitiveServices;
using Microsoft.CognitiveServices.Speech;
using Newtonsoft.Json;

namespace EDITH_Core
{
    public class Config
    {
        public static string ConversationKey;
        public static string LastS;
        public static string CurrentHost;
        public static string Keyword;
        public static SpeechConfig AzureSpeechConfig;
        public static string BaseURL = "http://api.wolframalpha.com";
        public static string WrConversation = "/v1/conversation.jsp";
        public static string WrSimple = "/v1/simple";
        public static string WrSpoken = "/v1/spoken";

        public static List<string> DefaultCommands = new List<string>();

        //configuration class to get configuration information from
        public static ConfigModel config;

        public static void configure()
        {
            string file;

            file = Path.Combine(AppContext.BaseDirectory, "_config.json");

            //ensures _config.json exists
            if (!File.Exists(file))
            {
                throw new ApplicationException("Unable to locate the _config.json file.");
            }

            //loads configuration from _config.json file into memory
            config = JsonConvert.DeserializeObject<ConfigModel>(File.ReadAllText(file));
        }

        public static void SetDefaultCommand()
        {
            DefaultCommands.Add("Time");
        }

        public static SpeechConfig GetAzureSpeechConfig()
        {
            if (AzureSpeechConfig == null)
            {
                AzureSpeechConfig = SpeechConfig.FromSubscription(config.AzureApiKey, config.AzureRegion);
            }
            return AzureSpeechConfig;
        }


        public static string GetWolframURLArgs(string query, int Mode)
        {
            string URLArgs = $"?appid={config.WolframAlphaAPIKey}";

            if ((!string.IsNullOrEmpty(ConversationKey)) && (Mode == 0))
            {
                URLArgs += $"&conversationid={ConversationKey}";
            }

            URLArgs += $"&i={query}";

            if (!string.IsNullOrEmpty(LastS))
            {
                URLArgs += $"&s={LastS}";
                LastS = null;
            }

            return URLArgs;
        }
    }
}
