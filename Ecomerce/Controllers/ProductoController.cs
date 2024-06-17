using Ecomerce.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Ecomerce.Controllers
{
    public class ProductoController : Controller
    {
        /*inyeccion de dependencias*/
        private readonly IConfiguration _configuration;

        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult GetAll()
        {
            ResultDTO resultDTO = new ResultDTO();

            ProductoDTO productsDTO = new ProductoDTO();
            productsDTO.Products = new List<object>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiProducto"]);
                var response = client.GetAsync("GetAll");
                response.Wait();

                var result = response.Result;
                if (result != null)
                {
                    var readTask = result.Content.ReadAsAsync<ResultDTO>();
                    readTask.Wait();

                    var jsonResult = readTask.Result;
                    if(jsonResult != null)
                    {
                        resultDTO.Objects = new List<object>();
                        foreach (var productos in jsonResult.Objects)
                        {
                            ProductoDTO productoDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductoDTO>(productos.ToString());
                            productsDTO.Products.Add(productoDTO);
                        }
                    }
                    return View(productsDTO);
                }
            }
            return View();
        }

        public IActionResult Form(int? IdProducto)
        {
            ProductoDTO producto = new ProductoDTO();
            if(IdProducto != null)
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiProducto"]);
                    var response = client.GetAsync($"GetById?IdProducto={IdProducto}");
                    response.Wait();

                    var result = response.Result;
                    if(result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ProductoDTO>();
                        readTask.Wait();

                        var jsonResult = readTask.Result;
                        return View(jsonResult);
                    }
                    else
                    {
                        ViewBag.Text = "El producto no se encontro";
                        return PartialView("Modal");

                    }
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult Form(int IdProducto, ProductoDTO productoDTO)
        {
            if(IdProducto != 0)
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiProducto"]);
                    var json = JsonConvert.SerializeObject(productoDTO);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PutAsync($"Update?IdProducto={IdProducto}", content);
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Text = "El producto se actualizo correctamente";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Text = "El producto no se actualizo";
                        return PartialView("Modal");
                    }
                }
                
            }
            else
            {
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["ApiProducto"]);
                    var json = JsonConvert.SerializeObject(productoDTO);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PostAsync("Add", content);
                    response.Wait();

                    var result = response.Result;
                    if(result.IsSuccessStatusCode)
                    {
                        ViewBag.Text = "El producto se registro correctamente";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Text = "El producto no se registro";
                        return PartialView("Modal");
                    }
                    
                }
            }
            return View();
        }

        public IActionResult Delete(int IdProducto)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["ApiProducto"]);
                var response = client.GetAsync($"Delete?IdProducto={IdProducto}");
                response.Wait();

                var result = response.Result; 
                if(result.IsSuccessStatusCode)
                {
                    ViewBag.Text = "El producto se elimino correctamente";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Text = "El producto no se elimino";
                    return PartialView("Modal");
                }
            }
        }
    }
}
