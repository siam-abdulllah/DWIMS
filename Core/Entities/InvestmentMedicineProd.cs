using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentMedicineProd : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public MedicineProduct MedicineProduct { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public double TpVat { get; set; }
        public int BoxQuantity { get; set; }
    }
}
