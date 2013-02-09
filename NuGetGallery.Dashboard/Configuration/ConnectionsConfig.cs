using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Configuration
{
    public class ConnectionsConfig
    {
        private readonly IDictionary<string, ConnectionString> _connectionStrings;

        public IDictionary<string, ConnectionString> ConnectionStrings { get { return _connectionStrings; } }

        public ConnectionsConfig() : this(new Dictionary<string, ConnectionString>(StringComparer.OrdinalIgnoreCase)) { }
        public ConnectionsConfig(IDictionary<string, ConnectionString> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public T Connect<T>(string connectionStringName) where T : class
        {
            ConnectionString str = null;
            if (!_connectionStrings.TryGetValue(connectionStringName, out str))
            {
                throw new KeyNotFoundException("No such connection string '" + connectionStringName + "'");
            }
            return str.Connect<T>();
        }
    }
}
