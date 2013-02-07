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

        public LayoutModel()
        {
        }
    }
}