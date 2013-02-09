using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Model;

namespace NuGetGallery.Dashboard.Services.Ping
{
    public class UrlPinger : Pinger
    {
        private readonly string _name;
        private readonly Func<DeploymentEnvironment, Uri> _urlSelector;

        public UrlPinger(string name, Func<DeploymentEnvironment, Uri> urlSelector)
        {
            _name = name;
            _urlSelector = urlSelector;
        }

        public override async Task<PingResult> Ping(DeploymentEnvironment target)
        {
            var url = _urlSelector(target);
            var client = new HttpClient(
                new WebRequestHandler() { 
                    CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache) 
                });
            try
            {
                var resp = await client.GetAsync(url);
                if (resp.IsSuccessStatusCode)
                {
                    return new PingResult(
                        _name,
                        detail: String.Format("Successfully pinged!", url.AbsoluteUri),
                        target: url.AbsoluteUri,
                        success: true);
                }
                return new PingResult(
                    _name,
                    detail: String.Format("Recieved {0} error!", (int)resp.StatusCode, url.AbsoluteUri),
                    target: url.AbsoluteUri,
                    success: false);
            }
            catch (Exception ex)
            {
                return new PingResult(
                    _name,
                    detail: String.Format("Unknown error!", url.AbsoluteUri),
                    target: url.AbsoluteUri,
                    success: false);
            }
        }
    }
}