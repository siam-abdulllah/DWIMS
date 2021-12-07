namespace Core.Entities
{
    public class Donation:BaseEntity
    {
        public string DonationTypeName { get; set; }
        public string DonationShortName { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        
    }
}