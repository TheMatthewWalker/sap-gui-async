using System.Collections.Generic;
using System;

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
        public DateTimeOffset? CreationDate { get; set; }   // ← DateTimeOffset for DatePicker
        public DateTimeOffset? DateTo { get; set; }         // ← DateTimeOffset for DatePicker
        public string CreationTime { get; set; }
        public string Comment { get; set; }

        // Helper properties to convert to dd.mm.yyyy for the API
        public string CreationDateFormatted =>
            CreationDate.HasValue ? CreationDate.Value.ToString("dd.MM.yyyy") : null;
        public string DateToFormatted =>
            DateTo.HasValue ? DateTo.Value.ToString("dd.MM.yyyy") : null;


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


    // ── Internal DTO to control exactly what gets sent to the API ──
    public class MixingSearchDto
    {
        public string mixingID { get; set; }
        public string mixCode { get; set; }
        public string totalWeight { get; set; }
        public string shift { get; set; }
        public string @operator { get; set; }
        public string supplierBatch { get; set; }
        public string batchTub { get; set; }
        public string creationDate { get; set; }   // Sent as dd.MM.yyyy string
        public string dateTo { get; set; }         // Sent as dd.MM.yyyy string
        public string creationTime { get; set; }
        public string comment { get; set; }
    }


}

