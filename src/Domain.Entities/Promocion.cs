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
        public Promocion()
        {
            this._EntidadFinancieraIds = "";
            this._MedioPagoIds = "";
            this._ProductoCategoriaIds="";
            this._TipoMedioPagoIds = "";

        }
        public int Id { get; set; }

        [JsonIgnore]
        public String _MedioPagoIds { get; set; }
        [NotMapped]
        public  List<int> MedioPagoIds
        {
            get
            {
                if (_MedioPagoIds==null || _MedioPagoIds.Trim().Length==0)
                    return null;
                return Array.ConvertAll(_MedioPagoIds.Split(','), Int32.Parse).ToList();
            }
            set
            {
                if(value !=null)
                _MedioPagoIds = String.Join(",", value.Select(p => p.ToString()));
            }
        }


        [JsonIgnore]
        public String _TipoMedioPagoIds { get; set; }
        [NotMapped]
        public  List<int> TipoMedioPagoIds
        {
            get
            {
                if (_TipoMedioPagoIds==null ||_TipoMedioPagoIds.Trim().Length==0)
                    return null;
                return Array.ConvertAll(_TipoMedioPagoIds.Split(','), Int32.Parse).ToList();
            }
            set
            {
                if (value != null)
                 _TipoMedioPagoIds = String.Join(",", value.Select(p => p.ToString()));
            }
        }
        [JsonIgnore]
        public String _EntidadFinancieraIds { get; set; }
        [NotMapped]
        public List<int> EntidadFinancieraIds
        {
            get
            {
                if (_EntidadFinancieraIds==null ||_EntidadFinancieraIds.Trim().Length==0)
                    return null;
                return Array.ConvertAll(_EntidadFinancieraIds.Split(','), Int32.Parse).ToList();
            }
            set
            {
                if (value != null)
                 _EntidadFinancieraIds = String.Join(",", value.Select(p => p.ToString()));
            }
        }
        [JsonIgnore]
        public String _ProductoCategoriaIds { get; set; }
        [NotMapped]
        public List<int> ProductoCategoriaIds
        {
            get
            {
                if (_ProductoCategoriaIds==null ||_ProductoCategoriaIds.Trim().Length==0)
                    return null;

                return Array.ConvertAll(_ProductoCategoriaIds.Split(','), Int32.Parse).ToList();
               
                
            }
            set
            {
                if (value != null)
                  _ProductoCategoriaIds = String.Join(",", value.Select(p => p.ToString()));
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
