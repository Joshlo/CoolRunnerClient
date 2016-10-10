using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CRClient.HttpClient
{
    internal class HttpCoolRunnerClient : IHttpCoolRunnerClient
    {
        private readonly System.Net.Http.HttpClient _client;

        public HttpCoolRunnerClient(AuthenticationHeaderValue authentication, string xDeveloperId)
        {
            _client = new System.Net.Http.HttpClient();
            _client.DefaultRequestHeaders.Authorization = authentication;
            if(!string.IsNullOrWhiteSpace(xDeveloperId))
                _client.DefaultRequestHeaders.Add("X-Developer_Id", xDeveloperId);
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