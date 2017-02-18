using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using IdentityMinimal.Identities;
using IdentityServer3.AspNetIdentity;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
//using Katarina.IdentityServer.Identities;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace IdentityMinimal.Config
{
    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory, string connString)
        {
            //factory.UserService = new Registration<IUserService, UserService>();
            factory.Register(new Registration<IdentityContext>(resolver => new IdentityContext(connString)));
            factory.Register(new Registration<UserStore>());
            factory.Register(new Registration<RoleStore>());
            factory.Register(new Registration<UserManager>());
            factory.Register(new Registration<RoleManager>());

            factory.UserService = new Registration<IUserService, UserService>();
        }
    }

    public class UserService : AspNetIdentityUserService<User, string>
    {
        public UserService(UserManager userMgr)
            : base(userMgr)
        {

        }

        //public override async Task GetProfileDataAsync(ClaimsPrincipal subject, IEnumerable requestedClaimTypes = null)
        //{

        //}

        protected override async Task<IEnumerable<System.Security.Claims.Claim>> GetClaimsFromAccount(User user)
        {
            var claims = (await base.GetClaimsFromAccount(user)).ToList();
            
            //var prueba = userGroups.ApplicationUsersMapping.First().ApplicationUser.FirstName;

            if (!String.IsNullOrWhiteSpace(user.FirstName))
            {
                claims.Add(new Claim("given_name", user.FirstName));
            }
            if (!String.IsNullOrWhiteSpace(user.LastName))
            {
                claims.Add(new Claim("family_name", user.LastName));
            }
            if (!String.IsNullOrWhiteSpace(user.FullName))
            {
                claims.Add(new Claim("full_name", user.FullName));
            }

            return claims;
        }
    }

    public static class UserServiceFactory
    {
        public static AspNetIdentityUserService<User, string> Create()
        {
            var context = new IdentityContext("Identity");
            var userStore = new UserStore(context);
            var userManager = new UserManager(userStore);

            return new UserService(userManager);
        }
    }
}