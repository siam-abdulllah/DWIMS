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

namespace API.Controllers
{
    public class DepotPrintingController : BaseApiController
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("pendingForPrint")]
        public async Task<IReadOnlyList<SearchPendingDepotLetter>> ReportDepotLetter()
        {
            try
            {
                string qry = " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName, " +
                    " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount, depo.DepotName " +
                    " from InvestmentInit a " +
                    " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
                    " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId " +
                    " left join Employee e on a.EmployeeId = e.Id " +
                    " left join Donation d on a.DonationId = d.Id " +
                    " inner join InvestmentDetail inDetail on a.id = inDetail.InvestmentInitId " +
                    " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId " +
                    " left join DoctorInfo doc on inDc.DoctorId = doc.Id " +
                    " where a.DonationTo = 'Doctor' and ir.RecStatus = 'Approved' " +
                    " AND inDetail.PaymentMethod = 'Cash' " +
                    " Order by  a.id DESC ";

                var results = _db.RptDepotLetter.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
