
namespace Core.Entities
{
    public class RptSBUWiseExpSummart : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public string SBUName { get; set; }
        public int SBU { get; set; }
        public string DonationType { get; set; }
        public int DonationId { get; set; }
        public long Expense { get; set; }
        public long Budget { get; set; }
    }
}
