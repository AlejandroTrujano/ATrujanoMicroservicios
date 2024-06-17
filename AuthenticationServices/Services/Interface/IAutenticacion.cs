using AuthenticationServices.DTO;

namespace AuthenticationServices.Services.Interface
{
    public interface IAutenticacion
    {
        public Task<LoginResponseDTO> Lgin(LoginRequestDTO model);
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
