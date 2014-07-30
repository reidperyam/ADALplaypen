using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.ActiveDirectory;
using System;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = "https://developmenttesting.onmicrosoft.com/ADALplaypenAPI",
                    Tenant = "developmenttesting.onmicrosoft.com"
                });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.EnableSystemDiagnosticsTracing();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            app.UseWebApi(config);
        }
    }
}
