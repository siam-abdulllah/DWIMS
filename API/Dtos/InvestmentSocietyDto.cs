using System;

namespace API.Dtos
{
    public class InvestmentSocietyDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }

        public int SocietyId { get; set; }
        public SocietyDto SocietyDto { get; set; }
    }
    }