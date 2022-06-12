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
using System;
using System.Net;
using API.Errors;
namespace API.Controllers
{
    public class InvestmentCancelController : BaseApiController
    {
        private readonly StoreContext _db;
        private readonly IGenericRepository<InvestmentDetailTracker> _investmentDetailTrackerRepo;

        public InvestmentCancelController(IGenericRepository<InvestmentDetailTracker> investmentDetailTrackerRepo, StoreContext db)
        {
            _db = db;
            _investmentDetailTrackerRepo = investmentDetailTrackerRepo;
        }

        [HttpGet]
        [Route("investmentDetailTracker/{investmentInitId}")]
        public IReadOnlyList<InvestmentDetailTracker> GetinvestmentDetailTracker(int investmentInitId)
        {
            try
            {
                string qry = " SELECT [Id],[DataStatus],[SetOn],[ModifiedOn],[InvestmentInitId],[EmployeeId],[Month],[Year],[FromDate],[ToDate],[ApprovedAmount],[PaidStatus],[DonationId],[PaymentRefNo]" +
                    " FROM  [dbo].[InvestmentDetailTracker] " +
                " WHERE InvestmentInitId = " + investmentInitId + " " +
                " ";

                var results = _db.InvestmentDetailTracker.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("isInvestmentDetailExist")]
        public IActionResult IsInvestmentDetailExist(int id)
        {
            try
            {
                string qry = " SELECT [Id],[DataStatus],[SetOn],[ModifiedOn],[InvestmentInitId],[EmployeeId],[Month],[Year],[FromDate],[ToDate],[ApprovedAmount],[PaidStatus],[DonationId],[PaymentRefNo]" +
                        " FROM  [dbo].[InvestmentDetailTracker]" +
                    " WHERE [Id] = " + id + "  AND [PaymentRefNo] NOT IN (SELECT PayRefNo FROM DepotPrintTrack WHERE PayRefNo IS NOT NULL " +
                    " UNION SELECT PayRefNo FROM MedicineDispatch  WHERE PayRefNo IS NOT NULL)" +
                    " ";

                var results = _db.InvestmentDetailTracker.FromSqlRaw(qry).ToList();
                if (results.Count > 0)
                {
                    return Ok("Existed");
                }
                else { return Ok("Not Existed"); }
                //var alreadyExistInvestmentDetailTracker = await _investmentDetailTrackerRepo.GetByIdAsync(id);
                ////var alreadyExistInvestmentDetailTrackerList = await _investmentDetailTrackerRepo.ListAsync(alreadyExistSpec);
                ////if (alreadyExistInvestmentDetailTrackerList.Count > 0)
                //if (alreadyExistInvestmentDetailTracker.InvestmentInitId != null)
                //{
                //    var alreadyExistSpec = new InvestmentDetailTrackerSpecification(id);

                //    var alreadyExistInvestmentDetailTrackerList = await _investmentDetailTrackerRepo.ListAsync(alreadyExistSpec);
                //    //foreach (var v in alreadyExistInvestmentDetailTrackerList)
                //    //{

                //    _investmentDetailTrackerRepo.Delete(alreadyExistInvestmentDetailTracker);
                //    _investmentDetailTrackerRepo.Savechange();
                //    //}

                // Ok("Succsessfuly Deleted!!!");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("removeInvestmentDetalTracker/{id}/{empId}")]
        public IActionResult RemoveInvestmentDetalTracker(int id, int empId)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@ID", id),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IPADD", GetIPAddress()),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                var result = _db.Database.ExecuteSqlRaw("EXECUTE SP_InvApprIndividualDelete @ID,@EID,@IPADD,@r out", parms.ToArray());
                if (parms[3].Value.ToString() != "True")
                {
                    return BadRequest(new ApiResponse(400, parms[7].Value.ToString()));
                }

                //var alreadyExistInvestmentDetailTracker = await _investmentDetailTrackerRepo.GetByIdAsync(id);
                ////var alreadyExistInvestmentDetailTrackerList = await _investmentDetailTrackerRepo.ListAsync(alreadyExistSpec);
                ////if (alreadyExistInvestmentDetailTrackerList.Count > 0)
                //if (alreadyExistInvestmentDetailTracker.InvestmentInitId != null)
                //{
                //    var alreadyExistSpec = new InvestmentDetailTrackerSpecification(id);

                //    var alreadyExistInvestmentDetailTrackerList = await _investmentDetailTrackerRepo.ListAsync(alreadyExistSpec);
                //    //foreach (var v in alreadyExistInvestmentDetailTrackerList)
                //    //{

                //    _investmentDetailTrackerRepo.Delete(alreadyExistInvestmentDetailTracker);
                //    _investmentDetailTrackerRepo.Savechange();
                //    //}

                return Ok("Succsessfuly Deleted!!!");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("cancelInv/{invId}/{empId}")]
        public IActionResult CancelInvestment(int invId, int empId)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@InvID", invId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IPADD", GetIPAddress()),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                var result = _db.Database.ExecuteSqlRaw("EXECUTE SP_InvCancel @InvID,@EID,@IPADD,@r OUTPUT", parms.ToArray());
                // if (parms[3].Value.ToString() != "True")
                // {
                    return Ok(new ApiResponse(400, parms[3].Value.ToString()));
                // }

                // return Ok("Succsessfuly Deleted!!!");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public string GetIPAddress()
        {
            IPAddress ip;
            var headers = Request.Headers.ToList();
            if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
            {
                var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                ip = IPAddress.Parse(header.Remove(header.IndexOf(':')));
            }
            else
            {
                ip = Request.HttpContext.Connection.RemoteIpAddress;
            }

            return ip.ToString();
        }

    }
}
