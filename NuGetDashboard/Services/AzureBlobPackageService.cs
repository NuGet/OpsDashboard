using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NuGetDashboard.Model;

namespace NuGetDashboard.Services
{
    public class AzureBlobPackageService : PackageService
    {
        private readonly Regex v1BlobFormat = new Regex(@"NuGetGallery_(?<hash>[0-9a-fA-F]+)_(?<branch>.+)\.cspkg");

        private readonly ConfigurationService _configuration;
        private CloudStorageAccount _account;

        public AzureBlobPackageService(ConfigurationService configuration)
        {
            _configuration = configuration;
            _account = CloudStorageAccount.Parse(_configuration.PackageStoreConnectionString);
        }

        public override IEnumerable<PackageFile> GetAll()
        {
            // Connect to the blob service
            var client = _account.CreateCloudBlobClient();
            var container = client.GetContainerReference(_configuration.PackageContainerName);

            // List blobs in this container and create PackageFile objects from them
            return container.ListBlobs(prefix: "NuGetGallery_", useFlatBlobListing: true, blobListingDetails: BlobListingDetails.Metadata)
                            .OfType<CloudBlockBlob>()
                            .Select(ParseBlobMetadata)
                            .Where(b => b != null);
        }

        private PackageFile ParseBlobMetadata(CloudBlockBlob blob)
        {
            var match = v1BlobFormat.Match(blob.Name);
            if(match.Success) {
                return new PackageFile() {
                    Url = blob.Uri,
                    CommitHash = match.Groups["hash"].Value,
                    Branch = match.Groups["branch"].Value
                };
            }
            else {
                return null;
            }
        }
    }
}