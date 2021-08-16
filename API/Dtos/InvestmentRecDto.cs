using System;
using Core.Entities;


namespace API.Dtos
{
    public class InvestmentRecDto
    {
        public int Id { get; set; }
        public int? InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public string ChequeTitle { get; set; }
        public string PaymentMethod { get; set; }
        public string CommitmentAllSBU { get; set; }
        public string CommitmentOwnSBU { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public string TotalMonth { get; set; }
        public long ProposedAmount { get; set; }
        public string Purpose { get; set; }
    }
}
