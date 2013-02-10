using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.ViewModel.Environments
{
    public class ShowViewModel : LayoutModel
    {
        public string EnvironmentName { get; set; }
        public string EnvironmentTitle { get; set; }
    }
}