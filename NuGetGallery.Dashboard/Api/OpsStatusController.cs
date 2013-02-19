using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NuGetGallery.Dashboard.Api.Model;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Infrastructure;
using NuGetGallery.Dashboard.Model;

namespace NuGetGallery.Dashboard.Api
{
    public class OpsStatusController : BaseApiController
    {
        public OpsStatusController(IConfigurationService configuration) : base(configuration) { }

        [Authorize(Roles = "Administrator")]
        public async Task<string> Get(string id)
        {
            // Get the environment
            DeploymentEnvironment env;
            if (!ConfigService.Environments.TryGetValue(id, out env) || env.OperationsStatusBlob == null)
            {
                throw NotFound();
            }

            // Download the blob
            CloudStorageAccount acct = ConfigService.Connections.ConnectAzureStorage(env.OperationsStatusBlob);
            CloudBlobClient client = acct.CreateCloudBlobClient();
            if (env.OperationsStatusBlob.Segments.Length < 3)
            {
                throw new InvalidOperationException("Invalid azure:// url. Expected azure://[account]/[container]/[blobpath]");
            }
            CloudBlobContainer container =
                client.GetContainerReference(env.OperationsStatusBlob.Segments[1].TrimEnd('/'));

            // Build the blob path
            string path = String.Concat(env.OperationsStatusBlob.Segments.Skip(2));
            CloudBlockBlob blob = container.GetBlockBlobReference(path);

            // Download the blob to a string
            string json = await blob.DownloadToString();

            // For now, return it
            return json;
        }
    }
}