using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Microsoft.WindowsAzure.Storage.Blob
{
    public static class CloudBlockBlobExtensions
    {
        public static async Task<string> DownloadToString(this CloudBlockBlob blob)
        {
            using (var strm = new MemoryStream())
            {
                await
                    Task.Factory.FromAsync((cb, state) => blob.BeginDownloadToStream(strm, cb, state),
                                           ar => blob.EndDownloadToStream(ar), 
                                           state: null);
                await strm.FlushAsync();
                strm.Seek(0, SeekOrigin.Begin);
                return await new StreamReader(strm).ReadToEndAsync();
            }
        }
    }
}