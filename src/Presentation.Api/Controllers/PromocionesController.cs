using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Promociones.Application;
using Promociones.Domain.Core;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Promociones.Presentation.Api.Controllers
{
    [Route("api/[controller]")]
    public class PromocionesController : Controller
    {
        private IPromocionesManager _context;
        
        public PromocionesController(IPromocionesManager context)
        {
            this._context = context;
           
        }
    // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult> GetAllPromociones()
        {
            return Ok(await this._context.GetPromociones());
        }

        [HttpGet("{id}", Name = "PromocionById")]
        public async Task<ActionResult> GetPromocionById(int id)
        {
            return Ok(await this._context.GetPromocion(id));
        }

        [HttpGet("Vigentes/", Name = "Promociones_vigentes")]
        public async Task<IActionResult> GetPromocionesVigentes()
        {
             return Ok(await _context.GetPromocionesVigentes());

        }

        [HttpGet("Vigentes/{Fecha}", Name = "Promociones_vigentes_fecha")]
        public async Task<IActionResult> GetPromocionesVigentesEnFecha(DateTime Fecha)
        {
            return Ok(await _context.GetPromocionesVigentes(Fecha));   
        }

        [HttpGet("Venta/", Name = "Promociones_venta")]
        public async Task<IActionResult> GetPromocionesVenta(QueryPromocionesDTO dto)
        {
            return Ok(await _context.GetPromocionesVenta(dto));
        }

        // GET api/<controller>/5
        [HttpGet("ValidarVigencia/{id}")]
        public async Task<IActionResult> GetVigenciaPromocion(int id)
        {
            try
            {
                return Ok(await _context.ValidarVigencia(id));
            }
            catch (EntityNotFoundException)
            {
                return NotFound("No se encontraron promociones con el Id: "+id);
            }
            
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> CrearPromocion([FromBody] PromocionInsertDTO dto)
        {

            try
            {
                var promo = await _context.InsertPromocion(dto);
                return Created("api/Promociones/" + promo.Id, promo);

            }
            catch (InvalidCategoriaException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidMedioPagoException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task<IActionResult> ModificarPromocion([FromBody] PromocionUpdateDTO dto)
        {
            try
            {
                return Ok(await _context.UpdatePromocion(dto));
            }
            catch (EntityNotFoundException)
            {
                return NotFound("No se encontraron promociones con el Id: "+dto.Id);
            }
            catch (InvalidCategoriaException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidMedioPagoException ex)
            {
                return BadRequest(ex.Message);
            }


        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task<IActionResult> DeletePromocion([FromBody]PromocionDeleteDTO dto)
        {
            try
            {
                return Ok(await _context.DeletePromociones(dto));
            }
            catch (EntityNotFoundException)
            {
                return NotFound("No se encontraron las promociones indicadas");
            }
           

        }
    }
}
