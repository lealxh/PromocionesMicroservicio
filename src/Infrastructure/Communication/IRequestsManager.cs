using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace Promociones.Infrastructure
{
    public interface IRequestsManager
    {
        Task<JToken> GetRequest(string url);

    }
}
