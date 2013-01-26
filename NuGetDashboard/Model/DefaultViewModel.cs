using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetDashboard.Services;

namespace NuGetDashboard.Model
{
    public class DefaultViewModel
    {
        public ConfigurationService Configuration { get; private set; }
        public UserAccount User { get; private set; }

        public DefaultViewModel(ConfigurationService configuration, UserAccount user)
        {
            Configuration = configuration;
            User = user;
        }
    }
}