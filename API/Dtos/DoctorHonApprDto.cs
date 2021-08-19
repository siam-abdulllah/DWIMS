using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class DoctorHonApprDto
    {
        public int Id { get; set; }
        public int? InvestmentInitId { get; set; }
        public InvestmentInitDto InvestmentInit { get; set; }
        public int DoctorId { get; set; }
        public DoctorInfoDto DoctorInfo { get; set; }
        public long HonAmount { get; set; }
        public string HonMonth { get; set; }
        public string Status { get; set; }
    }
}