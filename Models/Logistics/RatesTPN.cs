using System;

namespace costing_tool.Models
{
    public class RatesTPN
    {
        public string? PostalZone { get; set; }
        public string? PalletCategory { get; set; }
        public string? ServiceLevel { get; set; }
        public decimal? AgreedRate { get; set; }
    }
}
