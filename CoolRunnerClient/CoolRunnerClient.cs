using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoolRunnerClient.Models;
using Newtonsoft.Json;

namespace CoolRunnerClient
{
    public class CoolRunnerClient : ICoolRunnerClient
    {
        private HttpClient _client;
        private HttpResponseMessage _response;

        public CoolRunnerClient()
        {
            _client = new HttpClient();
        }

        public Task PostAsync()
        {
            throw new NotImplementedException();
        }

        public void Post()
        {
            throw new NotImplementedException();
        }

        public async Task<PriceModel> GetPriceAsync(string url)
        {
            _response = await _client.GetAsync(url);
            var json = await _response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PriceModel>(json);
        }

        public PriceModel GetPrice(string url)
        {
            _response = _client.GetAsync(url).Result;
            var json = _response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<PriceModel>(json);
        }

        public Task<IEnumerable<PriceModel>> GetPricesAsync(string url)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PriceModel> GetPrices(string url)
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode? StatusCode => _response?.StatusCode;
    }
}
