using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using IdentityMinimal.Identities;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using IdentityServer3.Core.Validation;


namespace IdentityMinimal.IdentityManager
{
    public class IdentityClaimsProvider : DefaultClaimsProvider
    {
        public IdentityClaimsProvider(IUserService users) : base(users)
        {
        }

        public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject, Client client, IEnumerable<Scope> scopes, ValidatedRequest request)
        {
            var baseclaims = await base.GetAccessTokenClaimsAsync(subject, client, scopes, request);

            var claims = new List<Claim>();
            //if (subject.Identity.Name == "bob")
            //{
            //    claims.Add(new Claim("role", "super_user"));
            //    claims.Add(new Claim("role", "asset_manager"));
            //}

            claims.AddRange(baseclaims);

            var dbContext = new IdentityContext();

            return claims;
        }

        public override Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ClaimsPrincipal subject, Client client, IEnumerable<Scope> scopes, bool includeAllIdentityClaims, ValidatedRequest request)
        {
            var rst = base.GetIdentityTokenClaimsAsync(subject, client, scopes, includeAllIdentityClaims, request);
            return rst;
        }
    }
}