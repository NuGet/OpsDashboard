using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class AuthenticationService
    {
        public abstract UserAccount Login(string token);
    }
}