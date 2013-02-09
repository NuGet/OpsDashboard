using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NuGetGallery.Dashboard
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
