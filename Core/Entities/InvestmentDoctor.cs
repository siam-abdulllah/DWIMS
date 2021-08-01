using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentDoctor : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public DoctorInfo DoctorInfo { get; set; }
        public int InstitutionId { get; set; }
        [ForeignKey("InstitutionId")]
        public InstitutionInfo InstitutionInfo { get; set; }
        public string DoctorCategory { get; set; }
        public string DoctorType { get; set; }
        public string PracticeDayPerMonth { get; set; }
        public string PatientsPerDay { get; set; }
    }
}
