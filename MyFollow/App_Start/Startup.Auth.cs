using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MyFollow.DAL;
using Owin;

namespace MyFollow
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(MyFollowContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, int>(
                    validateInterval: TimeSpan.FromMinutes(30),
                    regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                    getUserIdCallback: (claim) => int.Parse(claim.GetUserId()))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

          
            app.UseFacebookAuthentication(
               appId: "946468195376138",
               appSecret: "e327c6fcd1540a1e215c5b14d3684b11");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "1015938086259-kc2u0pt7etjp91rfv1mfemtceb8nlng0.apps.googleusercontent.com",
            //    ClientSecret = "bfFVTrn60mcwHmKRQGFXze3n"
            //});
        }
    }
}