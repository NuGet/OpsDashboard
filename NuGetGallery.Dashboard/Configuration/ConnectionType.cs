using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Configuration
{
    public class ConnectionType
    {
        public string Scheme { get; private set; }
        public ConnectionStringCollection ConnectionStrings { get; private set; }

        public ConnectionType(string scheme)
        {
            Scheme = scheme;
            ConnectionStrings = new ConnectionStringCollection();
        }
    }
}
