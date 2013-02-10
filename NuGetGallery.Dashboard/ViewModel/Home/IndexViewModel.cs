using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.ViewModel.Home
{
    public class IndexViewModel : LayoutModel
    {
        public string DetailUrlTemplate { get; private set; }

        public IndexViewModel(string detailUrlTemplate)
        {
            DetailUrlTemplate = detailUrlTemplate;
        }

        public override object GetClientModel()
        {
            return new { detailsUrlTemplate = DetailUrlTemplate };
        }
    }
}