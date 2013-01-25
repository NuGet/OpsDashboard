using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject.Modules;
using NuGetDashboard.Services;

namespace NuGetDashboard.Boot
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ConfigurationService>()
                  .To<WebConfigConfigurationService>()
                  .InSingletonScope();

            Kernel.Bind<PackageService>()
                  .To<AzureBlobPackageService>();

            Kernel.Bind<JobStatusService>()
                  .To<AzureBlobJobStatusService>();
        }
    }
}
