using costing_tool.pages;
using System;
using System.Collections.Generic;

namespace costing_tool.Models
{
    public class CostSheetResult
    {
        public List<TableCOST> AllData { get; set; } = new();
        public List<InitialCost> QuickData { get; set; } = new();

        // Optional extras
        public DateTime RetrievedAt { get; set; } = DateTime.Now;
        public int TotalRows =>
            AllData.Count + QuickData.Count;
    }
}