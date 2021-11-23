using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using System;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace API.Controllers
{
    public class ReportInvestmentController : BaseApiController
    {
        private readonly StoreContext _db;
        private readonly IGenericRepository<ReportConfig> _rptConfitRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _investRepo;
        private readonly IMapper _mapper;


        public ReportInvestmentController(IGenericRepository<ReportInvestmentInfo> investRepo, IGenericRepository<ReportConfig> rptConfitRepo, IMapper mapper, StoreContext db)
        {
            _mapper = mapper;
            _investRepo = investRepo;
            _rptConfitRepo = rptConfitRepo;
            _db = db;
        }

        [HttpPost("GetInsSocietyBCDSWiseInvestment")]
        public ActionResult<IReadOnlyList<InstSocDocInvestmentDto>> GetInstituteInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams,ReportSearchDto search)
        {

            List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@UserId", search.UserId),
                        new SqlParameter("@FromDate", search.FromDate),
                        new SqlParameter("@ToDate", search.ToDate),
                        new SqlParameter("@SBU", search.SBU),
                        new SqlParameter("@DonationType", search.DonationType),
                        new SqlParameter("@InvestType", search.InvestType),
                        new SqlParameter("@InstitutionId", search.InstitutionId),
                        new SqlParameter("@SocietyId", search.SocietyId),
                        new SqlParameter("@BcdsId", search.BcdsId),
                        new SqlParameter("@LocationType", search.LocationType),
                        new SqlParameter("@TerritoryCode", search.TerritoryCode),
                        new SqlParameter("@MarketCode", search.MarketCode),
                        new SqlParameter("@regionCode", search.RegionCode),
                        new SqlParameter("@ZoneCode", search.ZoneCode),
                        new SqlParameter("@DivisionCode", search.DivisionCode),
                    };

            var results = _db.ReportInvestmentInfo.FromSqlRaw("EXECUTE SP_InvestmentReport @UserId,@FromDate,@ToDate, @SBU, @DonationType, @InvestType, " +
                " @InstitutionId, @SocietyId, @BcdsId, @LocationType, @TerritoryCode, @MarketCode, @regionCode, @ZoneCode,  @DivisionCode", parms.ToArray()).ToList();

            var data = _mapper.Map<IReadOnlyList<ReportInvestmentInfo>, IReadOnlyList<InstSocDocInvestmentDto>>(results);

            return Ok(new Pagination<InstSocDocInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, 10, data));
        }

        [HttpPost("GetDoctorCampaingWiseInvestment")]
        public ActionResult<IReadOnlyList<RptDocCampWiseInvestment>> GetDoctorCampaingWiseInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        {
            string qry = "SELECT * FROM RptDocCampWiseInvestment ";
            qry += "WHERE FromDate >= '"+ search.FromDate + "' AND  '" + search.ToDate + "' <= ToDate ";

            if (!string.IsNullOrEmpty(search.MarketCode))
            {
                qry += " AND MarketCode= '" + search.MarketCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.RegionCode))
            {
                qry += " AND RegionCode= '" + search.RegionCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.TerritoryCode))
            {
                qry += " AND TerritoryCode= '" + search.TerritoryCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.DivisionCode))
            {
                qry += " AND DivisionCode= '" + search.DivisionCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.ZoneCode))
            {
                qry += " AND ZoneCode= '" + search.ZoneCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.BrandCode))
            {
                qry += " AND Brand= '" + search.BrandCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.SBU))
            {
                qry += " AND SBUCode= '" + search.SBU + "' ";
            }
            if (!string.IsNullOrEmpty(search.CampaignName))
            {
                qry += " AND Campaign= '" + search.CampaignName + "' ";
            }
            if (!string.IsNullOrEmpty(search.SubCampaignName))
            {
                qry += " AND SubCampaign= '" + search.SubCampaignName + "' ";
            }
            if (!string.IsNullOrEmpty(search.DoctorId.ToString()))
            {
                qry += " AND DoctorId= '" + search.DoctorId + "' ";
            }
            if (!string.IsNullOrEmpty(search.InstitutionId.ToString()))
            {
                qry += " AND InstitutionId= '" + search.InstitutionId + "' ";
            }

            var results = _db.RptDocCampWiseInvestment.FromSqlRaw(qry).ToList();
            return results;
        }


        [HttpPost("GetSBUWiseExpSummaryReport")]
        public ActionResult<IReadOnlyList<RptSBUWiseExpSummart>> SBUWiseExpSummaryReport([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        {

            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                   "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                   "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                   "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                   "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                   "MM/d/yyyy HH:mm:ss.ffffff",
                   "dd-MMM-yy hh:mm:ss",
                   "dd-MM-yy", "dd-MM-yyyy"};

            CultureInfo provider = CultureInfo.InvariantCulture;  
            // It throws Argument null exception  
            //DateTime fd = DateTime.ParseExact(search.FromDate.ToString(), formats, provider);
            //DateTime td = DateTime.ParseExact(search.ToDate.ToString(), formats, provider);

            string qry = "select CAST(ROW_NUMBER() OVER (ORDER BY c.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, SUM (e.ApprovedAmount) Expense, c.SBUName, c.SBU, c.Amount Budget,  c.DonationId, d.DonationTypeName " +
            " from SBUWiseBudget c, InvestmentInit b  inner join InvestmentDetailTracker e on e.InvestmentInitId = b.Id " +
            " left join Donation d on d.Id = b.DonationId "+
            " where b.SBU = c.SBU AND c.DonationId = e.DonationId AND e.PaidStatus = 'Paid' " +
            " AND  (CONVERT(date,c.FromDate) >= CAST('01-01-2021' as Date) AND CAST('12-31-2021' as Date) >= CONVERT(date,c.ToDate)) "+
            " AND (CONVERT(date,e.FromDate) >= CAST('01-01-2021' as Date) AND CAST('12-31-2021' as Date) >= CONVERT(date,e.ToDate)) ";
    
            if (!string.IsNullOrEmpty(search.SBU))
            {
                qry += " AND C.SBUName = '" + search.SBU + "' ";
            }
            if (!string.IsNullOrEmpty(search.DonationType))
            {
                qry += " AND d.DonationTypeName = '" + search.DonationType + "' ";
            }

            qry += " group by  c.SBUName,c.SBU, c.Amount,  c.DonationId, d.DonationTypeName ";

            var results = _db.RptSBUWiseExpSummart.FromSqlRaw(qry).ToList();

            return results;
        }



        [HttpPost("GetDoctorLocationWiseInvestment")]
        public ActionResult<IReadOnlyList<RptDocLocWiseInvestment>> GetDoctorLocationWiseInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        {
            string qry = "SELECT * FROM RptDocLocWiseInvestment ";
            qry += "WHERE FromDate >= '" + search.FromDate + "' AND  '" + search.ToDate + "' <= ToDate ";
            
            if (!string.IsNullOrEmpty(search.MarketCode))
            {
                qry += " AND MarketCode= '" + search.MarketCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.RegionCode))
            {
                qry += " AND RegionCode= '" + search.RegionCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.TerritoryCode))
            {
                qry += " AND TerritoryCode= '" + search.TerritoryCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.DivisionCode))
            {
                qry += " AND DivisionCode= '" + search.DivisionCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.ZoneCode))
            {
                qry += " AND ZoneCode= '" + search.ZoneCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.BrandCode))
            {
                qry += " AND Brand= '" + search.BrandCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.SBU))
            {
                qry += " AND SBUCode= '" + search.SBU + "' ";
            }
            if (!string.IsNullOrEmpty(search.DonationType))
            {
                qry += " AND DonationType= '" + search.DonationType + "' ";
            }
            if (!string.IsNullOrEmpty(search.DoctorId.ToString()))
            {
                qry += " AND DoctorId= '" + search.DoctorId + "' ";
            }
            if (!string.IsNullOrEmpty(search.InstitutionId.ToString()))
            {
                qry += " AND InstitutionId= '" + search.InstitutionId + "' ";
            }

            var results = _db.RptDocLocWiseInvestment.FromSqlRaw(qry).ToList();
            return results;
        }


        [HttpPost("EmpLocationMapping")]
        public object EmpLocationMapping(ReportSearchDto search)
        {

            string qry = "SELECT [Id], [EmployeeSAPCode], [EmployeeName], [DesignationName], [SBU], [SBUName], [MarketCode], [MarketName], " +
                 " [TerritoryCode], [TerritoryName], [RegionCode], [RegionName], [ZoneCode], [ZoneName], [MarketGroupCode], [MarketGroupName]" +
                 " FROM[DIDS].[dbo].[Employee] WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(search.MarketCode))
            {
                qry += " AND MarketCode= '" + search.MarketCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.RegionCode))
            {
                qry += " AND RegionCode= '" + search.RegionCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.TerritoryCode))
            {
                qry += " AND TerritoryCode= '" + search.TerritoryCode + "' ";
            }
            if (!string.IsNullOrEmpty(search.ZoneCode))
            {
                qry += " AND ZoneCode= '" + search.ZoneCode + "' ";
            }

            qry += " GROUP BY [RegionCode],[ZoneCode],  [TerritoryCode] , DesignationName, [MarketCode], [Id], [EmployeeSAPCode], [EmployeeName], [SBU]," +
                " [SBUName], [ZoneName], [RegionName], [TerritoryName], [MarketName], [MarketGroupCode], [MarketGroupName]" +
                "  ORDER BY  [RegionCode],[ZoneCode],  [TerritoryCode] desc, DesignationName, [MarketCode]";

            var command = _db.Database.GetDbConnection().CreateCommand();

            command.CommandText = qry;
            command.CommandType = CommandType.Text;

            _db.Database.CloseConnectionAsync();
            _db.Database.OpenConnectionAsync();

            var results = command.ExecuteReader();

      

            return results;
        }

        [HttpGet("getReportList")]
        public async Task<ActionResult<IReadOnlyList<ReportConfigDto>>> getReportList([FromQuery] ReportConfigSpecParams rptParrams)
        {

            var spec = new ReportConfigSpecification(rptParrams);

            var countSpec = new ReportConfigWithFiltersForCountSpecificication(rptParrams);

            var totalItems = await _rptConfitRepo.CountAsync(countSpec);

            var posts = await _rptConfitRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ReportConfig>, IReadOnlyList<ReportConfigDto>>(posts);

            return Ok(new Pagination<ReportConfigDto>(rptParrams.PageIndex, rptParrams.PageSize, totalItems, data));
        }
    }
}
