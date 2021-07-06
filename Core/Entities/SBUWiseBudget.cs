using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class SBUWiseBudget : BaseEntity
    {
        public int SBUId { get; set; }
        public SBU SBU { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long Amount { get; set; }
        public string Remarks { get; set; }
    }
}
