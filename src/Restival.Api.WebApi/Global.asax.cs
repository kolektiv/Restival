﻿using System.Web;
using System.Web.Http;

namespace Restival.Api.WebApi {
    public class WebApiApplication : HttpApplication {
        protected void Application_Start() {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}