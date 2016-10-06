using System;

namespace CRClient.Responses
{
    public class ShipmentInfoResponse
    {
        public long ShipmentId { get; internal set; }
        public long OrderId { get; internal set; }
        public long PackageNumber { get; internal set; }
        public string PdfBase64 { get; internal set; }
        public string PdfLink { get; internal set; }

        public static ShipmentInfoResponse Map(dynamic o)
        {
            return new ShipmentInfoResponse
            {
                ShipmentId = o.shipment_id,
                OrderId = o.order_id,
                PackageNumber = o.package_number,
                PdfBase64 = o.pdf_base64,
                PdfLink = o.pdf_link
            };
        }
    }
}