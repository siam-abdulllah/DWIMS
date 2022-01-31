using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
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
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IGenericRepository<ApprovalAuthority> _approvalAuthorityRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;

        public ReportInvestmentController(IGenericRepository<ReportInvestmentInfo> investRepo, IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo, IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<ApprovalAuthority> approvalAuthorityRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
             IGenericRepository<ApprAuthConfig> apprAuthConfigRepo, IGenericRepository<ReportConfig> rptConfitRepo, IMapper mapper,
            IGenericRepository<InvestmentRec> investmentRecRepo, StoreContext db)
        {
            _mapper = mapper;
            _investRepo = investRepo;
            _rptConfitRepo = rptConfitRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentTargetedGroupRepo = investmentTargetedGroupRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _apprAuthConfigRepo = apprAuthConfigRepo;
            _approvalAuthorityRepo = approvalAuthorityRepo;
            _db = db;
            _investmentRecRepo = investmentRecRepo;
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
        //{ 
        // string qry = "select CAST(ROW_NUMBER() OVER (ORDER BY c.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, SUM (e.ApprovedAmount) Expense, c.SBUName, c.SBU, c.Amount Budget,  c.DonationId, d.DonationTypeName " +
        // " from SBUWiseBudget c, InvestmentInit b  inner join InvestmentDetailTracker e on e.InvestmentInitId = b.Id " +
        // " left join Donation d on d.Id = b.DonationId "+
        // " where b.SBU = c.SBU AND c.DonationId = e.DonationId AND e.PaidStatus = 'Paid' " +
        // " AND  (CONVERT(date,c.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,c.ToDate)) "+
        // " AND (CONVERT(date,e.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,e.ToDate)) ";

        // string qry = " select CAST(ROW_NUMBER() OVER (ORDER BY a.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.SBU, a.SBUName, a.DonationId, d.DonationTypeName,  a.Amount Budget,  " +
        //             " (select ISNULL(SUM(ApprovedAmount),0) from InvestmentDetailTracker e inner join InvestmentInit c on c.Id = e.InvestmentInitId  where e.PaidStatus = 'Paid' AND e.DonationId = a.DonationId AND c.SBU = a.SBU AND   " +
        //             " (CONVERT(date,e.FromDate) >= CAST('" + seGetInvestmentSummaryReportarch.FromDate + "' as Date) AND CAST('" + search.ToDate + "' as Date) >= CONVERT(date,e.ToDate))) Expense  " +
        //             " from SBUWiseBudget a inner join Donation d on d.Id = a.DonationId AND " +
        //             " (CONVERT(date,a.FromDate) >= CAST('" + search.FromDate + "' as Date) AND CAST('" + search.ToDate + "' as Date) >= CONVERT(date,a.ToDate)) ";

        {
            string qry = " select CAST(ROW_NUMBER() OVER (ORDER BY a.SBU) AS INT)  AS Id ,1 AS DataStatus, " +
                        " SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.SBU, a.SBUName, a.DonationId, " +
                        " d.DonationTypeName,  a.Amount Budget,  " +
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

            string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode='"+dt.EmpId+"'";
            var empData = _db.Employee.FromSqlRaw(empQry).ToList();
            // string qry = "select CAST(ROW_NUMBER() OVER (ORDER BY c.SBU) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, SUM (e.ApprovedAmount) Expense, c.SBUName, c.SBU, c.Amount Budget,  c.DonationId, d.DonationTypeName " +
            // " from SBUWiseBudget c, InvestmentInit b  inner join InvestmentDetailTracker e on e.InvestmentInitId = b.Id " +
            // " left join Donation d on d.Id = b.DonationId "+
            // " where b.SBU = c.SBU AND c.DonationId = e.DonationId AND e.PaidStatus = 'Paid' " +
            // " AND  (CONVERT(date,c.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,c.ToDate)) "+
            // " AND (CONVERT(date,e.FromDate) >= CAST('"+ search.FromDate +"' as Date) AND CAST('"+ search.ToDate +"' as Date) >= CONVERT(date,e.ToDate)) ";

            string qry = " select CAST(a.Id AS INT) as Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn," +
                " a.ReferenceNo, d.DonationTypeName, a.DonationTo, b.ProposedAmount, b.FromDate, b.ToDate, ' ' InvStatus, (select  count(*) from [InvestmentTargetedGroup] t where t.[InvestmentInitId] = a.Id and t.CompletionStatus = 0) InvStatusCount," +
                " E.EmployeeName," +
                " ISNULL ((select C.EmployeeName from InvestmentRecComment b join Employee c on c.Id=b.EmployeeId where b.InvestmentInitId = a.Id and b.Id = (select max(id) from InvestmentRecComment where InvestmentInitId = b.InvestmentInitId)), 'N/A') ApprovedBy," +
                " a.MarketName, " +
                " ISNULL(rcv.ReceiveStatus, 'Not Completed') ReceiveStatus," +
                " ISNULL((select a.EmployeeName + ', ' + a.DesignationName from [Employee] a where a.Id = rcv.EmployeeId), 'N/A') ReceiveBy " +
                " from InvestmentInit a INNER JOIN Employee E ON A.EmployeeId = E.Id" +
                " left join InvestmentDetail b on a.Id = b.InvestmentInitId " +
                " left join InvestmentRecv rcv on a.Id = rcv.InvestmentInitId " +
                " inner join Donation d on d.Id = a.DonationId " +
                //" left join Employee e on e.Id = a.EmployeeId " +
                //" left join Employee rcvBy on rcvBy.Id = rcv.EmployeeId " +
                " Where 1 = 1 " +
                " AND(CONVERT(date, a.SetOn) >= CAST('" + dt.FromDate + "' as Date) AND CAST('" + dt.ToDate + "' as Date) >= CONVERT(date, a.SetOn)) ";
            if (dt.UserRole != "Administrator")
            {
                qry = qry + " AND (" +
                            " a.MarketGroupCode = COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " a.MarketCode = COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " a.TerritoryCode = COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " a.RegionCode = COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " a.ZoneCode = COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All') = 'All'" +
                            " )";
            }



            var results = _db.RptInvestmentSummary.FromSqlRaw(qry).ToList();

            //var data = _mapper.Map<IReadOnlyList<RptInvestmentSummary>, IReadOnlyList<RptInvestmentSummaryDto>>(results);

            return Ok(new Pagination<RptInvestmentSummary>(rptParrams.PageIndex, rptParrams.PageSize, results.Count, results));
        }


        [HttpPost("GetParamInvestmentSummaryReport")]
        public ActionResult<IReadOnlyList<RptInvestmentSummary>> GetParamInvestmentSummaryReport(ParamSearchDto dt, [FromQuery] ReportInvestmentInfoSpecParams rptParrams)
        {

            string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode='"+dt.EmpId+"'";
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
                " Where 1 = 1 AND dbo.fnGetInvestmentStatus(a.Id) = '" + dt.ApproveStatus + "' AND a.Confirmation = 1 AND b.ProposedAmount is NOT NULL";

            if (dt.FromDate.Year > 1000 && dt.ToDate.Year > 1000)
            {
                qry = qry + " AND(CONVERT(date, b.FromDate) >= CAST('" + dt.FromDate + "' as Date) AND CAST('" + dt.ToDate + "' as Date) >= CONVERT(date, b.ToDate)) ";
            }
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


        [HttpGet("investmentInits/{id}")]
        public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetInvestmentInits(int id, [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {


                var spec = new InvestmentInitSpecification(id);

                var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                var investmentInits = await _investmentInitRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(investmentInits);
                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, data));

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        [Route("investmentTargetedGroups/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentTargetGroupSQL>> GetInvestmentTargetedGroups(int investmentInitId)
        {
            try
            {
                string qry = " SELECT  CAST(ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS INT) AS Id,  " +
                " 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.InvestmentInitId, a.SBU, a.SBUName,  " +
                " a.EmployeeId EmpId, emp.EmployeeName, e.MarketCode, e.MarketName,'' MarketGroupName, a.[Priority], ISNULL(a.RecStatus, 'N/A') RecStatus,  " +
                " ISNULL(c.ApprovalAuthorityName, 'N/A') ApprovalAuthorityName " +
                " FROM InvestmentInit i " +
                " LEFT JOIN InvestmentRecComment a ON a.InvestmentInitId = i.Id " +
                " LEFT JOIN Employee emp ON emp.Id = a.EmployeeId " +
                " LEFT JOIN InvestmentTargetedGroup e ON e.InvestmentInitId = i.Id " +
                " LEFT JOIN ApprAuthConfig b ON b.EmployeeId = a.EmployeeId " +
                " LEFT JOIN ApprovalAuthority c ON b.ApprovalAuthorityId = c.Id " +
                " WHERE i.Id = " + investmentInitId + " AND b.[Status] = 'A' AND a.SBU = e.SBU   " +
                " UNION " +
                " SELECT CAST(ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS INT) AS Id, " +
                " 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, r.InvestmentInitId, a.SBU, a.SBUName,  " +
                " r.EmployeeId EmpId, e.EmployeeName, a.MarketCode, a.MarketName,'' MarketGroupName, r.[Priority], ISNULL(r.RecStatus, 'N/A') RecStatus,  " +
                " ISNULL(c.ApprovalAuthorityName, 'N/A') ApprovalAuthorityName " +
                " FROM InvestmentInit a " +
                " LEFT JOIN InvestmentRecComment r ON r.InvestmentInitId = a.Id " +
                " LEFT JOIN Employee e ON e.Id = r.EmployeeId " +
                " LEFT JOIN ApprovalAuthority c ON r.[Priority] = c.[Priority] " +
                " WHERE a.Id = " + investmentInitId + " AND a.SBU = r.SBU " +
                " ORDER BY [Priority], RecStatus DESC ";

                var results = _db.InvestmentTargetGroupSQL.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("rptInvestDepo/{investmentInitId}")]
        public async Task<IReadOnlyList<RptDepotLetter>> ReportDepotLetter(int investmentInitId)
        {
            try
            {
                 string qry = " SELECT * FROM  ( " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName, " + 
                            " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount, inDetail.ChequeTitle, depo.DepotName " +
                            " from InvestmentInit a  " + 
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " + 
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " + 
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " + 
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " + 
                            " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId  left join DoctorInfo doc on inDc.DoctorId = doc.Id " + 
                            " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' " +
                            " AND inDetail.PaymentMethod = 'Cash'  " + 
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName, " + 
                            " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.DepotName " +
                            " from InvestmentInit a " + 
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " + 
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " +
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                            " inner join InvestmentCampaign IC on a.Id = IC.InvestmentInitId " +
                            " left join DoctorInfo doc on IC.DoctorId = doc.Id   " +
                            " where a.DonationTo = 'Campaign' AND  " +
                            " ir.RecStatus = 'Approved' " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Institution' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName,  " +
                            " doc.id as DocId, doc.InstitutionName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.DepotName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id  " +
                            " left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentInstitution IC on a.Id = IC.InvestmentInitId  " +
                            " left join InstitutionInfo doc on IC.InstitutionId = doc.Id  " +
                            " where a.DonationTo = 'Institution' " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Bcds' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName,  " +
                            " doc.id as DocId, doc.BcdsName, doc.BcdsAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.DepotName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentBcds IC on a.Id = IC.InvestmentInitId  " +
                            " left join Bcds doc on IC.BcdsId = doc.Id  " +
                            " where a.DonationTo = 'Bcds'  " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Society' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName,  " +
                            " doc.id as DocId, doc.SocietyName, doc.SocietyAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.DepotName     " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentSociety IC on a.Id = IC.InvestmentInitId  " +
                            " left join Society doc on IC.SocietyId = doc.Id  " +
                            " where a.DonationTo = 'Society'  " +
                            " AND inDetail.PaymentMethod = 'Cash'  " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " AND ir.RecStatus = 'Approved' ) x  " +                           
                            "  WHERE X.Id = " + investmentInitId +" ";

                var results = _db.RptDepotLetter.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }




        [HttpGet]
        [Route("rptChqPrint/{investmentInitId}")]
        public async Task<IReadOnlyList<RptDepotLetter>> ReportChequeLetter(int investmentInitId)
        {
            try
            {
                string qry = " SELECT * FROM  ( " +
                           " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName, " +
                           " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount, inDetail.ChequeTitle, depo.EmployeeName +', '+ depo.DesignationName 'DepotName' " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                           " left join Employee depo on depo.Id = ir.EmployeeId  " +
                           " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                           " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId  left join DoctorInfo doc on inDc.DoctorId = doc.Id " +
                           " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash'  " + 
                           " UNION " +
                           " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName, " +
                           " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.EmployeeName +', '+ depo.DesignationName 'DepotName' " +
                           " from InvestmentInit a " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
                           " left join Employee depo on depo.Id = ir.EmployeeId  " +
                           " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                           " inner join InvestmentCampaign IC on a.Id = IC.InvestmentInitId " +
                           " left join DoctorInfo doc on IC.DoctorId = doc.Id   " +
                           " where a.DonationTo = 'Campaign' AND  " +
                           " ir.RecStatus = 'Approved' " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash' " +
                           " UNION " +
                           " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Institution' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName,  " +
                           " doc.id as DocId, doc.InstitutionName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.EmployeeName +', '+ depo.DesignationName 'DepotName'  " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                           " left join Employee depo on depo.Id = ir.EmployeeId  " +
                           " left join Employee e on a.EmployeeId = e.Id  " +
                           " left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                           " inner join InvestmentInstitution IC on a.Id = IC.InvestmentInitId  " +
                           " left join InstitutionInfo doc on IC.InstitutionId = doc.Id  " +
                           " where a.DonationTo = 'Institution' " +
                           " AND ir.RecStatus = 'Approved'  " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash' " +
                           " UNION " +
                           " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Bcds' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName,  " +
                           " doc.id as DocId, doc.BcdsName, doc.BcdsAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.EmployeeName +', '+ depo.DesignationName 'DepotName'  " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                           " left join Employee depo on depo.Id = ir.EmployeeId " +
                           " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                           " inner join InvestmentBcds IC on a.Id = IC.InvestmentInitId  " +
                           " left join Bcds doc on IC.BcdsId = doc.Id  " +
                           " where a.DonationTo = 'Bcds'  " +
                           " AND ir.RecStatus = 'Approved'  " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash' " +
                           " UNION " +
                           " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Society' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName,  " +
                           " doc.id as DocId, doc.SocietyName, doc.SocietyAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle, depo.EmployeeName +', '+ depo.DesignationName 'DepotName' " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
                           " left join Employee depo on depo.Id = ir.EmployeeId " +
                           " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                           " inner join InvestmentSociety IC on a.Id = IC.InvestmentInitId  " +
                           " left join Society doc on IC.SocietyId = doc.Id  " +
                           " where a.DonationTo = 'Society'  " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           " AND ir.RecStatus = 'Approved' ) x  " +
                           // " AND inDetail.PaymentMethod = 'Cash'  " +
                           "  WHERE X.Id = " + investmentInitId + " ";

                var results = _db.RptDepotLetter.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentDetails/{investmentInitId}/{empId}/{userRole}")]
        public async Task<IReadOnlyList<InvestmentRec>> investmentRecDetails(int investmentInitId,int empId,string userRole)
        {
            try
            {
                if (userRole == "M") {
                    var initData = await _investmentInitRepo.GetByIdAsync(investmentInitId);
                    var spec = new InvestmentRecSpecification(investmentInitId);
                    var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                    string qry = "SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,  MAX(A.Priority) Count FROM ApprAuthConfig AC INNER JOIN ApprovalAuthority A ON AC.ApprovalAuthorityId = A.Id " +
                        " INNER JOIN Employee E ON Ac.EmployeeId = E.Id WHERE( E.ZoneCode = '" + initData.ZoneCode + "' )";
                    var result = _db.CountInt.FromSqlRaw(qry).ToList();
                    return investmentDetail.Where(x => x.Priority == result[0].Count).ToList();
                }
                else if (userRole == "GPM") {
                    var specAppr = new ApprAuthConfigSpecification(empId, "A");
                    var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                    var spec = new InvestmentRecSpecification(investmentInitId);
                    var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                    return investmentDetail.Where(x => x.Priority == 3).ToList();
                }
                else { 
                var specAppr = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                var spec = new InvestmentRecSpecification(investmentInitId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                return investmentDetail.Where(x => x.Priority == apprAuthConfigAppr.ApprovalAuthority.Priority - 1).ToList();
                }


            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
