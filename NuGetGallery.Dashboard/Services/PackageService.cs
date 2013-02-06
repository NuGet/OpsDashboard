using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class PackageService
    {
        public abstract IEnumerable<PackageFile> GetAll();
    }
}
