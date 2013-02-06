using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Services
{
    public class UserSession
    {
        private ClaimsPrincipal _principal;

        public bool Authenticated { get { return _principal != null && _principal.Identity != null && _principal.Identity.IsAuthenticated; } }
        public string DisplayName { get; private set; }

        public UserSession(ClaimsPrincipal principal)
        {
            _principal = principal;

            if (_principal != null)
            {
                LoadProperties();
            }
        }

        private void LoadProperties()
        {
            DisplayName = String.Format("{0} {1}",
                _principal.ClaimValue(ClaimTypes.GivenName),
                _principal.ClaimValue(ClaimTypes.Surname));

        }
    }

    public static class PrincipalExtensions
    {
        public static UserSession AsUserSession(this IPrincipal self)
        {
            return new UserSession(self as ClaimsPrincipal);
        }

        public static string ClaimValue(this ClaimsPrincipal self, string claimUrl)
        {
            if (self == null)
            {
                return null;
            }
            Claim claim = self.Claims.Where(c => String.Equals(c.Type, claimUrl, StringComparison.Ordinal)).FirstOrDefault();
            if (claim == null)
            {
                return null;
            }
            return claim.Value;
        }
    }
}
