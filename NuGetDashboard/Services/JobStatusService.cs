using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuGetDashboard.Services
{
    public abstract class JobStatusService
    {
        public abstract string GetJobStatusJson();
    }
}