using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Cors;
using System.Web.Helpers;
using System.Web.Http;
using IdentityManager.Configuration;
using IdentityManager.Core.Logging;
using IdentityManager.Logging;
using IdentityMinimal.Config;
using IdentityMinimal.IdentityManager;
using IdentityModel.Client;
using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Owin;
using AuthenticationOptions = IdentityServer3.Core.Configuration.AuthenticationOptions;

[assembly: OwinStartup(typeof(IdentityMinimal.Startup))]

namespace IdentityMinimal
{
    public class Startup
    {
        public readonly string iisUrl;

        public Startup()
        {
            //            iisUrl = "server2012.agriquem.com";
            //#if DEBUG
            //            iisUrl = "localhost";
            //#endif
            iisUrl = "localhost";

        }

        public void Configuration(IAppBuilder app)
        {
            //LogProvider.SetCurrentLogProvider(new DiagnosticsTraceLogProvider());

            //Log.Logger = new LoggerConfiguration()
            //   .MinimumLevel.Debug()
            //   .WriteTo.Trace()
            //   .CreateLogger();

            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            //json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
            //json.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.InboundClaimTypeMap.Clear(); //http://stackoverflow.com/questions/38080608/update-of-system-identitymodel-tokens-jwt-causing-breaking-change-in-identityser

            app.Map("/admin", adminApp =>
            {
                var factory = new IdentityManagerServiceFactory();

                factory.ConfigureSimpleIdentityManagerService("Identity");
                //factory.ConfigureCustomIdentityManagerServiceWithIntKeys("AspId_CustomPK");

                adminApp.UseIdentityManager(new IdentityManagerOptions()
                {
                    Factory = factory
                });
            });

            app.Map("/identity", idServiceApp =>
            {
                var idServiceFactory = Factory.Configure();
                idServiceFactory.ConfigureUserService("Identity");

                idServiceFactory.ViewService = new IdentityServer3.Core.Configuration.Registration<IViewService>(typeof(CustomViewService));
                idServiceFactory.CorsPolicyService = new IdentityServer3.Core.Configuration.Registration<ICorsPolicyService>(new DefaultCorsPolicyService { AllowAll = true });

                idServiceApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "IdentityMinimal - Identity Server",
                    IssuerUri = "https://"+ iisUrl + ":44337/aspidentity",
                    SigningCertificate = Certificate.Get(),
                    Factory = idServiceFactory,
                    AuthenticationOptions = new AuthenticationOptions
                    {
                        IdentityProviders = ConfigureAdditionalIdentityProviders,
                    },
                    LoggingOptions = new LoggingOptions
                    {
                        EnableHttpLogging = true,
                        EnableKatanaLogging = true,
                        WebApiDiagnosticsIsVerbose = true,
                        EnableWebApiDiagnostics = true
                    },
                    EventsOptions = new EventsOptions
                    {
                        RaiseFailureEvents = true,
                        RaiseInformationEvents = true,
                        RaiseSuccessEvents = true,
                        RaiseErrorEvents = true
                    }
                });
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });


            //app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            //{
            //    Authority = "https://" + iisUrl + ":44337/identity", //localhost

            //    ClientId = "mvc",
            //    Scope = "openid profile roles sampleApi",
            //    ResponseType = "id_token token",
            //    RedirectUri = "https://" + iisUrl + ":44337/identity",

            //    SignInAsAuthenticationType = "Cookies",
            //    UseTokenLifetime = false,

            //    Notifications = new OpenIdConnectAuthenticationNotifications
            //    {
            //        SecurityTokenValidated = async n =>
            //        {
            //            var nid = new ClaimsIdentity(
            //                n.AuthenticationTicket.Identity.AuthenticationType,
            //                Constants.ClaimTypes.GivenName,
            //                Constants.ClaimTypes.Role);
            //            var token = n.ProtocolMessage.AccessToken;
            //            // get userinfo data
            //            Uri someUri=new Uri(new Uri(n.Options.Authority + "/connect/userinfo"),n.ProtocolMessage.AccessToken);
                        
            //            var userInfoClient = new UserInfoClient(someUri.ToString());

            //            var userInfo = await userInfoClient.GetAsync(token);
            //            userInfo.Claims.ToList().ForEach(ui => nid.AddClaim(new Claim(ui.Type,ui.Value)));

            //            // keep the id_token for logout
            //            nid.AddClaim(new Claim("id_token", n.ProtocolMessage.IdToken));

            //            // add access token for sample API
            //            nid.AddClaim(new Claim("access_token", n.ProtocolMessage.AccessToken));

            //            // keep track of access token expiration
            //            nid.AddClaim(new Claim("expires_at",
            //                DateTimeOffset.Now.AddSeconds(int.Parse(n.ProtocolMessage.ExpiresIn)).ToString()));

            //            // add some other app specific claim
            //            nid.AddClaim(new Claim("app_specific", "some data"));

            //            n.AuthenticationTicket = new AuthenticationTicket(
            //                nid,
            //                n.AuthenticationTicket.Properties);
            //        },

            //        RedirectToIdentityProvider = n =>
            //        {
            //            if (n.ProtocolMessage.RequestType == OpenIdConnectRequestType.LogoutRequest)
            //            {
            //                var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");

            //                if (idTokenHint != null)
            //                {
            //                    n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
            //                }
            //            }

            //            return Task.FromResult(0);
            //        }
            //    }
            //});
        }

        public static void ConfigureAdditionalIdentityProviders(IAppBuilder app, string signInAsType)
        {
            var google = new GoogleOAuth2AuthenticationOptions
            {
                AuthenticationType = "Google",
                SignInAsAuthenticationType = signInAsType,
                ClientId = "767400843187-8boio83mb57ruogr9af9ut09fkg56b27.apps.googleusercontent.com",
                ClientSecret = "5fWcBT0udKY7_b6E3gEiJlze"
            };
            app.UseGoogleAuthentication(google);

            //var fb = new FacebookAuthenticationOptions
            //{
            //    AuthenticationType = "Facebook",
            //    SignInAsAuthenticationType = signInAsType,
            //    AppId = "676607329068058",
            //    AppSecret = "9d6ab75f921942e61fb43a9b1fc25c63"
            //};
            //app.UseFacebookAuthentication(fb);

            //var twitter = new TwitterAuthenticationOptions
            //{
            //    AuthenticationType = "Twitter",
            //    SignInAsAuthenticationType = signInAsType,
            //    ConsumerKey = "N8r8w7PIepwtZZwtH066kMlmq",
            //    ConsumerSecret = "df15L2x6kNI50E4PYcHS0ImBQlcGIt6huET8gQN41VFpUCwNjM"
            //};
            //app.UseTwitterAuthentication(twitter);
        }
    }
}