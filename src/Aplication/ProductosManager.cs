using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Promociones.Domain.Core;
using Promociones.Domain.Entities;
using  Microsoft.Extensions.Configuration;
using Promociones.Infrastructure;
using Newtonsoft.Json;

namespace Promociones.Application
{
    public class ProductosManager : IProductosManager
    {
        private readonly IRequestsManager _requestManager;
        public ProductosManager(IRequestsManager requestManager)
        {
            this._requestManager = requestManager;
      
        }
        
        public List<ProductoCategoria> GetCategorias()
        {

            var categorias = new List<ProductoCategoria>
            {
                new ProductoCategoria() { Id = 1, Descripcion = "Tvs" },
                new ProductoCategoria() { Id = 2, Descripcion = "Heladeras" },
                new ProductoCategoria() { Id = 3, Descripcion = "Lavarropas" },
                new ProductoCategoria() { Id = 4, Descripcion = "Celulares" },
                new ProductoCategoria() { Id = 5, Descripcion = "Notebooks" },
                new ProductoCategoria() { Id = 6, Descripcion = "Gaming" }
            };
            return categorias;

            var resp =  _requestManager.GetRequest( "producto/categorias").Result;

            if (resp != null)
                return JsonConvert.DeserializeObject<List<ProductoCategoria>>(resp.ToString());

            return null;

        }
    }
} 
