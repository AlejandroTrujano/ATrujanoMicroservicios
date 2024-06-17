using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.IdentityModel.Tokens.Jwt;
using Ecomerce.Services.Interfaces;
using Ecomerce.DTO;

namespace Ecomerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ItokenProvidercs _tokenProvider;

        public LoginController(IConfiguration configuration, ItokenProvidercs tokenProvider)
        {
            _configuration = configuration;
            _tokenProvider = tokenProvider;
        }
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {

            LoginResponseDTO response = new LoginResponseDTO();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiLogin"]);
                var postTask = client.PostAsJsonAsync<LoginRequestDTO>("Login", model);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var conent = result.Content.ReadAsAsync<ResultDTO>();
                    conent.Wait();
                    response = JsonConvert.DeserializeObject<LoginResponseDTO>(conent.Result.Object.ToString());

                    await SingInUser(response);
                    _tokenProvider.SetToken(response.Token);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("CustomError", "Error al iniciar sesion");
                    return View(response);
                }
            }
           /* //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri("https://localhost:7284");
            //    var json = JsonConvert.SerializeObject(loginRequestDTO);
            //    var content = new StringContent(json, Encoding.UTF8, "application/json");
            //    var response = client.PostAsync("/api/Authentication/Login", content).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var result = response.Content.ReadAsStringAsync().Result;
            //        TempData["object"] = result;
            //        ViewBag.Text = "inicio de sesion correcto ";
            //        return PartialView("Modal");
            //    }
            //}*/
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDTO register)
        {
            //RegistrationResponseDTO response = new RegistrationResponseDTO();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiLogin"]);
                var post = client.PostAsJsonAsync<RegistrationRequestDTO>("Register", register);
                post.Wait();

                var result = post.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Text = "Tu registro fue ! Exitoso ¡";
                    return PartialView("Modal");
                }
                else
                {
                    ModelState.AddModelError("CustomError", "Error al iniciar sesion");
                    return View(post);
                }
            }
        }
        private async Task SingInUser(LoginResponseDTO dto)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(dto.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

    
    }
}
