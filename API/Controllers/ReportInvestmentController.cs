﻿using System.Collections.Generic;
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
using System;

namespace API.Controllers
{
    public class ReportInvestmentController : BaseApiController
    {
        private readonly StoreContext _db;
        private readonly IGenericRepository<ReportConfig> _rptConfitRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _investRepo;
        private readonly IGenericRepository<InvestmentDetail> _investmentDetailRepo;
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IGenericRepository<ApprovalAuthority> _approvalAuthorityRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;

        public ReportInvestmentController(IGenericRepository<ReportInvestmentInfo> investRepo, IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo, IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<ApprovalAuthority> approvalAuthorityRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
             IGenericRepository<ApprAuthConfig> apprAuthConfigRepo, IGenericRepository<ReportConfig> rptConfitRepo, IMapper mapper,
            IGenericRepository<InvestmentRec> investmentRecRepo, StoreContext db, IGenericRepository<InvestmentDetail> investmentDetailRepo)
        {
            _mapper = mapper;
            _investRepo = investRepo;
            _rptConfitRepo = rptConfitRepo;
            _investmentDetailRepo = investmentDetailRepo;
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

        [HttpGet("IsInvestmentInActive/{referenceNo}")]
        public ActionResult<IReadOnlyList<CountInt>> IsInvestmentInActive(string referenceNo)
        {
            string qry = "SELECT [Id],[DataStatus] ,[SetOn],[ModifiedOn],[DataStatus] Count  FROM InvestmentInit WHERE ReferenceNo='"+ referenceNo +"'";
            

            var results = _db.CountInt.FromSqlRaw(qry).ToList();
            return results;
        }
         [HttpGet("IsInvestmentInActiveDoc/{referenceNo}/{doctorId}/{doctorName}")]
        public ActionResult<IReadOnlyList<CountInt>> IsInvestmentInActiveDoc(string referenceNo,int doctorId, string doctorName)
        {
            

            string qry = "SELECT A.[Id],A.[DataStatus] ,A.[SetOn],A.[ModifiedOn],A.[DataStatus] Count  " +
                " FROM InvestmentInit A " +
                " LEFT JOIN InvestmentDoctor B ON A.Id=B.InvestmentInitId " +
                " LEFT JOIN DoctorInfo C ON B.DoctorId=C.Id" +
                " WHERE 1=1";
            if (!string.IsNullOrEmpty(referenceNo) && referenceNo!="undefined")
            {
                qry = qry+" AND ReferenceNo = '" + referenceNo +"'";
            }
            if (!string.IsNullOrEmpty(doctorName) && doctorName!="undefined")
            {
                qry = qry + " AND C.DoctorName Like '%" + doctorName + "%'";
            }
            if (doctorId!=0)
            {
                qry = qry + " AND B.DoctorId = " + doctorId + " ";
            }

            var results = _db.CountInt.FromSqlRaw(qry).ToList();
            return results;
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

        [HttpPost("GetYearlyBudgetReport")]
        public ActionResult<IReadOnlyList<RptYearlyBudget>> GetYearlyBudgetReport(ReportSearchDto dt, [FromQuery] ReportInvestmentInfoSpecParams rptParrams)
        {
            int year = Convert.ToDateTime(dt.FromDate).Year;

            string qry = "   select distinct CAST(ABS(CHECKSUM(NewId())) % 200 AS int) as Id, 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.Amount, " +
                " (select SUM(x.ApprovedAmount) Expense from InvestmentDetailTracker x " +
                " inner join InvestmentInit i on x.InvestmentInitId = i.Id " +
                " where x.Year = "+ year +" and x.DonationId = b.DonationId and " +
                " i.sbuname = b.sbuname) Expense,  b.* " +
                " From SBUWiseBudget a" +
                " left join vw_MonthlyExpense b on a.SBUName = b.SBUName and a.DonationId = b.DonationId " +
                " where b.[year] = " + year + ""; 
                if (!string.IsNullOrEmpty(dt.SBU))
                {
                    qry += " AND b.SBUName = '" + dt.SBU + "' ";
                }
                if (!string.IsNullOrEmpty(dt.DonationType))
                {
                    qry += " AND b.DonationTypeName = '" + dt.DonationType +"' ";
                }

                 qry += " order by b.sbuname, b.donationid ";

            var results = _db.RptYearlyBudget.FromSqlRaw(qry).ToList();

            return results;
        }


         [HttpPost("GetSBUWiseExpenseReport")]
        public ActionResult<IReadOnlyList<RptSBUWiseExp>> GetSBUWiseExpenseReport(ReportSearchDto dt)
        {
            int year = Convert.ToDateTime(dt.FromDate).Year;

            string qry = " Select DISTINCT Cast(Abs(Checksum(Newid())) % 200 AS INT) AS Id,1 AS DataStatus,Sysdatetimeoffset() AS SetOn,Sysdatetimeoffset() AS ModifiedOn, " +
            " SUM(a.amount) amount,(SELECT Sum(x.approvedamount) Expense FROM   investmentdetailtracker x INNER JOIN investmentinit i ON x.investmentinitid = i.id  " +
            " WHERE  x.year = "+ year +" AND i.sbuname = b.sbuname) Expense, b.SBUName, b.Year " +
            " FROM   sbuwisebudget a  " +
            " LEFT JOIN vw_monthlyexpense b ON a.sbuname = b.sbuname AND a.donationid = b.donationid " +
            " WHERE  b.[year] = "+ year +" " +
            " GROUP  BY b.sbuname, b.Year " +
            " ORDER  BY b.sbuname ";

            var results = _db.RptSBUWiseExp.FromSqlRaw(qry).ToList();

            return results;
        }


        [HttpPost("GetEmpMonthlyExpense")]
        public ActionResult<IReadOnlyList<RptEmpWiseExp>> GetEmpMonthlyExpense(EmpWiseExpSearchDto dt)
        {
            int year = Convert.ToDateTime(dt.Year).Year;

            string qry = " Select Cast(Abs(Checksum(Newid())) % 200 AS INT) AS Id,1 AS DataStatus,Sysdatetimeoffset() AS SetOn,Sysdatetimeoffset() AS ModifiedOn, " +
                " X.ApprovalAuthorityId, X.Priority, X.EmployeeId, e.EmployeeName, X.DonationId, d.DonationTypeName, DateName(month, DateAdd(month, X.[Month], - 1)) [Month], X.Year, CAST (X.Budget as float) Budget, X.Expense " +
                " FROM ( " +
                " select aprCeiling.ApprovalAuthorityId, aprAuth.Priority, authConf.EmployeeId, aprCeiling.DonationId, aprCeiling.Month, aprCeiling.Year, aprCeiling.AmountPerMonth Budget, " +
                " (select SUM(a.ApprovedAmount) from InvestmentDetailTracker a " +
                " WHERE a.DonationId = aprCeiling.DonationId AND a.Month = aprCeiling.Month " +
                " AND a.Year = aprCeiling.Year AND a.EmployeeId = authConf.EmployeeId " +
                " group by a.Month, a.Year, a.DonationId, a.EmployeeId) Expense " +
                " from ApprAuthConfig authConf " +
                " inner join ApprovalAuthority aprAuth on aprAuth.Id = authConf.ApprovalAuthorityId " +
                " inner join ApprovalCeiling aprCeiling on aprAuth.Id = aprCeiling.ApprovalAuthorityId " +
                " where aprCeiling.AmountPerMonth > 0 " +
                " group by aprCeiling.ApprovalAuthorityId, aprAuth.Priority, authConf.EmployeeId, aprCeiling.AmountPerMonth, aprCeiling.DonationId, aprCeiling.Month, aprCeiling.Year " +
                " ) X " +
                " join Employee e on e.Id = x.EmployeeId " +
                " join Donation d on d.Id = x.DonationId " +
                " WHERE X.Expense > 0 AND X.Year = "+ year +" ";

            if (dt.ApprovalAuthorityId > 0)
            {
                qry = qry + " AND ApprovalAuthorityId = " + dt.ApprovalAuthorityId + " ";
            }
            if (dt.EmployeeId > 0 && dt.EmployeeId.ToString().Length > 0)
            {
                qry = qry + " AND EmployeeId = " + dt.EmployeeId + " ";
            }
            if (dt.DonationId > 0)
            {
                qry = qry + " AND DonationId = " + dt.DonationId + " ";
            }

            qry = qry + " Order by x.Priority desc, x.EmployeeId , x.Month, x.Year, x.DonationId  ";

            var results = _db.RptEmpWiseExp.FromSqlRaw(qry).ToList();

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

            string qry = " SELECT Cast(a.id AS INT) AS Id, a.DataStatus, Sysdatetimeoffset() AS SetOn, Sysdatetimeoffset() AS ModifiedOn, a.referenceno, b.ProposedAmount, d.donationtypename, a.donationto, b.fromdate, ISNULL (rcv.ReceiveStatus, 'N/A') ReceiveStatus, b.todate, 0 InvStatusCount, " +
            
            " CASE WHEN a.donationto = 'Doctor' THEN (SELECT DoctorId  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Institution' THEN (SELECT InstitutionId FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Campaign' THEN (SELECT y.MstId  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Bcds' THEN (SELECT x.BcdsId   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Society' THEN (SELECT x.SocietyId FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DId, "+
        
            " CASE WHEN a.donationto = 'Doctor' THEN (SELECT doctorname FROM investmentdoctor x INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) "+
            " WHEN a.donationto = 'Institution' THEN (SELECT institutionname FROM investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) "+
            " WHEN a.donationto = 'Campaign' THEN (SELECT subcampaignname FROM investmentcampaign x INNER JOIN campaigndtl y ON x.campaigndtlid = y.id INNER JOIN [dbo].[subcampaign] C ON y.subcampaignid = C.id WHERE  x.investmentinitid = a.id) "+
            " WHEN a.donationto = 'Bcds' THEN (SELECT bcdsname FROM investmentbcds x INNER JOIN bcds y ON x.bcdsid = y.id WHERE  x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Society' THEN (SELECT societyname FROM investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END NAME,  "+
            " (SELECT Isnull ((SELECT b.recstatus FROM investmentreccomment b WHERE  b.id IN (SELECT Max(id) FROM investmentreccomment WHERE  investmentinitid = a.id)), 'Pending')) InvStatus, "+

            // " CASE WHEN  (SELECT b.recstatus FROM investmentreccomment b WHERE  b.id IN (SELECT Max(id) FROM investmentreccomment WHERE  investmentinitid = a.id)) = NULL THEN  (SELECT DISTINCT ProposedAmount FROM investmentdetail WHERE InvestmentInitId=A.Id) "+
            // " WHEN  (SELECT b.recstatus FROM investmentreccomment b WHERE  b.id IN (SELECT Max(id) FROM investmentreccomment WHERE  investmentinitid = a.id))  = 'Approved' THEN  (SELECT DISTINCT ProposedAmount FROM InvestmentRec WHERE InvestmentInitId=A.Id AND  EmployeeId =(SELECT DISTINCT EmployeeId FROM InvestmentRecComment WHERE InvestmentInitId=A.Id AND CompletionStatus=1 and RecStatus='Approved')) "+
            // " WHEN  (SELECT b.recstatus FROM investmentreccomment b WHERE  b.id IN (SELECT Max(id) FROM investmentreccomment WHERE  investmentinitid = a.id))  NOT IN (NULL,'Approved') THEN  (SELECT ProposedAmount FROM InvestmentRec WHERE InvestmentInitId=A.Id AND  EmployeeId =(select EmployeeId from InvestmentRecComment where InvestmentInitId=A.Id and CompletionStatus=1 and Priority=(select max(Priority) from InvestmentRecComment where InvestmentInitId=A.Id))) END ProposedAmount, "+

            " E.employeename, Isnull ((SELECT C.employeename FROM investmentreccomment b JOIN employee c ON c.id = b.employeeid WHERE  b.investmentinitid = a.id AND b.id = (SELECT Max(id) FROM   investmentreccomment WHERE  investmentinitid = b.investmentinitid)), 'N/A') ApprovedBy, "+
            " a.MarketName, Isnull(M.employeename + ', ' + M.designationname,'N/A') ReceiveBy, b.PaymentMethod, a.ProposeFor, a.SBUName, depo.DepotName, a.Confirmation "+
            " FROM investmentinit a"+
            " INNER JOIN employee E ON A.employeeid = E.id "+
            " LEFT JOIN investmentdetail b ON a.id = b.investmentinitid " +
            " LEFT JOIN InvestmentRecDepot depo on depo.investmentinitid = a.Id " +
            " LEFT JOIN investmentrecv rcv ON a.id = rcv.investmentinitid " +
            " LEFT JOIN employee M ON  rcv.employeeid= M.id  "+
            " INNER JOIN donation d ON d.id = a.donationid "+
            " Where a.DataStatus=1 " +
            " AND(CONVERT(date, a.SetOn) >= CAST('" + dt.FromDate + "' as Date) AND CAST('" + dt.ToDate + "' as Date) >= CONVERT(date, a.SetOn)) ";
            if (dt.UserRole != "Administrator")
            {
                qry = qry + " AND (" +
                            " e.SBU = COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
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

            return Ok(new Pagination<RptInvestmentSummary>(rptParrams.PageIndex, rptParrams.PageSize, results.Count, results));
        }

        [HttpGet]
        [Route("getInvestmentSummarySingle/{referenceNo}")]
        public ActionResult<IReadOnlyList<RptInvestmentSummary>> GetInvestmentSummarySingle(string ReferenceNo)
        {
            string qry = " SELECT  DISTINCT Cast(a.id AS INT) AS Id, a.DataStatus, Sysdatetimeoffset() AS SetOn, Sysdatetimeoffset() AS ModifiedOn, a.referenceno, b.ProposedAmount, d.donationtypename, a.donationto, b.fromdate, ISNULL (rcv.ReceiveStatus, 'N/A') ReceiveStatus, b.todate, 0 InvStatusCount, " +
            
            " CASE WHEN a.donationto = 'Doctor' THEN (SELECT DoctorId  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Institution' THEN (SELECT InstitutionId FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Campaign' THEN (SELECT y.MstId  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Bcds' THEN (SELECT x.BcdsId   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Society' THEN (SELECT x.SocietyId FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DId, "+
        
            " CASE WHEN a.donationto = 'Doctor' THEN (SELECT doctorname FROM investmentdoctor x INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) "+
            " WHEN a.donationto = 'Institution' THEN (SELECT institutionname FROM investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) "+
            " WHEN a.donationto = 'Campaign' THEN (SELECT subcampaignname FROM investmentcampaign x INNER JOIN campaigndtl y ON x.campaigndtlid = y.id INNER JOIN [dbo].[subcampaign] C ON y.subcampaignid = C.id WHERE  x.investmentinitid = a.id) "+
            " WHEN a.donationto = 'Bcds' THEN (SELECT bcdsname FROM investmentbcds x INNER JOIN bcds y ON x.bcdsid = y.id WHERE  x.investmentinitid = a.id)  "+
            " WHEN a.donationto = 'Society' THEN (SELECT societyname FROM investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END NAME,  "+
            " (SELECT Isnull ((SELECT b.recstatus FROM investmentreccomment b WHERE  b.id IN (SELECT Max(id) FROM investmentreccomment WHERE  investmentinitid = a.id)), 'Pending')) InvStatus, "+

            " E.employeename, Isnull ((SELECT C.employeename FROM investmentreccomment b JOIN employee c ON c.id = b.employeeid WHERE  b.investmentinitid = a.id AND b.id = (SELECT Max(id) FROM   investmentreccomment WHERE  investmentinitid = b.investmentinitid)), 'N/A') ApprovedBy, "+
            " a.MarketName, Isnull(M.employeename + ', ' + M.designationname,'N/A') ReceiveBy, b.PaymentMethod, a.ProposeFor, a.SBUName, depo.DepotName, a.Confirmation "+
            " FROM investmentinit a"+
            " INNER JOIN employee E ON A.employeeid = E.id "+
            " LEFT JOIN investmentdetail b ON a.id = b.investmentinitid " +
            " LEFT JOIN InvestmentRecDepot depo on depo.investmentinitid = a.Id " +
            " LEFT JOIN investmentrecv rcv ON a.id = rcv.investmentinitid " +
            " LEFT JOIN employee M ON  rcv.employeeid= M.id  "+
            " INNER JOIN donation d ON d.id = a.donationid "+
            " Where a.referenceno = '"+ ReferenceNo +"' ";

            var results = _db.RptInvestmentSummary.FromSqlRaw(qry).ToList();

            //var data = _mapper.Map<IReadOnlyList<RptInvestmentSummary>, IReadOnlyList<RptInvestmentSummaryDto>>(results);

            return results;
        }

        [HttpGet]
        [Route("getInvestmentSummarySingleDoc/{referenceNo}/{doctorName}/{doctorId}")]
        public ActionResult<IReadOnlyList<RptInvestmentSummary>> GetInvestmentSummarySingleDoc(string referenceNo,string doctorName,int doctorId)
        {
            //string qry = " SELECT  DISTINCT Cast(a.id AS INT) AS Id, a.DataStatus, Sysdatetimeoffset() AS SetOn, Sysdatetimeoffset() AS ModifiedOn, a.referenceno, b.ProposedAmount, d.donationtypename, a.donationto, b.fromdate, ISNULL (rcv.ReceiveStatus, 'N/A') ReceiveStatus, b.todate, 0 InvStatusCount, " +

            //" CASE WHEN a.donationto = 'Doctor' THEN (SELECT DoctorId  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE x.investmentinitid = a.id)  " +
            //" WHEN a.donationto = 'Institution' THEN (SELECT InstitutionId FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id)  " +
            //" WHEN a.donationto = 'Campaign' THEN (SELECT y.MstId  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE x.investmentinitid = a.id)  " +
            //" WHEN a.donationto = 'Bcds' THEN (SELECT x.BcdsId   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id)  " +
            //" WHEN a.donationto = 'Society' THEN (SELECT x.SocietyId FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DId, " +

            //" CASE WHEN a.donationto = 'Doctor' THEN (SELECT doctorname FROM investmentdoctor x INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) " +
            //" WHEN a.donationto = 'Institution' THEN (SELECT institutionname FROM investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) " +
            //" WHEN a.donationto = 'Campaign' THEN (SELECT subcampaignname FROM investmentcampaign x INNER JOIN campaigndtl y ON x.campaigndtlid = y.id INNER JOIN [dbo].[subcampaign] C ON y.subcampaignid = C.id WHERE  x.investmentinitid = a.id) " +
            //" WHEN a.donationto = 'Bcds' THEN (SELECT bcdsname FROM investmentbcds x INNER JOIN bcds y ON x.bcdsid = y.id WHERE  x.investmentinitid = a.id)  " +
            //" WHEN a.donationto = 'Society' THEN (SELECT societyname FROM investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END NAME,  " +
            //" (SELECT Isnull ((SELECT b.recstatus FROM investmentreccomment b WHERE  b.id IN (SELECT Max(id) FROM investmentreccomment WHERE  investmentinitid = a.id)), 'Pending')) InvStatus, " +

            //" E.employeename, Isnull ((SELECT C.employeename FROM investmentreccomment b JOIN employee c ON c.id = b.employeeid WHERE  b.investmentinitid = a.id AND b.id = (SELECT Max(id) FROM   investmentreccomment WHERE  investmentinitid = b.investmentinitid)), 'N/A') ApprovedBy, " +
            //" a.MarketName, Isnull(M.employeename + ', ' + M.designationname,'N/A') ReceiveBy, b.PaymentMethod, a.ProposeFor, a.SBUName, depo.DepotName, a.Confirmation " +
            //" FROM investmentinit a" +
            //" INNER JOIN employee E ON A.employeeid = E.id " +
            //" LEFT JOIN investmentdetail b ON a.id = b.investmentinitid " +
            //" LEFT JOIN InvestmentRecDepot depo on depo.investmentinitid = a.Id " +
            //" LEFT JOIN investmentrecv rcv ON a.id = rcv.investmentinitid " +
            //" LEFT JOIN employee M ON  rcv.employeeid= M.id  " +
            //" INNER JOIN donation d ON d.id = a.donationid " +
            //" LEFT JOIN InvestmentDoctor ind ON a.Id=ind.InvestmentInitId " +
            //" LEFT JOIN DoctorInfo dc ON ind.DoctorId=dc.Id" +
            //" Where 1=1 ";
            string qry = "SELECT * FROM [DIDS].[dbo].[InvestmentSummaryVW] WHERE 1=1";
            
            if (!string.IsNullOrEmpty(referenceNo) && referenceNo!="undefined")
            {
                qry = qry + " AND refno = '" + referenceNo + "'";
            }
            if (!string.IsNullOrEmpty(doctorName) && doctorName!="undefined")
            {
                qry = qry + " AND DoctorName Like '%" + doctorName + "%'";
            }
            if (doctorId != 0)
            {
                qry = qry + " AND DoctorId = " + doctorId + " ";
            }
            var results = _db.RptInvestmentSummary.FromSqlRaw(qry).ToList();

            return results;
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

            string qry = " select CAST(a.Id AS INT) as Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, d.DonationTypeName, a.DonationTo, b.ProposedAmount, b.FromDate, b.ToDate, " +
                //" dbo.fnGetInvestmentStatus(a.Id) InvStatus, "+ 
                " (select ISNULL ((select b.RecStatus from InvestmentRecComment b where b.InvestmentInitId = a.Id and b.Id in (select max(id) from InvestmentRecComment where InvestmentInitId = b.InvestmentInitId)), 'Pending') from InvestmentInit a)  InvStatus" + 
                " e.EmployeeName,dbo.fnGetInvestmentApprovedBy(a.Id) ApprovedBy,e.MarketName, ISNULL(rcv.ReceiveStatus, 'Not Completed') ReceiveStatus, ISNULL(rcvBy.EmployeeName, 'N/A') ReceiveBy " +
                " from InvestmentInit a " +
                " left join InvestmentDetail b on a.Id = b.InvestmentInitId " +
                " left join InvestmentRecv rcv on a.Id = rcv.InvestmentInitId " +
                " inner join Donation d on d.Id = a.DonationId " +
                " left join Employee e on e.Id = a.EmployeeId " +
                " left join Employee rcvBy on rcvBy.Id = rcv.EmployeeId " +
                " Where 1 = 1 AND " +
                //" dbo.fnGetInvestmentStatus(a.Id) = '" + dt.ApproveStatus + "'  "+
                " AND a.Confirmation = 1 AND b.ProposedAmount is NOT NULL";

            if (dt.FromDate.Year > 1000 && dt.ToDate.Year > 1000)
            {
                qry = qry + " AND(CONVERT(date, b.FromDate) >= CAST('" + dt.FromDate + "' as Date) AND CAST('" + dt.ToDate + "' as Date) >= CONVERT(date, b.ToDate)) ";
            }
            if (dt.UserRole != "Administrator")
            {
                qry = qry + " AND (" +
                            " e.SBU = COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
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
                " FROM [DIDS].[dbo].[Employee] WHERE 1 = 1 ";

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
        [Route("rptInvestDepo/{referenceNo}")]
        public async Task<IReadOnlyList<RptDepotLetter>> ReportDepotLetter(string referenceNo)
        {
            try
            {
                 string qry = " SELECT * FROM  ( " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName, " + 
                            " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount, inDetail.ChequeTitle, aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.DepotName " +
                            " from InvestmentInit a  " + 
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " + 
                            " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " + 
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " + 
                            " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " + 
                            " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId  left join DoctorInfo doc on inDc.DoctorId = doc.Id " + 
                            " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' " +
                            " AND inDetail.PaymentMethod = 'Cash'  " + 
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName, " + 
                            " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy',depo.DepotName " +
                            " from InvestmentInit a " + 
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " + 
                            " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " +
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                            " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                            " inner join InvestmentCampaign IC on a.Id = IC.InvestmentInitId " +
                            " left join DoctorInfo doc on IC.DoctorId = doc.Id   " +
                            " where a.DonationTo = 'Campaign' AND  " +
                            " ir.RecStatus = 'Approved' " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Institution' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName,  " +
                            " doc.id as DocId, doc.InstitutionName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy',depo.DepotName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id  " +
                            " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                            " left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentInstitution IC on a.Id = IC.InvestmentInitId  " +
                            " left join InstitutionInfo doc on IC.InstitutionId = doc.Id  " +
                            " where a.DonationTo = 'Institution' " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Bcds' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName,  " +
                            " doc.id as DocId, doc.BcdsName, doc.BcdsAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.DepotName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentBcds IC on a.Id = IC.InvestmentInitId  " +
                            " left join Bcds doc on IC.BcdsId = doc.Id  " +
                            " where a.DonationTo = 'Bcds'  " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " UNION " + 
                            " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Society' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName,  " +
                            " doc.id as DocId, doc.SocietyName, doc.SocietyAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.DepotName     " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
                            " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentSociety IC on a.Id = IC.InvestmentInitId  " +
                            " left join Society doc on IC.SocietyId = doc.Id  " +
                            " where a.DonationTo = 'Society'  " +
                            " AND inDetail.PaymentMethod = 'Cash'  " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " AND ir.RecStatus = 'Approved' ) x  " +                           
                            "  WHERE X.ReferenceNo = '" + referenceNo + "' ";

                var results = _db.RptDepotLetter.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("rptChqPrint/{referenceNo}")]
        public IReadOnlyList<RptDepotLetter> ReportChequeLetter(string referenceNo)
        {
            try
            {
                string qry = " SELECT * FROM  ( " +
                           " Select dtl.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName, " +
                           " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount, inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.EmployeeName +', '+ depo.DesignationName 'DepotName' " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                           " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                           " left join Employee depo on depo.Id = ir.EmployeeId  " +
                           " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                           " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId " +
                           " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId  left join DoctorInfo doc on inDc.DoctorId = doc.Id " +
                           " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId AND dtl.PaymentRefNo is NOT NULL " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash'  " + 
                           " UNION " +
                           " Select dtl.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Doctor' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName, " +
                           " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.EmployeeName +', '+ depo.DesignationName 'DepotName' " +
                           " from InvestmentInit a " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
                           " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                           " left join Employee depo on depo.Id = ir.EmployeeId  " +
                           " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                           " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                           " inner join InvestmentCampaign IC on a.Id = IC.InvestmentInitId " +
                           " left join DoctorInfo doc on IC.DoctorId = doc.Id   " +
                           " where a.DonationTo = 'Campaign' AND  " +
                           " ir.RecStatus = 'Approved' " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId AND dtl.PaymentRefNo is NOT NULL " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash' " +
                           " UNION " +
                           " Select dtl.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Institution' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName,  " +
                           " doc.id as DocId, doc.InstitutionName, doc.[Address], inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.EmployeeName +', '+ depo.DesignationName 'DepotName'  " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                           " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                           " left join Employee depo on depo.Id = ir.EmployeeId  " +
                           " left join Employee e on a.EmployeeId = e.Id  " +
                           " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                           " left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                           " inner join InvestmentInstitution IC on a.Id = IC.InvestmentInitId  " +
                           " left join InstitutionInfo doc on IC.InstitutionId = doc.Id  " +
                           " where a.DonationTo = 'Institution' " +
                           " AND ir.RecStatus = 'Approved'  " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId AND dtl.PaymentRefNo is NOT NULL " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash' " +
                           " UNION " +
                           " Select dtl.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Bcds' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName,  " +
                           " doc.id as DocId, doc.BcdsName, doc.BcdsAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.EmployeeName +', '+ depo.DesignationName 'DepotName'  " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                           " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                           " left join Employee depo on depo.Id = ir.EmployeeId " +
                           " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                           " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                           " inner join InvestmentBcds IC on a.Id = IC.InvestmentInitId  " +
                           " left join Bcds doc on IC.BcdsId = doc.Id  " +
                           " where a.DonationTo = 'Bcds'  " +
                           " AND ir.RecStatus = 'Approved'  " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId AND dtl.PaymentRefNo is NOT NULL " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           //" AND inDetail.PaymentMethod = 'Cash' " +
                           " UNION " +
                           " Select dtl.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, 'Society' DonationTo, e.Id as EmpId, e.DesignationName, e.MarketName, dtl.PaymentRefNo ReferenceNo, d.DonationTypeName,  " +
                           " doc.id as DocId, doc.SocietyName, doc.SocietyAddress, inDetail.ProposedAmount,  inDetail.ChequeTitle,  aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', depo.EmployeeName +', '+ depo.DesignationName 'DepotName' " +
                           " from InvestmentInit a  " +
                           " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
                           " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                           " left join Employee depo on depo.Id = ir.EmployeeId " +
                           " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                           " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                           " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                           " inner join InvestmentSociety IC on a.Id = IC.InvestmentInitId  " +
                           " left join Society doc on IC.SocietyId = doc.Id  " +
                           " where a.DonationTo = 'Society'  " +
                           " AND  ir.EmployeeId = inDetail.EmployeeId AND dtl.PaymentRefNo is NOT NULL " +
                           " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                           " AND ir.RecStatus = 'Approved' ) x  " +
                           // " AND inDetail.PaymentMethod = 'Cash'  " +
                           "  WHERE X.ReferenceNo = '" + referenceNo + "' ";

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
    [HttpGet]
        [Route("investmentDetailsForSummary/{investmentInitId}/{empId}/{userRole}")]
        public async Task<IReadOnlyList<object>> investmentDetailsForSummary(int investmentInitId,int empId,string userRole)
        {
            try
            {
                var initData = await _investmentInitRepo.GetByIdAsync(investmentInitId);
                var spec = new InvestmentRecSpecification(investmentInitId); 
                string qry = "SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, isnull(Max(id),0) Count FROM investmentrec WHERE  investmentinitid =" + investmentInitId;
                var result = _db.CountInt.FromSqlRaw(qry).ToList();
                if (result[0].Count == 0)
                {
                    var specDetail = new InvestmentDetailSpecification(investmentInitId);
                    var investmentDetail = await _investmentDetailRepo.ListAsync(specDetail);
                    return investmentDetail;
                }
                  var investmentRec = await _investmentRecRepo.ListAsync(spec);
                return investmentRec.Where(x => x.Id == result[0].Count).ToList();
                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


    
    }
}
