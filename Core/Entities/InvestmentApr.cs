using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
   public class InvestmentApr : BaseEntity
    {
        public int? InvestmentRecId { get; set; }
        [ForeignKey("InvestmentRecId")]
        public InvestmentRec InvestmentRec { get; set; }
        public double ProposedAmt { get; set; }
        public string InvestmentPurpose { get; set; }
        public string CommitmentAllSBU { get; set; }
        public string CommitmentOwnSBU { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string PaymentMethod { get; set; }
        public string ChequeTitle { get; set; }
    }
}
