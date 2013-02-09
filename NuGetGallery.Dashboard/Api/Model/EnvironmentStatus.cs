using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Api.Model
{
    public class EnvironmentStatus
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? AccessibleFromDashboard { get; set; }
        public Uri Url { get; set; }
    }
}