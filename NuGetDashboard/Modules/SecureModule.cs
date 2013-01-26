using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Security;
using NuGetDashboard.Services;

namespace NuGetDashboard.Modules
{
    public class SecureModule : NancyModule
    {
        public SecureModule(PackageService packages, JobStatusService jobStatus)
        {
            this.RequiresAuthentication();
            
            Get["/api/v1/packages"] = _ =>
                // Get a list of packages from blob storage
                packages.GetAll();

            Get["/api/v1/jobs"] = _ =>
                // Get jobs status json
                jobStatus.GetJobStatusJson();
        }
    }
}