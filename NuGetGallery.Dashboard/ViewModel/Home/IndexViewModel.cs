using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.ViewModel.Home
{
    public class IndexViewModel : LayoutModel
    {
        public IList<EnvironmentViewModel> Environments { get; private set; }

        public IndexViewModel(IEnumerable<EnvironmentViewModel> environments)
        {
            Environments = environments.ToList();
        }
    }
}