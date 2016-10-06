namespace CRClient.Responses
{
    public class ShipmentResponse
    {
        public long OrderId { get; internal set; }
        public int GrandTotalExclTax { get; internal set; }
        public int GrandTotalInclTax { get; internal set; }
        public string Reference { get; internal set; }
        public int ShipmentId { get; internal set; }
        public int PriceExclTax { get; internal set; }
        public int PriceInclTax { get; internal set; }
        public long PackageNumber { get; internal set; }
        public string LabellessCode { get; internal set; }
        public string PdfBase64 { get; internal set; }
        public string PdfLink { get; internal set; }

        public static ShipmentResponse Map(dynamic o)
        {
            return new ShipmentResponse
            {
                OrderId = o.order_id,
                GrandTotalExclTax = o.grand_total_excl_tax,
                GrandTotalInclTax = o.grand_total_incl_tax,
                Reference = o.reference,
                ShipmentId = o.shipment_id,
                PriceExclTax = o.price_excl_tax,
                PriceInclTax = o.price_incl_tax,
                PackageNumber = o.package_number,
                LabellessCode = o.labelless_code,
                PdfBase64 = o.pdf_base64,
                PdfLink = o.pdf_link
            };
        }
    }
}
