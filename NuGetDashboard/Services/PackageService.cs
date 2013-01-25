using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGetDashboard.Model;

namespace NuGetDashboard.Services
{
    public abstract class PackageService
    {
        public abstract IEnumerable<PackageFile> GetAll();
    }
}
