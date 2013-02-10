using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                return HttpResponseResult(url, resp);
            }
            catch (HttpRequestException hrex)
            {
                WebException wex = hrex.InnerException as WebException;
                if (wex != null)
                {
                    return WebExceptionResult(url, wex);
                }
                return UnknownErrorResult(url);
            }
            catch (Exception)
            {
                return UnknownErrorResult(url);
            }
        }

        private PingResult WebExceptionResult(Uri url, WebException wex)
        {
            string detailString;
            switch (wex.Status)
            {
                case WebExceptionStatus.NameResolutionFailure:
                    detailString = "Host name could not be resolved";
                    break;
                default:
                    detailString = String.Format("{0} error", wex.Status);
                    break;
            }
            return new PingResult(
                _name,
                detailString,
                target: url.AbsoluteUri,
                success: false);
        }

        private PingResult HttpResponseResult(Uri url, HttpResponseMessage resp)
        {
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

        private PingResult UnknownErrorResult(Uri url)
        {
            return new PingResult(
                _name,
                detail: String.Format("Unknown error!", url.AbsoluteUri),
                target: url.AbsoluteUri,
                success: false);
        }
    }
}