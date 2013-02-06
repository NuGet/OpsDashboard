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
        public UserAccount User { get; private set; }

        public LayoutModel(string loginUrl, UserSession user) : this(loginUrl, user == null ? null : user.User) {}

        public LayoutModel(string loginUrl, UserAccount user)
        {
            LoginUrl = loginUrl;
            User = user;
        }
    }
}