using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRecCommentDto
    {
        public int Id { get; set; }
        public int? InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Comments { get; set; }
        public string RecStatus { get; set; }
    }
}
