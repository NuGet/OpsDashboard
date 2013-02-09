using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Model;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class Pinger
    {
        public abstract Task<PingResult> Ping(DeploymentEnvironment target);
    }
}