using System;
using System.Collections.Generic;
using System.Text;

namespace Promociones.Domain.Core
{
    public class InvalidMedioPagoException : Exception
    {
        public InvalidMedioPagoException(string id) : base("Medios de pago invalidos: " + id)
        {

        }
    }
}
