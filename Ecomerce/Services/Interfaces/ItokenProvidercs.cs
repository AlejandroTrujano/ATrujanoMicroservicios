namespace Ecomerce.Services.Interfaces
{
    public interface ItokenProvidercs
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
