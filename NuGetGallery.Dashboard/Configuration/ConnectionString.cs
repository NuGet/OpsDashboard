using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace NuGetGallery.Dashboard.Configuration
{
    public class ConnectionString
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public ConnectionString(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
