using Ecomerce.Services.Interfaces;

namespace Ecomerce.Services
{
    public class TokenProvider : ItokenProvidercs
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearToken()
        {
            throw new NotImplementedException();
        }

        public string? GetToken()
        {
            throw new NotImplementedException();
        }

        public void SetToken(string token)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append("TokenCookie", token);
        }
    }
}
