using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NuGetGallery.Dashboard.Configuration
{
    public class WebConfigSecretsService : ISecretsService
    {
        private readonly IDictionary<string, string> _overrideKeys;

        public WebConfigSecretsService() : this(new Dictionary<string, string>()) { }
        public WebConfigSecretsService(IDictionary<string, string> overrideKeys)
        {
            _overrideKeys = overrideKeys;

            // Fill the override keys from the Settings.json if provided
            var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "AppSettings.json");
            if (File.Exists(jsonPath))
            {
                FillFromJson(jsonPath);
            }
        }

        private void FillFromJson(string jsonPath)
        {
            var dict = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(jsonPath));
            foreach (var pair in dict)
            {
                // The provided _overrideKeys overrides the file
                if (!_overrideKeys.ContainsKey(pair.Key))
                {
                    _overrideKeys[pair.Key] = pair.Value;
                }
            }
        }

        public string GetSetting(string key)
        {
            // First try the override settings
            string value;
            if (!_overrideKeys.TryGetValue(key, out value))
            {
                // Next, try App Settings
                value = ConfigurationManager.AppSettings[key];
            }
            return value;
        }
    }
}