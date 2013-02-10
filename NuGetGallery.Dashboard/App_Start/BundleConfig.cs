using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace NuGetGallery.Dashboard
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/lib/script")
                .Include("~/Scripts/lib/jquery-{version}.js")
                .Include("~/Scripts/lib/jquery.timeago.js")
                .Include("~/Scripts/lib/knockout-{version}.js")
                .Include("~/Scripts/lib/underscore.js")
                .Include("~/Scripts/lib/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/css/styles")
                .Include("~/Content/css/bootstrap.css")
                .Include("~/Content/css/bootstrap-responsive.css")
                .Include("~/Content/css/font-awesome.css")
                .Include("~/Content/css/Master.css"));
        }
    }
}