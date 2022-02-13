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

                  string qry = " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor,  " + 
                " a.DonationTo, depo.DepotCode, d.DonationTypeName,   inDetail.ProposedAmount, e.EmployeeName, e.MarketName, " + 
                " CASE WHEN a.donationto = 'Doctor' THEN (SELECT DoctorId  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Institution' THEN (SELECT InstitutionId FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Campaign' THEN (SELECT y.MstId  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Bcds' THEN (SELECT x.BcdsId   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Society' THEN (SELECT x.SocietyId FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DId, "+
                " CASE  " + 
                " WHEN a.donationto = 'Doctor' THEN (SELECT doctorname  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Institution' THEN (SELECT institutionname FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Campaign' THEN (SELECT subcampaignname  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE  x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Bcds' THEN (SELECT bcdsname   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Society' THEN (SELECT societyname FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DoctorName," +
                " CONVERT(date, ir.SetOn) ApprovedDate, aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy' " + 
                " from InvestmentInit a " + 
                " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " + 
                " left join InvestmentRecDepot depo on a.id = depo.InvestmentInitId " +
                " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id " +
                " left join Employee aprBy on ir.EmployeeId = aprBy.Id" +
                " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId " +
                " where a.DataStatus= 1 AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cash' " +
                " AND  ir.EmployeeId = inDetail.EmployeeId AND a.DonationId <> 4" +
                " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                " AND  ir.InvestmentInitId not in (SELECT InvestmentInitId FROM DepotPrintTrack) ";

                if (userRole != "Administrator")
                {
                    qry = qry + " AND depo.DepotCode = '" + empData[0].DepotCode + "'";
                }
                qry = qry + " Order by ApprovedDate DESC ";

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
                string qry = " Select  DISTINCT a.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor,  " + 
                " a.DonationTo, depo.DepotCode, d.DonationTypeName,   inDetail.ProposedAmount, e.EmployeeName, e.MarketName, " + 
                " CASE WHEN a.donationto = 'Doctor' THEN (SELECT DoctorId  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Institution' THEN (SELECT InstitutionId FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Campaign' THEN (SELECT y.MstId  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Bcds' THEN (SELECT x.BcdsId   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id)  "+
                " WHEN a.donationto = 'Society' THEN (SELECT x.SocietyId FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DId, "+
                " CASE  " + 
                " WHEN a.donationto = 'Doctor' THEN (SELECT doctorname  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Institution' THEN (SELECT institutionname FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Campaign' THEN (SELECT subcampaignname  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE  x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Bcds' THEN (SELECT bcdsname   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id) " + 
                " WHEN a.donationto = 'Society' THEN (SELECT societyname FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DoctorName," +
                " CONVERT(date, ir.SetOn) ApprovedDate, aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy' " + 
                " from InvestmentInit a " + 
                " left join InvestmentRecComment ir on a.Id = ir.InvestmentInitId " + 
                " left join InvestmentRecDepot depo on a.id = depo.InvestmentInitId " +
                " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id " +
                " left join Employee aprBy on ir.EmployeeId = aprBy.Id" +
                " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId " +
                " where a.DataStatus= 1 AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cheque' " +
                " AND  ir.EmployeeId = inDetail.EmployeeId AND a.DonationId <> 4" +
                " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                " AND  ir.InvestmentInitId not in (SELECT InvestmentInitId FROM DepotPrintTrack) " +
                " Order by ApprovedDate DESC";

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
