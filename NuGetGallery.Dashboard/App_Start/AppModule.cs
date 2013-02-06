using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Ninject;
using Ninject.Modules;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.App_Start
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ConfigurationService>()
                  .To<WebConfigConfigurationService>()
                  .InSingletonScope();

            Kernel.Bind<PackageService>()
                  .To<AzureBlobPackageService>()
                  .InSingletonScope();

            Kernel.Bind<JobStatusService>()
                  .To<AzureBlobJobStatusService>()
                  .InSingletonScope();

            Kernel.Bind<AuthenticationService>()
                  .To<DefaultAuthenticationService>()
                  .InSingletonScope();

            Kernel.Bind<DataProtectionService>()
                  .To<MachineKeyDataProtectionService>()
                  .InSingletonScope();

            SetupFederatedLogin();
        }

        private void SetupFederatedLogin()
        {
            Kernel.Bind<FederationConfiguration>()
                  .ToMethod(ctx => {
                      var config = ctx.Kernel.Get<ConfigurationService>();
                      
                      What is this I don't even?
                      

                      return fedconfig;
                  }).InSingletonScope();
        }
    }
}
