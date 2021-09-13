using System;

namespace API.Dtos
{
    public class InvestmentBcdsDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }

        public int BcdsId { get; set; }
        public BcdsDto BcdsDto { get; set; }
        public int ResponsibleDoctorId { get; set; }
        public DoctorInfoDto DoctorInfoDto { get; set; }
    }
    }