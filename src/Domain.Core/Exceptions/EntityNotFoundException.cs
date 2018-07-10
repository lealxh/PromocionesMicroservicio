using System;
using System.Collections.Generic;
using System.Text;

namespace Promociones.Domain.Core
{
    public class EntityNotFoundException:Exception
    {
        public EntityNotFoundException(string name, string Id) : base("Entity not found " + name + ": " + Id)
        {
            
        }
    }
}
