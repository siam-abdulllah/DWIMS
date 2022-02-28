using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class InvestmentDetailDto
    {
        public int Id { get; set; }
        public string ChequeTitle { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentFreq { get; set; }
        public string CommitmentAllSBU { get; set; }
        public string CommitmentOwnSBU { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public DateTime CommitmentFromDate { get; set; }
        public DateTime CommitmentToDate { get; set; }
        public int TotalMonth { get; set; }
        public int CommitmentTotalMonth { get; set; }
        public double ProposedAmount { get; set; }
        public string Purpose { get; set; }
        public int InvestmentInitId { get; set; }

    }
}