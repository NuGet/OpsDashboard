using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetDashboard.Services;

namespace NuGetDashboard.Modules
{
    public class MainModule : Nancy.NancyModule
    {
        public MainModule(PackageService packages, JobStatusService jobStatus)
        {
            Get["/api/v1/packages"] = _ => 
                // Get a list of packages from blob storage
                packages.GetAll();

            Get["/api/v1/jobs"] = _ =>
                // Get jobs status json
                jobStatus.GetJobStatusJson();
        }
    }
}