using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentRecDepot : BaseEntity
    {
        public int? InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
    }
}
