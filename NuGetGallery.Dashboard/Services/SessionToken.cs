using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Services
{
    public class SessionToken
    {
        public UserAccount User { get; private set; }
        public DateTime ExpiresUtc { get; set; }

        public SessionToken(UserAccount user, DateTime expiresUtc)
        {
            User = user;
            ExpiresUtc = expiresUtc;
        }
    }
}
