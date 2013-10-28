using NuGetDashboard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NuGetDashboard.Controllers.Diagnostics
{
    /// <summary>
    /// Provides details about the resource utilization on the server : CPU, memory, Disk, DB.
    /// </summary>
    public class ResourceMonitoringController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("~/Views/ResourceMonitoring/ResourceMonitoring_Index.cshtml");
        }
        
        [HttpGet]
        public ActionResult Now()
        {
            ViewBag.ControllerName = "ResourceMonitoring";
            return PartialView("~/Views/ResourceMonitoring/ResourceMonitoring_Now.cshtml");
        }

        [HttpGet]
        public ActionResult Details()
        {
            return PartialView("~/Views/Shared/PartialFrames.cshtml");
        }

        [HttpGet]
        public JsonResult DownloadLog()
        {
            return Json(BlobStorageService.DownloadLatest("wad-iis-requestlogs") , JsonRequestBehavior.AllowGet);
        }

    }
}
