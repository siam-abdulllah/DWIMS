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

        public MedDispatchController(IMapper mapper, StoreContext db, IGenericRepository<InvestmentMedicineProd> investmentMedicineProdRepo, IGenericRepository<MedicineDispatch> dispRepo)
        {
            _db = db;
            _mapper = mapper;
            _investmentMedicineProdRepo = investmentMedicineProdRepo;
            _dispRepo = dispRepo;
        }

        [HttpGet]
        [Route("getMedicineProds/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentMedicineProd>> GetInvestmentMedicineProds(int investmentInitId)
        {
            try
            {
                ///var spec = new InvestmentMedicineProdSpecification(investmentInitId,sbu);
                var spec = new InvestmentMedicineProdSpecification(investmentInitId);
                var investmentMedicineProd = await _investmentMedicineProdRepo.ListAsync(spec);
                return investmentMedicineProd;
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
                IssueDate = trackDto.IssueDate,
                ProposeAmt = trackDto.ProposeAmt,
                DispatchAmt = trackDto.DispatchAmt,
                DepotCode = dptCode,
                DepotName = dptName,
                Remarks = trackDto.Remarks,
                SetOn = DateTimeOffset.Now,
                EmployeeId = trackDto.EmployeeId,
            };

            _dispRepo.Add(bcds);
            _dispRepo.Savechange();

            var t = _db.CountInt.FromSqlRaw("EXECUTE [dbo].[SP_UpdateApprovedAmount] {0},{1}", trackDto.InvestmentInitId, trackDto.DispatchAmt);

            return new MedicineDispatchDto
            {
                Id = bcds.Id,
                InvestmentInitId = trackDto.InvestmentInitId,
                IssueReference = trackDto.IssueReference,
                IssueDate = trackDto.IssueDate,
                DepotCode = dptCode,
                DepotName = trackDto.DepotName,
                Remarks = trackDto.Remarks,
                ProposeAmt = trackDto.ProposeAmt,
                DispatchAmt = trackDto.DispatchAmt,
                EmployeeId = trackDto.EmployeeId,           
            };
        }

        [HttpGet]
        [Route("pendingDispatch/{empId}/{userRole}")]
        public async Task<IReadOnlyList<RptDepotLetterSearch>> PendingForDispatch(int empId, string userRole)
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
                            " where a.DonationTo = 'Doctor' AND  ir.RecStatus = 'Approved' AND inDetail.PaymentMethod = 'Cash' AND a.DonationId = 4 " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
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
                            " inDetail.PaymentMethod = 'Cash' AND a.DonationId = 4 " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
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
                            " AND inDetail.PaymentMethod = 'Cash' AND a.DonationId = 4 " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
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
                            " AND inDetail.PaymentMethod = 'Cash' AND a.DonationId = 4 " +
                            " AND  ir.EmployeeId = inDetail.EmployeeId " +
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
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
                            " AND inDetail.Id in (select max(ID) from investmentrec where InvestmentInitId = a.Id) " +
                            " AND inDetail.PaymentMethod = 'Cash' AND a.DonationId = 4 ) X " +
                            " WHERE X.ID not in (SELECT InvestmentInitId FROM MedicineDispatch) ";
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
    }
}
