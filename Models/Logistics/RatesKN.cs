using System;

namespace costing_tool.Models
{
    public class RatesKN
    {
        public string? CountryCode { get; set; }
        public string? PostalCode { get; set; }
        public int? MinWeight { get; set; }
        public int? MaxWeight { get; set; }
        public decimal? AgreedRate { get; set; }
        public int? TransitTime { get; set; }
    }
}
