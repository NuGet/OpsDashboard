using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using NuGetGallery.Dashboard.Configuration;
using NuGetGallery.Dashboard.Services;
using NuGetGallery.Dashboard.ViewModel;

namespace NuGetGallery.Dashboard.Infrastructure
{
    public abstract class BaseApiController : ApiController
    {
        public IConfigurationService ConfigService { get; private set; }

        protected BaseApiController(IConfigurationService configuration)
        {
            ConfigService = configuration;
        }

        protected HttpResponseException NotFound()
        {
            return new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}