using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Promociones.Infrastructure;
using Promociones.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft;
using Promociones.Domain.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace Promociones.Application
{
    public class MedioPagoManager : IMedioPagoManager
    {
        public IConfiguration Configuration { get; set; }
        IRequestsManager _requestManeger;
        public MedioPagoManager(IRequestsManager requestManager, IConfiguration Configuration)
        {
            this._requestManeger = requestManager;
            this.Configuration = Configuration;
        }
        public MedioPagoManager()
        {

        }

        public virtual MedioPago GetMedioPago(int Id)
        {

            String baseurl=Configuration.GetValue<String>("BaseUrl");
            String requesturi = Configuration.GetValue<String>("MedioPagoURI");
            JToken resp = _requestManeger.GetRequest(baseurl,requesturi + Id).Result;

            if (resp != null)
                return JsonConvert.DeserializeObject<MedioPago>(resp.ToString());

            return null;

        }
    }
}
