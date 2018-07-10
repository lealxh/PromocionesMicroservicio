using System;
using System.Collections.Generic;
using System.Text;
using Promociones.Domain.Entities;
using Promociones.Domain.Core;
using Promociones.Infrastructure.Persistence;

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
namespace Promociones.Application
{
    public class PromocionesManager : IPromocionesManager
    {
        private  PromocionesDbContext _DbContext;
        private  IDateTime _datetime;
        private  IMapper _mapper;
        public PromocionesManager(PromocionesDbContext DbContext,IDateTime datetime,IMapper mapper)
        {
            this._DbContext = DbContext;
            this._datetime = datetime;
            this._mapper = mapper;
        }


        public async Task<IEnumerable<Promocion>> GetPromociones()
        {
            return await _DbContext.Promociones.Where(p=>p.Activo).ToListAsync();
        }

        public async Task<IEnumerable<Promocion>> GetPromocionesVigentes()
        {   
            return await _DbContext.Promociones.Where(x => _datetime.Now <= x.FechaFin && _datetime.Now >= x.FechaInicio && x.Activo).ToListAsync();
        }

        public async Task<IEnumerable<Promocion>> GetPromocionesVigentes(DateTime Fecha)
        {
            return await _DbContext.Promociones.Where(x => Fecha <= x.FechaFin && Fecha >= x.FechaInicio && x.Activo).ToListAsync();
        }

        public async Task<bool> ValidarVigencia(int Id)
        {

            var promo = await _DbContext.Promociones.Where(p => p.Id == Id && p.Activo).SingleOrDefaultAsync();

            if (promo == null)
                throw new EntityNotFoundException(nameof(Promocion), Id.ToString());

            return (_datetime.Now >= promo.FechaInicio && _datetime.Now <= promo.FechaFin);

        }

        public async Task<IEnumerable<Promocion>> GetPromocionesVigentesVenta(QueryPromocionesDTO venta)
        {

            return await _DbContext.Promociones.
                Where(promo => promo.MedioPagoIds.Contains(venta.MedioPagoId) &&
                promo.TipoMedioPagoIds.Contains(venta.TipoMedioPagoId) && 
                promo.ProductoCategoriaIds.Contains(venta.ProductoCategoriaId)&&
                promo.EntidadFinancieraIds.Contains(venta.EntidadFinancieraId)&&
                promo.MaxCantidadDeCuotas>=venta.CantidadDeCuotas &&
                promo.ProductoCategoriaIds.Contains(venta.ProductoCategoriaId) 
                ).ToListAsync();
        }

        public async Task<bool> DeletePromociones(PromocionDeleteDTO dto)
        {
            var promociones = await _DbContext.Promociones.Where(p => p.Activo && dto.PromocionesIds.Contains(p.Id)).ToListAsync();

            if (promociones.Count > 0)
            {
                foreach (var item in promociones)
                {
                    item.Activo = false;
                    _DbContext.Update(item);
                }
                await _DbContext.SaveChangesAsync();
                return true;

            }
            else
            {
                throw new EntityNotFoundException(nameof(Promocion), dto.PromocionesIds.ToString());
            }

        }

        public async Task<Promocion> UpdatePromocion(PromocionUpdateDTO dto)
        {
            var promo = await _DbContext.Promociones.Where(p => p.Id == dto.Id).SingleOrDefaultAsync();
            if(promo==null)
                throw new EntityNotFoundException(nameof(Promocion), dto.Id.ToString());

            Mapper.Map<PromocionUpdateDTO,Promocion>(dto, promo);
            promo.FechaModificacion = _datetime.Now;

            _DbContext.Update(promo);
            _DbContext.SaveChanges();
            return promo;

        }

        public async Task<Promocion> InsertPromocion(PromocionInsertDTO dto)
        {
            var promo = _mapper.Map<PromocionInsertDTO, Promocion>(dto);
            promo.FechaCreacion = _datetime.Now;
            _DbContext.Add(promo);
           await _DbContext.SaveChangesAsync();

            return promo;
        }

        public async Task<Promocion> GetPromocion(int Id)
        {
            var promo = await _DbContext.Promociones.Where(p => p.Id == Id).SingleOrDefaultAsync();
            if (promo == null)
                throw new EntityNotFoundException(nameof(Promocion), Id.ToString());
            return promo;


        }
    }
}

