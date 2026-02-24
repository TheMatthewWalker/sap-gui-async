using System.Collections.Generic;

namespace costing_tool.Models
{

    public class MixingCriteria
    {
        public int RowNumber { get; set; }
        public string MixingID { get; set; }
        public string MixCode { get; set; }
        public string TotalWeight { get; set; }
        public string Shift { get; set; }
        public string Operator { get; set; }
        public string SupplierBatch { get; set; }
        public string BatchTub { get; set; }
        public string CreationDate { get; set; }
        public string DateTo { get; set; }        // ← Added for date range search
        public string CreationTime { get; set; }
        public string Comment { get; set; }
    }
    public class Mixing
    {
        public string MixingID { get; set; }
        public string MixCode { get; set; }
        public string TotalWeight { get; set; }
        public string Shift { get; set; }
        public string Operator { get; set; }
        public string SupplierBatch { get; set; }
        public string BatchTub { get; set; }
        public string CreationDate { get; set; }
        public string CreationTime { get; set; }
        public string Comment { get; set; }
    }

    public static class SqlMixingResults
    {
        public static List<Mixing>? MixingResults { get; set; }
        public static bool IsRunning { get; set; } = false;
        public static bool HasResults =>
            MixingResults != null && MixingResults.Count > 0;
        public static string[][]? LastMaterialFilters { get; set; }
    }
}

