using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityMinimal.Identities
{
    public class Role : IdentityRole
    {
        public Role() : base() { }
        public Role(string name) : base(name) { }
        public Role(string name, string description)
            : base(name)
        {
            Description = description;
            //this.GroupsMapping = new HashSet<ApplicationUsersGroups>();
        }

        public string Description { get; set; }
        //public virtual ICollection<ApplicationUsersGroups> GroupsMapping { get; set; }

    }

    public class RoleStore : RoleStore<Role>
    {
        public RoleStore(IdentityContext ctx)
            : base(ctx)
        {
        }
    }

    public class RoleManager : RoleManager<Role>
    {
        public RoleManager(RoleStore store)
            : base(store)
        {
        }
        //public RoleManager(IRoleStore<Role, string> store)
        //    : base(store)
        //{
        //}
    }
}