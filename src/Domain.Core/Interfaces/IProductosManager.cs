using System;
using System.Collections.Generic;
using System.Text;
using Promociones.Domain.Entities;

using System.Threading;
using System.Threading.Tasks;

namespace Promociones.Domain.Core
{
    public interface IProductosManager
    {
        List<ProductoCategoria> GetCategorias();
    }
}
