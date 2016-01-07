using Owin;
using Microsoft.Owin;
using SportsStore.Infrastructure.Identity;
using System;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(SportsStore.IdentityConfig))]

namespace SportsStore
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<StoreIdentityDbContext>(StoreIdentityDbContext.Create);
            app.CreatePerOwinContext<StoreUserManager>(StoreUserManager.Create);
            app.CreatePerOwinContext<StoreRoleManager>(StoreRoleManager.Create);
            //app.UseCookieAuthentication(new CookieAuthenticationOptions { AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie });

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions();
            options.Provider = new StoreAuthProvider();
            options.AllowInsecureHttp = true;
            options.TokenEndpointPath = new PathString("/Authenticate");

            app.UseOAuthBearerTokens(options);
        }
    }
}