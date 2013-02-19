using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.WindowsAzure.Storage;

namespace NuGetGallery.Dashboard.Configuration
{
    public class ConnectionsConfig
    {
        public ConnectionTypeCollection ConnectionTypes { get; private set; } 

        public ConnectionsConfig()
        {
            ConnectionTypes = new ConnectionTypeCollection();
        }

        public CloudStorageAccount ConnectAzureStorage(Uri url)
        {
            // Check for a connection type matching the scheme
            if (!ConnectionTypes.Contains(url.Scheme))
            {
                throw new InvalidOperationException(String.Format("Unknown scheme '{0}'", url.Scheme));
            }
            ConnectionType type = ConnectionTypes[url.Scheme];
            if (!type.ConnectionStrings.Contains(url.Host))
            {
                throw new KeyNotFoundException(String.Format("No connection string for '{0}'", url.Host));
            }
            return CloudStorageAccount.Parse(type.ConnectionStrings[url.Host].Value);
        }
    }
}
