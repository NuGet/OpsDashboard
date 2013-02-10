using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Dashboard.Model;

namespace NuGetGallery.Dashboard.Configuration
{
    public interface IConfigurationService
    {
        AuthenticationConfig Auth { get; }
        ConnectionsConfig Connections { get; }
        IDictionary<string, DeploymentEnvironment> Environments { get; }

        void Reload(bool force);
    }
}