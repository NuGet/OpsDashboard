using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class EnvironmentConfiguration
    {
        public string DiagnosticsStore { get { return GetSetting("DiagnosticsStore"); } }

        protected abstract string GetSetting(string name);
    }
}
