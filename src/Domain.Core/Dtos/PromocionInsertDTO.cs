using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Promociones.Domain.Core
{
    public class PromocionInsertDTO
    {
        public int Id { get; set; }

         public List<int> MedioPagoIds { get; set; }
     
        public List<int> TipoMedioPagoIds { get; set; }
        
        public List<int> EntidadFinancieraIds { get; set; }
      
        public List<int> ProductoCategoriaIds { get; set; }

        public Nullable<int> MaxCantidadDeCuotas { get; set; }

        public Nullable<decimal> PorcentajeDecuento { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public bool Activo { get; set; }

      
    }
}
