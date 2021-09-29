using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    class InvestmentDetailTracker : BaseEntity
    {
        public int? InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string DonationType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public long ApprovedAmount { get; set; }
        public string PaidStatus { get; set; }
    }
}
