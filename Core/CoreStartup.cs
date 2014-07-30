namespace Core
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Net;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.Owin.Extensions;
    using Microsoft.Owin.FileSystems;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.OpenIdConnect;
    using Microsoft.Owin.StaticFiles;
    using Owin;

    public class Startup
    {
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The App Key is a credential used to authenticate the application to Azure AD.  Azure AD supports password and certificate credentials.
        // The Metadata Address is used by the application to retrieve the signing keys used by Azure AD.
        // The AAD Instance is the instance of Azure, for example public Azure or Azure China.
        // The Authority is the sign-in URL of the tenant.
        // The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        //
        private static string clientId              = "23377903-9168-4f13-86ed-761cdaa6a94d";
        private static string appKey                = "uLakJEalBYYWVrG+QDweadDd1zpF45ZCBKlu7zY9eLM=";
        private static string aadInstance           = "https://login.windows.net/{0}";
        private static string tenant                = "developmenttesting.onmicrosoft.com";

        public static readonly string Authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);

        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
                {
                    AuthenticationMode = AuthenticationMode.Active                  
                });
         
            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = Authority,
                    
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {            
                        //
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        //
                        AuthorizationCodeReceived = (context) =>
                        {
                            var code = context.Code;
                            context.Code = code;
                            ClientCredential credential = new ClientCredential(clientId, appKey);
                            AuthenticationContext authContext = new AuthenticationContext(Authority);
                            AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(code, new Uri("https://localhost:44310/home"), credential);
                            context.AuthenticationTicket.Identity.AddClaim(new Claim("accessToken", result.AccessToken));

                            return Task.FromResult(0);
                        }
                    }
                });
            app.UseStaticFiles(new StaticFileOptions
            { 
                ServeUnknownFileTypes = true,
                FileSystem = new EmbeddedResourceFileSystem(typeof(UI.Hooker).Assembly, "UI")
            });
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler); // http://goo.gl/XrYGMh
        }
    }
}
