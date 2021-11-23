
namespace Core.Entities
{
    public class RptSBUWiseExpSummart : BaseEntity
    {
        public string SBUName { get; set; }
        public string SBU { get; set; }
        public string DonationTypeName { get; set; }
        public int DonationId { get; set; }
        public long Expense { get; set; }
        public long Budget { get; set; }
    }
}
