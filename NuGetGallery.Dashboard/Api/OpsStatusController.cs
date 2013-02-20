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

            // Get the blob
            CloudBlockBlob blob = ConfigService.GetBlobFromUrl(env.Name, env.OperationsStatusBlob);

            // Download the blob to a string
            string json = await blob.DownloadToString();

            // For now, return it
            return json;
        }
    }
}