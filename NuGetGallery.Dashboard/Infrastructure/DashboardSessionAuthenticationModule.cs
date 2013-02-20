using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Infrastructure
{
    public class DashboardSessionAuthenticationModule : SessionAuthenticationModule
    {
        protected override void OnAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            base.OnAuthenticateRequest(sender, eventArgs);
        }

        protected override void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            base.OnPostAuthenticateRequest(sender, e);
        }
    }
}