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
        private IProductosManager _prmanager;
        private IMedioPagoManager _mpmanager;
        public PromocionesManager(PromocionesDbContext DbContext,IDateTime datetime,IProductosManager prmanager,IMedioPagoManager mpmanager)
        {
            this._DbContext = DbContext;
            this._datetime = datetime;
            this._prmanager = prmanager;
            this._mpmanager = mpmanager;
          
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

        public async Task<IEnumerable<Promocion>> GetPromocionesVenta(QueryPromocionesDTO venta)
        {

            return await _DbContext.Promociones.
                Where(promo => ((promo.MedioPagoIds == null || promo.MedioPagoIds.Contains(venta.MedioPagoId)) &&
                (promo.TipoMedioPagoIds == null || promo.TipoMedioPagoIds.Contains(venta.TipoMedioPagoId)) &&
                (promo.ProductoCategoriaIds == null || promo.ProductoCategoriaIds.Contains(venta.ProductoCategoriaId)) &&
                (promo.EntidadFinancieraIds == null || promo.EntidadFinancieraIds.Contains(venta.EntidadFinancieraId)) &&
                (promo.MaxCantidadDeCuotas==null || promo.MaxCantidadDeCuotas >= venta.CantidadDeCuotas))).ToListAsync();
        }

        public async Task<int> DeletePromociones(PromocionDeleteDTO dto)
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
                return promociones.Count;

            }
            else
            {
                throw new EntityNotFoundException(nameof(Promocion), dto.PromocionesIds.ToString());
            }

        }
        private List<Promocion>GetOverlappingDates(DateTime inicio,DateTime fin)
        {
            //( start1 <= end2 and start2 <= end1 )
            var overlappin= _DbContext.Promociones.Where(x => x.FechaInicio <=fin && inicio<=x.FechaFin).ToList();

            return overlappin;
        }

        private List<int> JoinedLists(List<int> list1, List<int> list2)
        {
            var IDs = list2.Select(item => item);
            var result = list1.Where(item => IDs.Contains(item));




            return result.ToList<int>();
        }
        private bool ValidarCategorias(List<int> categoriasPromocion)
        {
            var allCategorias = _prmanager.GetCategorias();
            if(categoriasPromocion!=null)
            foreach (int idCategoria in categoriasPromocion)
            {
                if (!allCategorias.Any(x => x.Id == idCategoria))
                    return false;
            }

            return true;
        }
        private bool ValidarMediosPago(List<int> mediosPagoPromocion)
        {
            
            if (mediosPagoPromocion != null)
            foreach (int idMedioPago in mediosPagoPromocion)
            {
                if (_mpmanager.GetMedioPago(idMedioPago)==null)
                    return false;
            }

            return true;
        }
        public async Task<Promocion> UpdatePromocion(PromocionUpdateDTO dto)
        {
            var promo = await _DbContext.Promociones.Where(p => p.Id == dto.Id).SingleOrDefaultAsync();
            if (promo == null)
                throw new EntityNotFoundException(nameof(Promocion), dto.Id.ToString());

            if (!ValidarCategorias(dto.ProductoCategoriaIds))
                throw new InvalidCategoriaException(String.Join(",", dto.ProductoCategoriaIds.Select(p => p.ToString())));

            if (!ValidarMediosPago(dto.MedioPagoIds))
                throw new InvalidMedioPagoException(String.Join(",", dto.MedioPagoIds.Select(p => p.ToString())));

            

            //_mapper.Map<PromocionUpdateDTO,Promocion>(dto, promo);
            promo.MedioPagoIds = dto.MedioPagoIds;
            promo.EntidadFinancieraIds = dto.EntidadFinancieraIds;
            promo.ProductoCategoriaIds = dto.ProductoCategoriaIds;
            promo.TipoMedioPagoIds = dto.TipoMedioPagoIds;

            if (dto.MaxCantidadDeCuotas!=null)
            promo.MaxCantidadDeCuotas = dto.MaxCantidadDeCuotas;
            if (dto.PorcentajeDecuento != null)
            promo.PorcentajeDecuento = dto.PorcentajeDecuento;

            promo.FechaModificacion = _datetime.Now;

            _DbContext.Update(promo);

           await _DbContext.SaveChangesAsync();

            return promo;

        }

        public async Task<Promocion> InsertPromocion(PromocionInsertDTO dto)
        {
            if (!ValidarCategorias(dto.ProductoCategoriaIds))
                throw new InvalidCategoriaException(String.Join(",", dto.ProductoCategoriaIds.Select(p => p.ToString())));

            if (!ValidarMediosPago(dto.MedioPagoIds))
                throw new InvalidMedioPagoException(String.Join(",", dto.MedioPagoIds.Select(p => p.ToString())));

            var overlapping = GetOverlappingDates(dto.FechaInicio, dto.FechaFin);
            if (overlapping.Count() > 0)
            {
                var result = overlapping.
                    Where(x => (x.MedioPagoIds == null || JoinedLists(x.MedioPagoIds, dto.MedioPagoIds).Count() > 0) &&
                   (x.TipoMedioPagoIds == null || JoinedLists(x.TipoMedioPagoIds, dto.TipoMedioPagoIds).Count() > 0)).ToList();

                if (result.Count() > 0)
                    throw new DuplicidadException();

            }

        Promocion promo = new Promocion()
            {
                Activo = true,
                EntidadFinancieraIds = dto.EntidadFinancieraIds,
                FechaFin = dto.FechaFin,
                FechaCreacion = _datetime.Now,
                FechaInicio = dto.FechaInicio,
                MaxCantidadDeCuotas = dto.MaxCantidadDeCuotas,
                MedioPagoIds = dto.MedioPagoIds,
                PorcentajeDecuento = dto.PorcentajeDecuento,
                ProductoCategoriaIds = dto.ProductoCategoriaIds,
                TipoMedioPagoIds = dto.TipoMedioPagoIds

            };

            DateTime startDt = dto.FechaInicio;
            DateTime endDt = dto.FechaFin;
         
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

