using System;

namespace costing_tool.Models
{
    public class PalletPackages
    {
        // IDENTITY column — assigned by SQL Server on insert. Do not set manually.
        public long? PalletItemID { get; set; }
        public long? PalletID { get; set; }
        public long? PackagingID { get; set; }
        public int? PalletLayer { get; set; }
        public string? SapMaterial { get; set; }
        public decimal? SapQuantity { get; set; }
        public string? SapBatch { get; set; }
        public string? SapDelivery { get; set; }
        public string? SapDeliveryItem { get; set; }
        public string? SapCustomer { get; set; }
        public string? SapCustomerMaterial { get; set; }
        public DateTime? ScanTime { get; set; }
    }
}
