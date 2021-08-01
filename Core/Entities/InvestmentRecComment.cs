using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentRecComment : BaseEntity
    {
        public int InvestmenRecId { get; set; }
        [ForeignKey("InvestmenRecId")]
        public InvestmentRec InvestmentRec { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public string Comments { get; set; }
        public string RecStatus { get; set; }
    }
}
