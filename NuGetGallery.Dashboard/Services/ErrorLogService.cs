using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Dashboard.Model;

namespace NuGetGallery.Dashboard.Services
{
    public abstract class ErrorLogService
    {
        public abstract PageView<ElmahError> GetPage(int? pageIndex,int? pageSize);
    }
}