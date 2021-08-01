using System;
using Core.Entities;


namespace API.Dtos
{
    public class InvestmentRecDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
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
