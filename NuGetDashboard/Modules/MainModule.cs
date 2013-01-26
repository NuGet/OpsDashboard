using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using Newtonsoft.Json;
using NuGetDashboard.Infrastructure;
using NuGetDashboard.Model;
using NuGetDashboard.Services;

namespace NuGetDashboard.Modules
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule(AuthenticationService authService, ConfigurationService configuration)
        {
            Post["/"] = _ =>
            {
                // Get the token
                string token = Request.Form["wresult"];
                if (String.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("No token response!");
                }

                // Process the token
                var userAccount = authService.ProcessRecievedToken(token);

                // Issue a session token
                var sessionToken = authService.IssueSessionToken(userAccount);
                Context.CurrentUser = new DashboardUserIdentity(sessionToken);
                Context.Items["IssueAuthCookie"] = true;

                // Render the main view
                return View["Default", new DefaultViewModel(configuration, userAccount)];
            };

            Get["/(.*)"] = _ =>
            {
                // Check for a user
                UserAccount acct = null;
                if (Context.CurrentUser != null)
                {
                    var id = (DashboardUserIdentity)Context.CurrentUser;
                    acct = id.Token.User;
                }

                // Just show the app
                return View["Default", new DefaultViewModel(configuration, acct)];
            };
        }
    }
}