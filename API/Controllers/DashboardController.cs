using API.Dtos;
using API.Errors;
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
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        public DashboardController(IMapper mapper, StoreContext dbContext, IGenericRepository<ApprAuthConfig> apprAuthConfigRepo)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _apprAuthConfigRepo = apprAuthConfigRepo;
        }

        [HttpGet("totalApproved/{role}/{empCode}")]
        public object GetTotalApproved(string role, string empCode)
        {
            try
            {
                DateTime fd = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime td = DateTime.Today;


                string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empCode + "' ";
                var empData = _dbContext.Employee.FromSqlRaw(empQry).ToList();

                //string qry = " select CAST(a.Id AS INT) as Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, d.DonationTypeName, a.DonationTo, b.ProposedAmount, b.FromDate, b.ToDate, dbo.fnGetInvestmentStatus(a.Id) InvStatus, e.EmployeeName,dbo.fnGetInvestmentApprovedBy(a.Id) ApprovedBy,e.MarketName, ISNULL(rcv.ReceiveStatus, 'Not Completed') ReceiveStatus, ISNULL(rcvBy.EmployeeName, 'N/A') ReceiveBy " +
                string qry = " SELECT CAST('1' AS INT) AS Id ,1 AS DataStatus , SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(a.Id) Count " +
                    "  FROM InvestmentRecComment a LEFT JOIN InvestmentRecv rcv ON a.Id = rcv.InvestmentInitId " +
                    " WHERE 1 = 1 AND a.RecStatus = 'Approved'  AND rcv.ReceiveStatus IS NULL ";
                if (role != "Administrator")
                {
                    qry = qry + " AND a.SBU='" + empData[0].SBU + "' AND (" +
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

                var results = _dbContext.CountInt.FromSqlRaw(qry).ToList();
                return results[0].Count.ToString();

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        [HttpGet("GetApprAuth/{empId}")]
        public async Task<ApprovalAuthority> GetApprAuth(int empId)
        {
            var specAppr = new ApprAuthConfigSpecification(Convert.ToInt32(empId), "A");
            var appPriority = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);

            return appPriority.ApprovalAuthority;
        }


        [HttpGet("myPendingCount/{sbu}/{role}/{empCode}")]
        public async Task<object> GetMyPendingAsync(string sbu, string role, string empCode)
        {
            try
            {
                var appPriority = await GetApprAuth(Convert.ToInt32(empCode));

                string qry = "";

                if (appPriority.Priority == 1)
                {
                    //qry = " select CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count from InvestmentInit I " +
                    //    " where EXISTS ( (select InvestmentInitId from InvestmentTargetedGroup IT " +
                    //    " where EXISTS ( (select MarketCode from Employee " +
                    //    " where Id = " + empCode + " AND MarketCode=It.MarketCode ) and CompletionStatus = 0) and Confirmation = 1 AND IT.InvestmentInitId=I.Id ";
                    qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count" +
                        " FROM InvestmentInit I" +
                        " WHERE EXISTS (" +
                        "			SELECT InvestmentInitId" +
                        "			FROM InvestmentTargetedGroup IT" +
                        "			WHERE EXISTS (SELECT MarketCode FROM Employee WHERE Id = " + empCode + "  AND MarketCode=IT.MarketCode)" +
                        "					AND " +
                        "					IT.CompletionStatus = 0" +
                        "				    AND IT.InvestmentInitId = I.Id" +
                        "		)" +
                        "		AND I.Confirmation = 1";

                }
                else if (appPriority.Priority == 2)
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", Convert.ToInt32(empCode))
                    };
                    var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentRecSearch @SBU,@EID", parms.ToArray()).ToList();
                    return results.Count();
                    //qry = " select CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count from InvestmentInit " +
                    //    " where Id in (select InvestmentInitId from InvestmentRecComment " +
                    //    " where TerritoryCode in (select TerritoryCode from Employee " +
                    //    " where Id = "+empCode+" ) and CompletionStatus = 0 and [Priority] = 2) and Confirmation = 1 ";
                }
                else if (role == "RSM")
                {
                    string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empCode + "' ";
                    var empData = _dbContext.Employee.FromSqlRaw(empQry).ToList();
                    qry = " select CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count from InvestmentInit I  " +
                        " where  Exists (select InvestmentInitId from InvestmentRecComment a " +
                            //" where RegionCode in (select RegionCode from Employee " +
                            //" where Id = "+empCode+"  ) and CompletionStatus = 0 and [Priority] = 3) "
                            " where " +
                            " (a.SBU = COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All')" +
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
                            " ) AND " +
                            //" 'Completed' = [dbo].[fnGetRecStatusByPriority](A.InvestmentInitId, " + appPriority.Priority + " - 1)" +
                            " EXISTS (SELECT InvestmentInitId FROM [InvestmentRecComment] a WHERE a.[InvestmentInitId] = I.Id AND a.CompletionStatus = '1'" +
                            " AND a.Priority = " + appPriority.Priority + " - 1 AND a.RecStatus<>'Cancelled' )" +
                            " AND a.InvestmentInitId=I.Id" +
                            " )" +
                            " AND  Not Exists" +
                            " ( select InvestmentInitId from InvestmentRecComment a " +
                            " where " +
                            " (a.SBU = COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All')" +
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
                            " ) AND " +
                            " A.Priority = " + appPriority.Priority + " AND A.InvestmentInitId=I.Id )" +
                            " ";
                }
                else if (appPriority.Priority > 3)
                {
                    // string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empCode + "' ";
                    // var empData = _dbContext.Employee.FromSqlRaw(empQry).ToList();
                    // qry = " select CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count from InvestmentInit I  " +
                    //         " where DonationTo <>'Campaign' AND  EXISTS (select InvestmentInitId from InvestmentRecComment a " +
                    //         //" where RegionCode in (select RegionCode from Employee " +
                    //         //" where Id = "+empCode+"  ) and CompletionStatus = 0 and [Priority] = 3) "
                    //         " where " +
                    //         " (a.SBU = COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.MarketGroupCode = COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.MarketCode = COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.TerritoryCode = COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.RegionCode = COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.ZoneCode = COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All') = 'All'" +
                    //         " )  " +
                    //         //" 'Completed' = [dbo].[fnGetRecStatusByPriority](A.InvestmentInitId, " + appPriority.Priority + " - 1)" +
                    //         " AND EXISTS ( SELECT InvestmentInitId  FROM [InvestmentRecComment] a WHERE a.[InvestmentInitId] = I.Id AND a.CompletionStatus = '1'" +
                    //         " AND a.Priority = " + appPriority.Priority + " - 1)" +
                    //         " AND A.Priority=" + appPriority.Priority + " - 1" +
                    //          " AND RecStatus <> 'Approved' AND A.InvestmentInitId = Id " +
                    //         " )" +
                    //         " AND  Not EXISTS" +
                    //         " ( select InvestmentInitId from InvestmentRecComment a " +
                    //         " where " +
                    //         " (a.SBU = COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].SBU + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.MarketGroupCode = COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.MarketCode = COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.TerritoryCode = COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.RegionCode = COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All') = 'All'" +
                    //         " )" +
                    //         " AND (" +
                    //         " a.ZoneCode = COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All')" +
                    //         " OR COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All') = 'All'" +
                    //         " ) AND " +
                    //         " A.Priority = " + appPriority.Priority + " AND A.InvestmentInitId = Id )" +
                    //         " ";
                    if (sbu == "null")
                    {
                        List<SqlParameter> parmsNoSBU = new List<SqlParameter>
                    {
                        new SqlParameter("@EID", Convert.ToInt32(empCode)),
                    };
                        var resultsNoSBU = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchNoSbu @EID", parmsNoSBU.ToArray()).ToList();
                        return resultsNoSBU.Count();
                    }
                    else
                    {


                        List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", Convert.ToInt32(empCode)),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                        var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchNotCamp @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                        return results.Count();
                    }
                }
                else if (role == "GPM")
                {
                    // qry = " select CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count from InvestmentInit  " +
                    //     " where Id in (select InvestmentInitId from InvestmentRecComment " +
                    //     " where RegionCode in (select RegionCode from Employee " +
                    //     " where Id = " + empCode + "  ) and CompletionStatus = 0 and [Priority] = 3) ";
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", Convert.ToInt32(empCode)),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                    var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchForGPM @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                    return results.Count();
                }
                else
                {
                    return 0;
                }

                var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
                return result[0].Count.ToString();
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }


        [HttpGet("approvalPending/{role}/{empCode}")]
        public object GetApprovalPending(string role, string empCode)
        {
            try
            {
                DateTime fd = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime td = DateTime.Today;

                string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empCode + "' ";
                var empData = _dbContext.Employee.FromSqlRaw(empQry).ToList();

                //string qry = " select CAST(a.Id AS INT) as Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, d.DonationTypeName, a.DonationTo, b.ProposedAmount, b.FromDate, b.ToDate, dbo.fnGetInvestmentStatus(a.Id) InvStatus, e.EmployeeName,dbo.fnGetInvestmentApprovedBy(a.Id) ApprovedBy,e.MarketName, ISNULL(rcv.ReceiveStatus, 'Not Completed') ReceiveStatus, ISNULL(rcvBy.EmployeeName, 'N/A') ReceiveBy " +
                string qry = " select CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(a.Id) Count " +
                  " from InvestmentInit a " +
                  " left join InvestmentDetail b on a.Id = b.InvestmentInitId " +
                  " left join InvestmentRecv rcv on a.Id = rcv.InvestmentInitId " +
                  " inner join Donation d on d.Id = a.DonationId " +
                  " left join Employee e on e.Id = a.EmployeeId " +
                  " left join Employee rcvBy on rcvBy.Id = rcv.EmployeeId " +
                  " Where 1 = 1 AND dbo.fnGetInvestmentStatus(a.Id) = 'Pending' AND a.Confirmation = 1 AND b.ProposedAmount is NOT NULL";
                //" AND(CONVERT(date, b.FromDate) >= CAST('" + fd + "' as Date) AND CAST('" + td + "' as Date) >= CONVERT(date, b.ToDate)) ";
                if (role != "Administrator")
                {
                    qry = qry + " AND a.SBU='" + empData[0].SBU + "' AND (" +
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


                //var results = _dbContext.Set.FromSqlRaw(qry).ToList();
                var results = _dbContext.CountInt.FromSqlRaw(qry).ToList();
                return results[0].Count.ToString();

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
