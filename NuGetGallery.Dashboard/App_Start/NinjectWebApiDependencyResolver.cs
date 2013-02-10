using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.Dependencies;
using Ninject;
using Ninject.Syntax;

namespace NuGetGallery.Dashboard.App_Start
{
    public class NinjectWebApiDependencyResolver : NinjectWebApiDependencyScope, IDependencyResolver
    {
        private IKernel _kernel;
        public NinjectWebApiDependencyResolver(IKernel kernel) : base(kernel) {
            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectWebApiDependencyScope(_kernel);
        }
    }

    public class NinjectWebApiDependencyScope : IDependencyScope
    {
        private IResolutionRoot _root;

        public NinjectWebApiDependencyScope(IResolutionRoot root)
        {
            _root = root;
        }

        public object GetService(Type serviceType)
        {
            return _root.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _root.GetAll(serviceType);
        }

        public void Dispose()
        {
        }
    }
}
