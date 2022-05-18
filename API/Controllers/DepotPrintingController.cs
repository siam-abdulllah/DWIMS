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
        public IReadOnlyList<RptDepotLetterSearch> ReportDepotLetter(int empId, string userRole)
        {
            try
            {
                string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empId + "' ";
                var empData = _db.Employee.FromSqlRaw(empQry).ToList();

                string qry = " SELECT CAST(ROW_NUMBER() OVER (ORDER BY Id) AS INT)  AS Id, DataStatus, SetOn, ModifiedOn, ProposeFor, PayRefNo, DonationTo, DepotCode, DonationTypeName, " +
                    " ProposedAmount, EmployeeName, MarketName, InvestmentInitId, DId, DoctorName, ApprovedDate, ApprovedBy FROM ( " +
 
              " Select DISTINCT dtl.id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn, a.ProposeFor, dtl.PaymentRefNo 'PayRefNo', " +
              " a.DonationTo, depo.DepotCode, d.DonationTypeName,   dtl.ApprovedAmount   ProposedAmount, e.EmployeeName, a.MarketName, dtl.InvestmentInitId, " +
              " CASE WHEN a.donationto = 'Doctor' THEN (SELECT DISTINCT DoctorId  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE x.investmentinitid = a.id)  " +
              " WHEN a.donationto = 'Institution' THEN (SELECT DISTINCT InstitutionId FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id)  " +
              " WHEN a.donationto = 'Campaign' THEN (SELECT DISTINCT y.MstId  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE x.investmentinitid = a.id)  " +
              " WHEN a.donationto = 'Bcds' THEN (SELECT DISTINCT x.BcdsId   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id)  " +
              " WHEN a.donationto = 'Society' THEN (SELECT DISTINCT x.SocietyId FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DId, " +
              " CASE  " +
              " WHEN a.donationto = 'Doctor' THEN (SELECT DISTINCT doctorname  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) " +
              " WHEN a.donationto = 'Institution' THEN (SELECT DISTINCT institutionname FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) " +
              " WHEN a.donationto = 'Campaign' THEN (SELECT  dc.DoctorName  + ',' + subcampaignname FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id INNER JOIN DoctorInfo dc on x.DoctorId = dc.Id   WHERE  x.investmentinitid = a.id)   "+
              
              " WHEN a.donationto = 'Bcds' THEN (SELECT DISTINCT bcdsname   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id) " +
              " WHEN a.donationto = 'Society' THEN (SELECT DISTINCT societyname FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DoctorName," +
              " CONVERT(date, ir.SetOn) ApprovedDate, aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy' " +
              " from InvestmentInit a " +
              " inner join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
              " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
              " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId " +
              " left join InvestmentRecDepot depo on a.id = depo.InvestmentInitId " +
              " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id " +
              " left join Employee aprBy on ir.EmployeeId = aprBy.Id" +           
              " where a.DataStatus= 1 AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cash' " +
              " AND  ir.EmployeeId = inDetail.EmployeeId AND a.DonationId <> 4" +
              //" AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
              //" AND  ir.InvestmentInitId not in (SELECT InvestmentInitId FROM DepotPrintTrack) " +
              "  AND dtl.PaymentRefNo not in (SELECT PayRefNo FROM DepotPrintTrack where PayRefNo is not null) ";

                if (userRole != "Administrator")
                {
                    qry = qry + " AND depo.DepotCode = '" + empData[0].DepotCode + "'";
                }
                qry = qry + " ) A Order by A.ApprovedDate ";

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
        public IReadOnlyList<RptChequePrintSearch> ReportChequeDepotLetter(int empId)
        {
            try
            {
                string qry = " SELECT CAST(ROW_NUMBER() OVER (ORDER BY Id) AS INT)  AS Id, DataStatus, SetOn, ModifiedOn, ProposeFor, PayRefNo, DonationTo, SBUName, ChequeTitle, DepotCode, DonationTypeName, " +
                    " ProposedAmount, EmployeeName, MarketName, InvestmentInitId, DId, DoctorName, ApprovedDate, ApprovedBy FROM ( " + 
                
                 " Select  DISTINCT dtl.Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn,  SYSDATETIMEOFFSET() AS ModifiedOn,  a.ProposeFor, dtl.PaymentRefNo PayRefNo, " +
                " a.DonationTo, a.SBUName, inDetail.ChequeTitle, depo.DepotCode, d.DonationTypeName,   dtl.ApprovedAmount   ProposedAmount, e.EmployeeName, a.MarketName, dtl.InvestmentInitId, " +
                " CASE WHEN a.donationto = 'Doctor' THEN (SELECT DoctorId  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE x.investmentinitid = a.id)  " +
                " WHEN a.donationto = 'Institution' THEN (SELECT InstitutionId FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id)  " +
                " WHEN a.donationto = 'Campaign' THEN (SELECT y.MstId  FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id  WHERE x.investmentinitid = a.id)  " +
                " WHEN a.donationto = 'Bcds' THEN (SELECT x.BcdsId   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id)  " +
                " WHEN a.donationto = 'Society' THEN (SELECT x.SocietyId FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DId, " +
                " CASE  " +
                " WHEN a.donationto = 'Doctor' THEN (SELECT doctorname  FROM   investmentdoctor x  INNER JOIN doctorinfo y ON x.doctorid = y.id WHERE  x.investmentinitid = a.id) " +
                " WHEN a.donationto = 'Institution' THEN (SELECT institutionname FROM  investmentinstitution x INNER JOIN institutioninfo y ON x.institutionid = y.id WHERE x.investmentinitid = a.id) " +
                " WHEN a.donationto = 'Campaign' THEN (SELECT  dc.DoctorName  + ',' + subcampaignname FROM   investmentcampaign x INNER JOIN campaigndtl y  ON x.campaigndtlid = y.id  INNER JOIN [dbo].[subcampaign] C  ON y.subcampaignid = C.id INNER JOIN DoctorInfo dc on x.DoctorId = dc.Id   WHERE  x.investmentinitid = a.id) " +
                " WHEN a.donationto = 'Bcds' THEN (SELECT bcdsname   FROM   investmentbcds x  INNER JOIN bcds y   ON x.bcdsid = y.id   WHERE  x.investmentinitid = a.id) " +
                " WHEN a.donationto = 'Society' THEN (SELECT societyname FROM   investmentsociety x INNER JOIN society y ON x.societyid = y.id WHERE  x.investmentinitid = a.id) END  DoctorName," +
                " CONVERT(date, ir.SetOn) ApprovedDate, aprBy.EmployeeName + ',' + aprBy.DesignationName  'ApprovedBy' " +
                " from InvestmentInit a " +
                " inner join InvestmentRecComment ir on a.Id = ir.InvestmentInitId  " +
                " inner join InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                " inner join InvestmentRec inDetail on a.id = inDetail.InvestmentInitId " +
                " left join InvestmentRecDepot depo on a.id = depo.InvestmentInitId " +
                " left join Employee e on a.EmployeeId = e.Id  left join Donation d on a.DonationId = d.Id " +
                " left join Employee aprBy on ir.EmployeeId = aprBy.Id" +
                " where a.DataStatus= 1 AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cheque' " +
                " AND  ir.EmployeeId = inDetail.EmployeeId AND a.DonationId <> 4" +
                //" AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                //" AND  ir.InvestmentInitId not in (SELECT InvestmentInitId FROM DepotPrintTrack) " +
                " AND dtl.PaymentRefNo not in (SELECT PayRefNo FROM DepotPrintTrack where PayRefNo is not null) " +
                "  ) A  Order by ApprovedDate";

                var results = _db.RptChequePrintSearch.FromSqlRaw(qry).ToList();

                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("mpoPendingPayment/{empId}/{userRole}")]
        public object MPOPaymentPending(int empId, string userRole)
        {
            try
            {
                string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empId + "' ";
                var empData = _db.Employee.FromSqlRaw(empQry).ToList();

                string qry = " SELECT DISTINCT dtl.id,1 AS DataStatus, Sysdatetimeoffset() AS SetOn, Sysdatetimeoffset() AS ModifiedOn, a.referenceno, dtl.PaymentRefNo PayRefNo, a.proposefor,a.donationto,a.donationid,ISNULL(depo.DepotName, 'CHQ') DepotCode,'NA'SAPRefNo, '' DoctorName, a.RegionName, a.SBUName, " +
                    " null PaymentDate,dtl.ApprovedAmount as DispatchAmt, null Remarks, d.donationtypename, inDetail.proposedamount, e.employeename, e.marketname, ir.seton 'ApprovedDate', aprBy.employeename + ',' + aprBy.designationname 'ApprovedBy' " +
                    " FROM investmentinit a  " +
                    " INNER JOIN investmentrec inDetail ON a.id = inDetail.investmentinitid " +
                    " INNER JOIN InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                    " LEFT JOIN investmentreccomment ir ON a.id = ir.investmentinitid " +
                    " LEFT JOIN investmentrecdepot depo ON depo.investmentinitid = ir.investmentinitid " +
                    " LEFT JOIN employee e ON a.employeeid = e.id " +
                    " LEFT JOIN donation d ON a.donationid = d.id " +
                    " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +
                    " WHERE a.DataStatus = 1 AND DTL.PaymentRefNo is not null " +
                    " AND NOT EXISTS(SELECT PayRefNo FROM depotprinttrack WHERE PayRefNo = dtl.PaymentRefNo)" +
                    " AND NOT EXISTS(SELECT PayRefNo FROM medicinedispatch WHERE PayRefNo = dtl.PaymentRefNo) " +
                    " AND IR.RecStatus = 'Approved' ";

                if (userRole != "Administrator")
                {
                    qry = qry + "  AND (A.SBU = COALESCE(NULLIF('"+ empData[0].SBU  + "', ''), 'All') OR COALESCE(NULLIF('" + empData[0].SBU + "', ''), 'All') = 'All') " +
                        "  AND (A.MarketGroupCode = COALESCE(NULLIF('"+ empData[0].MarketGroupCode  + "', ''), 'All') OR COALESCE(NULLIF('" + empData[0].MarketGroupCode + "', ''), 'All') = 'All') " +
                        " AND (A.MarketCode = COALESCE(NULLIF('" + empData[0].MarketCode + "', ''), 'All') OR COALESCE(NULLIF('" + empData[0].MarketCode + "', ''), 'All') = 'All') " +
                        " AND (A.TerritoryCode = COALESCE(NULLIF('" + empData[0].TerritoryCode + "', ''), 'All') OR COALESCE(NULLIF('" + empData[0].TerritoryCode + "', ''), 'All') = 'All') " +
                        " AND (A.RegionCode = COALESCE(NULLIF('" + empData[0].RegionCode + "', ''), 'All') OR COALESCE(NULLIF('" + empData[0].RegionCode + "', ''), 'All') = 'All') " +
                        " AND (A.ZoneCode = COALESCE(NULLIF('" + empData[0].ZoneCode + "', ''), 'All') OR COALESCE(NULLIF('" + empData[0].ZoneCode + "', ''), 'All') = 'All') ";
                }
                qry = qry + " Order by ir.seton DESC ";

                var results = _db.RptMedDisp.FromSqlRaw(qry).ToList();

                return results;
                
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("getRptChqDis")]
        public object MedicineDispReport(RptMedicineDispatchSearchDto searchDto)
        {
            try
            {
                if (searchDto.DisStatus == "Pending")
                {
                    string qry = "SELECT DISTINCT a.id,1 AS DataStatus,Sysdatetimeoffset() AS SetOn,Sysdatetimeoffset() AS ModifiedOn,dtl.PaymentRefNo PayRefNo, a.referenceno,a.proposefor,a.donationto,a.donationid,'NA' SAPRefNo, '' DepotCode, " +
                                " null PaymentDate,CAST(0 as float) DispatchAmt, null Remarks, d.donationtypename,  dtl.ApprovedAmount  ProposedAmount, e.employeename, e.marketname, ir.seton 'ApprovedDate', aprBy.employeename + ',' + aprBy.designationname 'ApprovedBy' " +
                                " FROM   investmentinit a LEFT JOIN investmentreccomment ir ON a.id = ir.investmentinitid " +
                                " INNER JOIN investmentrec inDetail ON a.id = inDetail.investmentinitid " +
                                " INNER JOIN InvestmentDetailTracker dtl on dtl.InvestmentInitId = a.Id " +
                                " LEFT JOIN employee e ON a.employeeid = e.id  " +
                                " LEFT JOIN donation d ON a.donationid = d.id " +
                                " LEFT JOIN employee aprBy ON ir.employeeid = aprBy.id " +

                                " WHERE a.DataStatus= 1 AND IR.RecStatus = 'Approved'  AND a.DonationId <> 4  " +
                                " AND ir.seton BETWEEN '" + searchDto.FromDate + "' AND '" + searchDto.ToDate + "' " +
                                " AND inDetail.PaymentMethod = 'Cheque' " +
                                " AND dtl.PaymentRefNo not in (SELECT PayRefNo FROM DepotPrintTrack where PayRefNo is not null) ";

                    if (searchDto.DonationId != "0")
                    {
                        qry = qry + " AND a.donationid = " + searchDto.DonationId + "";
                    }

                    var results = _db.RptMedDisp.FromSqlRaw(qry).ToList();

                    return results;
                }
                else
                {
                    string qry = "  SELECT * FROM(  " +
                        " select DISTINCT a.id,1 AS DataStatus, Sysdatetimeoffset() AS SetOn, Sysdatetimeoffset() AS ModifiedOn, b.ReferenceNo, d.DonationTypeName, prep.EmployeeName, apr.EmployeeName 'ApprovedBy', c.SetOn 'ApprovedDate',  " +
                        " b.MarketName, a.PayRefNo, a.SAPRefNo, a.PaymentDate AS PaymentDate, ir.ApprovedAmount AS DispatchAmt, a.Remarks, b.DonationId, a.DepotId as DepotCode " +
                        " from[dbo].[DepotPrintTrack]  a left join InvestmentInit b on a.InvestmentInitId = b.id " +
                        " left join InvestmentRecComment C on a.InvestmentInitId = c.InvestmentInitId " +
                        " left join Donation d on b.DonationId = d.Id left join Employee prep on b.EmployeeId = prep.Id " +
                        " left join InvestmentDetailTracker ir on ir.InvestmentInitId = b.Id " +
                        " left join Employee apr on c.EmployeeId = apr.Id WHERE c.RecStatus = 'Approved' AND b.DataStatus= 1) X " +
                        " WHERE X.PaymentDate between '" + searchDto.FromDate + "' AND '" + searchDto.ToDate + "' " +
                        " AND LEN(X.DepotCode) = 0 ";

                    if (searchDto.DonationId != "0")
                    {
                        qry = qry + " AND X.DonationId = " + searchDto.DonationId + "";
                    }

                    var results = _db.RptMedDisp.FromSqlRaw(qry).ToList();
                    return results;                   
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
