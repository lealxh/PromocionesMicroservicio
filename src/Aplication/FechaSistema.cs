using System;
using System.Collections.Generic;
using System.Text;
using Promociones.Domain.Core;

namespace Promociones.Application
{
   public class FechaSistema: IDateTime
    {
            public DateTime Now
            {
                get { return DateTime.Now; }
            }

    }
}
