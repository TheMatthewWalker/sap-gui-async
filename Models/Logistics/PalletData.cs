using System;

namespace costing_tool.Models
{
    public class PalletData
    {
        public string? PalletID { get; set; }
        public string? PalletDescription { get; set; }
        public decimal? PalletWeight { get; set; }
        public int? PalletLength { get; set; }
        public int? PalletWidth { get; set; }
        public int? PalletHeight { get; set; }
    }
}
