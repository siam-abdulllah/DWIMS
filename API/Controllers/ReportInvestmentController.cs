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
        public ActionResult<IReadOnlyList<InstSocDocInvestmentDto>> GetInstituteInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
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

            // string qry = "select CAST(ROW_NUMBER() OVER (ORDER BY c.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, SUM (e.ApprovedAmount) Expense, c.SBUName, c.SBU, c.Amount Budget,  c.DonationId, d.DonationTypeName " +
            // " from SBUWiseBudget c, InvestmentInit b  inner join InvestmentDetailTracker e on e.InvestmentInitId = b.Id " +
            // " left join Donation d on d.Id = b.DonationId "+
            // " where b.SBU = c.SBU AND c.DonationId = e.DonationId AND e.PaidStatus = 'Paid' " +
            // " AND  (CONVERT(date,c.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,c.ToDate)) "+
            // " AND (CONVERT(date,e.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,e.ToDate)) ";

            // string qry = " select CAST(ROW_NUMBER() OVER (ORDER BY a.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.SBU, a.SBUName, a.DonationId, d.DonationTypeName,  a.Amount Budget,  " +
            //             " (select ISNULL(SUM(ApprovedAmount),0) from InvestmentDetailTracker e inner join InvestmentInit c on c.Id = e.InvestmentInitId  where e.PaidStatus = 'Paid' AND e.DonationId = a.DonationId AND c.SBU = a.SBU AND   " +
            //             " (CONVERT(date,e.FromDate) >= CAST('" + search.FromDate + "' as Date) AND CAST('" + search.ToDate + "' as Date) >= CONVERT(date,e.ToDate))) Expense  " +
            //             " from SBUWiseBudget a inner join Donation d on d.Id = a.DonationId AND " +
            //             " (CONVERT(date,a.FromDate) >= CAST('" + search.FromDate + "' as Date) AND CAST('" + search.ToDate + "' as Date) >= CONVERT(date,a.ToDate)) ";

        {      
            string qry = " select CAST(ROW_NUMBER() OVER (ORDER BY a.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.SBU, a.SBUName, a.DonationId, d.DonationTypeName,  a.Amount Budget,  "+
                        " (select ISNULL(SUM(ApprovedAmount),0) from InvestmentDetailTracker e inner join InvestmentInit c on c.Id = e.InvestmentInitId  where e.PaidStatus = 'Paid' AND e.DonationId = a.DonationId AND c.SBU = a.SBU AND   "+
                        " (CONVERT(date,e.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,e.ToDate))) Expense  "+
                        " from SBUWiseBudget a inner join Donation d on d.Id = a.DonationId AND "+
                        " (CONVERT(date,a.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,a.ToDate)) ";

            if (!string.IsNullOrEmpty(search.SBU))
            {
                qry += " AND a.SBUName = '" + search.SBU + "' ";
            }
            if (!string.IsNullOrEmpty(search.DonationType))
            {
                qry += " AND d.DonationTypeName = '" + search.DonationType + "' ";
            }

            var results = _db.RptSBUWiseExpSummart.FromSqlRaw(qry).ToList();

            return results;
        }



        [HttpPost("GetEmpWiseExpSummaryReport")]
        public ActionResult<IReadOnlyList<RptSBUWiseExpSummart>> EmpWiseExpSummaryReport([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        {
            string qry = " select CAST(ROW_NUMBER() OVER (ORDER BY a.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.SBU, a.SBUName, a.DonationId, d.DonationTypeName,  a.Amount Budget,  " +
                        " (select ISNULL(SUM(ApprovedAmount),0) from InvestmentDetailTracker e inner join InvestmentInit c on c.Id = e.InvestmentInitId  where e.PaidStatus = 'Paid' AND e.DonationId = a.DonationId AND c.SBU = a.SBU AND   " +
                        " (CONVERT(date,e.FromDate) >= CAST('" + search.FromDate + "' as Date) AND CAST('" + search.ToDate + "' as Date) >= CONVERT(date,e.ToDate))) Expense  " +
                        " from SBUWiseBudget a inner join Donation d on d.Id = a.DonationId AND " +
                        " (CONVERT(date,a.FromDate) >= CAST('" + search.FromDate + "' as Date) AND CAST('" + search.ToDate + "' as Date) >= CONVERT(date,a.ToDate)) ";

            if (!string.IsNullOrEmpty(search.SBU))
            {
                qry += " AND a.SBUName = '" + search.SBU + "' ";
            }
            if (!string.IsNullOrEmpty(search.DonationType))
            {
                qry += " AND d.DonationTypeName = '" + search.DonationType + "' ";
            }

            var results = _db.RptSBUWiseExpSummart.FromSqlRaw(qry).ToList();

            return results;
        }



        [HttpPost("GetInvestmentSummaryReport")]
        public ActionResult<IReadOnlyList<RptInvestmentSummary>> GetInvestmentSummaryReport(SearchDto dt, [FromQuery] ReportInvestmentInfoSpecParams rptParrams)
        {

            string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode=" + dt.EmpId;
            var empData = _db.Employee.FromSqlRaw(empQry).ToList();
            // string qry = "select CAST(ROW_NUMBER() OVER (ORDER BY c.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, SUM (e.ApprovedAmount) Expense, c.SBUName, c.SBU, c.Amount Budget,  c.DonationId, d.DonationTypeName " +
            // " from SBUWiseBudget c, InvestmentInit b  inner join InvestmentDetailTracker e on e.InvestmentInitId = b.Id " +
            // " left join Donation d on d.Id = b.DonationId "+
            // " where b.SBU = c.SBU AND c.DonationId = e.DonationId AND e.PaidStatus = 'Paid' " +
            // " AND  (CONVERT(date,c.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,c.ToDate)) "+
            // " AND (CONVERT(date,e.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,e.ToDate)) ";

            string qry = " select CAST(a.Id AS INT) as Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, d.DonationTypeName, a.DonationTo, b.ProposedAmount, b.FromDate, b.ToDate, dbo.fnGetInvestmentStatus(a.Id) InvStatus, e.EmployeeName,dbo.fnGetInvestmentApprovedBy(a.Id) ApprovedBy,e.MarketName, ISNULL(rcv.ReceiveStatus, 'Not Completed') ReceiveStatus, ISNULL(rcvBy.EmployeeName, 'N/A') ReceiveBy " +
                " from InvestmentInit a " +
                " left join InvestmentDetail b on a.Id = b.InvestmentInitId " +
                " left join InvestmentRecv rcv on a.Id = rcv.InvestmentInitId " +
                " inner join Donation d on d.Id = a.DonationId " +
                " left join Employee e on e.Id = a.EmployeeId " +
                " left join Employee rcvBy on rcvBy.Id = rcv.EmployeeId " +
                " Where 1 = 1 " +
                " AND(CONVERT(date, b.FromDate) >= CAST('" + dt.FromDate + "' as Date) AND CAST('" + dt.ToDate + "' as Date) >= CONVERT(date, b.ToDate)) ";
            if (dt.UserRole != "Administrator")
            {
                qry = qry + " AND (" +
                            " e.MarketGroupCode = COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.MarketCode = COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.TerritoryCode = COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.RegionCode = COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.ZoneCode = COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All') = 'All'" +
                            " )";
            }



            var results = _db.RptInvestmentSummary.FromSqlRaw(qry).ToList();

            //var data = _mapper.Map<IReadOnlyList<RptInvestmentSummary>, IReadOnlyList<RptInvestmentSummaryDto>>(results);

            return Ok(new Pagination<RptInvestmentSummary>(rptParrams.PageIndex, rptParrams.PageSize, results.Count, results));
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
