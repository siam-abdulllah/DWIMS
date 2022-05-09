using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BgtOwnController : BaseApiController
    {
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<Employee> _employeeRepo;
        public BgtOwnController(StoreContext dbContext, IGenericRepository<Employee> employeeRepo)

        {
            _dbContext = dbContext;
            _employeeRepo = employeeRepo;
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
        public async Task<IReadOnlyList<Employee>> GetAllEmp()
        {
            try
            {
                string qry = "SELECT DISTINCT C.[Id], C.[DataStatus],C.[SetOn],C.[ModifiedOn],C.[EmployeeSAPCode],C.[EmployeeName]," +
                    " C.[DepartmentId],C.[DepartmentName],C.[DesignationId],C.[DesignationName],C.[CompanyId],C.[CompanyName]," +
                    " C.[JoiningDate],C.[JoiningPlace],C.[Phone],C.[Email],C.[SBU],C.[SBUName],C.[ZoneCode],C.[ZoneName]," +
                    " C.[RegionCode],C.[RegionName],C.[TerritoryCode],C.[TerritoryName],C.[MarketCode],C.[MarketName]," +
                    " C.[MarketGroupCode],C.[MarketGroupName],C.[DepotCode],C.[DepotName] " +
                    " FROM ApprovalAuthority A INNER JOIN ApprAuthConfig B ON A.Id = B.ApprovalAuthorityId INNER JOIN Employee C ON B.EmployeeId = C.Id" +
                    " WHERE C.DataStatus=1 AND (A.ApprovalAuthorityName = 'GPM' OR A.Priority > 2)";


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
                    " A.[Priority] FROM ApprovalAuthority A INNER JOIN ApprAuthConfig B ON A.Id = B.ApprovalAuthorityId INNER JOIN Employee C ON B.EmployeeId = C.Id" +
                    " WHERE C.DataStatus=1 AND (A.ApprovalAuthorityName = 'GPM' OR A.Priority > 2) AND C.Id=" + employeeId;


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
                    " FROM [DIDS].[dbo].[EmpSbuMapping] A " +
                    " WHERE A.DataStatus=1 AND  A.EmployeeId=" + employeeId;


                var results = _dbContext.SBU.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEmpWiseBgt/{employeeId}/{sbu}/{year}/{compId}/{deptId}")]
        public IReadOnlyList<BgtEmployee> GetEmpWiseBgt(int employeeId, string sbu, int year, int compId, int deptId)
        {
            try
            {
                string qry = "SELECT  [Id],[DataStatus],[SetOn],[ModifiedOn] ,[CompId],[DeptId] ," +
                    "[Year],[SBU],[EmployeeId],[AuthId],[Amount] ,[Segment],[PermEdit],[PermView]," +
                    "[PermAmt],[PermDonation],[Remarks],[EnteredBy] FROM [DIDS].[dbo].[BgtEmployee]" +
                    " WHERE [DataStatus]=1 AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " AND [Year]=" + year;


                var results = _dbContext.BgtEmployee.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } 
        [HttpGet("getEmpOwnBgt/{employeeId}/{sbu}/{year}/{compId}/{deptId}")]
        public IReadOnlyList<BgtOwn> GetEmpOwnBgt(int employeeId, string sbu, int year, int compId, int deptId)
        {
            try
            {
                string qry = "SELECT [Id],[DataStatus],[SetOn],[ModifiedOn],[CompId],[DeptId],[Year],[Month],[EmployeeId],[SBU]," +
                    "[DonationId],[Amount],[AmtLimit],[Segment],[Remarks],[EnteredBy]" +
                    " FROM BgtOwn WHERE [DataStatus]=1 AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " " +
                    " AND [Month]=Month(GETDATE()) AND [Year]=" + year;

                var results = _dbContext.BgtOwn.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } 
        [HttpGet("getDonWiseBgt/{employeeId}/{sbu}/{year}/{compId}/{deptId}/{donationId}")]
        public IReadOnlyList<BgtOwn> GetDonWiseBgt(int employeeId, string sbu, int year, int compId, int deptId, int donationId)
        {
            try
            {
                string qry = "SELECT [Id],[DataStatus],[SetOn],[ModifiedOn],[CompId],[DeptId],[Year],[Month],[EmployeeId],[SBU]," +
                    "[DonationId],[Amount],[AmtLimit],[Segment],[Remarks],[EnteredBy]" +
                    " FROM BgtOwn WHERE [DataStatus]=1 AND [DonationId]=" + donationId + " AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " " +
                    " AND [Month]=Month(GETDATE()) AND [Year]=" + year;

                var results = _dbContext.BgtOwn.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getEmpWiseTotPipe/{employeeId}/{sbu}/{year}/{compId}/{deptId}")]
        public IReadOnlyList<BgtEmployee> GetEmpWiseTotExp(int employeeId, string sbu, int year, int compId, int deptId)
        {
            try
            {
                string qry = "SELECT  [Id],[DataStatus],[SetOn],[ModifiedOn] ,[CompId],[DeptId] ," +
                    "[Year],[SBU],[EmployeeId],[AuthId],[Amount] ,[Segment],[PermEdit],[PermView]," +
                    "[PermAmt],[PermDonation],[Remarks],[EnteredBy] FROM [DIDS].[dbo].[BgtEmployee]" +
                    " WHERE [DataStatus]=1 AND [EmployeeId]=" + employeeId + " AND [SBU]='" + sbu + "' AND [CompId]=" + compId + " AND [DeptId]=" + deptId + " AND [Year]=" + year;


                var results = _dbContext.BgtEmployee.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEmpWiseTotExp/{employeeId}/{sbu}/{year}/{compId}/{deptId}")]
        public IReadOnlyList<CountDouble> GetEmpWiseTotPipe(int employeeId, string sbu, int year, int compId, int deptId)
        {
            try
            {
                string qry = "SELECT  1  AS Id ,1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn," +
                    " ISNULL(SUM(ApprovedAmount), 0) Count FROM [DIDS].[dbo].[InvestmentDetailTracker]  " +
                    " A INNER JOIN [DIDS].[dbo].[InvestmentRecComment] B ON A.InvestmentInitId=B.InvestmentInitId AND A.EmployeeId=B.EmployeeId" +
                    " WHERE A.[DataStatus]=1 AND A.[EmployeeId]=" + employeeId + " AND B.[SBU]='" + sbu + "' AND  A.Month<=Month(GETDATE()) AND A.[Year]=" + year;


                var results = _dbContext.CountDouble.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
