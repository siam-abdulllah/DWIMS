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
        public ActionResult<IReadOnlyList<CountInt>> GetDeptSBUWiseBudget(int deptId, string sbu, int year)
        {
            try
            {
                string qry = "";

                qry = " select 1 as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, CAST(SBUAmount AS INT)  Count from BgtSBUTotal " +
                    " where DeptId = "+ deptId + " " +
                    " and SBU = '"+ sbu + "' " +
                    " and [Year] = '"+ year  + "' " +
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
                    " where ApprovalAuthorityId = "+ authId + " ";

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
