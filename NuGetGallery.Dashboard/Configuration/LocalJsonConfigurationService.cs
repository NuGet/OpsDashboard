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
        public IDictionary<string, DeploymentEnvironment> Environments { get; private set; }

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
                // HACK: The dictionary key -> name thing could be done cleaner, but I'm lazy
                var envs = JsonConvert.DeserializeObject<IDictionary<string, DeploymentEnvironment>>(File.ReadAllText(path));
                foreach (var pair in envs)
                {
                    pair.Value.Name = pair.Key;
                }
                Environments = envs.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase);
            }
        }

        private void LoadConnections(string path)
        {
            if (File.Exists(path))
            {
                // Load the JSON
                // HACK: The dictionary key -> name thing could be done cleaner, but I'm lazy
                var connections = JsonConvert.DeserializeObject<IDictionary<string, ConnectionString>>(
                    File.ReadAllText(path));
                foreach (var pair in connections)
                {
                    pair.Value.Name = pair.Key;
                }
                Connections = new ConnectionsConfig(connections.ToDictionary(pair => pair.Key, pair => pair.Value, StringComparer.OrdinalIgnoreCase));
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