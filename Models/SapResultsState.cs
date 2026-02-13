using costing_tool.pages;
using System.Collections.Generic;

public static class SapResultsState
{
    public static List<TableCOST>? CostSheetResults { get; set; }
    public static List<InitialCost>? InitialCostResults { get; set; }
    public static bool IsRunning { get; set; } = false;
    public static bool HasResults =>
        CostSheetResults != null && CostSheetResults.Count > 0;
    public static string[][]? LastMaterialFilters { get; set; }
}