using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace costing_tool.Models
{
    public class SearchCriteriaRow
    {
        public int RowNumber { get; set; } // For automatic numbering
        public string? Material { get; set; }
        public string? Volume { get; set; }
        public string? Incoterms { get; set; }
        public string? Country { get; set; }
    }
}
