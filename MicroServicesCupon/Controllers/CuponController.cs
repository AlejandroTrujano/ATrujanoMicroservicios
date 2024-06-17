using Azure.Messaging;
using MicroServicesCupon.DTO;
using MicroServicesCupon.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace MicroServicesCupon.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponController : ControllerBase
    {
        //las propiedades tienen que ser privadas y de solo lectura
        private readonly Model.AppDbContext _context;  //inyeccion de dependencias
                                                       //instancia de una clase, con un parametro
        public CuponController(Model.AppDbContext dbContext) //constructor 
        {
            _context = dbContext;
        }

        //nos ayuda a reutilizar codigo 

        //arquitectura rest con http
        //common response
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            ResultDTO result = new ResultDTO();
            try
            {
                var listCupones = _context.Cupones.ToList(); //esto es un get all con linq
                if (listCupones != null && listCupones.Count > 0)
                {
                    result.Objects = new List<Object>();
                    foreach (var cupon in listCupones)
                    {
                        CuponDTO dtoCupon = new CuponDTO();
                        dtoCupon.IdCupon = cupon.IdCupon;
                        dtoCupon.CantidadMinima = cupon.CantidadMinima;
                        dtoCupon.Descuento = cupon.Descuento;
                        dtoCupon.Codigo = cupon.Codigo;

                        result.Objects.Add(dtoCupon);
                    }
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Ocurrio un error");
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Exception = ex;
                result.ErrorMessage = ex.Message;
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int IdCupon)
        {
            ResultDTO resultDTO = new ResultDTO();
            try
            {
                var resultCupon = _context.Cupones.FirstOrDefault(c => c.IdCupon == IdCupon);
                if (resultCupon != null)
                {

                    CuponDTO dtoCupon = new CuponDTO();
                    dtoCupon.IdCupon = resultCupon.IdCupon;
                    dtoCupon.CantidadMinima = resultCupon.CantidadMinima;
                    dtoCupon.Descuento = resultCupon.Descuento;
                    dtoCupon.Codigo = resultCupon.Codigo;

                    return Ok(dtoCupon);
                }
                else
                {
                    return BadRequest("Error");
                }
            }
            catch (Exception ex)
            {
                resultDTO.Correct = false;
                resultDTO.ErrorMessage = ex.Message;
                resultDTO.Exception = ex;
                return BadRequest(resultDTO);
            }
        }

        [HttpGet]
        [Route("GetByCode")]
        public IActionResult GetByCode(string code)
        {
            ResultDTO resultDTO = new ResultDTO();

            try
            {
                //expresion lambda
                var resultCode = _context.Cupones.SingleOrDefault(c => c.Codigo == code);

                if (resultCode != null)
                {
                    CuponDTO dtoCupon = new CuponDTO();
                    dtoCupon.IdCupon = resultCode.IdCupon;
                    dtoCupon.CantidadMinima = resultCode.CantidadMinima;
                    dtoCupon.Descuento = resultCode.Descuento;
                    dtoCupon.Codigo = resultCode.Codigo;

                    return Ok(dtoCupon);
                }
                else
                {
                    return BadRequest("Error");
                }
            }
            catch (Exception ex)
            {
                resultDTO.Correct = false;
                resultDTO.ErrorMessage = ex.Message;
                resultDTO.Exception = ex;

                return BadRequest(resultDTO);
            }
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult Add([FromBody] CuponDTO cuponDTO)
        {
            //([FromQuery] int cantidadminima, double descuento, string codigo)
            ResultDTO result = new ResultDTO();
            try
            {
                Cupon cupon = new Cupon
                {
                   CantidadMinima = cuponDTO.CantidadMinima,
                   Descuento = cuponDTO.Descuento,
                   Codigo = cuponDTO.Codigo
                };
             

                _context.Cupones.Add(cupon);
               int rowsAffected =  _context.SaveChanges();

                if (rowsAffected > 0 )
                {
                    
                    return Ok(new { MessageContent = "productos registrados" });
                }
                else
                {
                    return BadRequest(new { MessageContent = "productos registrados" });
                }
            }


            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Exception = ex;

                return BadRequest(result);
            }
        }

        [HttpGet]
        [Route("Delete")]
        public IActionResult Delete(int IdCupon)
        {
            ResultDTO resultDTO = new ResultDTO();
            try
            {
                var resultCode = _context.Cupones.FirstOrDefault(c => c.IdCupon == IdCupon);
                
                if(resultCode != null)
                {
                    _context.Cupones.Remove(resultCode);
                    _context.SaveChanges();
                    return Ok(new { MessageContent = "producto eliminados" });
                }
                else
                {
                    return BadRequest(new { MessageContent = "error" });
                }

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPut]
        [Route("Update")]
        public IActionResult Update(int IdCupon, [FromBody] CuponDTO cuponDTO)
        {
            ResultDTO result = new ResultDTO();
            try
            {

                var resultUdpate = _context.Cupones.FirstOrDefault(c => c.IdCupon == IdCupon);
                if (resultUdpate != null)
                {
                    resultUdpate.CantidadMinima = cuponDTO.CantidadMinima;
                    resultUdpate.Descuento= cuponDTO.Descuento;
                    resultUdpate.Codigo = cuponDTO.Codigo;

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

        //[HttpPut]
        //[Route("Update")]
        //public IActionResult Update()
        //{
        //    ResultDTO resultDTO = new ResultDTO();
        //    try
        //    {

        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}


/*
 DTO(Data Transfer Object)
 
es un modelo creado para transferir la informacion de mi modelo de BD 
a un modelo especificamente creado para la tranasferencia

 */