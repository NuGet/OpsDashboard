using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NuGetDashboard.Utilities;

namespace NuGetDashboard.Controllers.LiveSiteMonitoring
{
    /// <summary>
    /// Provides details about the server side SLA : Error rate, throughout, Response time.
    /// </summary>
    public class SLAController : Controller
    {
        public ActionResult Index()
        {            
            return PartialView("~/Views/SLA/SLA_Index.cshtml" );
        }

        [HttpGet]
        public ActionResult Now()
        {
            return PartialView("~/Views/SLA/SLA_Now.cshtml");
        }

        [HttpGet]
        public ActionResult Details()
        {
            return PartialView("~/Views/SLA/SLA_Details.cshtml");
        }
        
        [HttpGet]
        public JsonResult GetStatus()
        {
            List<threshold_value> metrics = NewRelicHelper.GetLatestMetrics();
            return Json(metrics, JsonRequestBehavior.AllowGet);
        }
    }
}
