using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolRunnerClient.Models
{
    public class PriceModel
    {
        public string ZoneFrom { get; internal set; }
        public string ZoneTo { get; internal set; }
        public int WeightFrom { get; internal set; }
        public int WeightTo { get; internal set; }
        public string Title { get; internal set; }
        public int PriceInclTax { get; internal set; }
        public int PriceExclTax { get; internal set; }
        public string Reference { get; internal set; }
    }
}
