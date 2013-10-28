using DotNet.Highcharts;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using NuGetDashboard.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NuGetDashboard.Controllers.Trending
{
    public class TrendingController : Controller
    {
        //
        // GET: /Trending/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details()
        {
            return PartialView("~/Views/Trending/Trending_Details.cshtml");
        }

        public ActionResult Monthly()
        {
            return PartialView("~/Views/Trending/Trending_Monthly.cshtml");
        }

        public ActionResult Daily()
        {
            return PartialView("~/Views/Trending/Trending_Details.cshtml");
        }

        //Returns the Net trend chart for packages 
        public ActionResult PackagesChart()
        {
            DotNet.Highcharts.Highcharts chart = GetChart("UploadsoctoberMonthlyReport.json", "Packages");
            return PartialView("~/Views/Shared/PartialChart.cshtml", chart);
        }

        //Returns the Net trend chart for Downloads
        public ActionResult DownloadsChart()
        {
            DotNet.Highcharts.Highcharts chart = GetChart("DownloadsoctoberMonthlyReport.json", "Downloads");
            return PartialView("~/Views/Shared/PartialChart.cshtml", chart);
        }

        //Returns the Net trend chart for Downloads
        public ActionResult UsersChart()
        {
            DotNet.Highcharts.Highcharts chart = GetChart("UsersoctoberMonthlyReport.json", "Users");
            return PartialView("~/Views/Shared/PartialChart.cshtml", chart);
        }

        private static Highcharts GetChart(string blobName, string chartName)
        {
            List<string> xValues = new List<string>();
            List<Object> yValues = new List<Object>();
            BlobStorageService.GetJsonDataFromBlob(blobName, out xValues, out yValues);

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts(chartName);
            chart.SetXAxis(new XAxis
            {
                Categories = xValues.ToArray(),
            });
            chart.SetSeries(new DotNet.Highcharts.Options.Series
            {
                Data = new Data(yValues.ToArray()),
                Name = chartName
            });
            chart.SetTitle(new DotNet.Highcharts.Options.Title { Text = chartName });
            return chart;
        }



    }
}
