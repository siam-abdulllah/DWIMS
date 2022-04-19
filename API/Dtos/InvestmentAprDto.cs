using System;
using Core.Entities;

namespace API.Dtos
{
    public class InvestmentAprDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string ChequeTitle { get; set; }
        public string PaymentMethod { get; set; }
        public string CommitmentAllSBU { get; set; }
        public string CommitmentOwnSBU { get; set; }
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public int TotalMonth { get; set; }
        public double ProposedAmount { get; set; }
        public string Purpose { get; set; }
    }
}
