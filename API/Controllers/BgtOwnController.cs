using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class BgtOwnController : BaseApiController
    {
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<BgtEmployeeDetail> _employeeDetailRepo;
        public BgtOwnController(StoreContext dbContext, IGenericRepository<Employee> employeeRepo, IGenericRepository<BgtEmployeeDetail> employeeDetailRepo)

        {
            _dbContext = dbContext;
            _employeeRepo = employeeRepo;
            _employeeDetailRepo = employeeDetailRepo;
        }

        [HttpGet("getDeptSBUBudget/{deptId}/{sbu}/{year}")]
        public ActionResult<IReadOnlyList<CountInt>> GetDeptSBUWiseBudget(int deptId, string sbu, int year)
        {
            try
            {
                string qry = "";
                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, CAST(SBUAmount AS INT)  Count from BgtSBUTotal " +
                    " where DeptId = " + deptId + " " +
                    " and SBU = '" + sbu + "' " +
                    " and [Year] = '" + year + "' " +
                    " and DataStatus = 1 ";
                var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getAuthPersonCount/{authId}")]
        public ActionResult<IReadOnlyList<CountInt>> GetTotalAuthPerson(int authId)
        {
            try
            {
                string qry = "";

                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, count(*) Count from ApprAuthConfig " +
                    " where ApprovalAuthorityId = " + authId + " ";

                var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getAllEmp")]
        public IReadOnlyList<Employee> GetAllEmp()
        {
            try
            {
                string qry = "SELECT DISTINCT C.[Id], C.[DataStatus],C.[SetOn],C.[ModifiedOn],C.[EmployeeSAPCode],C.[EmployeeName]," +
                    " C.[DepartmentId],C.[DepartmentName],C.[DesignationId],C.[DesignationName],C.[CompanyId],C.[CompanyName]," +
                    " C.[JoiningDate],C.[JoiningPlace],C.[Phone],C.[Email],C.[SBU],C.[SBUName],C.[ZoneCode],C.[ZoneName]," +
                    " C.[RegionCode],C.[RegionName],C.[TerritoryCode],C.[TerritoryName],C.[MarketCode],C.[MarketName]," +
                    " C.[MarketGroupCode],C.[MarketGroupName],C.[DepotCode],C.[DepotName] " +
                    " FROM ApprovalAuthority A INNER JOIN ApprAuthConfig B ON A.Id = B.ApprovalAuthorityId INNER JOIN Employee C ON B.EmployeeId = C.Id" +
                    " WHERE C.DataStatus=1 AND  A.ID NOT IN (1,2,9,10,11,13,15) AND B.Status = 'A'";


                var results = _dbContext.Employee.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getSbuWiseEmp/{sbu}")]
        public async Task<IReadOnlyList<Employee>> GetSbuWiseEmp(string sbu)
        {
            try
            {
                var empData = await _employeeRepo.ListAllAsync();
                var data = (from e in empData
                            where e.SBU == sbu
                            orderby e.EmployeeName
                            select new Employee
                            {
                                Id = e.Id,
                                EmployeeSAPCode = e.EmployeeSAPCode,
                                EmployeeName = e.EmployeeName,
                                DepartmentName = e.DepartmentName,
                                DesignationName = e.DesignationName,
                                Phone = e.Phone,
                                Email = e.Email,
                                MarketName = e.MarketName,
                                RegionName = e.RegionName,
                                ZoneName = e.ZoneName,
                                TerritoryName = e.TerritoryName,
                                MarketGroupName = e.MarketGroupName,
                                SBU = e.SBU,

                            }
                             ).Distinct().ToList();
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEmpWiseData/{employeeId}")]
        public IReadOnlyList<ApprovalAuthority> GetEmpWiseData(int employeeId)
        {
            try
            {
                string qry = "SELECT DISTINCT A.[Id], A.[DataStatus],A.[SetOn],A.[ModifiedOn],A.[ApprovalAuthorityName],A.[Remarks],A.[Status]," +
                    " A.[Priority],A.[CompId],A.[DeptId] FROM ApprovalAuthority A INNER JOIN ApprAuthConfig B ON A.Id = B.ApprovalAuthorityId INNER JOIN Employee C ON B.EmployeeId = C.Id" +
                    // " WHERE C.DataStatus=1 AND (A.ApprovalAuthorityName = 'GPM' OR A.Priority > 2) AND C.Id=" + employeeId;
                    " WHERE C.DataStatus=1 AND B.Status='A' AND C.Id=" + employeeId;


                var results = _dbContext.ApprovalAuthority.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEmpWiseSBU/{employeeId}")]
        public IReadOnlyList<SBU> GetEmpWiseSBU(int employeeId)
        {
            try
            {
                string qry = "SELECT A.[Id], A.[DataStatus],A.[SetOn],A.[ModifiedOn],SBU SBUCode,SBUName,NULL Remarks " +
                    " FROM [dbo].[EmpSbuMapping] A " +
                    " WHERE A.DataStatus=1 AND  A.EmployeeId=" + employeeId;


                var results = _dbContext.SBU.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEmpWiseBgt/{employeeId}/{sbu}/{year}/{compId}/{deptId}/{authId}")]
        public IReadOnlyList<BgtEmployeeDetail> GetEmpWiseBgt(int employeeId, string sbu, int year, int compId, int deptId, int authId)
        {
            try
            {
                //string qry = "SELECT  [Id],[DataStatus],[SetOn],[ModifiedOn] ,[CompId],[DeptId] ," +
                //    "[Year],[SBU],[EmployeeId],[AuthId],[Amount] ,[Segment],[PermEdit],[PermView]," +
                //    "[PermAmt],[PermDonation],[Remarks],[EnteredBy] FROM  [dbo].[BgtEmployee]" +
                //    " WHERE [DataStatus]=1 AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " AND [Year]=" + year;

                string qry = "SELECT A.[Id] , A.[DataStatus] ,A.[SetOn] ,A.[ModifiedOn] ,A.[CompId] ,A.[DeptId] ,A.[Year] " +
                    " ,A.[SBU] ,A.[AuthId] ,A.[Code] ,A.[CompoCode] ,A.[Amount] ,A.[PermEdit] ,A.[PermView] ,A.[PermAmt] " +
                    ",A.[PermDonation] ,A.[Remarks] ,A.[EnteredBy] FROM BgtEmployeeDetail A INNER JOIN EmpSbuMapping B ON A.Code = B.TagCode " +
                    " WHERE B.EmployeeId =" + employeeId + " AND B.SBU = '" + sbu + "'  AND B.DeptId = " + deptId + " " +
                    " AND B.CompId = " + compId + " AND A.AuthId = " + authId + "  AND A.DataStatus = 1 " +
                    " ";


                var results = _dbContext.BgtEmployeeDetail.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEmpOwnBgt/{employeeId}/{sbu}/{year}/{compId}/{deptId}/{authId}")]
        public List<BgtOwnTotal> GetEmpOwnBgt(int employeeId, string sbu, int year, int compId, int deptId, int authId)
        {
            try
            {
                //string qry = "SELECT [Id],[DataStatus],[SetOn],[ModifiedOn],[CompId],[DeptId],[Year],[Month],[EmployeeId],[SBU]," +
                //    "[DonationId],[Amount],[AmtLimit],[Segment],[Remarks],[EnteredBy]" +
                //    " FROM BgtOwn WHERE [DataStatus]=1 AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " " +
                //    " AND [Month]=Month(GETDATE()) AND [Year]=" + year;
                string qry = "SELECT  A.[Id] , A.[DataStatus] ,A.[SetOn] ,A.[ModifiedOn] ,A.[CompId] ,A.[DeptId] ,A.[Year] ,A.[Month] ,A.[SBU] " +
                  " ,A.[DonationId] ,A.[Amount] ,A.[AmtLimit] ,A.[Segment] ,A.[Remarks] ,A.[EnteredBy] ,A.[AuthId] ,A.[Code] ,A.[CompoCode] " +
                  ",(SELECT isnull(sum(ISNULL(C.[Amount], 0)), 0) FROM BgtOwn C WHERE C.SBU = A.SBU AND C.CompoCode = A.CompoCode" +
                  " AND C.DeptId = A.DeptId AND C.CompId = A.CompId AND C.AuthId = A.AuthId AND C.DataStatus = 1 AND C.DonationId = A.DonationId" +
                  " AND C.[Year] = A.[Year] ) TotalAmount ,(SELECT isnull(sum(ISNULL(C.[Amount], 0)), 0) FROM BgtOwn C WHERE C.SBU = A.SBU AND " +
                  " C.CompoCode = A.CompoCode AND C.DeptId = A.DeptId AND C.CompId = A.CompId AND C.AuthId = A.AuthId AND C.DataStatus = 1 " +
                  " AND C.DonationId = A.DonationId AND C.[Year] = A.[Year] AND C.Month Between 1 and (Month(GETDATE())-1) ) PrevMonthsAmount" +
                  " FROM BgtOwn A INNER JOIN EmpSbuMapping B ON A.Code = B.TagCode " +
                  " WHERE B.EmployeeId =" + employeeId + " AND B.SBU = '" + sbu + "'  AND B.DeptId = " + deptId + " " +
                  " AND B.CompId = " + compId + " AND B.DataStatus=1 AND A.AuthId = " + authId + "  AND A.DataStatus = 1 " +
                  " AND ( A.Month = Month(GETDATE()) OR A.Month=0)  AND [Year]=" + year;

                var results = _dbContext.BgtOwnTotal.FromSqlRaw(qry).ToList();
                

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getDonWiseBgt/{employeeId}/{sbu}/{year}/{compId}/{deptId}/{donationId}")]
        public List<BgtOwnTotal> GetDonWiseBgt(int employeeId, string sbu, int year, int compId, int deptId, int donationId, int authId)
        {
            try
            {
                //string qry = "SELECT [Id],[DataStatus],[SetOn],[ModifiedOn],[CompId],[DeptId],[Year],[Month],[EmployeeId],[SBU]," +
                //    "[DonationId],[Amount],[AmtLimit],[Segment],[Remarks],[EnteredBy]" +
                //    " FROM BgtOwn WHERE [DataStatus]=1 AND [DonationId]=" + donationId + " AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " " +
                //    " AND [Month]=Month(GETDATE()) AND [Year]=" + year;

                string qry = "SELECT A.[Id] , A.[DataStatus] ,A.[SetOn] ,A.[ModifiedOn] ,A.[CompId] ,A.[DeptId] ,A.[Year] ,A.[Month] ,A.[SBU] " +
                 " ,A.[DonationId] ,A.[Amount] ,A.[AmtLimit] ,A.[Segment] ,A.[Remarks] ,A.[EnteredBy] ,A.[AuthId] ,A.[Code] ,A.[CompoCode] " +
                 " FROM BgtOwn A INNER JOIN EmpSbuMapping B ON A.Code = B.TagCode " +
                 " WHERE B.EmployeeId =" + employeeId + " AND B.SBU = '" + sbu + "'  AND B.DeptId = " + deptId + " " +
                 " AND B.CompId = " + compId + " AND A.AuthId = " + authId + " AND A.[DonationId]=" + donationId + "  AND A.DataStatus = 1 AND B.DataStatus = 1  " +
                 " AND ( A.Month = Month(GETDATE()) OR A.Month=0)  AND [Year]=" + year;


                //var results = _dbContext.BgtOwn.FromSqlRaw(qry).ToList();
                var results = _dbContext.ExecSQL<BgtOwnTotal>(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        //[HttpGet("getEmpWiseTotPipe/{employeeId}/{sbu}/{year}/{compId}/{deptId}")]
        //public IReadOnlyList<BgtEmployee> GetEmpWiseTotExp(int employeeId, string sbu, int year, int compId, int deptId)
        //{
        //    try
        //    {
        //        string qry = "SELECT  [Id],[DataStatus],[SetOn],[ModifiedOn] ,[CompId],[DeptId] ," +
        //            "[Year],[SBU],[EmployeeId],[AuthId],[Amount] ,[Segment],[PermEdit],[PermView]," +
        //            "[PermAmt],[PermDonation],[Remarks],[EnteredBy] FROM [dbo].[BgtEmployee]" +
        //            " WHERE [DataStatus]=1 AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " AND [Year]=" + year;


        //        var results = _dbContext.BgtEmployee.FromSqlRaw(qry).ToList();
        //        return results;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet("getEmpWiseTotExp/{employeeId}/{sbu}/{year}/{compId}/{deptId}")]
        public ActionResult<IReadOnlyList<CountDouble>> GetEmpWiseTotExp(int employeeId, string sbu, int year, int compId, int deptId)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@EID", employeeId),
                        new SqlParameter("@Year", year),
                         new SqlParameter("@SBU", sbu),

                    };
                if (deptId == 1)
                {
                    return _dbContext.CountDouble.FromSqlRaw("EXECUTE [SP_InvestmentExpByEmpSales] @EID, @Year,@SBU", parms.ToArray()).ToList();

                }
                else
                {
                    return _dbContext.CountDouble.FromSqlRaw("EXECUTE [SP_InvestmentExpByEmpPMD] @EID, @Year,@SBU", parms.ToArray()).ToList();

                }
                 


            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

      [HttpGet("getEmpDonWiseTotExp/{employeeId}/{sbu}/{year}/{compId}/{deptId}")]
        public ActionResult<IReadOnlyList<DonWiseExpByEmp>> GetEmpDonWiseTotExp(int employeeId, string sbu, int year, int compId, int deptId)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@EID", employeeId),
                        new SqlParameter("@Year", year),
                         new SqlParameter("@SBU", sbu),

                    };
                if (deptId == 1)
                {
                    return _dbContext.DonWiseExpByEmp.FromSqlRaw("EXECUTE [SP_InvestmentDonWiseExpByEmpSales] @EID, @Year,@SBU", parms.ToArray()).ToList();

                }
                else
                {
                    return _dbContext.DonWiseExpByEmp.FromSqlRaw("EXECUTE [SP_InvestmentDonWiseExpByEmpPMD] @EID, @Year,@SBU", parms.ToArray()).ToList();

                }
                

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

     

        [HttpPost("insertBgtEmployeeDetail")]
        public ActionResult<BgtEmpDetailInsert> BgtEmpInsertDetail(BgtEmpDetailInsert dto)
        {
            try
            {
                string qry = "SELECT A.[Id] , A.[DataStatus] ,A.[SetOn] ,A.[ModifiedOn] ,A.[CompId] ,A.[DeptId] ,A.[Year] " +
                       " ,A.[SBU] ,A.[AuthId] ,A.[Code] ,A.[CompoCode] ,A.[Amount] ,A.[PermEdit] ,A.[PermView] ,A.[PermAmt] " +
                       ",A.[PermDonation] ,A.[Remarks] ,A.[EnteredBy] FROM BgtEmployeeDetail A INNER JOIN EmpSbuMapping B ON A.Code = B.TagCode " +
                       " WHERE  A.Year =" + dto.Year + " AND B.EmployeeId =" + dto.EmployeeId + " AND B.SBU = '" + dto.SBU + "'  AND B.DeptId = " + dto.DeptId + " " +
                       " AND B.CompId = " + dto.CompId + " AND A.AuthId = " + dto.AuthId + "  AND A.DataStatus = 1 " +
                       " ";


                var results = _dbContext.BgtEmployeeDetail.FromSqlRaw(qry).ToList();
                if (results.Count > 0)
                {
                    foreach (var v in results)
                    {

                        v.DataStatus = 0;
                        v.ModifiedOn = DateTimeOffset.Now;
                        _employeeDetailRepo.Update(v);
                        _employeeDetailRepo.Savechange();
                    }

                }
                var bgtEmployeeDetail = new BgtEmployeeDetail();
                bgtEmployeeDetail = new BgtEmployeeDetail
                {
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now,
                    DataStatus = 1,
                    CompId = dto.CompId,
                    DeptId = dto.DeptId,
                    Year = dto.Year,
                    SBU = dto.SBU,
                    AuthId = dto.AuthId,
                    Code = results[0].Code,
                    CompoCode = results[0].CompoCode,
                    Amount = dto.Amount,
                    PermEdit = dto.PermEdit,
                    PermView = dto.PermView,


                };

                _employeeDetailRepo.Add(bgtEmployeeDetail);
                _employeeDetailRepo.Savechange();

                return new BgtEmpDetailInsert
                {

                    CompId = bgtEmployeeDetail.CompId,
                    DeptId = bgtEmployeeDetail.DeptId,
                    Year = bgtEmployeeDetail.Year,
                    SBU = bgtEmployeeDetail.SBU,
                    AuthId = bgtEmployeeDetail.AuthId,
                    Amount = bgtEmployeeDetail.Amount,
                    PermEdit = bgtEmployeeDetail.PermEdit,
                    PermView = bgtEmployeeDetail.PermView,


                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("insertBgtOwnDetail")]
        public Int32 BgtOwnDetail(List<BgtOwnDetailInsert> bgtOwnDetailInsert)
        {
            try
            {
                var result = 0;
                foreach (var item in bgtOwnDetailInsert)
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                        {
                            new SqlParameter("@DeptId", item.DeptId),
                            new SqlParameter("@Year", item.Year),
                            new SqlParameter("@SBU ", item.SBU),
                            new SqlParameter("@AuthId", item.AuthId),
                            new SqlParameter("@Amount", item.Amount),
                            new SqlParameter("@AmtLimit", item.AmtLimit),
                            new SqlParameter("@Segment", item.Segment),
                            new SqlParameter("@EnteredBy", item.EnteredBy),
                            new SqlParameter("@DonationId", item.DonationId),
                            new SqlParameter("@EmployeeId", item.EmployeeId),
                        };
                    if (item.AuthId == 3)
                    {
                        result = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsertSingleRSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId, @EmployeeId", parms.ToArray());
                    }
                    else if (item.AuthId == 5)
                    {
                        result = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsertSingleDSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId, @EmployeeId", parms.ToArray());
                    }
                    else if (item.DeptId == 2)//for ONLY PMD
                    {
                        result = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsertSinglePMD] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId, @EmployeeId", parms.ToArray());
                    }
                    else
                    {
                        result = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsertSingle] @DeptId, @Year, @SBU , @AuthId, @Amount,  @AmtLimit, @Segment, @EnteredBy, @DonationId, @EmployeeId", parms.ToArray());
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



    }
}
