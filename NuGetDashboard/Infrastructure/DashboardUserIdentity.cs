using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using NuGetDashboard.Services;

namespace NuGetDashboard.Infrastructure
{
    public class DashboardUserIdentity : IUserIdentity
    {
        public static readonly string AdminClaim = "https://claims.nuget.org/admin";

        public SessionToken Token { get; private set; }
        public IEnumerable<string> Claims { get; set; }
        public string UserName { get; set; }

        public DashboardUserIdentity(SessionToken token)
        {
            Token = token;
            Claims = new[] { AdminClaim };
            UserName = token.User.UserName;
        }
    }
}