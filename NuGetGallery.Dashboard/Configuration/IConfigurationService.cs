using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Configuration
{
    public interface IConfigurationService
    {
        AuthenticationConfig Auth { get; }
        ConnectionsConfig Connections { get; }
        EnvironmentsConfig Environments { get; }

        void Reload();
    }
}