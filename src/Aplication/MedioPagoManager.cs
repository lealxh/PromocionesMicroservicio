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
        public MedioPagoManager()
        {

        }
        IRequestsManager _requestManeger;
        public MedioPagoManager(IRequestsManager requestManager)
        {
            this._requestManeger = requestManager;
        }

        public virtual MedioPago GetMedioPago(int Id)
        {
           
            JToken resp = _requestManeger.GetRequest("http://localhost:17479/", "mediodepago/" + Id).Result;

            if (resp != null)
                return JsonConvert.DeserializeObject<MedioPago>(resp.ToString());

            return null;

        }
    }
}
