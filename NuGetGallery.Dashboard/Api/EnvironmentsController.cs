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
using NuGetGallery.Dashboard.Model;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.Api
{
    public class EnvironmentsController : BaseApiController
    {
        private readonly IEnumerable<Pinger> _pingers;

        public EnvironmentsController(IConfigurationService configuration, IEnumerable<Pinger> pingers) : base(configuration) {
            _pingers = pingers;
        }

        public IEnumerable<EnvironmentStatus> GetAll()
        {
            return ConfigService
                .Environments
                .Values
                .Where(e => e.PubliclyVisible || (User != null && User.IsInRole("Administrator")))
                .Select(e => CreateStatus(e));
        }

        public async Task<EnvironmentStatus> Get(string id)
        {
            DeploymentEnvironment env;
            if (!ConfigService.Environments.TryGetValue(id, out env))
            {
                throw NotFound();
            }
            if (!env.PubliclyVisible && !User.IsInRole("Administrator"))
            {
                throw NotFound();
            }

            // Run the pings
            var pings = await Task.WhenAll(_pingers.Select(async p => await p.Ping(env)));

            return CreateStatus(env, pings);
        }

        private EnvironmentStatus CreateStatus(DeploymentEnvironment env)
        {
            return CreateStatus(env, null);
        }

        private EnvironmentStatus CreateStatus(DeploymentEnvironment env, IList<PingResult> pings)
        {
            return new EnvironmentStatus()
            {
                Title = env.Title,
                Name = env.Name,
                Description = env.Description,
                Url = env.Url,
                PingResults = pings ?? new List<PingResult>()
            };
        }
    }
}