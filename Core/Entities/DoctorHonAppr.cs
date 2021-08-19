using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class DoctorHonAppr : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public DoctorInfo DoctorInfo { get; set; }
        public long HonAmount { get; set; }
        public string HonMonth { get; set; }
        public string Status { get; set; }
    }
}
