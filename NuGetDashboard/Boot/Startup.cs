using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Hosting.Owin;
using Nancy.Bootstrappers.Ninject;
using Ninject;
using Ninject.Modules;
using Owin;
using NuGetDashboard.Boot;

namespace NuGetDashboard
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Create Ninject Kernel
            var kernel = new StandardKernel(new NinjectModule[] { new FactoryModule(), new AppModule() });

            // Attach Middleware
            app.UseShowExceptions();
            
            // Attach Nancy
            var bootstrapper = new DashboardNancyBootstrapper(kernel);
            app.UseNancy(bootstrapper);
        }
    }
}