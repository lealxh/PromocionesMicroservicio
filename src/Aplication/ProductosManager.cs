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
        public ProductosManager()
        {

        }
        private readonly IRequestsManager _requestManager;
        public ProductosManager(IRequestsManager requestManager)
        {
            this._requestManager = requestManager;
      
        }
        
        public virtual List<ProductoCategoria> GetCategorias()
        {

            var resp =  _requestManager.GetRequest("http://localhost:17479/", "producto/categorias").Result;

            if (resp != null)
                return JsonConvert.DeserializeObject<List<ProductoCategoria>>(resp.ToString());

            return null;

        }
    }
} 
