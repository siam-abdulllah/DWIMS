using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentAprComment : BaseEntity
    {
        public int? InvestmentAprId { get; set; }
        [ForeignKey("InvestmentAprId")]
        public InvestmentApr InvestmentApr { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string Comments { get; set; }
        public string RecStatus { get; set; }
    }
}
