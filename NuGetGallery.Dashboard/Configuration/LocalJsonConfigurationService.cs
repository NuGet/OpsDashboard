using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGetGallery.Dashboard.Model;

namespace NuGetGallery.Dashboard.Configuration
{
    public class LocalJsonConfigurationService : IConfigurationService
    {
        private AuthenticationConfig _auth;
        private IDictionary<string, DeploymentEnvironment> _envs;
        private ISecretsService _secrets;

        public string Root { get; private set; }
        public AuthenticationConfig Auth { get { EnsureLoaded(); return _auth; } }
        public IDictionary<string, DeploymentEnvironment> Environments { get { EnsureLoaded(); return _envs; } }

        public LocalJsonConfigurationService(ISecretsService secrets)
        {
            _secrets = secrets;
            Root = _secrets.GetSetting("Configuration.Path");

            if (String.IsNullOrEmpty(Root))
            {
                Root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            }
        }

        public virtual void Reload(bool force)
        {
            LoadAuth(Path.Combine(Root, "Authentication.json"));
            LoadEnvironments(Path.Combine(Root, "Environments.json"));
        }

        public string GetConnectionString(string environment, string type, string name)
        {
            // Construct a secret name
            string secretName = String.Format("Connections.{0}.{1}.{2}", environment, type, name);

            // Try getting it from the secret service (pun intended)
            return _secrets.GetSetting(secretName);
        }

        private void EnsureLoaded()
        {
            if (_auth == null || _envs == null)
            {
                Reload(false);
            }
        }

        private void LoadEnvironments(string path)
        {
            if (File.Exists(path))
            {
                // Load the JSON
                // HACK: The dictionary key -> name thing could be done cleaner, but I'm lazy
                var envs = JsonConvert.DeserializeObject<IDictionary<string, DeploymentEnvironment>>(File.ReadAllText(path));
                foreach (var pair in envs)
                {
                    pair.Value.Name = pair.Key;
                }
                _envs = envs.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                _envs = new Dictionary<string, DeploymentEnvironment>();
            }
        }

        private void LoadAuth(string path)
        {
            if (File.Exists(path))
            {
                // Load the JSON
                _auth =
                    JsonConvert.DeserializeObject<AuthenticationConfig>(
                        File.ReadAllText(path));
            }
            else
            {
                _auth = new AuthenticationConfig();
            }
        }
    }
}