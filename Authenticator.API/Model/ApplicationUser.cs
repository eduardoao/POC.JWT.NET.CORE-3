using Microsoft.AspNetCore.Identity;

namespace Authenticator.API.Model
{
    public class ApplicationUser : IdentityUser
    {

        public string Nome { get; set; }
        public string Telefone { get; set; }

    }
}
