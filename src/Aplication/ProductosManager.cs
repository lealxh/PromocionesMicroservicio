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
        public IConfiguration Configuration { get; set; }
        public ProductosManager(IRequestsManager requestManager, IConfiguration Configuration)
        {
            this._requestManager = requestManager;
            this.Configuration = Configuration;

        }
        
        public virtual List<ProductoCategoria> GetCategorias()
        {

            String baseurl = Configuration.GetValue<String>("BaseUrl");
            String requesturi = Configuration.GetValue<String>("CategoriasURI");
            var resp =  _requestManager.GetRequest(baseurl, requesturi).Result;

            if (resp != null)
                return JsonConvert.DeserializeObject<List<ProductoCategoria>>(resp.ToString());

            return null;

        }
    }
} 
