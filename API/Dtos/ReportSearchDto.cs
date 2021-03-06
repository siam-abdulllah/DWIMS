using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ReportSearchDto
    {
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SBU { get; set; }
        public string DonationType { get; set; }
        public string InvestType { get; set; }
        public int? InstitutionId { get; set; }
        public int? SocietyId { get; set; }
        public int? BcdsId { get; set; }
        public int? DoctorId { get; set; }
        public string LocationType { get; set; }
        public string TerritoryCode { get; set; }
        public string MarketCode { get; set; }
        public string RegionCode { get; set; }
        public string ZoneCode { get; set; }
        public string DivisionCode { get; set; }
        public string BrandCode { get; set; }
        public string CampaignName { get; set; }
        public string SubCampaignName { get; set; }
    }


    public class SearchDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
        public int EmpId { get; set; } 
        public string UserRole { get; set; } 
    }

    public class ParamSearchDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; } 
        public int EmpId { get; set; } 
        public string UserRole { get; set; } 
        public string ApproveStatus { get; set; } 
    }


    public class EmpWiseExpSearchDto
    {
        public int EmployeeId { get; set; }
        public int ApprovalAuthorityId { get; set; }
        public int DonationId { get; set; }
        public DateTime Year { get; set; }
    }

    public class SummaryExpSearchDto
    {
        public int? DoctorId { get; set; }
        public int? InstitutionId { get; set; }
        public int? CampaignId { get; set; }
        public string BcdsName { get; set; }
        public string SocietyName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DonationId { get; set; }
        public string MarketCode { get; set; }
        public string SBU { get; set; }
    } public class DoctorSummaryExpSearchDto
    {
        public int? DoctorId { get; set; }
        public int? InstitutionId { get; set; }
        public int? CampaignId { get; set; }
        public string BcdsName { get; set; }
        public string SocietyName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DonationId { get; set; }
        public string MarketCode { get; set; }
        public string SBU { get; set; }
    } 
    public class InstituteSummaryExpSearchDto
    {
        public int? DoctorId { get; set; }
        public int? InstitutionId { get; set; }
        public int? CampaignId { get; set; }
        public string BcdsName { get; set; }
        public string SocietyName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DonationId { get; set; }
        public string MarketCode { get; set; }
        public string SBU { get; set; }
    } 
    public class CampaignSummaryExpSearchDto
    {
        public int? CampaignId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? InstitutionId { get; set; }
        public int? DoctorId { get; set; }
        public int? DonationId { get; set; }
        public string MarketCode { get; set; }
        public string SBU { get; set; }
    } 
    public class BcdsSummaryExpSearchDto
    {
        public int? DoctorId { get; set; }
        public int? InstitutionId { get; set; }
        public int? CampaignId { get; set; }
        public string BcdsName { get; set; }
        public string SocietyName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DonationId { get; set; }
        public string MarketCode { get; set; }
        public string SBU { get; set; }
    } 
    public class SocietySummaryExpSearchDto
    {
        public int? DoctorId { get; set; }
        public int? InstitutionId { get; set; }
        public int? CampaignId { get; set; }
        public string BcdsName { get; set; }
        public string SocietyName { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? DonationId { get; set; }
        public string MarketCode { get; set; }
        public string SBU { get; set; }
    } 
   

}
