using System;

namespace costing_tool.Models
{
    public class ForwarderApproval
    {
        public long? ForwarderID { get; set; }
        public bool? RatesAgreed { get; set; }
        public bool? UsageAgreed { get; set; }
    }
}
