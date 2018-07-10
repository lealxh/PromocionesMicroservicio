using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using System.Linq.Expressions;
using Promociones.Domain.Entities;

namespace Promociones.Domain.Core
{
    public class PromocionUpdateDTO
    {
        public int Id { get; set; }

        public int[] MedioPagoIds { get; set; }

        public int[] TipoMedioPagoIds { get; set; }

        public int[] EntidadFinancieraIds { get; set; }

        public int[] ProductoCategoriaIds { get; set; }

        public Nullable<int> MaxCantidadDeCuotas { get; set; }

        public Nullable<decimal> PorcentajeDecuento { get; set; }

       
    }
}
