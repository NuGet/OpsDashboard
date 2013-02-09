using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Infrastructure;
using NuGetGallery.Dashboard.Services;

namespace NuGetGallery.Dashboard.Areas.SiteStatus.Controllers
{
    public class ErrorsController : BaseController
    {
        private ErrorLogService _errorLog;

        public ErrorsController(IConfigurationService config, ErrorLogService errorLog) : base(config) {
            _errorLog = errorLog;
        }

        public ActionResult List()
        {
            return List(0, 10);
        }

        public ActionResult List(int? pageIndex, int? pageSize)
        {
            pageIndex = pageIndex ?? 0;
            pageSize = pageSize ?? 10;

            // Fetch that page of data and display it
            var result = _errorLog.GetPage(pageIndex, pageSize);
            return View(result);
        }
    }
}