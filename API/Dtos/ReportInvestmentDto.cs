using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ReportInvestmentDto
    {
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string NationalCode { get; set; }
        public string BcdsId { get; set; }
        public BcdsDto Bcds { get; set; }
        public string SocietyId { get; set; }
        public InvestmentSocietyDto InvestmentSociety { get; set; }
        public string DoctorId { get; set; }
        public DoctorInfoDto DoctorInfo { get; set; }
        public string InstituteId { get; set; }
        public InstitutionInfoDto InstitutionInfo { get; set; }
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


    public class InstSocDocInvestmentDto
    {
        public int Id { get; set; }
        public string SBU { get; set; }
        public string Location { get; set; }
        public string DonationType { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public float InvestedAmount { get; set; }
        public float Commitment { get; set; }
        public float ActualShare { get; set; }
        public float CompetitorShare { get; set; }
    }
}
