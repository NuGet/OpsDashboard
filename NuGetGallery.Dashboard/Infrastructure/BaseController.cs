using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Services;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Infrastructure
{
    public abstract class BaseController : Controller
    {
        public ConfigurationService Configuration { get; private set; }

        protected BaseController(ConfigurationService configuration)
        {
            Configuration = configuration;
        }

        protected override ViewResult View(IView view, object model)
        {
            return base.View(view, EnsureModelFitsLayout(model));
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            return base.View(viewName, masterName, EnsureModelFitsLayout(model));
        }

        private object EnsureModelFitsLayout(object model)
        {
            if (model == null)
            {
                // Just use an empty layout model
                model = new LayoutModel();
            }

            // Make sure the model is a subclass of LayoutModel
            LayoutModel baseModel = model as LayoutModel;
            if (baseModel == null)
            {
                throw new InvalidCastException("Models must be sub-classes of LayoutModel");
            }

            // Set common layout properties
            baseModel.LoginUrl = String.Format(Configuration.LoginUrlFormat, Configuration.AuthenticationRealm);
            baseModel.User = HttpContext.User.AsUserSession();
            return model;
        }
    }
}