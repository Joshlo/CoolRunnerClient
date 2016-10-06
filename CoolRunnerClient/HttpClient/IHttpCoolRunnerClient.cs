using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CRClient.HttpClient
{
    public interface IHttpCoolRunnerClient
    {
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string url);
    }
}