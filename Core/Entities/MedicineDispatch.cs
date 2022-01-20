using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MedicineDispatch : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
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
}
