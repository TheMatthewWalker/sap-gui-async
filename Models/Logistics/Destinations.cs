using System;

namespace costing_tool.Models
{
    public class Destinations
    {
        public long? DestinationID { get; set; }
        public string? DestinationName { get; set; }
        public string? DestinationStreet { get; set; }
        public string? DestinationCity { get; set; }
        public string? DestinationPostCode { get; set; }
        public string? DestinationCountry { get; set; }
        public string? DefaultIncoterms { get; set; }
        public string? DestinationComment { get; set; }
        public string? DestinationEmail { get; set; }
        public string? DestinationZone { get; set; }
    }
}
