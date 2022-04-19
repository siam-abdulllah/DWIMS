using System;

namespace API.Dtos
{
    public class InvestmentInstitutionDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        
        //public InvestmentInitDto InvestmentInit { get; set; }
        public int InstitutionId { get; set; }
        
        public InstitutionInfoDto InstitutionInfoDto { get; set; }
        public int ResponsibleDoctorId { get; set; }
        public DoctorInfoDto DoctorInfoDto { get; set; }
        public string NoOfBed { get; set; }
        public string DepartmentUnit { get; set; }
    }
}