using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NuGetGallery.Dashboard.Configuration;

namespace NuGetGallery.Dashboard.Configuration
{
    public class BlobJsonConfigurationService : LocalJsonConfigurationService
    {
        private CloudStorageAccount _account;
        private string _containerName;

        public BlobJsonConfigurationService(ISecretsService secrets)
            : base(secrets)
        {
            _account = CloudStorageAccount.Parse(secrets.GetSetting("Configuration.Connection"));
            _containerName = secrets.GetSetting("Configuration.Container");
        }

        public override void Reload(bool force)
        {
            if (force || 
                !File.Exists(Path.Combine(Root, "Environments.json")) || 
                !File.Exists(Path.Combine(Root, "Authentication.json")))
            {
                try
                {
                    CloudBlobClient client = _account.CreateCloudBlobClient();
                    var container = client.GetContainerReference(_containerName);

                    // Get the blobs
                    var envBlob = container.GetBlockBlobReference("Environments.json");
                    var authBlob = container.GetBlockBlobReference("Authentication.json");

                    // Download any that exist
                    DownloadIfExists(envBlob);
                    DownloadIfExists(authBlob);
                }
                catch (Exception ex)
                {
                    // Any errors? Just use the local files if present and let it be. But do write to trace
                    Trace.WriteLine(String.Format("Error loading Dashboard Configuration: {0}", ex));
                }
            }
            base.Reload(false);
        }

        private void DownloadIfExists(CloudBlockBlob blob)
        {
            string target = Path.Combine(Root, blob.Name);
            if (blob.Exists())
            {
                if (!Directory.Exists(Root))
                {
                    Directory.CreateDirectory(Root);
                }
                using (var file = File.OpenWrite(target))
                {
                    blob.DownloadToStream(file);
                }
            }
        }
    }
}
