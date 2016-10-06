using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using CRClient.Enums;
using CRClient.Exceptions;
using CRClient.Extensions;
using CRClient.HttpClient;
using CRClient.Models;
using CRClient.Responses;
using Newtonsoft.Json;

namespace CRClient
{
    public class CoolRunnerClient : ICoolRunnerClient
    {
        private readonly IHttpCoolRunnerClient _client;
        private HttpResponseMessage _response;
        private string _baseUrl = "https://api.coolrunner.dk/v1";
        private JsonSerializerSettings _settings = new JsonSerializerSettings();

        public CoolRunnerClient() : this(new HttpCoolRunnerClient())
        {
        }

        public CoolRunnerClient(IHttpCoolRunnerClient httpClient)
        {
            _client = httpClient;
            _settings.ContractResolver = new UnderscoreMappingResolver();
        }

        public async Task<ShipmentResponse> CreateShipmentAsync(ShipmentModel model)
        {
            _response =
                await
                    _client.PostAsync($"{_baseUrl}/shipment/create", new FormUrlEncodedContent(model.ToCoolRunnerNamingFormat()));

            var json = await _response.Content.ReadAsStringAsync();

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return ShipmentResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public ShipmentResponse CreateShipment(ShipmentModel model)
        {
            _response =
                _client.PostAsync($"{_baseUrl}/shipment/create", new FormUrlEncodedContent(model.ToCoolRunnerNamingFormat())).Result;

            var json = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return ShipmentResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public async Task<PriceResponse> GetPriceAsync(ShipmentModel model)
        {
            _response = await _client.PostAsync($"{_baseUrl}/shipment/price", new FormUrlEncodedContent(model.ToCoolRunnerNamingFormat()));

            var json = await _response.Content.ReadAsStringAsync();

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return PriceResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public PriceResponse GetPrice(ShipmentModel model)
        {
            _response = _client.PostAsync($"{_baseUrl}/shipment/price", new FormUrlEncodedContent(model.ToCoolRunnerNamingFormat())).Result;
            var json = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return PriceResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public async Task<ShipmentInfoResponse> GetShipmentInfoAsync(long shipmentId)
        {
            _response = await _client.GetAsync($"{_baseUrl}/shipment/info/{shipmentId}");

            var json = await _response.Content.ReadAsStringAsync();
            
            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return ShipmentInfoResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public ShipmentInfoResponse GetShipmentInfo(long shipmentId)
        {
            _response = _client.GetAsync($"{_baseUrl}/shipment/info/{shipmentId}").Result;

            var json = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return ShipmentInfoResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public async Task<DroppointResponse> GetDroppointsAsync(Carrier carrier, DroppointModel model)
        {
            _response = await _client.PostAsync($"{_baseUrl}/droppoints/{carrier}", new FormUrlEncodedContent(model.ToCoolRunnerNamingFormat()));

            var json = await _response.Content.ReadAsStringAsync();

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return DroppointResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public DroppointResponse GetDroppoints(Carrier carrier, DroppointModel model)
        {
            _response = _client.PostAsync($"{_baseUrl}/droppoints/{carrier}", new FormUrlEncodedContent(model.ToCoolRunnerNamingFormat())).Result;

            var json = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return DroppointResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }


        public async Task<bool> DeletePackageLabelAsync(long packageNumber)
        {
            _response = await _client.GetAsync($"{_baseUrl}/shipment/delete/{packageNumber}");

            if (_response.IsSuccessStatusCode)
                return true;

            throw new CoolRunnerException(_response.StatusCode, await _response.Content.ReadAsStringAsync());
        }

        public bool DeletePackageLabel(long packageNumber)
        {
            _response = _client.GetAsync($"{_baseUrl}/shipment/delete/{packageNumber}").Result;

            if (_response.IsSuccessStatusCode)
                return true;

            throw new CoolRunnerException(_response.StatusCode, _response.Content.ReadAsStringAsync().Result);
        }

        public HttpStatusCode? StatusCode => _response?.StatusCode;
    }
}