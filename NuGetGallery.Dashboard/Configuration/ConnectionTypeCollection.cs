using System.Collections.ObjectModel;

namespace NuGetGallery.Dashboard.Configuration
{
    public class ConnectionTypeCollection : KeyedCollection<string, ConnectionType>
    {
        protected override string GetKeyForItem(ConnectionType item)
        {
            return item.Scheme;
        }
    }
}