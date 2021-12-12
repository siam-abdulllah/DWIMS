
namespace Core.Entities
{
    public class LastFiveInvestmentInfo : BaseEntity
    {
        public string? DonationShortName { get; set; }
        public long? InvestmentAmount { get; set; }
        public string? ComtSharePrcntAll { get; set; }
        public string? ComtSharePrcnt { get; set; }
        public double? PrescribedSharePrcnt { get; set; }
        public double? PrescribedSharePrcntAll { get; set; }
       
       
        
        
    }
}
