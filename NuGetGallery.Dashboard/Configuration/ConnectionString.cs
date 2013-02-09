using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace NuGetGallery.Dashboard.Configuration
{
    [JsonConverter(typeof(ConnectionStringConverter))]
    public class ConnectionString
    {
        public string Value { get; private set; }

        protected ConnectionString(string value)
        {
            Value = value;
        }

        public T Connect<T>() where T : class
        {
            if (typeof(T) == typeof(CloudStorageAccount))
            {
                return CloudStorageAccount.Parse(Value) as T;
            }
            else if (typeof(T) == typeof(SqlConnection))
            {
                return new SqlConnection(Value) as T;
            }
            else
            {
                throw new InvalidOperationException("Unknown Connection Type: " + typeof(T).FullName);
            }
        }

        public class ConnectionStringConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ConnectionString);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                return new ConnectionString(reader.ReadAsString());
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                writer.WriteValue(((ConnectionString)value).Value);
            }
        }
    }
}
