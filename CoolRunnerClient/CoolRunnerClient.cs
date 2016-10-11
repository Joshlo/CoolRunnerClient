using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
        private bool _hasSetCredentials;

        public CoolRunnerClient()
        {
            _client = new HttpCoolRunnerClient();
        }

        public CoolRunnerClient(string username, string passwordOrToken, string callerIndetifier = null)
        {
            _client = new HttpCoolRunnerClient();
            SetCredentials(username, passwordOrToken);
            SetCallerIdentifier(callerIndetifier);
        }

        /// <summary>
        /// This constructor is only meant for testing purposes.
        /// </summary>
        /// <param name="httpClient">To mock HttpCoolRunnerClient.</param>
        protected CoolRunnerClient(IHttpCoolRunnerClient httpClient)
        {
            _client = httpClient;
        }

        public void SetCredentials(string username, string passwordOrToken)
        {
            var authentication = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{passwordOrToken}")));
            _client.SetCredentials(authentication);
            _hasSetCredentials = true;
        }

        public void SetCallerIdentifier(string callerIdentifier)
        {
            if(!string.IsNullOrWhiteSpace(callerIdentifier))
                _client.SetCallerIdentifier(callerIdentifier);
        }

        /// <summary>
        /// Creates a new shipment.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A label and the data for that label.</returns>
        public async Task<ShipmentResponse> CreateShipmentAsync(ShipmentModel model)
        {
            if(!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

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

        /// <summary>
        /// Creates a new shipment.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A label and the data for that label.</returns>
        public ShipmentResponse CreateShipment(ShipmentModel model)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

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

        /// <summary>
        /// Gets the price for a shipment.
        /// </summary>
        /// <param name="model">The shipment that the price will be based on.</param>
        /// <returns>A price model</returns>
        public async Task<PriceResponse> GetPriceAsync(ShipmentModel model)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

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
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

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
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

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
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

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
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

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
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

            _response = _client.PostAsync($"{_baseUrl}/droppoints/{carrier}", new FormUrlEncodedContent(model.ToCoolRunnerNamingFormat())).Result;

            var json = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return DroppointResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public async Task<FreightRatesResponse> GetFreightRatesAsync(string fromCountryIso)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

            _response = await _client.GetAsync($"{_baseUrl}/freight_rates/{fromCountryIso}");

            var json = await _response.Content.ReadAsStringAsync();

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return FreightRatesResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public FreightRatesResponse GetFreightRates(string fromCountryIso)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

            _response = _client.GetAsync($"{_baseUrl}/freight_rates/{fromCountryIso}").Result;

            var json = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return FreightRatesResponse.Map(obj.result);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }


        public async Task<bool> DeletePackageLabelAsync(long packageNumber)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

            _response = await _client.GetAsync($"{_baseUrl}/shipment/delete/{packageNumber}");

            if (_response.IsSuccessStatusCode)
                return true;

            throw new CoolRunnerException(_response.StatusCode, await _response.Content.ReadAsStringAsync());
        }

        public bool DeletePackageLabel(long packageNumber)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

            _response = _client.GetAsync($"{_baseUrl}/shipment/delete/{packageNumber}").Result;

            if (_response.IsSuccessStatusCode)
                return true;

            throw new CoolRunnerException(_response.StatusCode, _response.Content.ReadAsStringAsync().Result);
        }

        public async Task<TrackingResponse> GetTrackingDataAsync(long packageNumber)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

            _response = await _client.GetAsync($"{_baseUrl}/tracking/{packageNumber}");

            var json = await _response.Content.ReadAsStringAsync();

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return TrackingResponse.Map(obj);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public TrackingResponse GetTrackingData(long packageNumber)
        {
            if (!_hasSetCredentials)
                throw new AuthenticationException("Credentials not set");

            _response = _client.GetAsync($"{_baseUrl}/tracking/{packageNumber}").Result;

            var json = _response.Content.ReadAsStringAsync().Result;

            if (_response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<dynamic>(json);
                return TrackingResponse.Map(obj);
            }

            throw new CoolRunnerException(_response.StatusCode, json);
        }

        public HttpStatusCode? StatusCode => _response?.StatusCode;
    }
}