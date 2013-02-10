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
            bundles.Add(new ScriptBundle("~/Scripts/lib/script").IncludeDirectory("~/Scripts/lib", "*.js"));
            bundles.Add(new StyleBundle("~/Content/css/styles").IncludeDirectory("~/Content/css", "*.css"));

        }
    }
}