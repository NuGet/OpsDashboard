using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Services;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConfigurationService _configuration;

        public HomeController(ConfigurationService configuration)
        {
            _configuration = configuration;
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View(new LayoutModel(_configuration.LoginUrl, HttpContext.User.AsUserSession()));
        }
    }
}
