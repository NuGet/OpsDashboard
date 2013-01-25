using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NuGetDashboard.Services
{
    public class WebConfigConfigurationService : ConfigurationService
    {
        private IDictionary<string, string> _overrideSettings = new Dictionary<string, string>();

        public WebConfigConfigurationService()
        {
            // Load the private settings overlay
            var ctxt = HttpContext.Current;
            if (ctxt != null)
            {
                string mapped = ctxt.Server.MapPath("/App_Data/settings.private.json");
                if (!String.IsNullOrEmpty(mapped) && File.Exists(mapped))
                {
                    using(StreamReader rdr = new StreamReader(mapped)) {
                        _overrideSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(rdr.ReadToEnd());
                    }
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