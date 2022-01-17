using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class InvestmentDetailDto
    {
        public int Id { get; set; }
        public string ChequeTitle { get; set; }
        public string PaymentMethod { get; set; }
        public string CommitmentAllSBU { get; set; }
        public string CommitmentOwnSBU { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public int TotalMonth { get; set; }
        public double ProposedAmount { get; set; }
        public string Purpose { get; set; }
        public int InvestmentInitId { get; set; }

    }
}