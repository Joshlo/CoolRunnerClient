namespace CRClient.Responses
{
    public class PriceResponse
    {
        public string ZoneFrom { get; internal set; }
        public string ZoneTo { get; internal set; }
        public int WeightFrom { get; internal set; }
        public int WeightTo { get; internal set; }
        public string Title { get; internal set; }
        public int PriceInclTax { get; internal set; }
        public int PriceExclTax { get; internal set; }
        public string Reference { get; internal set; }

        internal static PriceResponse Map(dynamic o)
        {
            return new PriceResponse
            {
                ZoneFrom = o.zone_from,
                ZoneTo = o.zone_to,
                WeightFrom = o.weight_from,
                WeightTo = o.weight_to,
                Title = o.title,
                PriceInclTax = o.price_incl_tax,
                PriceExclTax = o.price_excl_tax,
                Reference = o.reference
            };
        }
    }
}
