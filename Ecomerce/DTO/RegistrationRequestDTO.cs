namespace Ecomerce.DTO
{
    public class RegistrationRequestDTO
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Numero { get; set; }
        public string Password { get; set; }
        public string? RolName { get; set; }
    }
}
