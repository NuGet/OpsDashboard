using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.Api.Model
{
    public class EnvironmentStatus
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
        public IEnumerable<PingResult> PingResults { get; set; }
    }
}