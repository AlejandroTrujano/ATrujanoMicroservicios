using Microsoft.AspNetCore.Identity;

namespace AuthenticationServices.Services.Interface
{
    public interface IJWTTokenGenerator
    { 
        //Contrato
        string GenerateToken(IdentityUser user);
    }
}
