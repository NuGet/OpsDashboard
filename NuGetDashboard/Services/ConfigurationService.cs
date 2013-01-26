using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetDashboard.Services
{
    public abstract class ConfigurationService
    {
        public virtual string PackageStoreConnectionString { get { return GetSetting("AzurePackageStoreConnectionString"); } }
        public virtual string PackageContainerName { get { return GetSetting("PackageContainerName"); } }

        public virtual string OpsStatusConnectionString { get { return GetSetting("AzureOpsStatusConnectionString"); } }
        public virtual string OpsStatusContainerName { get { return GetSetting("OpsStatusContainerName"); } }

        public virtual string LoginPageUrl { get { return GetSetting("LoginPageUrl"); } }
        public virtual string TokenSigningCert { get { return GetSetting("TokenSigningCert"); } }

        protected abstract string GetSetting(string name);
    }
}