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
        IRequestsManager _requestManeger;
        public MedioPagoManager(IRequestsManager requestManager)
        {
            this._requestManeger = requestManager;
        }

        public MedioPago GetMedioPago(int Id)
        {
            IEnumerable<MedioPago> mediosPago = new List<MedioPago>()
            {
                new MedioPago(){ Id=1,Descripcion="MercadoPago"},
                new MedioPago(){ Id=2,Descripcion="Visa Debito"},
                new MedioPago(){ Id=3,Descripcion="Visa"},
                new MedioPago(){ Id=4,Descripcion="MasterCard"},
                new MedioPago(){ Id=5,Descripcion="American Express"}
            };
            return mediosPago.Where(x => x.Id == Id).SingleOrDefault<MedioPago>();
            

            JToken resp = _requestManeger.GetRequest("mediodepago/" + Id).Result;

            if (resp != null)
                return JsonConvert.DeserializeObject<MedioPago>(resp.ToString());

            return null;

        }
    }
}
