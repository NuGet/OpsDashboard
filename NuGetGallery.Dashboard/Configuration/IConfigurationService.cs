using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using NuGetGallery.Dashboard.Model;

namespace NuGetGallery.Dashboard.Configuration
{
    public interface IConfigurationService
    {
        AuthenticationConfig Auth { get; }
        IDictionary<string, DeploymentEnvironment> Environments { get; }
        string GetConnectionString(string environment, string type, string name);

        void Reload(bool force);
    }

    public static class ConfigurationConstants
    {
        public static readonly string AzureConnectionType = "Azure";
    }

    public static class ConfigurationServiceExtensions
    {
        public static CloudStorageAccount ConnectToAzure(this IConfigurationService self, string environment, string connectionName)
        {
            string connectionString = self.GetConnectionString(environment, ConfigurationConstants.AzureConnectionType, connectionName);
            CloudStorageAccount acct;
            if (String.IsNullOrEmpty(connectionString) || !CloudStorageAccount.TryParse(connectionString, out acct))
            {
                throw new InvalidOperationException(
                    String.Format("Unknown or invalid {2} connection string for {0} environment: {1}",
                        environment, connectionName, ConfigurationConstants.AzureConnectionType));
            }
            return acct;
        }
    }
}