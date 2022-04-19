using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class InvestmentDoctorDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public string DoctorCategory { get; set; }
        public string DoctorType { get; set; }
        public string PracticeDayPerMonth { get; set; }
        public string PatientsPerDay { get; set; }
        public int DoctorId { get; set; }
        public DoctorInfoDto DoctorInfoDto { get; set; }
        public int InstitutionId { get; set; }
        public InstitutionInfoDto InstitutionInfoDto { get; set; }

    }
}