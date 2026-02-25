using System;

namespace costing_tool.Models
{
    public class ShipmentMain
    {
        // IDENTITY column — assigned by SQL Server on insert. Do not set manually.
        public long? ShipmentID { get; set; }
        public long? OriginID { get; set; }
        public string? OriginName { get; set; }
        public string? OriginStreet { get; set; }
        public string? OriginCity { get; set; }
        public string? OriginPostCode { get; set; }
        public string? OriginCountry { get; set; }
        public long? DestinationID { get; set; }
        public string? DestinationName { get; set; }
        public string? DestinationStreet { get; set; }
        public string? DestinationCity { get; set; }
        public string? DestinationPostCode { get; set; }
        public string? DestinationCountry { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? GrossWeight { get; set; }
        public long? PalletCount { get; set; }
        public decimal? ShipmentVolume { get; set; }
        public DateTime? PlannedCollection { get; set; }
        public DateTime? ActualCollection { get; set; }
        public bool? CollectionStatus { get; set; }
        public long? ForwarderID { get; set; }
        public string? TrackingNumber { get; set; }
        public string? IncoTerms { get; set; }
        public bool? CustomsRequired { get; set; }
        public bool? CustomsComplete { get; set; }
        public bool? ShipmentCancelled { get; set; }
    }
}
