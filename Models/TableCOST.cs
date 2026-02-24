namespace costing_tool.Models
{
    public class TableCOST
    {
        public string? Material { get; set; }
        public string? CostingDate { get; set; }
        public string? ProfitCenter { get; set; }
        public decimal DirectMaterial { get; set; }
        public decimal DirectLabour { get; set; }
        public decimal VariableProductionCost { get; set; }
        public decimal Scrap { get; set; }
        public decimal InboundFreight { get; set; }
        public decimal OutboundFreight { get; set; }
        public decimal Customs { get; set; }
        public decimal Packaging { get; set; }
        public decimal ExtrusionLabour { get; set; }
        public decimal Total { get; set; }
        public string? Unit { get; set; }
    }
}