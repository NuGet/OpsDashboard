using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Configuration
{
    public class ConnectionStringCollection : KeyedCollection<string, ConnectionString>
    {
        protected override string GetKeyForItem(ConnectionString item)
        {
            return item.Name;
        }
    }
}
