using System;
using System.Collections.Generic;
using System.Text;
using Promociones.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Promociones.Domain.Core
{
    public interface IPromocionesManager
    {
        Task<IEnumerable<Promocion>> GetPromociones();
        Task<Promocion> GetPromocion(int Id);
        Task<IEnumerable<Promocion>> GetPromocionesVigentes();
        Task<IEnumerable<Promocion>> GetPromocionesVigentes(DateTime Fecha);
        Task<Boolean> ValidarVigencia(int Id);
        Task<IEnumerable<Promocion>> GetPromocionesVenta(QueryPromocionesDTO producto);
        Task<int> DeletePromociones(PromocionDeleteDTO dto);
        Task<Promocion> UpdatePromocion(PromocionUpdateDTO dto);
        Task<Promocion> InsertPromocion(PromocionInsertDTO dto);
    }
}
