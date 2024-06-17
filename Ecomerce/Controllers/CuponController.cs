using Ecomerce.DTO;
using Ecomerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ecomerce.Controllers
{
    public class CuponController : Controller
    {
        /*inyeccion de dependencias*/
        private readonly IConfiguration _configuration;

        public CuponController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult GetAll()
        {
            ResultDTO resultDTO = new ResultDTO();
            CuponDTO cupones = new CuponDTO();
            cupones.Cupones = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiCupon"]);
               
                var response = client.GetAsync("GetAll");
                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ResultDTO>();
                    readTask.Wait();

                    var jsonResult = readTask.Result;
                   if (jsonResult != null)
                    {
                        resultDTO.Objects = new List<object>();
                        foreach (var datos in jsonResult.Objects)
                        {
                            CuponDTO cuponDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CuponDTO>(datos.ToString());
                            cupones.Cupones.Add(cuponDTO);
                        }
                       
                    }
                    return View(cupones);   
                }
                else
                {
                    return View();
                }

            }

            return View();
        }

        public IActionResult Form(int? IdCupon)
        {
            CuponDTO cuponDTO = new CuponDTO();
            ResultDTO resultDTO = new ResultDTO();
            if (IdCupon != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiCupon"]);
                    var response = client.GetAsync($"GetById?IdCupon={IdCupon}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<CuponDTO>();
                        readTask.Wait();
                        var result1 = readTask.Result;
                        //cuponDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CuponDTO>(readTask.Result.ToString());
                        return View(result1);
                    }
                    else
                    {
                        return View(cuponDTO);
                    }
                }
               
            }
            else
            {
                return View();

            }
        }

        [HttpPost]
        public IActionResult Form(int IdCupon, CuponDTO cuponDTO)
        {
            ResultDTO resultDTO = new ResultDTO();
           if(IdCupon != 0) //https://localhost:7200/api/Cupon/Update?IdCupon=1
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiCupon"]);
                    var json = JsonConvert.SerializeObject(cuponDTO); 
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PutAsync($"Update?IdCupon={IdCupon}", content);
                    response.Wait(); // espera la respuesta de la peticion

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Text = "el cupon se actualizo exitosamente";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Text = "el cupon no se actualizo";
                        return PartialView("Modal");
                    }
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiCupon"]);
                    var json = JsonConvert.SerializeObject(cuponDTO);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync("Add", content);
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Text = "el cupon se registro exitosamente";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Text = "el cupon no se registro";
                        return PartialView("Modal");
                    }
                }
            }
        }

        public IActionResult Delete(int IdCupon)
        {
            ResultDTO ResultDTO = new ResultDTO();
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiCupon"]);
                var response = client.GetAsync($"Delete?IdCupon={IdCupon}");
                response.Wait();

                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Text = "el cupon se elimino exitosamente";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Text = "el cupon no se elimino";
                    return PartialView("Modal");
                }
            }
        }
    }
}
