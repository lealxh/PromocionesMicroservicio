using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Promociones.Domain.Core;
using RestSharp;

using System.Threading;
using System.Threading.Tasks;

namespace Promociones.Infrastructure
{
    public class RequestManager : IRequestsManager
    {
        public async Task<JToken> GetRequest(string url)
        {
            var client = new RestClient();
            var request = new RestRequest(url);
            var cancellationTokenSource = new CancellationTokenSource();

            var restResponse = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
            if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                return JToken.Parse(restResponse.Content);
            else
                return null;
        }

      
    }
}
