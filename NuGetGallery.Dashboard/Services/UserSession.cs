using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Services
{
    public class UserSession : IPrincipal, IIdentity
    {
        public UserAccount User { get; private set; }
        public DateTime ExpiresUtc { get; set; }

        public UserSession(UserAccount user, DateTime expiresUtc)
        {
            User = user;
            ExpiresUtc = expiresUtc;
        }

        public IIdentity Identity
        {
            get { return this; }
        }

        public bool IsInRole(string role)
        {
            return false;
        }

        public string AuthenticationType
        {
            get { return "Session"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return User.UserName; }
        }
    }
}
