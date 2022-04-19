using Core.Entities;
using System;

namespace API.Dtos
{
    public class MedicineDispatchDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public string PayRefNo { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public string SAPRefNo { get; set; }
        public string IssueReference { get; set; }
        public DateTimeOffset IssueDate { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public double ProposeAmt { get; set; }
        public double DispatchAmt { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public string Remarks { get; set; }
    }


    public class RptMedicineDispatchSearchDto
    {
        public DateTimeOffset FromDate { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public string DepotCode { get; set; }
        public string DonationId { get; set; }
        public string DisStatus { get; set; }

    }
}
