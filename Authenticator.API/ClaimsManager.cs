using Authenticator.API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authenticator.API
{
    public class ClaimsManager : IClaimsManager
    {
        protected readonly IHttpContextAccessor contextAccessor;
        protected readonly IServiceProvider serviceProvider;

        public ClaimsManager(IHttpContextAccessor contextAccessor, IServiceProvider serviceProvider)
        {
            this.contextAccessor = contextAccessor;
            this.serviceProvider = serviceProvider;
        }

        public async Task AddUpdateClaim(string userId, IDictionary<string, string> newClaims)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var applicationUser = await userMgr.FindByIdAsync(userId);
                var claims = await userMgr.GetClaimsAsync(applicationUser);
                foreach (var claim in claims)
                {
                    if (newClaims.ContainsKey(claim.Type))
                    {
                        var newClaim = new Claim(claim.Type, newClaims[claim.Type]);
                        await userMgr.ReplaceClaimAsync(applicationUser, claim, newClaim);
                    }
                }

                applicationUser.Nome = newClaims["name"];
                applicationUser.Email = newClaims["email"];            
                applicationUser.Telefone = newClaims["phone"];
                await userMgr.UpdateAsync(applicationUser);
            }
        }
    }
}
