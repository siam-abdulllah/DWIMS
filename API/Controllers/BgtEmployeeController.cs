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

        public BgtEmployeeController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("approvalAuthoritiesForConfig")]
        public IReadOnlyList<ApprovalAuthority> GetApprovalAuthorities()
        {
            try
            {
                var result = _dbContext.ApprovalAuthority.FromSqlRaw("select * from ApprovalAuthority where ApprovalAuthorityName = 'GPM' OR Priority > 2 ORDER by Priority").ToList();
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
                    " where DeptId = " + deptId + " " +
                    " and SBU = '" + sbu + "' " +
                    " and [Year] = '" + year + "' " +
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
                    " where DeptId = " + deptId + " " +
                    " and SBU = '" + sbu + "' " +
                    " and [Year] = '" + year + "' " +
                    " and DataStatus = 1 ";

                var result = _dbContext.CountLong.FromSqlRaw(qry).ToList();

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getSBUWiseDonationLocation/{donationId}/{deptId}/{year}/{authId}")]
        public ActionResult<IReadOnlyList<BgtEmployeeLocationWiseSBUExp>> GetTotalAuthPerson(int donationId, int deptId, int year, int authId)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@DonationId ", donationId),
                        new SqlParameter("@DeptId", deptId),
                        new SqlParameter("@Year", year),
                        new SqlParameter("@AuthId", authId),
                    };

                var results = _dbContext.BgtEmployeeLocationWiseSBUExp.FromSqlRaw("EXECUTE [SP_BgtGetSBUWiseLocation] @DonationId, @DeptId, @Year, @AuthId", parms.ToArray()).ToList();
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost("insertBgtEmployee")]
        public Int32 BgtEmpInsert(BgtEmpInsertDto dto)
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
        //public Int32 BgtOwnInsert(BgtOwnInsertDto dto)
        public Int32 BgtOwnInsert(List<BgtOwnInsertDto> dto)
        {
            var results = 0;

            foreach (var a in dto)
            {
                if (a.Amount > 0)  
                {
                    try
                    {


                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@DeptId", a.DeptId),
                        new SqlParameter("@Year", a.Year),
                        new SqlParameter("@SBU ", a.SBU),
                        new SqlParameter("@AuthId", a.AuthId),
                        new SqlParameter("@Amount", a.Amount),
                        new SqlParameter("@AmtLimit", a.Limit),
                        new SqlParameter("@Segment", a.Segment),
                        new SqlParameter("@EnteredBy", a.EnteredBy),
                        new SqlParameter("@DonationId", a.DonationId),
                        new SqlParameter("@StMonth", a.StMonth),
                    };

                        if (a.AuthId == 8)   // GPM
                        {
                            results = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsertPMD] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId, @StMonth", parms.ToArray());
                        }
                        else if (a.AuthId == 3)   // RSM
                        {
                            results = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsertRSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId, @StMonth ", parms.ToArray());
                        }
                        else if (a.AuthId == 5)    // DSM
                        {
                            results = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsertDSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId, @StMonth ", parms.ToArray());
                        }
                        else //if (a.AuthId == 6 || a.AuthId == 4 || a.AuthId == 7 || a.AuthId == 12)     // SM  - GM - Director - MD
                        {
                            results = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtOwnInsert] @DeptId, @Year, @SBU , @AuthId, @Amount, @AmtLimit, @Segment, @EnteredBy, @DonationId, @StMonth ", parms.ToArray());
                        }
                    }

                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            return results;
        }
    }
}
