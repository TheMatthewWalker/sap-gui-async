using System;

namespace costing_tool.Models
{
    public class DeliveryMain
    {
        public long? DeliveryID { get; set; }
        public long? CustomerID { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public bool? CompletionStatus { get; set; }
        public string? OperatorName { get; set; }
        public string? SupervisorName { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? PalletCount { get; set; }
        public decimal? DeliveryVolume { get; set; }
        public string? PicksheetComment { get; set; }
        public bool? DeliveryCancelled { get; set; }
        public int? DeliveryPriority { get; set; }
    }
}
