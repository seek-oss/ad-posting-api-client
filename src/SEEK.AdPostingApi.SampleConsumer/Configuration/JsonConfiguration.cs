using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SEEK.AdPostingApi.Client.Models;

namespace SEEK.AdPostingApi.Configuration
{
    public class JsonConfiguration : IConfiguration
    {
        public JsonConfiguration()
        {
            JsonConvert.PopulateObject(File.ReadAllText(GetConfigPath()), this);
        }

        private static string GetConfigPath()
        {
            var environment = System.Environment.GetEnvironmentVariable("ENVIRONMENT_NAME") ?? "integration";

            return "conf/" + environment + ".json";
        }

        public string ClientKey { get; set; }

        public string ClientSecret { get; set; }

        public string AdPostingApiBaseUrl { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EnvironmentType Environment { get; set; }
    }
}
