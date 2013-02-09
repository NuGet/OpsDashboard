using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NuGetGallery.Dashboard.Configuration
{
    public class LocalJsonConfigurationService : IConfigurationService
    {
        public AuthenticationConfig Auth { get; private set; }
        public ConnectionsConfig Connections { get; private set; }
        public EnvironmentsConfig Environments { get; private set; }

        public LocalJsonConfigurationService()
        {
            Reload();
        }

        public void Reload()
        {
            // Load each file
            string root = AppDomain.CurrentDomain.BaseDirectory;

            LoadAuth(Path.Combine(root, "App_Data", "Authentication.json"));
            LoadConnections(Path.Combine(root, "App_Data", "Connections.json"));
            LoadEnvironments(Path.Combine(root, "App_Data", "Environments.json"));
        }

        private void LoadEnvironments(string path)
        {
            if (File.Exists(path))
            {
                // Load the JSON
                Environments = new EnvironmentsConfig(
                    JsonConvert.DeserializeObject<IDictionary<string, DeploymentEnvironment>>(
                        File.ReadAllText(path)));
            }
        }

        private void LoadConnections(string path)
        {
            if (File.Exists(path))
            {
                // Load the JSON
                Connections = new ConnectionsConfig(
                    JsonConvert.DeserializeObject<IDictionary<string, ConnectionString>>(
                        File.ReadAllText(path)));
            }
        }

        private void LoadAuth(string path)
        {
            if (File.Exists(path))
            {
                // Load the JSON
                Auth = 
                    JsonConvert.DeserializeObject<AuthenticationConfig>(
                        File.ReadAllText(path));
            }
        }
    }
}