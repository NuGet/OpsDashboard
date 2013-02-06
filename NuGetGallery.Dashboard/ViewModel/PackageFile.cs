using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.ViewModel
{
    public class PackageFile
    {
        public Uri Url { get; set; }
        public string CommitHash { get; set; }
        public string Branch { get; set; }
    }
}