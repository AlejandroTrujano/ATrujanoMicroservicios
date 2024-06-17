using Azure.Messaging;
using MicroServicesProducto.DTO;
using MicroServicesProducto.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroServicesProducto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductoController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            ResultProductoDTO result = new ResultProductoDTO();

            try
            {
                var listProducts = _context.Productos.ToList();
                if (listProducts.Count > 0 || listProducts != null)
                {
                    result.Objects = new List<Object>();
                    foreach (var product in listProducts)
                    {
                        ProductoDTO producto = new ProductoDTO();
                        producto.IdProducto = product.IdProducto;
                        producto.Nombre = product.Nombre;
                        producto.Precio = product.Precio;
                        producto.Categoria = product.Categoria;
                        producto.UrlImagen = product.UrlImagen;

                        result.Objects.Add(producto);
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Ocurrio un error inesperado");
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int IdProducto)
        {
            ResultProductoDTO result = new ResultProductoDTO();
            try
            {
                var resultById = _context.Productos.SingleOrDefault(c => c.IdProducto == IdProducto);
                if(result != null)
                {
                    ProductoDTO producto = new ProductoDTO();
                    producto.IdProducto = resultById.IdProducto;
                    producto.Nombre = resultById.Nombre;
                    producto.Precio = resultById.Precio;
                    producto.Categoria = resultById.Categoria;
                    producto.UrlImagen= resultById.UrlImagen;

                    return Ok(producto);
                }
                else
                {
                    return BadRequest("error");
                }
            }
            catch(Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage=ex.Message;
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] ProductoDTO productoDTO)
        {
            ResultProductoDTO result = new ResultProductoDTO();
            try
            {
                Producto producto = new Producto
                {
                    Nombre = productoDTO.Nombre,
                    Precio = productoDTO.Precio,
                    Categoria = productoDTO.Categoria,
                    UrlImagen = productoDTO.UrlImagen
                };

                _context.Productos.Add(producto);
                int rowsAffected = _context.SaveChanges();
                if (rowsAffected > 0)
                {

                    return Ok(result);
                    //, new { MessageContent = "Productos registrados" }
                }
                else
                {
                    return BadRequest(new { MessageContent = "El producto no fue registrado" });
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(result);
            }

        }

        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int IdProducto)
        {
            ResultProductoDTO result = new ResultProductoDTO();
            try
            {
                var resultDelete = _context.Productos.FirstOrDefault(c => c.IdProducto == IdProducto);
                _context.Productos.Remove(resultDelete);
                int rowsAffected = _context.SaveChanges();
                if (rowsAffected > 0)
                {

                    return Ok(result);
                }
                else
                {
                    return BadRequest(new { MessageContent = "El producto no fue eliminado" });
                }


            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(result);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(int IdProducto, [FromBody] ProductoDTO productoDTO)
        {
            ResultProductoDTO result = new ResultProductoDTO();
            try
            {
                
                var resultUdpate = _context.Productos.FirstOrDefault( c=> c.IdProducto == IdProducto);
                if(resultUdpate != null)
                {
                    resultUdpate.Nombre = productoDTO.Nombre;
                    resultUdpate.Precio = productoDTO.Precio;
                    resultUdpate.Categoria = productoDTO.Categoria;
                    resultUdpate.UrlImagen = productoDTO.UrlImagen;

                    int rowsAffected = _context.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(new { MessageContent = "El producto no se actualizo" });
                    }
                }
                else
                {
                    return BadRequest(new { MessageContent = "El Id del producto no coincide con la base de datos" });

                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(result);
            }
        }

       

    }
}
