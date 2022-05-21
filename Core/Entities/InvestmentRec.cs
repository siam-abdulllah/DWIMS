using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentRec : BaseEntity
    {
        public int? InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
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
        public bool? CompletionStatus { get; set; }
        public int Priority { get; set; }
    }
    public class PipeLineExpense: BaseEntity
    {
        
        public double Pipeline { get; set; }
        public string SBUName { get; set; }
        public string SBUCode { get; set; }
    }

    public class AppAuthDetails : BaseEntity
    {
        public string Authority { get; set; }
        public int Priority { get; set; }
        public int AuthId { get; set; }
        public int NoOfEmployee { get; set; }
        public int Expense { get; set; }
        public int NewAmount { get; set; }
        public int Amount { get; set; }
    }
    public class TotalExpense : BaseEntity
    {

        public double Expense { get; set; }
     
    }
    public class TotalPipeLine : BaseEntity
    {

        public double PipeLine { get; set; }

    }

    public class AuthExpense : BaseEntity
    {
        public double Remarks { get; set; }
        public double Expense { get; set; }

    }
}
