using System.Collections.Generic;
using CRClient.Enums;

namespace CRClient.Responses
{
    public class FreightRatesResponse
    {
        public Dictionary<string, List<FreightRate>> Countries { get; internal set; } = new Dictionary<string, List<FreightRate>>();

        public static FreightRatesResponse Map(dynamic result)
        {
            return new FreightRatesResponse();
        }
    }

    public class FreightRate
    {
        public Carrier Carrier { get; internal set; }
        public CarrierProduct CarrierProduct { get; internal set; }
        public CarrierService CarrierService { get; internal set; }
        public string Title { get; internal set; }
        public int WeightFrom { get; internal set; }
        public int WeightTo { get; internal set; }
        public int PriceInclTax { get; internal set; }
        public int PriceExclTax { get; internal set; }
    }
}