using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CRClient.HttpClient
{
    internal class HttpCoolRunnerClient : IHttpCoolRunnerClient
    {
        private readonly System.Net.Http.HttpClient _client;

        public HttpCoolRunnerClient()
        {
            _client = new System.Net.Http.HttpClient();
        }

        public void SetCredentials(AuthenticationHeaderValue authorize)
        {
            _client.DefaultRequestHeaders.Authorization = authorize;
        }

        public void SetCallerIdentifier(string caller)
        {
            _client.DefaultRequestHeaders.Add("X-Developer_Id", caller);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }
    }
}