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
        Task<ShipmentResponse> CreateShipmentAsync(ShipmentModel model);
        ShipmentResponse CreateShipment(ShipmentModel model);
        Task<PriceResponse> GetPriceAsync(ShipmentModel model);
        PriceResponse GetPrice(ShipmentModel model);
        Task<ShipmentInfoResponse> GetShipmentInfoAsync(long shipmentId);
        ShipmentInfoResponse GetShipmentInfo(long shipmentId);
        Task<DroppointResponse> GetDroppointsAsync(Carrier carrier, DroppointModel model);
        DroppointResponse GetDroppoints(Carrier carrier, DroppointModel model);
        Task<FreightRatesResponse> GetFreightRatesAsync(string fromCountryIso);
        FreightRatesResponse GetFreightRates(string fromCountryIso);
        Task<bool> DeletePackageLabelAsync(long packageNumber);
        bool DeletePackageLabel(long packageNumber);
        Task<TrackingResponse> GetTrackingDataAsync(long packageNumber);
        TrackingResponse GetTrackingData(long packageNumber);
        HttpStatusCode? StatusCode { get; }
    }
}