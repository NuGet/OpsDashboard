using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.ViewModel.Home
{
    public class EnvironmentViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri Url { get; set; }
    }
}
