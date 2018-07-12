using System;
using System.Collections.Generic;
using System.Text;

namespace Promociones.Domain.Core
{
    public class DuplicidadException : Exception
    {
        public DuplicidadException() : base("Ya existe una promocion para ese rango de fecha y medio de pago" )
        {

        }
    }
}
