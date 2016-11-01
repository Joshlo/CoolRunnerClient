using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CRClient.Enums;
using CRClient.Models;
using CRClient.Responses;

namespace CRClient
{
    public interface ICoolRunnerClient
    {
        void SetCredentials(string username, string passwordOrToken);
        void SetCallerIdentifier(string callerIdentifier);
        Task<ShipmentResponse> CreateShipmentAsync(ShipmentModel model);
        ShipmentResponse CreateShipment(ShipmentModel model);
        Task<PriceResponse> GetPriceAsync(ShipmentModel model);
        PriceResponse GetPrice(ShipmentModel model);
        Task<ShipmentInfoResponse> GetShipmentInfoAsync(string shipmentId);
        ShipmentInfoResponse GetShipmentInfo(string shipmentId);
        Task<DroppointResponse> GetDroppointsAsync(Carrier carrier, DroppointModel model);
        DroppointResponse GetDroppoints(Carrier carrier, DroppointModel model);
        Task<FreightRatesResponse> GetFreightRatesAsync(string fromCountryIso);
        FreightRatesResponse GetFreightRates(string fromCountryIso);
        Task<bool> DeletePackageLabelAsync(string packageNumber);
        bool DeletePackageLabel(string packageNumber);
        Task<TrackingResponse> GetTrackingDataAsync(string packageNumber);
        TrackingResponse GetTrackingData(string packageNumber);
        HttpStatusCode? StatusCode { get; }
    }
}