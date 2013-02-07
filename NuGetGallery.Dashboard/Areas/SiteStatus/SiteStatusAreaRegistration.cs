using System.Web.Mvc;

namespace NuGetGallery.Dashboard.Areas.SiteStatus
{
    public class SiteStatusAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SiteStatus";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SiteStatus_default",
                "SiteStatus/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
