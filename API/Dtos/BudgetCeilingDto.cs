using System;
namespace API.Dtos
{
    public class BudgetCeilingDto
    {
        public long AmountPerTransacion { get; set; }
        public long AmountPerMonth { get; set; }
        public long Additional { get; set; }
        public string DonationType { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public long Amount { get; set; }
    }
}