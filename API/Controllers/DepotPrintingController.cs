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
        private readonly StoreContext _db;
        private readonly IMapper _mapper;

        public DepotPrintingController(IMapper mapper, StoreContext db)
        {
            _db = db;
            _mapper = mapper;
        }

        // [HttpGet("pendingForPrint/{empId}")]
        // public ActionResult<IReadOnlyList<RptDepotLetter>> ReportDepotLetter(int empId ,RptDepotLetterSpecParams parrams)
        // {
        //     try
        //     {
        //         string qry = " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName, " +
        //             " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount, depo.DepotName " +
        //             " from InvestmentInit a " +
        //             " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
        //             " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId " +
        //             " left join Employee e on a.EmployeeId = e.Id " +
        //             " left join Donation d on a.DonationId = d.Id " +
        //             " inner join InvestmentDetail inDetail on a.id = inDetail.InvestmentInitId " +
        //             " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId " +
        //             " left join DoctorInfo doc on inDc.DoctorId = doc.Id " +
        //             " where a.DonationTo = 'Doctor' and ir.RecStatus = 'Approved' " +
        //             " AND inDetail.PaymentMethod = 'Cash' " +
        //             " Order by  a.id DESC ";

        //         var results = _db.RptDepotLetter.FromSqlRaw(qry).ToList();
        //         return Ok(new Pagination<RptDepotLetter>(parrams.PageIndex, parrams.PageSize, 50, results));
        //     }
        //     catch (System.Exception ex)
        //     {
        //         throw ex;
        //     }
        // }

        // [HttpGet("pendingForPrint/{empId}")]
        // public object ReportDepotLetter(int empId)
        // {
        //     try
        //     {
        //         string qry = " Select a.id, a.SetOn, e.EmployeeName, SYSDATETIMEOFFSET() AS ModifiedOn, 1 AS DataStatus, e.Id as EmpId, e.DesignationName, e.MarketName, a.ReferenceNo, d.DonationTypeName, " +
        //             " doc.id as DocId, doc.DoctorName, doc.[Address], inDetail.ProposedAmount, depo.DepotName " +
        //             " from InvestmentInit a " +
        //             " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " +
        //             " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId " +
        //             " left join Employee e on a.EmployeeId = e.Id " +
        //             " left join Donation d on a.DonationId = d.Id " +
        //             " inner join InvestmentDetail inDetail on a.id = inDetail.InvestmentInitId " +
        //             " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId " +
        //             " left join DoctorInfo doc on inDc.DoctorId = doc.Id " +
        //             " where a.DonationTo = 'Doctor' and ir.RecStatus = 'Approved' " +
        //             " AND inDetail.PaymentMethod = 'Cash' " +
        //             " Order by  a.id DESC ";

        //         var results = _db.RptDepotLetter.FromSqlRaw(qry);


        //         return results;
        //     }
        //     catch (System.Exception ex)
        //     {
        //         throw ex;
        //     }
        // }

        [HttpGet]
        [Route("pendingForPrint/{empId}")]
        public async Task<IReadOnlyList<RptDepotLetterSearch>> ReportDepotLetter(int empId)
        {
            try
            {
                string qry = "  SELECT * FROM  ( " + 
                            " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor, a.DonationTo,  " + 
                            " d.DonationTypeName, doc.DoctorName,  inDetail.ProposedAmount, e.EmployeeName, e.MarketName   " + 
                            " from InvestmentInit a  " + 
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " + 
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " + 
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " + 
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " + 
                            " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId  left join DoctorInfo doc on inDc.DoctorId = doc.Id " + 
                            " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cash'  " + 
                            " UNION " + 
                            " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor, a.DonationTo, " + 
                            " d.DonationTypeName, doc.DoctorName,  inDetail.ProposedAmount, e.EmployeeName, e.MarketName " + 
                            " from InvestmentInit a " + 
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " + 
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " +
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                            " inner join InvestmentCampaign IC on a.Id = IC.InvestmentInitId " +
                            " left join DoctorInfo doc on IC.DoctorId = doc.Id   " +
                            " where a.DonationTo = 'Campaign' AND  " +
                            " ir.RecStatus = 'Approved'  AND  " +
                            " inDetail.PaymentMethod = 'Cash') x " +
                            " WHERE X.ID not in (SELECT InvestmentInitId FROM DepotPrintTrack) ";
                            //" AND X.ReferenceNo IN ('20220107058','20220107179','20220107229','20220107133')" ; 

                var results = _db.RptDepotLetterSearch.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
