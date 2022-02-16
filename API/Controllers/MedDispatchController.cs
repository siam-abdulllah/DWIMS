using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class MedDispatchController : BaseApiController
    {

        private readonly StoreContext _db;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<InvestmentMedicineProd> _investmentMedicineProdRepo;
        private readonly IGenericRepository<MedicineDispatch> _dispRepo;
        private readonly IGenericRepository<MedicineDispatchDtl> _dispdtlRepo;

        public MedDispatchController(IMapper mapper, StoreContext db, IGenericRepository<InvestmentMedicineProd> investmentMedicineProdRepo, IGenericRepository<MedicineDispatch> dispRepo, IGenericRepository<MedicineDispatchDtl> dispdtlRepo)
        {
            _db = db;
            _mapper = mapper;
            _investmentMedicineProdRepo = investmentMedicineProdRepo;
            _dispRepo = dispRepo;
            _dispdtlRepo = dispdtlRepo;
        }

        [HttpGet]
        [Route("getMedicineProds/{investmentInitId}")]
        public List<MedicineDispatchDtl> GetInvestmentMedicineProds(int investmentInitId)
        {
            try
            {
                var t = _db.MedicineDispatchDtl.FromSqlRaw("select CAST(ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS INT) AS Id,  " +
                " 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.InvestmentInitId, a.ProductId, b.ProductName,a.EmployeeId, a.BoxQuantity, a.TpVat, a.BoxQuantity 'DispatchQuantity', a.TpVat 'DispatchTpVat' from[dbo].[InvestmentMedicineProd] a left join MedicineProduct b on a.ProductId = b.Id where a.InvestmentInitId='" + investmentInitId + "' ").ToList();
                return t;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentMedicineProd")]
        public async Task<IActionResult> RemoveInvestmentMedicineProd(InvestmentMedicineProd investmentMedicineProd)
        {
            try
            {
                //var response = new HttpResponseMessage();
                var alreadyExistSpec = new InvestmentMedicineProdSpecification(investmentMedicineProd.InvestmentInitId, investmentMedicineProd.ProductId);
                var alreadyExistInvestmentMedicineProdList = await _investmentMedicineProdRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentMedicineProdList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentMedicineProdList)
                    {
                        _investmentMedicineProdRepo.Delete(v);
                        _investmentMedicineProdRepo.Savechange();
                    }

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("createDispatch")]
        public ActionResult<MedicineDispatchDto> InsertDepoTracker(MedicineDispatch trackDto)
        {
            string dptCode = "";
            string dptName = "";

            var depoCode = _db.InvestmentRecDepot.Where(x => x.InvestmentInitId == trackDto.InvestmentInitId).FirstOrDefault();

            if (depoCode != null)
            {
                dptCode = depoCode.DepotCode;
                dptName = depoCode.DepotName;
            }

            var bcds = new MedicineDispatch
            {
                InvestmentInitId = trackDto.InvestmentInitId,
                IssueReference = trackDto.IssueReference,
                SAPRefNo = trackDto.IssueReference,
                IssueDate = trackDto.IssueDate,
                ProposeAmt = trackDto.ProposeAmt,
                PayRefNo = trackDto.PayRefNo,
                DispatchAmt = trackDto.DispatchAmt,
                DepotCode = dptCode,
                DepotName = dptName,
                Remarks = trackDto.Remarks,
                SetOn = DateTimeOffset.Now,
                EmployeeId = trackDto.EmployeeId,
            };

            _dispRepo.Add(bcds);
            _dispRepo.Savechange();

            var t = _db.Database.ExecuteSqlRaw("EXECUTE [dbo].[SP_UpdateApprovedAmount] {0},{1}", trackDto.DispatchAmt, trackDto.InvestmentInitId);
            var y = _db.Database.ExecuteSqlRaw("EXECUTE [dbo].[SP_UpdateDetailTrackerAmount] {0},{1}", trackDto.DispatchAmt, trackDto.InvestmentInitId);

            return new MedicineDispatchDto
            {
                Id = bcds.Id,
                InvestmentInitId = trackDto.InvestmentInitId,
                SAPRefNo = trackDto.IssueReference,
                IssueReference = trackDto.IssueReference,
                IssueDate = trackDto.IssueDate,
                PayRefNo = trackDto.PayRefNo,
                DepotCode = dptCode,
                DepotName = trackDto.DepotName,
                Remarks = trackDto.Remarks,
                ProposeAmt = trackDto.ProposeAmt,
                DispatchAmt = trackDto.DispatchAmt,
                EmployeeId = trackDto.EmployeeId,
            };
        }


        [HttpPost("insertMedicineDetail")]
        public async Task<IActionResult> InsertMedDtl(List<MedicineDispatchDtl> dt)
        {
            try
            {
                foreach (var trackDto in dt)
                {
                    var bcds = new MedicineDispatchDtl
                    {
                        InvestmentInitId = trackDto.InvestmentInitId,
                        ProductName = trackDto.ProductName,
                        ProductId = trackDto.ProductId,
                        BoxQuantity = trackDto.BoxQuantity,
                        TpVat = trackDto.TpVat,
                        DispatchQuantity = trackDto.DispatchQuantity,
                        DispatchTpVat = trackDto.DispatchTpVat,
                        SetOn = DateTimeOffset.Now,
                        EmployeeId = trackDto.EmployeeId,
                    };

                    _dispdtlRepo.Add(bcds);
                }
                _dispdtlRepo.Savechange();
                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("pendingDispatch/{empId}/{userRole}")]
        public async Task<IReadOnlyList<MedDispSearch>> PendingForDispatch(int empId, string userRole)
        {
            try
            {
                string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empId + "' ";
                var empData = _db.Employee.FromSqlRaw(empQry).ToList();

                string qry = " Select  DISTINCT CAST(ROW_NUMBER() OVER (ORDER BY dtl.Id) AS INT)  AS Id, 1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ReferenceNo, a.ProposeFor,  dtl.PaymentRefNo PayRefNo, depo.DepotName, dtl.InvestmentInitId, " +
              " a.DonationTo, depo.DepotCode, d.DonationTypeName,   dtl.ApprovedAmount  ProposedAmount, e.EmployeeName, e.MarketName, ir.SetOn 'ApprovedDate', aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy', " +
              " CASE  " +
              " WHEN a.donationto = 'Doctor' THEN (SELECT doctorname  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) " +
              " WHEN a.donationto = 'Institution' THEN (SELECT institutionname FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) " +
              " WHEN a.donationto = 'Campaign' THEN (SELECT subcampaignname  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE  x.investmentinitid = a.id) " +
              " WHEN a.donationto = 'Bcds' THEN (SELECT bcdsname   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id) " +
              " WHEN a.donationto = 'Society' THEN (SELECT societyname FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DoctorName " +
              " from InvestmentInit a " +
              " inner join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
              " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
              " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId " +
              " left join InvestmentRecDepot depo on a.id = depo.InvestmentInitId " +
              " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id " +
              " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
        
              " where a.DataStatus= 1 AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cash' " +
              " AND a.DonationId = 4 AND  ir.EmployeeId = inDetail.EmployeeId " +
              //" AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
              //" AND  ir.InvestmentInitId not in (SELECT InvestmentInitId FROM MedicineDispatch) ";
              " AND dtl.PaymentRefNo not in (SELECT PayRefNo FROM DepotPrintTrack where PayRefNo is not null) ";
                if (userRole != "Administrator")
                {
                    qry = qry + " AND depo.DepotCode = '" + empData[0].DepotCode + "'";
                }

                qry = qry + " ORDER BY ir.SetOn ";

                var results = _db.MedDispSearch.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost("medDispReport")]
        public object MedicineDispReport(RptMedicineDispatchSearchDto searchDto)
        {
            try
            {
                if (searchDto.DisStatus == "Pending")
                {
                    string qry = "SELECT DISTINCT a.id,1 AS DataStatus,Sysdatetimeoffset() AS SetOn,Sysdatetimeoffset() AS ModifiedOn,a.referenceno,a.proposefor,a.donationto,a.donationid,depo.depotcode,'NA'PaymentRefNo, " +
                                " null PaymentDate,CAST(0 as float) DispatchAmt, null Remarks, d.donationtypename, inDetail.proposedamount, e.employeename, e.marketname, ir.seton 'ApprovedDate', aprBy.employeename + ',' + aprBy.designationname 'ApprovedBy' " +
                                " FROM   investmentinit a LEFT JOIN investmentreccomment ir ON a.id = ir.investmentinitid " +
                                " LEFT JOIN investmentrecdepot depo ON depo.investmentinitid = ir.investmentinitid " +
                                " LEFT JOIN employee e ON a.employeeid = e.id  " +
                                " LEFT JOIN donation d                       ON a.donationid = d.id " +
                                " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                                " INNER JOIN investmentrec inDetail ON a.id = inDetail.investmentinitid " +
                                " WHERE  a.id NOT IN (SELECT investmentinitid FROM   medicinedispatch) " +
                                " AND IR.RecStatus = 'Approved'  " +
                                " AND ir.seton BETWEEN '" + searchDto.FromDate + "' AND '" + searchDto.ToDate + "'  " +
                                " AND depo.DepotCode = '" + searchDto.DepotCode + "' ";

                    if (searchDto.DonationId != "0")
                    {
                        qry = qry + " AND a.donationid = " + searchDto.DonationId + "";
                    }

                    var results = _db.RptMedDisp.FromSqlRaw(qry).ToList();

                    return results;
                }
                else
                {

                    if (searchDto.DonationId == "4")
                    {
                        string qry = "  SELECT * FROM  ( " +
                            " select DISTINCT a.id,1 AS DataStatus,Sysdatetimeoffset() AS SetOn,Sysdatetimeoffset() AS ModifiedOn, b.ReferenceNo, d.DonationTypeName, prep.EmployeeName, apr.EmployeeName 'ApprovedBy', c.SetOn 'ApprovedDate'," +
                            " b.MarketName, a.IssueReference AS PaymentRefNo, a.IssueDate AS PaymentDate, a.DispatchAmt, a.Remarks, b.DonationId, a.DepotCode " +
                            " from MedicineDispatch a left join InvestmentInit b on a.InvestmentInitId = b.id " +
                            " left join InvestmentRecComment C on a.InvestmentInitId = c.InvestmentInitId " +
                            " left join Donation d on b.DonationId = d.Id left join Employee prep on b.EmployeeId = prep.Id " +
                            " left join Employee apr on c.EmployeeId = apr.Id WHERE c.RecStatus = 'Approved' ) X " +
                            " WHERE X.PaymentDate between '" + searchDto.FromDate + "' AND '" + searchDto.ToDate + "'  AND X.DonationId = " + searchDto.DonationId + " " +
                            " AND X.DepotCode = '" + searchDto.DepotCode + "' ";

                        var results = _db.RptMedDisp.FromSqlRaw(qry).ToList();
                        return results;
                    }
                    else
                    {
                        string qry = "  SELECT* FROM(  " +
                            " select DISTINCT a.id,1 AS DataStatus, Sysdatetimeoffset() AS SetOn, Sysdatetimeoffset() AS ModifiedOn, b.ReferenceNo, d.DonationTypeName, prep.EmployeeName, apr.EmployeeName 'ApprovedBy', c.SetOn 'ApprovedDate',  " +
                            " b.MarketName, a.PaymentRefNo AS PaymentRefNo, a.PaymentDate AS PaymentDate, ir.ApprovedAmount AS DispatchAmt, a.Remarks, b.DonationId, a.DepotId " +
                            " from[dbo].[DepotPrintTrack]  a left join InvestmentInit b on a.InvestmentInitId = b.id " +
                            " left join InvestmentRecComment C on a.InvestmentInitId = c.InvestmentInitId " +
                            " left join Donation d on b.DonationId = d.Id left join Employee prep on b.EmployeeId = prep.Id " +
                            " left join InvestmentDetailTracker ir on ir.InvestmentInitId = b.Id " +
                            " left join Employee apr on c.EmployeeId = apr.Id WHERE c.RecStatus = 'Approved' ) X " +
                            " WHERE X.PaymentDate between '" + searchDto.FromDate + "' AND '" + searchDto.ToDate + "' " +
                            " AND X.DepotId = '" + searchDto.DepotCode + "' ";

                        if (searchDto.DonationId != "0")
                        {
                            qry = qry + " AND X.DonationId = " + searchDto.DonationId + "";
                        }

                        var results = _db.RptMedDisp.FromSqlRaw(qry).ToList();
                        return results;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
