using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using ShareTube.Models;
using System.Configuration;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace ShareTube
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);










            //disabled all auth - DJL 11/16/20s14






            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    Provider = new CookieAuthenticationProvider
            //    {
            //        OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
            //            validateInterval: TimeSpan.FromMinutes(30),
            //            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    }
            //});

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Uncomment the following lines to enable logging in with third party login providers
            ////app.UseMicrosoftAccountAuthentication(
            ////    clientId: "",
            ////    clientSecret: "");

            ////app.UseTwitterAuthentication(
            ////   consumerKey: "",
            ////   consumerSecret: "");

            ////app.UseFacebookAuthentication(
            ////   appId: "1443260015935777",
            ////   appSecret: "0daa73a7bcb510b291cf8f19d74589be");

            ////app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            ////{
            ////    ClientId = ConfigurationManager.AppSettings["google-ouath-clientid"],
            ////    ClientSecret = ConfigurationManager.AppSettings["google-ouath-secret"]
            ////});
            //var googleCreds = new GoogleOAuth2AuthenticationOptions
            //{
            //    ClientId = ConfigurationManager.AppSettings["google-ouath-clientid"],
            //    ClientSecret = ConfigurationManager.AppSettings["google-ouath-secret"],

            //    Provider = new GoogleOAuth2AuthenticationProvider
            //    {
            //        OnApplyRedirect = context =>
            //        {
            //            string redirect = context.RedirectUri;
            //            redirect += "&access_type=offline";
            //            redirect += "&approval_prompt=force";
            //            redirect += "&include_granted_scopes=true";

            //            context.Response.Redirect(redirect);

            //        },
            //        OnAuthenticated = context =>
            //        {
            //            TimeSpan expiryDuration = context.ExpiresIn ?? new TimeSpan();
            //            context.Identity.AddClaim(new Claim("urn:tokens:google:email", context.Email));
            //            context.Identity.AddClaim(new Claim("urn:tokens:google:url", context.GivenName));
            //            if (!string.IsNullOrEmpty(context.RefreshToken))
            //            {
            //                context.Identity.AddClaim(new Claim("urn:tokens:google:refreshtoken", context.RefreshToken));
            //            }
            //            context.Identity.AddClaim(new Claim("urn:tokens:google:accesstoken", context.AccessToken));
            //            if (context.User.GetValue("hd") != null)
            //            {

            //                context.Identity.AddClaim(new Claim("urn:tokens:google:hd", context.User.GetValue("hd").ToString()));
            //            }
            //            context.Identity.AddClaim(new Claim("urn:tokens:google:accesstokenexpiry", DateTime.UtcNow.Add(expiryDuration).ToString()));

            //            return System.Threading.Tasks.Task.FromResult<object>(null);
            //        }
            //    }
            //};
            //googleCreds.Scope.Add("openid");
            //googleCreds.Scope.Add("email");

            //app.UseGoogleAuthentication(googleCreds);
        }
    }
}