namespace costing_tool.Models
{
    public class TOCreateResult
    {
        public string? TransferOrderNumber { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
        public bool Success => string.IsNullOrEmpty(Error);
    }
}