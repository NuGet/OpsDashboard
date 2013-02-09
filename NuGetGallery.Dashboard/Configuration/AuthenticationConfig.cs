using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NuGetGallery.Dashboard.Configuration
{
    public class AuthenticationConfig
    {
        public virtual string LoginUrlFormat { get; set; }
        public virtual string AudienceUrl { get; set; }
        public virtual string AuthenticationRealm { get; set; }
        public virtual string AuthenticationIssuer { get; set; }
        public virtual string TokenCertificateThumbprint { get; set; }

        [JsonIgnore]
        public string LoginUrl { get { return String.Format(LoginUrlFormat, AuthenticationRealm); } }
    }
}