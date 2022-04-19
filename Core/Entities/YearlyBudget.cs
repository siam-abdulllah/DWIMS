using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
   public class YearlyBudget : BaseEntity
    {
        public long Amount { get; set; }
        public string Year { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
    }
}
