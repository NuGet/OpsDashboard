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
        private ConnectionsConfig _connections;
        private IDictionary<string, DeploymentEnvironment> _envs;

        public string Root { get; private set; }
        public AuthenticationConfig Auth { get { EnsureLoaded(); return _auth; } }
        public ConnectionsConfig Connections { get { EnsureLoaded(); return _connections; } }
        public IDictionary<string, DeploymentEnvironment> Environments { get { EnsureLoaded(); return _envs; } }

        public LocalJsonConfigurationService(string root)
        {
            Root = root;
        }

        public virtual void Reload(bool force)
        {
            LoadAuth(Path.Combine(Root, "Authentication.json"));
            LoadConnections(Path.Combine(Root, "Connections.json"));
            LoadEnvironments(Path.Combine(Root, "Environments.json"));
        }

        private void EnsureLoaded()
        {
            if (_auth == null || _connections == null || _envs == null)
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

        private void LoadConnections(string path)
        {
            _connections = new ConnectionsConfig();
            if (File.Exists(path))
            {
                // Load the JSON
                var root = JObject.Parse(File.ReadAllText(path));

                foreach (var type in root.Properties().Select(LoadConnectionType))
                {
                    _connections.ConnectionTypes.Add(type);
                }
            }
        }

        private static ConnectionType LoadConnectionType(JProperty prop)
        {
            var type = new ConnectionType(prop.Name);
            if (prop.Value.Type == JTokenType.Object)
            {
                foreach (var str in ((JObject)prop.Value).Properties().Select(LoadConnectionString))
                {
                    type.ConnectionStrings.Add(str);
                }
            }
            return type;
        }

        private static ConnectionString LoadConnectionString(JProperty prop)
        {
            if (prop.Value.Type != JTokenType.String)
            {
                throw new InvalidDataException(String.Format("Expected a string for property '{0}'", prop.Name));
            }
            return new ConnectionString(prop.Name, (string) prop.Value);
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