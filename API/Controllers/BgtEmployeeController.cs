using API.Dtos;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BgtEmployeeController : BaseApiController
    {
        private readonly StoreContext _dbContext;

        public BgtEmployeeController( StoreContext dbContext)
        {
            _dbContext = dbContext;
        }


          [HttpGet("approvalAuthoritiesForConfig")]
        public async Task<IReadOnlyList<ApprovalAuthority>> GetApprovalAuthorities()
        {
            try
            {
                var result = _dbContext.ApprovalAuthority.FromSqlRaw("select * from ApprovalAuthority where ApprovalAuthorityName = 'GPM' OR Priority > 2").ToList();
                return result;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("getDeptSBUBudget/{deptId}/{sbu}/{year}")]
        public ActionResult<IReadOnlyList<CountLong>> GetDeptSBUWiseBudget(int deptId, string sbu, int year)
        {
            try
            {
                string qry = "";

                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,  ISNULL(SUM(SBUAmount), 0)  Count from BgtSBUTotal " +
                    " where DeptId = "+ deptId + " " +
                    " and SBU = '"+ sbu + "' " +
                    " and [Year] = '"+ year  + "' " +
                    " and DataStatus = 1 ";

                var result = _dbContext.CountLong.FromSqlRaw(qry).ToList();

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getPrevAlloc/{deptId}/{sbu}/{year}")]
        public ActionResult<IReadOnlyList<CountLong>> GetPreviousAllocated(int deptId, string sbu, int year)
        {
            try
            {
                string qry = "";

                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, ISNULL(SUM(Amount), 0) Count from BgtOwn " +
                    " where DeptId = "+ deptId + " " +
                    " and SBU = '"+ sbu + "' " +
                    " and [Year] = '"+ year  + "' " +
                    " and DataStatus = 1 ";

                var result = _dbContext.CountLong.FromSqlRaw(qry).ToList();

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getAuthPersonCount/{authId}/{sbu}")]
        public ActionResult<IReadOnlyList<CountInt>> GetTotalAuthPerson(int authId, string sbu)
        {
            try
            {
                string qry = "";

                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, count(*) Count from ApprAuthConfig a " +
                      " left join Employee b on a.EmployeeId = b.Id " + 
                      " where a.ApprovalAuthorityId = "+ authId + " " + 
                      " and b.SBU = '"+ sbu + "' and b.DataStatus = 1 ";

                var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost("insertBgtEmployee")]
        public async Task<Int32> BgtEmpInsert(BgtEmpInsertDto dto)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@DeptId", dto.DeptId),
                        new SqlParameter("@Year", dto.Year),
                        new SqlParameter("@SBU ", dto.SBU),
                        new SqlParameter("@AuthId", dto.AuthId),
                        new SqlParameter("@Amount", dto.Amount),
                        new SqlParameter("@Segment", dto.Segment),
                        new SqlParameter("@PermView", dto.PermView),
                        new SqlParameter("@PermEdit", dto.PermEdit),
                        new SqlParameter("@EnteredBy", dto.EnteredBy),
                    };

                var results = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmployeeInsert] @DeptId, @Year, @SBU , @AuthId, @Amount, @Segment, @PermView, @PermEdit, @EnteredBy", parms.ToArray());
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


         [HttpPost("insertBgtOwn")]
        public async Task<Int32> BgtOwnInsert(BgtOwnInsertDto dto)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@DeptId", dto.DeptId),
                        new SqlParameter("@Year", dto.Year),
                        new SqlParameter("@SBU ", dto.SBU),
                        new SqlParameter("@AuthId", dto.AuthId),
                        new SqlParameter("@Amount", dto.Amount),
                        new SqlParameter("@AmtLimit", dto.AmtLimit),
                        new SqlParameter("@Segment", dto.Segment),
                        new SqlParameter("@EnteredBy", dto.EnteredBy),
                        new SqlParameter("@DonationId", dto.DonationId),
                    };

                var results = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsert] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId", parms.ToArray());
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
