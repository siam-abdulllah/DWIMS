using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class DepotPrintTrack : BaseEntity
    {
        public int? InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public string PayRefNo { get; set; }
        public string SAPRefNo { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Remarks { get; set; }
        public string PaymentRefNo { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTimeOffset? LastPrintTime { get; set; }
        public int PrintCount { get; set; }  
        public string ChequeNo { get; set; }
        public string BankName { get; set; }
    }
}