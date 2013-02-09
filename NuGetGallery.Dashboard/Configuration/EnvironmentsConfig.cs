using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NuGetGallery.Dashboard.Configuration
{
    public class EnvironmentsConfig
    {
        private readonly IDictionary<string, DeploymentEnvironment> _environments;

        public IDictionary<string, DeploymentEnvironment> Environments { get { return _environments; } }

        public EnvironmentsConfig() : this(new Dictionary<string, DeploymentEnvironment>(StringComparer.OrdinalIgnoreCase)) { }
        public EnvironmentsConfig(IDictionary<string, DeploymentEnvironment> environments)
        {
            _environments = environments;
        }
    }
}
