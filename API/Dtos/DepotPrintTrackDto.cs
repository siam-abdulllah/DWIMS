using Core.Entities;
using System;

namespace API.Dtos
{
    public class DepotPrintTrackDto
    {
        public int Id { get; set; }
        public int? InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public string PayRefNo { get; set; }
        public string SAPRefNo { get; set; }
        public string DepotId { get; set; }
        public string DepotName { get; set; }
        public string Remarks { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public string PaymentRefNo { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTimeOffset? LastPrintTime { get; set; }
        public int PrintCount { get; set; }
    }
}
