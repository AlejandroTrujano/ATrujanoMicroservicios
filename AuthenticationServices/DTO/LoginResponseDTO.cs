namespace AuthenticationServices.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public UserDTO user { get; set; }
    }
}
