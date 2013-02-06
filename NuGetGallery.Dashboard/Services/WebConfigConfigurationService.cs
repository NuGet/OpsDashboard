using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NuGetGallery.Dashboard.Services
{
    public class WebConfigConfigurationService : ConfigurationService
    {
        private IDictionary<string, string> _overrideSettings = new Dictionary<string, string>();

        public WebConfigConfigurationService()
        {
            string mapped = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data\Settings.private.json");
            if (!String.IsNullOrEmpty(mapped) && File.Exists(mapped))
            {
                using(StreamReader rdr = new StreamReader(mapped)) {
                    _overrideSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(rdr.ReadToEnd());
                }
            }
        }

        protected override string GetSetting(string name)
        {
            string fullName = String.Concat("Dashboard.", name);
            string value = null;
            if (!_overrideSettings.TryGetValue(fullName, out value))
            {
                value = ConfigurationManager.AppSettings[fullName];
            }
            return value;
        }
    }
}