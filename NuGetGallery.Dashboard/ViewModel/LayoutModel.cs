using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.ViewModel
{
    public class LayoutModel
    {
        public string LoginUrl { get; set; }
        public UserSession User { get; set; }
        public bool IsAdmin { get { return User != null && User.IsAdmin; } }
        public bool? LastKnownServiceState { get; set; }

        public LayoutModel()
        {
        }

        public virtual object GetClientModel()
        {
            return null;
        }
    }
}