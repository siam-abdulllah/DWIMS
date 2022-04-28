using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentDetailTracker : BaseEntity
    {
        public int? InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int DonationId { get; set; }
        [ForeignKey("DonationId")]
        public Donation Donation { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public double ApprovedAmount { get; set; }
        public string PaidStatus { get; set; }
        public string PaymentRefNo { get; set; }
    }
  
}
