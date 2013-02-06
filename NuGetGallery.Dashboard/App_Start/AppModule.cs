using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Text;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
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

            Kernel.Bind<DataProtectionService>()
                  .To<MachineKeyDataProtectionService>()
                  .InSingletonScope();

            SetupFederatedLogin();
        }

        private void SetupFederatedLogin()
        {
            FederatedAuthentication.FederationConfigurationCreated += (sender, args) =>
            {
                var config = Kernel.Get<ConfigurationService>();
                var idconfig = new IdentityConfiguration();
                idconfig.AudienceRestriction.AllowedAudienceUris.Add(new Uri(config.AudienceUrl));

                var registry = new ConfigurationBasedIssuerNameRegistry();
                registry.AddTrustedIssuer(config.TokenCertificateThumbprint, config.AuthenticationIssuer);
                idconfig.IssuerNameRegistry = registry;
                idconfig.CertificateValidationMode = X509CertificateValidationMode.None;

                var wsfedconfig = new WsFederationConfiguration(config.AuthenticationIssuer, config.AuthenticationRealm);
                wsfedconfig.PersistentCookiesOnPassiveRedirects = true;
                
                args.FederationConfiguration.IdentityConfiguration = idconfig;
                args.FederationConfiguration.WsFederationConfiguration = wsfedconfig;
                args.FederationConfiguration.CookieHandler = new ChunkedCookieHandler();
            };
        }
    }
}
