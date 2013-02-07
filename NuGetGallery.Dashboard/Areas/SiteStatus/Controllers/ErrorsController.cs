using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Infrastructure;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.Areas.SiteStatus.Controllers
{
    public class ErrorsController : BaseController
    {
        public ErrorsController(ConfigurationService config) : base(config) { }

        public ActionResult List()
        {
            return View();
        }
    }
}