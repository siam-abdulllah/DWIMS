
namespace Core.Entities
{
    public class ReportInvestmentInfo : BaseEntity
    {
        public string MarketCode { get; set; }
        public string TerritoryCode { get; set; }
        public string RegionCode { get; set; }
        public string ZoneCode { get; set; }
        public string DivisionCode { get; set; }
        public string NationalCode { get; set; }
        public string SocietyId { get; set; }
        public string DoctorId { get; set; }
        public string InstituteId { get; set; }
        public string DonationType { get; set; }
        public string ExpenseDetail { get; set; }
        public string InvestmentAmount { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PrescribedSharePrcnt { get; set; }
        public string PrescShareFromDate { get; set; }
        public string PrescShareToDate { get; set; }
        public string ComtSharePrcnt { get; set; }
        public string DonationDuration { get; set; }
        public string DonationFromDate { get; set; }
        public string DonationToDate { get; set; }
    }
}
