using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityMinimal.Identities
{
    public class User : IdentityUser
    {
        public User() : base()
        {
            //this.Groups = new HashSet<ApplicationGroup>();
        }

        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(UserManager manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName).Trim(); }
        }
        //public virtual ICollection<ApplicationGroup> Groups { get; set; }
    }

    public class UserStore : UserStore<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public UserStore(IdentityContext ctx)
            : base(ctx)
        {
        }
    }


    public class UserManager : UserManager<User, string>
    {
        public UserManager(UserStore store)
            : base(store)
        {
            this.ClaimsIdentityFactory = new ClaimsFactory();
        }

        //public UserManager(UserStore<User, Role, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim> store)
        //    : base(store)
        //{
        //    this.ClaimsIdentityFactory = new ClaimsFactory();
        //}


        public async Task<User> FindByNameOrEmailAsync(string usernameOrEmail, string password)
        {
            if (usernameOrEmail.Contains("@"))
            {
                return await FindByEmailAsync(usernameOrEmail);
            }

            return await FindAsync(usernameOrEmail, password);
        }

        public async Task<User> FindByNameOrEmailAsync(string usernameOrEmail)
        {
            User userData = null;

            if (usernameOrEmail.Contains("@"))
            {
                userData = await FindByEmailAsync(usernameOrEmail);
            }
            else
            {
                userData = await FindByNameAsync(usernameOrEmail);
            }

            return userData;
        }
    }
}