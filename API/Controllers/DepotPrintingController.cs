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

        [HttpGet]
        [Route("pendingForPrint/{empId}/{userRole}")]
        public async Task<IReadOnlyList<RptDepotLetterSearch>> ReportDepotLetter(int empId,string userRole)
        {
            try
            {
                string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empId + "' ";
                var empData = _db.Employee.FromSqlRaw(empQry).ToList();
                string qry = "  SELECT * FROM  ( " +
                            " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor, a.DonationTo, depo.DepotCode, " +
                            " d.DonationTypeName, doc.DoctorName,  inDetail.ProposedAmount, e.EmployeeName, e.MarketName   " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " +
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                            " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId  left join DoctorInfo doc on inDc.DoctorId = doc.Id " +
                            " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cash' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor, a.DonationTo, depo.DepotCode," +
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
                            " inDetail.PaymentMethod = 'Cash'" +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select DISTINCT a.Id, 1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn,SYSDATETIMEOFFSET() AS ModifiedOn,a.ReferenceNo,  depo.DepotCode, " +
                            " a.ProposeFor,a.DonationTo,d.DonationTypeName,doc.InstitutionName,inDetail.ProposedAmount,e.EmployeeName,e.MarketName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentInstitution IC on a.Id = IC.InvestmentInitId  " +
                            " left join InstitutionInfo doc on IC.InstitutionId = doc.Id  " +
                            " where a.DonationTo = 'Institution'  " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select DISTINCT a.Id, 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, a.ProposeFor, a.DonationTo, d.DonationTypeName, depo.DepotCode,  " +
                            " doc.BcdsName, inDetail.ProposedAmount, e.EmployeeName, e.MarketName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentBcds IC on a.Id = IC.InvestmentInitId  " +
                            " left join Bcds doc on IC.BcdsId = doc.Id  " +
                            " where a.DonationTo = 'Bcds'  " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cash' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select DISTINCT a.Id, 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, a.ProposeFor, a.DonationTo, depo.DepotCode, " +
                            " d.DonationTypeName, doc.SocietyName, inDetail.ProposedAmount, e.EmployeeName, e.MarketName   " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentSociety IC on a.Id = IC.InvestmentInitId  " +
                            " left join Society doc on IC.SocietyId = doc.Id  " +
                            " where a.DonationTo = 'Society' " +
                            " AND ir.RecStatus = 'Approved' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " AND inDetail.PaymentMethod = 'Cash') X " +
                            " WHERE X.ID not in (SELECT InvestmentInitId FROM DepotPrintTrack) " ;
                            //" AND X.DepotCode = ''";
                            //" AND X.ReferenceNo IN ('20220107058','20220107179','20220107229','20220107133')" ;
                            if (userRole != "Administrator")
                            {
                                qry = qry + " AND X.DepotCode = '" + empData[0].DepotCode + "'";
                            }

                var results = _db.RptDepotLetterSearch.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("pendingChqForPrint/{empId}")]
        public async Task<IReadOnlyList<RptDepotLetterSearch>> ReportChequeDepotLetter(int empId)
        {
            try
            {
                string qry = "  SELECT * FROM  ( " +
                            " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor, a.DonationTo, depo.DepotCode, " +
                            " d.DonationTypeName, doc.DoctorName,  inDetail.ProposedAmount, e.EmployeeName, e.MarketName   " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId   " +
                            " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId   " +
                            " inner join InvestmentDoctor inDc on a.Id = inDc.InvestmentInitId  left join DoctorInfo doc on inDc.DoctorId = doc.Id " +
                            " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cheque'  " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor, a.DonationTo, depo.DepotCode," +
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
                            " inDetail.PaymentMethod = 'Cheque' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select DISTINCT a.Id, 1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn,SYSDATETIMEOFFSET() AS ModifiedOn,a.ReferenceNo,  depo.DepotCode, " +
                            " a.ProposeFor,a.DonationTo,d.DonationTypeName,doc.InstitutionName,inDetail.ProposedAmount,e.EmployeeName,e.MarketName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentInstitution IC on a.Id = IC.InvestmentInitId  " +
                            " left join InstitutionInfo doc on IC.InstitutionId = doc.Id  " +
                            " where a.DonationTo = 'Institution'  " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cheque' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select DISTINCT a.Id, 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, a.ProposeFor, a.DonationTo, d.DonationTypeName, depo.DepotCode,  " +
                            " doc.BcdsName, inDetail.ProposedAmount, e.EmployeeName, e.MarketName  " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentBcds IC on a.Id = IC.InvestmentInitId  " +
                            " left join Bcds doc on IC.BcdsId = doc.Id  " +
                            " where a.DonationTo = 'Bcds'  " +
                            " AND ir.RecStatus = 'Approved'  " +
                            " AND inDetail.PaymentMethod = 'Cheque' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " UNION " +
                            " Select DISTINCT a.Id, 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, a.ProposeFor, a.DonationTo, depo.DepotCode, " +
                            " d.DonationTypeName, doc.SocietyName, inDetail.ProposedAmount, e.EmployeeName, e.MarketName   " +
                            " from InvestmentInit a  " +
                            " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                            " left join InvestmentRecDepot depo on depo.InvestmentInitId = ir.InvestmentInitId  " +
                            " left join Employee e on a.EmployeeId = e.Id left join Donation d on a.DonationId = d.Id  " +
                            " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId  " +
                            " inner join InvestmentSociety IC on a.Id = IC.InvestmentInitId  " +
                            " left join Society doc on IC.SocietyId = doc.Id  " +
                            " where a.DonationTo = 'Society' " +
                            " AND ir.RecStatus = 'Approved' " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " AND inDetail.PaymentMethod = 'Cheque') " +
                            " x  WHERE X.ID not in (SELECT InvestmentInitId FROM DepotPrintTrack)  ";

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
