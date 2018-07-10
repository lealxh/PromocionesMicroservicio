using System;
using System.Collections.Generic;
using System.Text;

namespace Promociones.Domain.Core
{
    public class PromocionDeleteDTO
    {
        public IEnumerable<int> PromocionesIds { get; set; }
    }
}
