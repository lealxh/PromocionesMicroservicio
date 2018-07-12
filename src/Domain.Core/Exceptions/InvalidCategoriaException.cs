using System;
using System.Collections.Generic;
using System.Text;

namespace Promociones.Domain.Core
{
    public class InvalidCategoriaException : Exception
    {
        public InvalidCategoriaException(string id) : base("Categorias de productos invalida: "+id)
        {

        }
    }
}
