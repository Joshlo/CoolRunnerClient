using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRClient.HttpClient
{
    internal class HttpCoolRunnerClient : IHttpCoolRunnerClient
    {
        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            throw new NotImplementedException();
        }
    }
}