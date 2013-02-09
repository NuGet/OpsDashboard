using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Infrastructure;
using NuGetGallery.Dashboard.Services;
using NuGetGallery.Dashboard.ViewModel;
using NuGetGallery.Dashboard.ViewModel.Home;

namespace NuGetGallery.Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IConfigurationService configuration) : base(configuration) {}

        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View(new IndexViewModel(Configuration.Environments.Values.Select(e => new EnvironmentViewModel() {
                Name = e.Name,
                Description = e.Description,
                Url = e.Url
            })));
        }
    }
}
