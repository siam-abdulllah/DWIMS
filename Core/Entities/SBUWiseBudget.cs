using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class SBUWiseBudget : BaseEntity
    {
        public string SBU { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public long Amount { get; set; }
        public string Remarks { get; set; }
    }
}
