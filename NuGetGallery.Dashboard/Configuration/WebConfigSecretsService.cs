using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Configuration
{
    public class WebConfigSecretsService : ISecretsService
    {
        private IDictionary<string, string> _overrideKeys;

        public WebConfigSecretsService() : this(new Dictionary<string, string>()) { }
        public WebConfigSecretsService(IDictionary<string, string> overrideKeys)
        {
            _overrideKeys = overrideKeys;
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