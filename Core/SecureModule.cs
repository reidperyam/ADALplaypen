namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Web;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OpenIdConnect;
    using Nancy;
    using Nancy.Owin;
    using Nancy.Responses;
    using Nancy.Security;

    public class SecureModule : NancyModule
    {
        public SecureModule()
        {
            Before += ctx =>
            {

                IAuthenticationManager authenticationManager = Context.GetAuthenticationManager();
                authenticationManager.Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectAuthenticationDefaults.AuthenticationType);
                return null;
            };

            this.RequiresMSOwinAuthentication();

            Get["/"] = _ =>
            {
                IAuthenticationManager authenticationManager = Context.GetAuthenticationManager();
                return authenticationManager.User.Identity.Name;
            };

            Get["/diagnostics"] = _ =>
            {
                return Negotiate.WithView("diagnostics");
            };
        }
    }
}