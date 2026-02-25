using System;

namespace costing_tool.Models
{
    public class ShipmentCost
    {
        // IDENTITY column — assigned by SQL Server on insert. Do not set manually.
        public long? CostID { get; set; }
        public long? ShipmentID { get; set; }
        public string? CostType { get; set; }
        public string? CostElement { get; set; }
        public string? CostCenter { get; set; }
        public decimal? ExpectedCost { get; set; }
        public decimal? ActualCost { get; set; }
        public bool? MigoStatus { get; set; }
        public string? MaterialDocument { get; set; }
    }
}
