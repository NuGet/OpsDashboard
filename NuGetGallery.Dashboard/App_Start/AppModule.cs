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
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.App_Start
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IConfigurationService>()
                  .To<LocalJsonConfigurationService>()
                  .InSingletonScope();

            SetupFederatedLogin();
        }

        private void SetupFederatedLogin()
        {
            FederatedAuthentication.FederationConfigurationCreated += (sender, args) =>
            {
                var config = Kernel.Get<IConfigurationService>();
                var idconfig = new IdentityConfiguration();
                idconfig.AudienceRestriction.AllowedAudienceUris.Add(new Uri(config.Auth.AudienceUrl));

                var registry = new ConfigurationBasedIssuerNameRegistry();
                registry.AddTrustedIssuer(config.Auth.TokenCertificateThumbprint, config.Auth.AuthenticationIssuer);
                idconfig.IssuerNameRegistry = registry;
                idconfig.CertificateValidationMode = X509CertificateValidationMode.None;

                var wsfedconfig = new WsFederationConfiguration(config.Auth.AuthenticationIssuer, config.Auth.AuthenticationRealm);
                wsfedconfig.PersistentCookiesOnPassiveRedirects = true;
                
                args.FederationConfiguration.IdentityConfiguration = idconfig;
                args.FederationConfiguration.WsFederationConfiguration = wsfedconfig;
                args.FederationConfiguration.CookieHandler = new ChunkedCookieHandler();
            };
        }
    }
}
