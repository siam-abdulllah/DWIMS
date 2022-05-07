using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("getDeptSBUBudget/{deptId}/{sbu}/{year}")]
        public ActionResult<IReadOnlyList<CountDouble>> GetDeptSBUWiseBudget(int deptId, string sbu, int year)
        {
            try
            {
                string qry = "";

                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,  ISNULL(SUM(SBUAmount), 0)  Count from BgtSBUTotal " +
                    " where DeptId = "+ deptId + " " +
                    " and SBU = '"+ sbu + "' " +
                    " and [Year] = '"+ year  + "' " +
                    " and DataStatus = 1 ";

                var result = _dbContext.CountDouble.FromSqlRaw(qry).ToList();

                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getPrevAlloc/{deptId}/{sbu}/{year}")]
        public ActionResult<IReadOnlyList<CountDouble>> GetPreviousAllocated(int deptId, string sbu, int year)
        {
            try
            {
                string qry = "";

                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, ISNULL(SUM(Amount), 0) Count from BgtEmployee " +
                    " where DeptId = "+ deptId + " " +
                    " and SBU = '"+ sbu + "' " +
                    " and [Year] = '"+ year  + "' " +
                    " and DataStatus = 1 ";

                var result = _dbContext.CountDouble.FromSqlRaw(qry).ToList();

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


    }
}
