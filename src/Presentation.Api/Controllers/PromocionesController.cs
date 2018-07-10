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
        private ILogger _logger;
        public PromocionesController(IPromocionesManager context, ILogger<PromocionesController> logger)
        {
            this._context = context;
            this._logger = logger;
        }
    // GET: api/<controller>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await this._context.GetPromociones());
        }

        [HttpGet("{id}", Name = "PromocionById")]
        public async Task<ActionResult> Get(int id)
        {
            return Ok(await this._context.GetPromocion(id));
        }

        [HttpGet("Vigentes/", Name = "Promociones_vigentes")]
        public async Task<IActionResult> Vigentes()
        {
             return Ok(await _context.GetPromocionesVigentes());

        }

        [HttpGet("Vigentes/{Fecha}", Name = "Promociones_vigentes_fecha")]
        public async Task<IActionResult> Vigentes(DateTime Fecha)
        {
              return Ok(await _context.GetPromocionesVigentes(Fecha));

        }

        // GET api/<controller>/5
        [HttpGet("ValidarVigencia/{id}")]
        public async Task<IActionResult> ValidarVigencia(int id)
        {
              return Ok(await _context.ValidarVigencia(id));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PromocionInsertDTO dto)
        {
            var promo = await _context.InsertPromocion(dto);
            return Created("api/Promociones/"+promo.Id,promo);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] PromocionUpdateDTO dto)
        {
            return Ok(await _context.UpdatePromocion(dto));
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody]PromocionDeleteDTO dto)
        {
            return Ok(await _context.DeletePromociones(dto));
        }
    }
}
