using System;

namespace costing_tool.Models
{
    public class PackagingData
    {
        public string? PackID { get; set; }
        public string? PackMaterial { get; set; }
        public string? PackDescription { get; set; }
        public decimal? PackWeight { get; set; }
        public int? PackLength { get; set; }
        public int? PackWidth { get; set; }
        public int? PackHeight { get; set; }
    }
}
