
using System;

namespace Core.Entities
{
    public class LastFiveInvestmentInfo : BaseEntity
    {
        public string? DonationShortName { get; set; }
        public double? InvestmentAmount { get; set; }
        public string? ComtSharePrcntAll { get; set; }
        public string? ComtSharePrcnt { get; set; }
        public double? PrescribedSharePrcnt { get; set; }
        public double? PrescribedSharePrcntAll { get; set; }
        public DateTimeOffset? FromDate { get; set; }
       
       
        
        
    }
}
