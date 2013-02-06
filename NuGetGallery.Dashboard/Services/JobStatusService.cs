using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class JobStatusService
    {
        public abstract string GetJobStatusJson();
    }
}