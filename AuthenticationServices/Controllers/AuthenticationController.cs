using AuthenticationServices.DTO;
using AuthenticationServices.Model;
using AuthenticationServices.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAutenticacion _authService;
        public AuthenticationController(IAutenticacion authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            //password = Alejandro123@
            ResultDTO resultDTO = new ResultDTO();
            var result = await _authService.Lgin(model);
            if(result.user == null)
            {
                resultDTO.Correct = false;
                resultDTO.ErrorMessage = "El username/correo o contraseña son incorrectos";
                return BadRequest(resultDTO);
            }
            resultDTO.Object = result;
            return Ok(resultDTO);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            ResultDTO resultDTO = new ResultDTO();
            var errorMessage = await _authService.Register(registrationRequestDTO);
            if(errorMessage == null)
            {
                resultDTO.Correct = false;
                resultDTO.ErrorMessage = errorMessage;
                return BadRequest(resultDTO);
            }
            else
            {
                return Ok(resultDTO);
            }
        }
    }
}
