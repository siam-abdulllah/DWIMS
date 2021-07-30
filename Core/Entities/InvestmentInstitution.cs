using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentInstitution : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int InstitutionId { get; set; }
        [ForeignKey("InstitutionId")]
        public InstitutionInfo InstitutionInfo { get; set; }
        public int ResposnsibleDoctorId { get; set; }
        [ForeignKey("ResposnsibleDoctorId")]
        public DoctorInfo DoctorInfo { get; set; }
        public string NoOfBed { get; set; }
        public string DepartmentUnit { get; set; }
    }
}
