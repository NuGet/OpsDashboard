using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NuGetGallery.Dashboard
{
    public static class Global
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.None,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static JsonSerializerSettings SerializerSettings { get { return _serializerSettings; } }
    }
}