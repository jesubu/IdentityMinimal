using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;

namespace IdentityMinimal.Config
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new Scope[]
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                StandardScopes.OfflineAccess,
                StandardScopes.Roles,
                new Scope
                {
                    Name = "agqusers",
                    Type = ScopeType.Identity,
                    ShowInDiscoveryDocument = false,
                    IncludeAllClaimsForUser = true
                },
                new Scope
                {
                    Name = "read",
                    DisplayName = "Read data",
                    Type = ScopeType.Resource,
                    Emphasize = false
                },
                new Scope
                {
                    Name = "write",
                    DisplayName = "Write data",
                    Type = ScopeType.Resource,
                    Emphasize = true,
                },
                new Scope
                {
                    Name = "forbidden",
                    DisplayName = "Forbidden scope",
                    Type = ScopeType.Resource,
                    Emphasize = true
                },
                new Scope
                {
                    Enabled = true,
                    Name = "publicApi",
                    Description = "Access to our public API",
                    Type = ScopeType.Resource
                },
                new Scope
                {
                    Name = "dataEventRecords",
                    DisplayName = "Data Event Records Scope",
                    Type = ScopeType.Resource,

                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                }

             };
        }
    }
}