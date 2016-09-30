using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CoolRunnerClient.Models;

namespace CoolRunnerClient
{
    public interface ICoolRunnerClient
    {
        Task CreateShipmentAsync();
        void CreateShipment();
        Task CreateShipmentsAsync();
        void CreateShipments();
        Task<PriceModel> GetPriceAsync(string url);
        PriceModel GetPrice(string url);
        Task<IEnumerable<PriceModel>> GetPricesAsync(string url);
        IEnumerable<PriceModel> GetPrices(string url);

        HttpStatusCode? StatusCode { get; }
    }
}