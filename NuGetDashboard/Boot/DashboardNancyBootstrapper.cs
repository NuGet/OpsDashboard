using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Cookies;
using Nancy.Responses.Negotiation;
using Ninject;
using NuGetDashboard.Infrastructure;
using NuGetDashboard.Services;

namespace NuGetDashboard.Boot
{
    public class DashboardNancyBootstrapper : NinjectNancyBootstrapper
    {
        private IKernel _kernel;

        public DashboardNancyBootstrapper(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override IKernel GetApplicationContainer()
        {
            return _kernel;
        }

        protected override void ApplicationStartup(IKernel container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            // Get the Authentication Service
            var authService = container.Get<AuthenticationService>();

            // Attach to the Auth Pipeline
            pipelines.BeforeRequest.AddItemToEndOfPipeline(ctx =>
            {
                // Check for a token
                string token = null;
                if (!String.IsNullOrEmpty(ctx.Request.Headers.Authorization))
                {
                    // There's an authorization header, it's there
                    string[] splitted = ctx.Request.Headers.Authorization.Split(' ');
                    if (splitted.Length == 2 && String.Equals(splitted[0], "Token"))
                    {
                        token = splitted[1];
                    }
                }
                else if (ctx.Request.Cookies.ContainsKey("Auth"))
                {
                    ctx.Items["IssueAuthCookie"] = true;
                    token = ctx.Request.Cookies["Auth"];
                }

                if (!String.IsNullOrEmpty(token))
                {
                    SessionToken parsedToken;
                    try
                    {
                        parsedToken = authService.DecodeSessionToken(WebUtility.UrlDecode(token));
                    }
                    catch (Exception)
                    {
                        var resp = new Response();
                        resp.StatusCode = Nancy.HttpStatusCode.Unauthorized;
                        return resp;
                    }

                    // Create a user
                    ctx.CurrentUser = new DashboardUserIdentity(parsedToken);
                }
                return null;
            });

            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                if (ctx.Items.ContainsKey("IssueAuthCookie") && (bool)ctx.Items["IssueAuthCookie"])
                {
                    if (ctx.CurrentUser == null)
                    {
                        // Issue an empty, expired, cookie to clear it
                        ctx.Response.AddCookie(new NancyCookie("Auth", String.Empty, httpOnly: true) { Expires = DateTime.UtcNow.AddDays(-1) });
                    }
                    else
                    {
                        // Reissue the cookie
                        var token = ((DashboardUserIdentity)ctx.CurrentUser).Token;
                        string tokenString = authService.EncodeSessionToken(token, renew: true);
                        ctx.Response.AddCookie(new NancyCookie("Auth", tokenString, httpOnly: true) { Expires = token.ExpiresUtc });
                    }
                }
            });
        }
    }
}
