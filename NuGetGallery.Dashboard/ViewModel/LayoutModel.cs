using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.ViewModel
{
    public class LayoutModel
    {
        public string LoginUrl { get; private set; }
        public UserSession User { get; private set; }

        public LayoutModel(string loginUrl, UserSession user)
        {
            LoginUrl = loginUrl;
            User = user;
        }
    }
}