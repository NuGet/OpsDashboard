using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Infrastructure
{
    public class ApiAwareWSFederationAuthenticationModule : WSFederationAuthenticationModule
    {
        protected override void OnAuthorizationFailed(AuthorizationFailedEventArgs e)
        {
            base.OnAuthorizationFailed(e);
            e.RedirectToIdentityProvider = e.RedirectToIdentityProvider && !IsApi();
        }

        private bool IsApi()
        {
            if (HttpContext.Current == null)
            {
                return false;
            }
            string fullPath = HttpContext.Current.Request.Url.AbsolutePath;
            string appRoot = HttpContext.Current.Request.ApplicationPath;
            return fullPath.StartsWith(appRoot.TrimEnd('/') + "/api");
        }
    }
}