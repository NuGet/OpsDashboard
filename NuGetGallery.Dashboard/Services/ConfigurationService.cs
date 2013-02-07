using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class ConfigurationService
    {
        public virtual string PackageStoreConnectionString { get { return GetSetting("AzurePackageStoreConnectionString"); } }
        public virtual string PackageContainerName { get { return GetSetting("PackageContainerName"); } }

        public virtual string OpsStatusConnectionString { get { return GetSetting("AzureOpsStatusConnectionString"); } }
        public virtual string OpsStatusContainerName { get { return GetSetting("OpsStatusContainerName"); } }

        public virtual string LoginUrlFormat { get { return GetSetting("LoginUrlFormat"); } }
        public virtual string AudienceUrl { get { return GetSetting("AudienceUrl"); } }
        public virtual string AuthenticationIssuer { get { return GetSetting("AuthenticationIssuer"); } }
        public virtual string AuthenticationRealm { get { return GetSetting("AuthenticationRealm"); } }
        public virtual string TokenCertificateThumbprint { get { return GetSetting("TokenCertificateThumbprint"); } }

        protected abstract string GetSetting(string name);
    }
}