using System.Collections.Generic;

namespace Core.Entities
{
    public class InvestmentType : BaseEntity
    {
        public string InvesetmentTypeName { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }
    }
}
