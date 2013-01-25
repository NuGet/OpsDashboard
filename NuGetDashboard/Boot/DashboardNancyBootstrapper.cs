using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Ninject;
using Nancy.Responses.Negotiation;
using Ninject;

namespace NuGetDashboard.Boot
{
    public class DashboardNancyBootstrapper : NinjectNancyBootstrapper
    {
        private IKernel _kernel;

        public DashboardNancyBootstrapper(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override IKernel GetApplicationContainer()
        {
            return _kernel;
        }
    }
}
