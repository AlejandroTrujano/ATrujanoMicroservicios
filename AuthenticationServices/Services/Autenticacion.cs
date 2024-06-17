using AuthenticationServices.DTO;
using AuthenticationServices.Model;
using AuthenticationServices.Services.Interface;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationServices.Services
{
    public class Autenticacion : IAutenticacion //implementacion de la interface
    {
        /*Inyeccion de dependencias*/
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJWTTokenGenerator _jwtTokenGenerator;
        public Autenticacion(AppDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IJWTTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDTO> Lgin(LoginRequestDTO model)
        {
            LoginResponseDTO responseDTO = new LoginResponseDTO();
            var usuario = _context.Users.SingleOrDefault(c => c.UserName == model.Username);
            
            if(usuario != null)
            {
                //valida si la contraseña de la bd y la que escribimos es correcta
                bool isValid = await _userManager.CheckPasswordAsync(usuario, model.Password);
                if(isValid)
                {
                    var token = _jwtTokenGenerator.GenerateToken(usuario);

                    UserDTO userDTO = new UserDTO();
                    userDTO.UserName = usuario.UserName;
                    userDTO.IdUsuario = usuario.Id;
                    userDTO.Numero = usuario.PhoneNumber;
                    userDTO.Email = usuario.Email;

                    responseDTO.Token = token;
                    responseDTO.user = userDTO;
                }
                else
                {
                    responseDTO.Token = "";
                    responseDTO.user = null;
                }
            }
            else
            {
                responseDTO.Token = "";
                responseDTO.user = null;
            }
            return responseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = registrationRequestDTO.Username,
                Email = registrationRequestDTO.Email,
                NormalizedEmail = registrationRequestDTO.Email.ToUpper(),
                PhoneNumber = registrationRequestDTO.Numero
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDTO.Password);
                if (result.Succeeded)
                {
                    var userResult = _context.Users.First(u => u.UserName == registrationRequestDTO.Username);
                    UserDTO userDTO = new UserDTO
                    {
                        UserName = userResult.UserName,
                        Email = userResult.Email,
                        IdUsuario = userResult.Id,
                        Numero = userResult.PhoneNumber
                    };
                    return  "";
                }
                else
                {
                    return result.Errors.First().Description;
                }
            }
            catch(Exception ex)
            {

            }
            return "Error";
        }

        //public async Task<LoginResponseDTO> Login(LoginRequestDTO model)
        //{
        //    _context.Users.SingleOrDefault(c => c.UserName == model.Username);
        //}
    }
}
