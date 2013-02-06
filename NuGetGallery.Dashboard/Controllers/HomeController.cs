using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Services;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ConfigurationService _configuration;
        private readonly AuthenticationService _auth;

        public HomeController(ConfigurationService configuration, AuthenticationService auth)
        {
            _configuration = configuration;
            _auth = auth;
        }

        //
        // GET: /Home/
        [HttpGet]
        [ActionName("Index")]
        public ActionResult GetIndex()
        {
            return View(new LayoutModel(_configuration.LoginPageUrl, HttpContext.User as UserSession));
        }

        [HttpPost]
        [ActionName("Index")]
        [ValidateInput(false)]
        public ActionResult PostIndex(string wresult)
        {
            // Parse a JWT token
            var user = _auth.ProcessRecievedToken(wresult);

            // Issue a session token and set it as the current user
            var session = _auth.IssueSessionToken(user);
            HttpContext.User = session;

            // Run the get code
            return GetIndex();
        }
    }
}
