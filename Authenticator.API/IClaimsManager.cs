using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authenticator.API
{
    public interface IClaimsManager
    {
        Task AddUpdateClaim(string userId, IDictionary<string, string> claims);
    }
}
