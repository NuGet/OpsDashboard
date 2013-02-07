using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Infrastructure;
using NuGetGallery.Dashboard.Services;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ConfigurationService configuration) : base(configuration) {}

        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
    }
}
