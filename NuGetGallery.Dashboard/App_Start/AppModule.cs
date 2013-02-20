using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Text;
using System.Web.Http;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Modules;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Services;
using NuGetGallery.Dashboard.Services.Ping;

namespace NuGetGallery.Dashboard.App_Start
{
    public class AppModule : NinjectModule
    {
        public override void Load()
        {
            // Initialize Config
            Kernel.Bind<ISecretsService>()
                  .To<WebConfigSecretsService>()
                  .InSingletonScope();

            var configMode = ConfigurationManager.AppSettings["Configuration.Mode"];
            if (String.Equals(configMode, "blob", StringComparison.OrdinalIgnoreCase))
            {
                Kernel.Bind<IConfigurationService>()
                      .To<BlobJsonConfigurationService>()
                      .InSingletonScope();
            }
            else
            {
                Kernel.Bind<IConfigurationService>()
                      .To<LocalJsonConfigurationService>()
                      .InSingletonScope();
            }

            SetupFederatedLogin();

            SetupPingers();
        }

        private void DeleteIfExists(string path)
        {
            if (File.Exists(path)) { 
                File.Delete(path); 
            }
        }

        private void SetupPingers()
        {
            Kernel.Bind<Pinger>()
                  .ToMethod(_ => new UrlPinger("Home Page", env => env.Url))
                  .InSingletonScope();
            Kernel.Bind<Pinger>()
                  .ToMethod(_ => new UrlPinger("v1 Feed", env => SetPath(env.Url, "/api/v1/")))
                  .InSingletonScope();
            Kernel.Bind<Pinger>()
                  .ToMethod(_ => new UrlPinger("v2 Feed", env => SetPath(env.Url, "/api/v2/")))
                  .InSingletonScope();
        }

        private Uri SetPath(Uri uri, string path)
        {
            return (new UriBuilder(uri) { Path = path }).Uri;
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

                var sessionTransforms = new List<CookieTransform>() {
                    new DeflateCookieTransform(),
                    new MachineKeyTransform()
                };
                idconfig.SecurityTokenHandlers.AddOrReplace(new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly()));

                var wsfedconfig = new WsFederationConfiguration(config.Auth.AuthenticationIssuer, config.Auth.AuthenticationRealm);
                wsfedconfig.PersistentCookiesOnPassiveRedirects = true;
                
                args.FederationConfiguration.IdentityConfiguration = idconfig;
                args.FederationConfiguration.WsFederationConfiguration = wsfedconfig;
                args.FederationConfiguration.CookieHandler = new ChunkedCookieHandler();
            };
        }
    }
}
