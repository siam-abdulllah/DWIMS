using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ChangeDepot : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public DateTimeOffset ChangeDate { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Remarks { get; set; }
        public string OldDepotCode { get; set; }
        public string DepotCode { get; set; }
    }

    public class ChangeDepotSearch : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string DonationTo { get; set; }
        public string ProposeFor { get; set; }
        public string DoctorName { get; set; }
        public double ProposedAmount { get; set; }
        public string MarketName { get; set; }
        public string EmployeeName { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
    }
}
