using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Homwe/

        public ActionResult Index()
        {
            return View(new LayoutModel("http://microsoft.com", null));
        }

    }
}
