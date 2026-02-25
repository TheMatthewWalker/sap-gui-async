using System;

namespace costing_tool.Models
{
    public class PalletMain
    {
        // IDENTITY column — assigned by SQL Server on insert. Do not set manually.
        public long? PalletID { get; set; }
        public string? PalletType { get; set; }
        public bool? PalletFinish { get; set; }
        public decimal? PackagingWeight { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? PalletVolume { get; set; }
        public int? PalletLength { get; set; }
        public int? PalletWidth { get; set; }
        public int? PalletHeight { get; set; }
        public bool? PalletRemoved { get; set; }
        public string? PalletCategory { get; set; }
        public string? PalletLocation { get; set; }
        public DateTime? PalletCreationDate { get; set; }
        public DateTime? PalletFinishDate { get; set; }
    }
}
