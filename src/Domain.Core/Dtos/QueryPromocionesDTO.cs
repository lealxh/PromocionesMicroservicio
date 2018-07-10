using System;
using System.Collections.Generic;
using System.Text;

namespace Promociones.Domain.Core
{
    public class QueryPromocionesDTO
    {
       
        public int MedioPagoId { get; set; }

        public int TipoMedioPagoId { get; set; }

        public int EntidadFinancieraId { get; set; }

        public int ProductoCategoriaId { get; set; }

        public int CantidadDeCuotas { get; set; }
    }
}
