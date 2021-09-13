using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentTargetedProd : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public string SBU { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductInfo ProductInfo { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
