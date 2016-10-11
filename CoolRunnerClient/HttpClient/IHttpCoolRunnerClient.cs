using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CRClient.HttpClient
{
    public interface IHttpCoolRunnerClient
    {
        void SetCallerIdentifier(string caller);
        void SetCredentials(AuthenticationHeaderValue authorize);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}