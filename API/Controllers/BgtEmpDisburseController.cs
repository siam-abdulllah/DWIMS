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
    public class BgtEmpDisburseController :BaseApiController
    {
        private readonly StoreContext _dbContext;

        public BgtEmpDisburseController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("getSBUWiseEmpDisburse/{sbu}/{deptId}/{year}/{authId}")]
        public ActionResult<IReadOnlyList<BgtEmpSbuDisburse>> GetTotalAuthPerson(string sbu, int deptId, int year, int authId)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU ", sbu),
                        new SqlParameter("@DeptId", deptId),
                        new SqlParameter("@Year", year),
                        new SqlParameter("@AuthId", authId),
                        new SqlParameter("@CompID", 1000),
                    };

                if (authId == 3)
                {
                    var results = _dbContext.BgtEmpSbuDisburse.FromSqlRaw("EXECUTE [SP_BgtEmpDisburseRSM] @SBU, @DeptId, @Year, @AuthId, @CompID", parms.ToArray()).ToList();
                    return results;
                }
                else if (authId == 5)
                {
                    var results = _dbContext.BgtEmpSbuDisburse.FromSqlRaw("EXECUTE [SP_BgtEmpDisburseDSM] @SBU, @DeptId, @Year, @AuthId, @CompID", parms.ToArray()).ToList();
                    return results;
                }
                else
                {
                    var results = _dbContext.BgtEmpSbuDisburse.FromSqlRaw("EXECUTE [SP_BgtEmpDisburse] @SBU, @DeptId, @Year, @AuthId, @CompID", parms.ToArray()).ToList();
                    return results;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getAuthBudget/{sbu}/{deptId}/{year}/{authId}")]
        public ActionResult<IReadOnlyList<CountLong>> GetAuthorityBudget(string sbu, int deptId, int year, int authId)
        {
            try
            {
                var results = _dbContext.CountLong.FromSqlRaw("select CAST(ABS(CHECKSUM(NewId())) % 200 AS int) as Id, 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, " +
                    " Amount as Count from BgtEmployee where DeptId = "+ deptId + " and Year = " + year + " and SBU = '" + sbu + "' and AuthId = " + authId + " and DataStatus = 1").ToList();
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("insertBgtEmpDetails")]
        public Int32 BgtEmpDetailInsert(BgtEmpDetailsInsertDto dto)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@DeptId", dto.DeptId),
                        new SqlParameter("@Year", dto.Year),
                        new SqlParameter("@SBU ", dto.SBU),
                        new SqlParameter("@AuthId", dto.AuthId),
                        new SqlParameter("@Amount", dto.NewAllocated),
                        new SqlParameter("@EnteredBy", dto.EnteredBy),
                        new SqlParameter("@Code", dto.Code),
                    };

                var results = _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmpDetailsDisburse] @DeptId, @Year, @SBU , @AuthId, @Amount, @EnteredBy, @Code", parms.ToArray());
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
