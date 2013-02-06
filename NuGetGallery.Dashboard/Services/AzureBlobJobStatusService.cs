using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace NuGetGallery.Dashboard.Services
{
    public class AzureBlobJobStatusService : JobStatusService
    {
        private readonly ConfigurationService _configuration;
        private CloudStorageAccount _account;

        public AzureBlobJobStatusService(ConfigurationService configuration)
        {
            _configuration = configuration;
            _account = CloudStorageAccount.Parse(_configuration.OpsStatusConnectionString);
        }

        public override string GetJobStatusJson()
        {
            var client = _account.CreateCloudBlobClient();
            var container = client.GetContainerReference(_configuration.OpsStatusContainerName);
            var blob = container.GetBlockBlobReference("jobs.json");

            using (var memStrm = new MemoryStream())
            {
                blob.DownloadToStream(memStrm);
                memStrm.Flush();
                memStrm.Seek(0, SeekOrigin.Begin);
                using (var rdr = new StreamReader(memStrm))
                {
                    return rdr.ReadToEnd();
                }
            }
        }
    }
}