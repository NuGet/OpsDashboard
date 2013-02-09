using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using NuGetGallery.Dashboard.Api.Model;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Infrastructure;

namespace NuGetGallery.Dashboard.Api
{
    public class EnvironmentsController : BaseApiController
    {
        public EnvironmentsController(IConfigurationService configuration) : base(configuration) { }

        public IEnumerable<EnvironmentStatus> GetAll()
        {
            return ConfigService
                .Environments
                .Values
                .Where(e => e.PubliclyVisible || (User != null && User.IsInRole("Administrator")))
                .Select(e => new EnvironmentStatus()
                {
                    Name = e.Name,
                    Description = e.Description,
                    Url = e.Url,
                    AccessibleFromDashboard = null
                });
        }

        public async Task<EnvironmentStatus> Get(string id)
        {
            DeploymentEnvironment env;
            if (!ConfigService.Environments.TryGetValue(id, out env))
            {
                throw NotFound();
            }

            // Ping it from the dashboard
            bool visible = false;
            try
            {
                var handler = new WebRequestHandler() { CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache) };
                var client = new HttpClient(handler);
                var resp = await client.GetAsync(env.Url);
                visible = resp.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Swallow it!
            }

            return new EnvironmentStatus()
            {
                Name = env.Name,
                Description = env.Description,
                Url = env.Url,
                AccessibleFromDashboard = visible
            };
        }
    }
}