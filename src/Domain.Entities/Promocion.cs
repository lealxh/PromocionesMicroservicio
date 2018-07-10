using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace Promociones.Domain.Entities
{
    public class Promocion
    {

        public int Id { get; set; }

        [JsonIgnore]
        public String _MedioPagoIds { get; set; }
        [NotMapped]
        public int[] MedioPagoIds
        {
            get
            {
                return Array.ConvertAll(_MedioPagoIds.Split(','), Int32.Parse);
            }
            set
            {
                _MedioPagoIds = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }
        [JsonIgnore]
        public String _TipoMedioPagoIds { get; set; }
        [NotMapped]
        public int[] TipoMedioPagoIds
        {
            get
            {
                return Array.ConvertAll(_TipoMedioPagoIds.Split(','), Int32.Parse);
            }
            set
            {
                _TipoMedioPagoIds = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }
        [JsonIgnore]
        public String _EntidadFinancieraIds { get; set; }
        [NotMapped]
        public int[] EntidadFinancieraIds
        {
            get
            {
                return Array.ConvertAll(_EntidadFinancieraIds.Split(','), Int32.Parse);
            }
            set
            {
                _EntidadFinancieraIds = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }
        [JsonIgnore]
        public String _ProductoCategoriaIds { get; set; }
        [NotMapped]
        public int[] ProductoCategoriaIds
        {
            get
            {
                return Array.ConvertAll(_ProductoCategoriaIds.Split(','), Int32.Parse);
            }
            set
            {
                _ProductoCategoriaIds = String.Join(",", value.Select(p => p.ToString()).ToArray());
            }
        }

        public Nullable<int> MaxCantidadDeCuotas { get; set; }

        public Nullable<decimal> PorcentajeDecuento { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public bool Activo { get; set; }
        [JsonIgnore]
        public DateTime FechaCreacion { get; set; }
        [JsonIgnore]
        public Nullable<DateTime> FechaModificacion { get; set; }
    }
    
    
    
}
