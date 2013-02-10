using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Infrastructure;
using NuGetGallery.Dashboard.Model;
using NuGetGallery.Dashboard.ViewModel.Environments;

namespace NuGetGallery.Dashboard.Controllers
{
    public class EnvironmentsController : BaseController
    {
        public EnvironmentsController(IConfigurationService configuration) : base(configuration) { }

        public ActionResult Show(string name)
        {
            DeploymentEnvironment env;
            if (!Configuration.Environments.TryGetValue(name, out env))
            {
                return HttpNotFound();
            }

            return View(new ShowViewModel()
            {
                EnvironmentName = env.Name,
                EnvironmentTitle = env.Title
            });
        }
    }
}