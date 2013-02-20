using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NuGetGallery.Dashboard.Configuration;

namespace NuGetGallery.Dashboard.Infrastructure
{
    public static class BlobConfigurationServiceExtensions
    {
        public static CloudBlockBlob GetBlobFromUrl(this IConfigurationService self, string environmentName, Uri url)
        {
            if (!url.Scheme.Equals("azure", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("URL is not an 'azure://' url");
            }

            CloudStorageAccount acct = self.ConnectToAzure(environmentName, "OperationsStatus");
            CloudBlobClient client = acct.CreateCloudBlobClient();
            if (url.Segments.Length < 3)
            {
                throw new InvalidOperationException("Invalid azure:// url. Expected azure://[conectionName]/[container]/[blobpath]");
            }
            CloudBlobContainer container =
                client.GetContainerReference(url.Segments[1].TrimEnd('/'));

            // Build the blob path
            string path = String.Concat(url.Segments.Skip(2));
            return container.GetBlockBlobReference(path);
        }
    }
}