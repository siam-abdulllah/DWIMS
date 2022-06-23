using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentRapidController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRapid> _investmentRapidRepo;
        private readonly IGenericRepository<InvestmentCampaign> _investmentCampaignRepo;
        private readonly IGenericRepository<InvestmentRapidAppr> _InvestmentRapidApprRepo;
        private readonly IGenericRepository<MedicineProduct> _medicineProductRepo;
        private readonly IGenericRepository<ProductInfo> _productInfoRepo;
        private readonly IGenericRepository<InvestmentMedicineProd> _investmentMedicineProdRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductsRepo;
        private readonly IGenericRepository<InvestmentRecDepot> _investmentRecDepotRepo;
        private readonly IGenericRepository<InvestmentDetailTracker> _investmentDetailTrackerRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<ApprAuthConfig> _appAuthConfigRepo;
        private readonly IGenericRepository<ApprovalAuthority> _approvalAuthorityRepo;
        private readonly IGenericRepository<InvestmentDoctor> _investmentDoctorRepo;
        private readonly IGenericRepository<InvestmentSociety> _investmentSocietyRepo;
        private readonly IGenericRepository<InvestmentBcds> _investmentBcdsRepo;
        private readonly IGenericRepository<InvestmentInstitution> _investmentInstitutionRepo;
        private readonly IGenericRepository<InvestmentOther> _investmentOtherRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;


        public InvestmentRapidController(IMapper mapper,
            IGenericRepository<InvestmentInit> investmentInitRepo,
            IGenericRepository<InvestmentRapid> investmentRapidRepo,
            IGenericRepository<InvestmentRapidAppr> InvestmentRapidApprRepo,
            IGenericRepository<InvestmentDetailTracker> investmentDetailTrackerRepo,
        IGenericRepository<MedicineProduct> medicineProductRepo,
        IGenericRepository<InvestmentRecDepot> investmentRecDepotRepo,
        IGenericRepository<InvestmentMedicineProd> investmentMedicineProdRepo,
        IGenericRepository<InvestmentCampaign> investmentCampaignRepo,
        IGenericRepository<InvestmentRecProducts> investmentRecProductsRepo,
        IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
        IGenericRepository<ProductInfo> productInfoRepo,
        IGenericRepository<ApprAuthConfig> appAuthConfigRepo,
        IGenericRepository<Employee> employeeRepo,
        IGenericRepository<ApprovalAuthority> approvalAuthorityRepo,
        IGenericRepository<InvestmentRec> investmentRecRepo,
        StoreContext dbContext,
             IGenericRepository<InvestmentDoctor> investmentDoctorRepo,
            IGenericRepository<InvestmentSociety> investmentSocietyRepo,
            IGenericRepository<InvestmentBcds> investmentBcdsRepo,
            //IGenericRepository<InvestmentCampaign> investmentCampaignRepo,
            IGenericRepository<InvestmentInstitution> investmentInstitutionRepo,
            IGenericRepository<InvestmentOther> investmentOtherRepo,
            IGenericRepository<ApprAuthConfig> apprAuthConfigRepo)
        {
            _mapper = mapper;
            _investmentCampaignRepo = investmentCampaignRepo;
            _investmentRapidRepo = investmentRapidRepo;
            _InvestmentRapidApprRepo = InvestmentRapidApprRepo;
            _investmentInitRepo = investmentInitRepo;
            _dbContext = dbContext;
            _medicineProductRepo = medicineProductRepo;
            _productInfoRepo = productInfoRepo;
            _investmentRecDepotRepo = investmentRecDepotRepo;
            _investmentMedicineProdRepo = investmentMedicineProdRepo;
            _investmentRecProductsRepo = investmentRecProductsRepo;
            _investmentDetailTrackerRepo = investmentDetailTrackerRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _employeeRepo = employeeRepo;
            _approvalAuthorityRepo = approvalAuthorityRepo;
            _appAuthConfigRepo = appAuthConfigRepo;
            _investmentRecRepo = investmentRecRepo;
            _apprAuthConfigRepo = apprAuthConfigRepo;
            _investmentDoctorRepo = investmentDoctorRepo;
            _investmentInstitutionRepo = investmentInstitutionRepo;
            _investmentCampaignRepo = investmentCampaignRepo;
            _investmentBcdsRepo = investmentBcdsRepo;
            _investmentSocietyRepo = investmentSocietyRepo;
            _investmentOtherRepo = investmentOtherRepo;
        }

        [HttpPost("saveInvestmentRapid/{empId}")]
        public async Task<ActionResult<InvestmentRapidDto>> saveInvestmentRapid(int empId, InvestmentRapidDto investmentRapidDto)
        {
            try
            {
                int ApprovedBy = investmentRapidDto.ApproverId;

                InvestmentInit investmentInit = new InvestmentInit();
                ApprovalAuthority appAuth = new ApprovalAuthority();
                Employee emp = new Employee();
                ApprAuthConfig authConfig = new ApprAuthConfig();
                investmentInit = await _investmentInitRepo.GetByIdAsync(investmentRapidDto.InvestmentInitId);


                InvestmentRapid investmentForm = new InvestmentRapid();
                InvestmentRapidAppr invRapidApr = new InvestmentRapidAppr();
                investmentForm = await _investmentRapidRepo.GetByIdAsync(investmentRapidDto.Id);

                #region Get Auth Id
                //int ApprovedBy = investmentRapidDto.ApproverId;
                //authConfig = new ApprAuthConfigSpecification(ApprovedBy, "A");
                var spec = new ApprAuthConfigSpecification(empId, "A");
                authConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                //if (authConfig != null)
                //{
                // appAuth = await _approvalAuthorityRepo.GetByIdAsync(authConfig.ApprovalAuthorityId);
                // investmentRapidDto.ApproverId = appAuth.Id;
                //}
                if (authConfig == null)
                {
                    investmentRapidDto.ApproverId = 0;
                }
                emp = await _employeeRepo.GetByIdAsync(ApprovedBy);
                #endregion
                #region Save On Investment Init

                if (investmentInit != null)
                {
                    //investmentInit.ProposeFor = investmentRapidDto.ProposeFor;
                    //investmentInit.DonationTo = investmentRapidDto.DonationTo;
                    var existedInvestment = await _investmentInitRepo.GetByIdAsync(investmentInit.Id);
                    investmentInit.ReferenceNo = investmentRapidDto.ReferenceNo;
                    investmentInit.EmployeeId = investmentRapidDto.InitiatorId;
                    investmentInit.SBUName = investmentRapidDto.SbuName;
                    investmentInit.SubmissionDate = existedInvestment.SubmissionDate;
                    investmentInit.SBU = investmentRapidDto.SBU;
                    investmentInit.Confirmation = true;
                    investmentInit.SetOn = existedInvestment.SetOn;
                    investmentInit.ModifiedOn = DateTimeOffset.Now;
                    investmentInit.DonationId = Convert.ToInt32(investmentRapidDto.Type);
                    _investmentInitRepo.Update(investmentInit);
                    _investmentInitRepo.Savechange();
                }
                else
                {
                    investmentInit = new InvestmentInit
                    {
                        ReferenceNo = null,
                        ProposeFor = investmentRapidDto.ProposeFor,
                        DonationTo = investmentRapidDto.DonationTo,
                        EmployeeId = investmentRapidDto.InitiatorId,
                        DonationId = Convert.ToInt32(investmentRapidDto.Type),
                        SBU = investmentRapidDto.SBU,
                        Confirmation = true,
                        SubmissionDate = DateTimeOffset.Now,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentInitRepo.Add(investmentInit);
                    _investmentInitRepo.Savechange();
                    List<SqlParameter> parms = new List<SqlParameter>
                    {

                        new SqlParameter("@IID", investmentInit.Id),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentRefNoInsert @IID,@r out", parms.ToArray());
                    investmentInit.ReferenceNo = parms[1].Value.ToString();

                    //if (investmentRapidDto.SubCampaignId != 0)
                    //{
                    //    var investmentCampaign = new InvestmentCampaign
                    //    {
                    //        //ReferenceNo = investmentCampaignDto.ReferenceNo,
                    //        InvestmentInitId = investmentInit.Id,
                    //        CampaignDtlId = investmentRapidDto.SubCampaignId,
                    //        DoctorId = 90000,
                    //        InstitutionId = 99999,
                    //        SetOn = DateTimeOffset.Now,
                    //        ModifiedOn = DateTimeOffset.Now
                    //    };
                    //    _investmentCampaignRepo.Add(investmentCampaign);
                    //    _investmentCampaignRepo.Savechange();

                    //}
                    if (investmentRapidDto.DonationTo == "Doctor")
                    { investmentRapidDto.InvestmentDoctor.InvestmentInitId=investmentInit.Id;
                        await InsertInvestmentDoctor(investmentRapidDto.InvestmentDoctor); }
                    else if (investmentRapidDto.DonationTo == "Institution")
                    { investmentRapidDto.InvestmentInstitution.InvestmentInitId=investmentInit.Id;
                        await InsertInvestmentInstitution(investmentRapidDto.InvestmentInstitution); }
                    else if (investmentRapidDto.DonationTo == "Campaign")
                    { investmentRapidDto.InvestmentCampaign.InvestmentInitId=investmentInit.Id;
                        await InsertInvestmentCampaign(investmentRapidDto.InvestmentCampaign); }
                    else if (investmentRapidDto.DonationTo == "Bcds")
                    { investmentRapidDto.InvestmentBcds.InvestmentInitId=investmentInit.Id;
                        await InsertInvestmentBcds(investmentRapidDto.InvestmentBcds); }
                    else if (investmentRapidDto.DonationTo == "Society")
                    { investmentRapidDto.InvestmentSociety.InvestmentInitId=investmentInit.Id;
                        await InsertInvestmentSociety(investmentRapidDto.InvestmentSociety); }
                    else if (investmentRapidDto.DonationTo == "Other")
                    { investmentRapidDto.InvestmentOther.InvestmentInitId=investmentInit.Id;
                        await InsertInvestmentOther(investmentRapidDto.InvestmentOther); }
                }
                #endregion
                #region Save On Investment Rapid

                if (investmentForm != null)
                {
                    if (!string.IsNullOrEmpty(investmentForm.DepotCode) && investmentRapidDto.PaymentMethod != "Cash")
                    {
                        var alreadyExistSpecRecDepot = new InvestmentRecDepotSpecification(investmentInit.Id, investmentForm.DepotCode);
                        var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpecRecDepot);
                        if (alreadyExistInvestmentRecDepotList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentRecDepotList)
                            {
                                _investmentRecDepotRepo.Delete(v);
                                _investmentRecDepotRepo.Savechange();
                            }
                        }
                    }
                    investmentForm.ModifiedOn = DateTime.Now;
                    investmentForm = _mapper.Map(investmentRapidDto, investmentForm);
                    _investmentRapidRepo.Update(investmentForm);
                    _investmentRapidRepo.Savechange();


                    string qry = string.Format(@"select * from InvestmentRapidAppr where InvestmentRapidId = {0}", investmentForm.Id);
                    invRapidApr = _dbContext.InvestmentRapidAppr.FromSqlRaw(qry).ToList().FirstOrDefault();
                    if (invRapidApr != null)
                    {
                        invRapidApr.InvestmentInitId = investmentInit.Id;
                        invRapidApr.InvestmentRapidId = investmentForm.Id;

                        invRapidApr.ApprovalRemarks = investmentRapidDto.Approval;
                        invRapidApr.ApprovedStatus = investmentRapidDto.ApprovedStatus;
                        invRapidApr.ModifiedOn = DateTime.Now;
                        _InvestmentRapidApprRepo.Update(invRapidApr);

                    }
                }
                else
                {
                    DateTime ProposalDate = new DateTime();
                    DateTime.TryParse(investmentRapidDto.ProposalDateStr, out ProposalDate);
                    investmentRapidDto.PropsalDate = ProposalDate;
                    investmentRapidDto.ModifiedOn = DateTime.Now;
                    investmentRapidDto.SetOn = DateTime.Now;
                    investmentRapidDto.InitiatorId = investmentInit.EmployeeId;
                    investmentRapidDto.InvestmentInitId = investmentInit.Id;
                    investmentRapidDto.ReferenceNo = investmentInit.ReferenceNo;
                    investmentRapidDto.InvestmentInitId = investmentInit.Id;
                    investmentForm = _mapper.Map<InvestmentRapid>(investmentRapidDto);
                    _investmentRapidRepo.Add(investmentForm);
                    _investmentRapidRepo.Savechange();

                    invRapidApr = new InvestmentRapidAppr();
                    invRapidApr.InvestmentInitId = investmentInit.Id;
                    invRapidApr.InvestmentRapidId = investmentForm.Id;
                    invRapidApr.ApprovedBy = ApprovedBy;

                    invRapidApr.ApproverId = investmentRapidDto.ApproverId;
                    invRapidApr.SetOn = investmentForm.SetOn;
                    invRapidApr.ModifiedOn = DateTime.Now;
                    _InvestmentRapidApprRepo.Add(invRapidApr);
                    _InvestmentRapidApprRepo.Savechange();

                }


                #endregion
                #region Insert Medicine Product
                InvestmentMedicineProd invMedicineProd = new InvestmentMedicineProd();
                if (investmentRapidDto.InvestmentMedicineProd != null && investmentRapidDto.InvestmentMedicineProd.Count > 0)
                {

                    var alreadyExistSpec = new InvestmentMedicineProdSpecification(investmentForm.InvestmentInitId);
                    var alreadyExistInvestmentMedicineProdList = await _investmentMedicineProdRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentMedicineProdList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentMedicineProdList)
                        {
                            _investmentMedicineProdRepo.Delete(v);
                            _investmentMedicineProdRepo.Savechange();
                        }
                    }
                    foreach (var item in investmentRapidDto.InvestmentMedicineProd)
                    {
                        var medicineProd = await _medicineProductRepo.GetByIdAsync(item.ProductId);
                        var iMedicineProd = new InvestmentMedicineProd
                        {
                            //ReferenceNo = investmentMedicineProdDto.ReferenceNo,
                            InvestmentInitId = investmentForm.InvestmentInitId,
                            ProductId = item.ProductId,
                            EmployeeId = investmentInit.EmployeeId,
                            BoxQuantity = item.BoxQuantity,
                            TpVat = (medicineProd.UnitTp + medicineProd.UnitVat) * item.BoxQuantity,
                            SetOn = DateTimeOffset.Now,
                            //ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentMedicineProdRepo.Add(iMedicineProd);

                        _investmentMedicineProdRepo.Savechange();
                    }
                }
                #endregion
                #region Insert Targeted Products
                InvestmentRecProducts invRecProduct = new InvestmentRecProducts();
                if (investmentRapidDto.InvestmentRecProducts != null && investmentRapidDto.InvestmentRecProducts.Count > 0)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification(investmentForm.InvestmentInitId);
                    var alreadyExistInvestmentRecProdList = await _investmentRecProductsRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProdList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecProdList)
                        {
                            _investmentRecProductsRepo.Delete(v);
                            _investmentRecProductsRepo.Savechange();
                        }
                    }
                    foreach (var item in investmentRapidDto.InvestmentRecProducts)
                    {
                        var ProductInfo = await _medicineProductRepo.GetByIdAsync(item.ProductId);
                        var investmentRecProducts = new InvestmentRecProducts
                        {
                            //ReferenceNo = investmentMedicineProdDto.ReferenceNo,
                            InvestmentInitId = investmentForm.InvestmentInitId,
                            ProductId = item.ProductId,
                            EmployeeId = investmentInit.EmployeeId,
                            SBU = item.SBU,
                            DataStatus = item.DataStatus,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecProductsRepo.Add(investmentRecProducts);
                    }
                    _investmentRecProductsRepo.Savechange();
                }

                #endregion
                #region Insert Depot
                if (investmentRapidDto.DepotCode != null)
                {
                    var alreadyExistSpecRecDepot = new InvestmentRecDepotSpecification(investmentInit.Id, investmentRapidDto.DepotCode);
                    var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpecRecDepot);
                    if (alreadyExistInvestmentRecDepotList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecDepotList)
                        {
                            _investmentRecDepotRepo.Delete(v);
                            _investmentRecDepotRepo.Savechange();
                        }
                    }
                    InvestmentRecDepot invRecDepot = new InvestmentRecDepot();
                    //if (investmentRapidDto.investmentMedicineProd != null && investmentRapidDto.investmentMedicineProd.Count>0)
                    //{
                    invRecDepot = new InvestmentRecDepot
                    {
                        //ReferenceNo = investmentRecDto.ReferenceNo,
                        InvestmentInitId = investmentInit.Id,
                        DepotCode = investmentRapidDto.DepotCode,
                        DepotName = investmentRapidDto.DepotName,
                        EmployeeId = investmentRapidDto.InitiatorId,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecDepotRepo.Add(invRecDepot);
                    _investmentRecDepotRepo.Savechange();
                    //}

                }
                #endregion




                return new InvestmentRapidDto
                {
                    Id = investmentForm.Id,
                    ReferenceNo = investmentForm.ReferenceNo,
                    ProposeFor = investmentForm.ProposeFor,
                    DonationTo = investmentForm.DonationTo,
                    InitiatorId = investmentForm.InitiatorId

                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost("saveInvestmentRapidAppr/{empId}")]
        public async Task<ActionResult<InvestmentRapidDto>> saveInvestmentRapidAppr(int empId, InvestmentRapidDto investmentRapidDto)
        {
            try
            {
                int ApprovedBy = investmentRapidDto.ApproverId;

                InvestmentInit investmentInit = new InvestmentInit();
                ApprovalAuthority appAuth = new ApprovalAuthority();
                Employee emp = new Employee();
                ApprAuthConfig authConfig = new ApprAuthConfig();
                investmentInit = await _investmentInitRepo.GetByIdAsync(investmentRapidDto.InvestmentInitId);


                InvestmentRapid investmentForm = new InvestmentRapid();
                InvestmentRapidAppr invRapidApr = new InvestmentRapidAppr();
                investmentForm = await _investmentRapidRepo.GetByIdAsync(investmentRapidDto.Id);

                #region Get Auth Id

                var spec = new ApprAuthConfigSpecification(empId, "A");
                authConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);

                if (authConfig == null)
                {
                    investmentRapidDto.ApproverId = 0;
                }
                emp = await _employeeRepo.GetByIdAsync(ApprovedBy);
                #endregion

                #region Investment Rapid Approved
                //InvestmentRapidAppr invRapidApr = new InvestmentRapidAppr();
                if (!string.IsNullOrEmpty(investmentRapidDto.ApprovedStatus))
                {
                    if (investmentRapidDto.ApprovedStatus == "Approved")
                    {
                        if (investmentRapidDto.DonationTo != "Campaign")
                        {
                            if (investmentRapidDto.ProposeFor != "Others Rapid")
                            {
                                if (investmentRapidDto.ProposeFor == "Sales")
                                {
                                    List<SqlParameter> parms = new List<SqlParameter>
                                 {
                                    new SqlParameter("@SBU", investmentForm.SBU),
                                    new SqlParameter("@DID", investmentForm.Type),
                                    new SqlParameter("@EID", ApprovedBy),
                                    new SqlParameter("@IID", investmentForm.InvestmentInitId),
                                    new SqlParameter("@PRAMOUNT", investmentRapidDto.ProposedAmount),
                                    new SqlParameter("@ASTATUS", investmentRapidDto.ApprovedStatus),
                                    new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                                 };
                                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckNewRapidSales @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                                    if (parms[6].Value.ToString() != "True")
                                    {
                                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                                    }
                                }
                                else
                                {
                                    List<SqlParameter> parms = new List<SqlParameter>
                                 {
                                    new SqlParameter("@SBU", investmentForm.SBU),
                                    new SqlParameter("@DID", investmentForm.Type),
                                    new SqlParameter("@EID", ApprovedBy),
                                    new SqlParameter("@IID", investmentForm.InvestmentInitId),
                                    new SqlParameter("@PRAMOUNT", investmentRapidDto.ProposedAmount),
                                    new SqlParameter("@ASTATUS", investmentRapidDto.ApprovedStatus),
                                    new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                                 };
                                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckNewRapidPMD @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                                    if (parms[6].Value.ToString() != "True")
                                    {
                                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                                    }

                                }
                            }
                        }
                        else
                        {
                            List<SqlParameter> parms = new List<SqlParameter>
                                {
                                    new SqlParameter("@SBU", investmentForm.SBU),
                                    new SqlParameter("@DID", investmentForm.Type),
                                    new SqlParameter("@EID", ApprovedBy),
                                    new SqlParameter("@IID", investmentForm.InvestmentInitId),
                                    new SqlParameter("@PRAMOUNT", investmentRapidDto.ProposedAmount),
                                    new SqlParameter("@ASTATUS", investmentRapidDto.ApprovedStatus),
                                    new SqlParameter("@CDTLID", investmentRapidDto.InvestmentCampaign.CampaignDtlId),
                                    new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                                };
                            var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckForCampaign @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@CDTLID,@r out", parms.ToArray());
                            if (parms[7].Value.ToString() != "True")
                            {
                                return BadRequest(new ApiResponse(400, parms[7].Value.ToString()));
                            }


                        }

                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmentForm.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmentForm.InvestmentInitId,
                            DonationId = investmentForm.Type != null ? Convert.ToInt32(investmentForm.Type) : 0,
                            ApprovedAmount = investmentRapidDto.ProposedAmount,
                            Month = DateTime.Now.Month,
                            Year = DateTime.Now.Year,
                            FromDate = DateTime.Now,
                            ToDate = DateTime.Now,
                            PaidStatus = "Paid",
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentDetailTrackerRepo.Add(invDT);
                        _investmentDetailTrackerRepo.Savechange();



                    }
                    else
                    {
                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmentForm.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var result = _dbContext.Database.ExecuteSqlRaw("DELETE FROM InvestmentRapidAppr WHERE InvestmentInitId=" + investmentInit.Id + " AND ApproverId=" + investmentRapidDto.ApproverId + "");
                        var invRapidAprForApr = new InvestmentRapidAppr();
                        invRapidAprForApr.InvestmentInitId = investmentInit.Id;
                        invRapidAprForApr.InvestmentRapidId = investmentForm.Id;
                        invRapidAprForApr.ApprovedBy = ApprovedBy;
                        invRapidAprForApr.ApproverId = investmentRapidDto.ApproverId;
                        invRapidAprForApr.SetOn = investmentForm.SetOn;
                        invRapidAprForApr.ModifiedOn = DateTime.Now;
                        _InvestmentRapidApprRepo.Add(invRapidAprForApr);
                        _InvestmentRapidApprRepo.Savechange();
                    }
                    #region Insert Into Rec Comment


                    var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmentForm.InvestmentInitId, emp.Id);
                    var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
                    if (investmentRecCmnts.Count > 0)
                    {
                        foreach (var v in investmentRecCmnts)
                        {
                            _investmentRecCommentRepo.Delete(v);
                            _investmentRecCommentRepo.Savechange();
                        }
                    }
                    bool complitionStatus = false;
                    if (investmentRapidDto.ApprovedStatus == "Approved")
                    {
                        complitionStatus = true;
                    }
                    var invRecComment = new InvestmentRecComment
                    {
                        InvestmentInitId = investmentForm.InvestmentInitId,
                        SBU = emp.SBU,
                        SBUName = emp.SBUName,
                        ZoneCode = emp.ZoneCode,
                        ZoneName = emp.ZoneName,
                        RegionName = emp.RegionName,
                        RegionCode = emp.RegionCode,
                        TerritoryCode = emp.TerritoryCode,
                        TerritoryName = emp.TerritoryName,
                        MarketCode = emp.MarketCode,
                        MarketName = emp.MarketName,
                        MarketGroupCode = emp.MarketGroupCode,
                        MarketGroupName = emp.MarketGroupName,
                        Comments = investmentForm.Remarks,
                        RecStatus = investmentRapidDto.ApprovedStatus,
                        CompletionStatus = complitionStatus,
                        // Priority = appAuth.Priority,
                        Priority = authConfig.ApprovalAuthority.Priority,
                        EmployeeId = emp.Id,
                        SetOn = DateTime.Now

                    };
                    _investmentRecCommentRepo.Add(invRecComment);
                    _investmentRecCommentRepo.Savechange();
                    #endregion
                    #region Insert Into Investment Rec
                    var alreadyExistRecSpec = new InvestmentRecSpecification((int)investmentForm.InvestmentInitId, emp.Id);
                    var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
                    if (alreadyExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprList)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }
                    var invRec = new InvestmentRec
                    {
                        InvestmentInitId = investmentForm.InvestmentInitId,
                        ProposedAmount = investmentForm.ProposedAmount,
                        //Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                        PaymentFreq = "Yearly",
                        //CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                        //CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                        FromDate = DateTime.Now,
                        ToDate = DateTime.Now,
                        CommitmentFromDate = DateTime.Now,
                        CommitmentToDate = DateTime.Now,
                        TotalMonth = 1,
                        CommitmentTotalMonth = 1,
                        PaymentMethod = investmentForm.PaymentMethod,
                        ChequeTitle = investmentForm.ChequeTitle,
                        EmployeeId = emp.Id,
                        //Priority = appAuth.Priority,
                        Priority = authConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRec);
                    _investmentRecRepo.Savechange();
                    #endregion
                    #region Update Investment Rapid Appr
                    string qry = string.Format(@"select * from InvestmentRapidAppr where InvestmentRapidId = {0}", investmentForm.Id);
                    invRapidApr = _dbContext.InvestmentRapidAppr.FromSqlRaw(qry).ToList().FirstOrDefault();
                    invRapidApr.InvestmentInitId = investmentInit.Id;
                    invRapidApr.InvestmentRapidId = investmentForm.Id;
                    invRapidApr.ApprovalRemarks = investmentRapidDto.Approval;
                    invRapidApr.ApprovedStatus = investmentRapidDto.ApprovedStatus;
                    invRapidApr.ApproverId = investmentRapidDto.ApproverId;
                    invRapidApr.ModifiedOn = DateTime.Now;
                    _InvestmentRapidApprRepo.Update(invRapidApr);
                    _InvestmentRapidApprRepo.Savechange();
                    #endregion
                }
                #endregion
                #region Insert Medicine Product
                InvestmentMedicineProd invMedicineProd = new InvestmentMedicineProd();
                if (investmentRapidDto.InvestmentMedicineProd != null && investmentRapidDto.InvestmentMedicineProd.Count > 0)
                {
                    var alreadyExistSpec = new InvestmentMedicineProdSpecification(investmentForm.InvestmentInitId);
                    var alreadyExistInvestmentMedicineProdList = await _investmentMedicineProdRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentMedicineProdList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentMedicineProdList)
                        {
                            _investmentMedicineProdRepo.Delete(v);
                            _investmentMedicineProdRepo.Savechange();
                        }
                    }
                    foreach (var item in investmentRapidDto.InvestmentMedicineProd)
                    {
                        var medicineProd = await _medicineProductRepo.GetByIdAsync(item.ProductId);
                        var iMedicineProd = new InvestmentMedicineProd
                        {
                            //ReferenceNo = investmentMedicineProdDto.ReferenceNo,
                            InvestmentInitId = investmentForm.InvestmentInitId,
                            ProductId = item.ProductId,
                            EmployeeId = investmentInit.EmployeeId,
                            BoxQuantity = item.BoxQuantity,
                            TpVat = (medicineProd.UnitTp + medicineProd.UnitVat) * item.BoxQuantity,
                            SetOn = DateTimeOffset.Now,
                            //ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentMedicineProdRepo.Add(iMedicineProd);
                        _investmentMedicineProdRepo.Savechange();
                    }
                }
                #endregion
                #region Insert Targeted Products
                InvestmentRecProducts invRecProduct = new InvestmentRecProducts();
                if (investmentRapidDto.InvestmentRecProducts != null && investmentRapidDto.InvestmentRecProducts.Count > 0)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification(investmentForm.InvestmentInitId);
                    var alreadyExistInvestmentRecProdList = await _investmentRecProductsRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProdList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecProdList)
                        {
                            _investmentRecProductsRepo.Delete(v);
                            _investmentRecProductsRepo.Savechange();
                        }
                    }
                    foreach (var item in investmentRapidDto.InvestmentRecProducts)
                    {
                        var ProductInfo = await _medicineProductRepo.GetByIdAsync(item.ProductId);
                        var investmentRecProducts = new InvestmentRecProducts
                        {
                            //ReferenceNo = investmentMedicineProdDto.ReferenceNo,
                            InvestmentInitId = investmentForm.InvestmentInitId,
                            ProductId = item.ProductId,
                            EmployeeId = investmentInit.EmployeeId,
                            SBU = item.SBU,
                            DataStatus = item.DataStatus,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecProductsRepo.Add(investmentRecProducts);
                    }
                    _investmentRecProductsRepo.Savechange();
                }

                #endregion
                #region Insert Depot
                if (investmentRapidDto.DepotCode != null)
                {
                    var alreadyExistSpecRecDepot = new InvestmentRecDepotSpecification(investmentInit.Id, investmentRapidDto.DepotCode);
                    var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpecRecDepot);
                    if (alreadyExistInvestmentRecDepotList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecDepotList)
                        {
                            _investmentRecDepotRepo.Delete(v);
                            _investmentRecDepotRepo.Savechange();
                        }
                    }
                    InvestmentRecDepot invRecDepot = new InvestmentRecDepot();
                    //if (investmentRapidDto.investmentMedicineProd != null && investmentRapidDto.investmentMedicineProd.Count>0)
                    //{
                    invRecDepot = new InvestmentRecDepot
                    {
                        //ReferenceNo = investmentRecDto.ReferenceNo,
                        InvestmentInitId = investmentInit.Id,
                        DepotCode = investmentRapidDto.DepotCode,
                        DepotName = investmentRapidDto.DepotName,
                        EmployeeId = investmentRapidDto.InitiatorId,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecDepotRepo.Add(invRecDepot);
                    _investmentRecDepotRepo.Savechange();
                    //}

                }
                #endregion
                return new InvestmentRapidDto
                {
                    Id = investmentForm.Id,
                    ReferenceNo = investmentForm.ReferenceNo,
                    ProposeFor = investmentForm.ProposeFor,
                    DonationTo = investmentForm.DonationTo,
                    InitiatorId = investmentForm.InitiatorId

                };

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("employeesForRapid")]
        public IReadOnlyList<Employee> GetEmployeesForRapid()
        {
            List<Employee> empList = new List<Employee>();
            try
            {
                string qry = @"select emp.* from Employee emp 
                                        left join ApprAuthConfig ac on emp.Id = ac.EmployeeId
                                        where ac.ApprovalAuthorityId in(3,4,5,6,7,8)";
                empList = _dbContext.Employee.FromSqlRaw(qry).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return empList;
        }

        [HttpGet("getEmployeesforRapidByDpt/{proposeFor}/{sbu}/{empId}")]
        public IReadOnlyList<Employee> GetEmployeesforRapidByDpt(string proposeFor, string sbu, int empId)
        {
            var deptId = "";
            var sbuQry = "";
            if (proposeFor == "Sales")
            {
                deptId = " AND emp.DepartmentId=1";
            }
            else if (proposeFor == "PMD")
            {
                deptId = " AND emp.DepartmentId=2";
            }
            else
            { deptId = " AND emp.DepartmentId IN (1,2)"; }
            if (!string.IsNullOrEmpty(sbu) && sbu != "null")
            {
                sbuQry = " AND esm.SBU='" + sbu + "'";
            }

            else
            { sbuQry = ""; }
            List<Employee> empList = new List<Employee>();
            try
            {
                //string qry = string.Format(@"select emp.* from Employee emp 
                //                        left join ApprAuthConfig ac on emp.Id = ac.EmployeeId
                //                        where ac.ApprovalAuthorityId in(3,4,5,6,7,8) {0} {1}", deptId, sbuQry); 
                string qry = string.Format(@"select DISTINCT emp.* from Employee emp 
                                        INNER JOIN  ApprAuthConfig ac on emp.Id = ac.EmployeeId
                                       INNER JOIN EmpSbuMapping esm ON emp.Id = esm.EmployeeId
                                        where  ac.ApprovalAuthorityId not  in (1,2,9,10,11,13,15) AND ac.Status='A' 
                                        AND esm.DataStatus=1  {0} {1}", deptId, sbuQry);
                empList = _dbContext.Employee.FromSqlRaw(qry).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return empList;
        }

        [HttpGet("getEmployeesforRapidBySBU/{proposeFor}/{sbu}/{empId}")]
        public IReadOnlyList<Employee> GetEmployeesforRapidBySBU(string proposeFor, string sbu,int empId)
        {
            var deptId = "";
            var sbuQry = "";
            //var spec = new ApprAuthConfigSpecification(empId, "A");
            //var authConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            if (proposeFor == "Sales")
            {
                deptId = " AND emp.DepartmentId=1";
            }
            else if (proposeFor == "PMD")
            {
                deptId = " AND emp.DepartmentId=2";
            }
            else
            { deptId = " AND emp.DepartmentId IN (1,2)"; }

            if (!string.IsNullOrEmpty(sbu) && sbu != "null")
            {
                sbuQry = " AND esm.SBU='" + sbu + "'";
            }

            //else
            //{ sbuQry = ""; }
            List<Employee> empList = new List<Employee>();
            try
            {
                //string qry = string.Format(@"select emp.* from Employee emp 
                //                        left join ApprAuthConfig ac on emp.Id = ac.EmployeeId
                //                        where ac.ApprovalAuthorityId in (3,5,6,7,8)
                //                        {0} {1}
                //                        UNION select emp.* from Employee emp 
                //                        left join ApprAuthConfig ac on emp.Id = ac.EmployeeId
                //                        where ac.ApprovalAuthorityId in (4) {0} ", deptId, sbuQry); 
                string qry = string.Format(@"select DISTINCT  emp.* from Employee emp 
                                       INNER JOIN  ApprAuthConfig ac on emp.Id = ac.EmployeeId
                                       INNER JOIN EmpSbuMapping esm ON emp.Id = esm.EmployeeId
                                        where  ac.ApprovalAuthorityId not  in (1,2,9,10,11,13,15) AND ac.Status='A'
                                        AND esm.DataStatus=1  
                                        {0} {1}
                                        ", deptId, sbuQry);
                empList = _dbContext.Employee.FromSqlRaw(qry).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return empList;
        }

        [HttpGet("getEmployeesforRapidByCamp/{subCampaignId}/{empId}")]
        public IReadOnlyList<Employee> GetEmployeesforRapidByCamp(string subCampaignId, int empId)
        {

            List<Employee> empList = new List<Employee>();
            try
            {
                string qry = string.Format(@"select A.* from Employee A 
                                            INNER JOIN CampaignMst B ON A.Id=B.EmployeeId INNER JOIN CampaignDtl C ON B.Id=C.MstId
                                        WHERE  C.SubCampaignId={0} ", subCampaignId);
                empList = _dbContext.Employee.FromSqlRaw(qry).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return empList;
        }


        [HttpGet]
        [Route("getInvestmentRapids/{employeeId}/{from}/{For}")]
        public IReadOnlyList<InvestmentRapidVM> GetInvestmentRapids(int employeeId, string from, string For)
        {
            try
            {

                string qry = "";
                if (from == "init")
                {

                    qry = string.Format(@" SELECT  IR.*,IRA.ApproverId,d.DonationTypeName,IRA.ApprovedStatus,IRA.ApprovalRemarks as Approval  from InvestmentRapid IR 
                                        INNER JOIN Donation d on IR.Type = d.Id
                                        INNER JOIN InvestmentRapidAppr IRA on IR.Id = IRA.InvestmentRapidId
                                        WHERE  IR.InitiatorId={0} and (IRA.ApprovedStatus is  null or IRA.ApprovedStatus ='Pending') 
                                        AND IR.InvestmentInitId NOT IN (SELECT InvestmentInitId FROM InvestmentRapidAppr WHERE ApprovedStatus is not  null and
                                        ApprovedStatus <>'Pending')", employeeId);

                }
                else
                {
                    string statusQuery = "";
                    if (For == "reference")
                    {
                        // statusQuery = " and (IRA.ApprovedStatus is null OR (IRA.ApprovedStatus ='Recommended' AND IRA.ApprovedBy<>" + employeeId + "))";
                        qry = string.Format(@" SELECT  IR.*,IRA.ApproverId ,d.DonationTypeName,IRA.ApprovedStatus,IRA.ApprovalRemarks as Approval  from InvestmentRapid IR 
                                        INNER JOIN Donation d on IR.Type = d.Id
                                        INNER JOIN InvestmentRapidAppr IRA on IR.Id = IRA.InvestmentRapidId
                                        WHERE NOT EXISTS (SELECT IT.InvestmentInitId FROM InvestmentDetailTracker IT WHERE IT.InvestmentInitId = IR.InvestmentInitId 
                                        AND IT.PaymentRefNo is not null) AND IRA.ApprovedBy={0}  AND IRA.ApprovedStatus is null 
                                        AND NOT EXISTS (SELECT IRAP.InvestmentInitId FROM InvestmentRapidAppr IRAP WHERE IRA.InvestmentInitId = IRAP.InvestmentInitId 
                                        AND (IRA.ApprovedStatus='Approved' OR IRA.ApprovedStatus='Cancelled') )", employeeId);
                    }
                    else if (For == "search")
                    {
                        statusQuery = " and IRA.ApprovedStatus is not null";
                        qry = string.Format(@" SELECT  IR.*,IRA.ApproverId ,d.DonationTypeName,IRA.ApprovedStatus,IRA.ApprovalRemarks as Approval  from InvestmentRapid IR 
                                        INNER JOIN Donation d on IR.Type = d.Id
                                        INNER JOIN InvestmentRapidAppr IRA on IR.Id = IRA.InvestmentRapidId
                                        WHERE NOT EXISTS (SELECT IT.InvestmentInitId FROM InvestmentDetailTracker IT WHERE IT.InvestmentInitId = IR.InvestmentInitId 
                                        AND  IT.PaymentRefNo is not null) AND NOT EXISTS (SELECT IRAP.InvestmentInitId FROM InvestmentRapidAppr IRAP WHERE IRA.InvestmentInitId = IRAP.InvestmentInitId 
                                        AND (IRA.ApprovedStatus='Approved' OR IRA.ApprovedStatus='Cancelled') AND   IRAP.ApprovedBy<>{0}) AND   IRA.ApprovedBy={0} AND  IRA.ApprovedStatus is not null ", employeeId);
                    }

                }


                List<InvestmentRapidVM> dsResult = _dbContext.ExecSQL<InvestmentRapidVM>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }


        }

        [HttpGet]
        [Route("getInvestmentmedicineProducts/{invInitId}")]
        public async Task<ActionResult<List<InvestmentMedicineProdDto>>> GetInvestmentmedicineProducts(int invInitId)
        {
            try
            {
                List<InvestmentMedicineProdDto> medicineProdList = new List<InvestmentMedicineProdDto>();
                MedicineProduct medicine = new MedicineProduct();
                string qry = string.Format(@"select * from InvestmentMedicineProd where InvestmentInitId = {0}", invInitId);
                var results = _dbContext.InvestmentMedicineProd.FromSqlRaw(qry).ToList();
                if (results != null && results.Count > 0)
                {
                    foreach (var item in results)
                    {
                        medicine = await _medicineProductRepo.GetByIdAsync(item.ProductId);
                        medicineProdList.Add(new InvestmentMedicineProdDto
                        {
                            id = item.Id,
                            employeeId = item.EmployeeId.Value,
                            investmentInitId = item.InvestmentInitId,
                            medicineProduct = medicine,
                            productId = item.ProductId,
                            tpVat = item.TpVat,
                            boxQuantity = item.BoxQuantity
                        });
                    }
                }
                return medicineProdList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentTargetedProds/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentTargetedProds(int investmentInitId, string sbu)
        {
            try
            {
                List<InvestmentRecProducts> recProdList = new List<InvestmentRecProducts>();
                ProductInfo product = new ProductInfo();
                string qry = string.Format(@"select * from InvestmentRecProducts where InvestmentInitId = {0} and SBU = {1}", investmentInitId, sbu);
                var results = _dbContext.InvestmentRecProducts.FromSqlRaw(qry).ToList();
                if (results != null && results.Count > 0)
                {
                    foreach (var item in results)
                    {
                        product = await _productInfoRepo.GetByIdAsync(item.ProductId);
                        recProdList.Add(new InvestmentRecProducts
                        {
                            Id = item.Id,
                            EmployeeId = item.EmployeeId.Value,
                            InvestmentInitId = item.InvestmentInitId,
                            ProductInfo = product,
                            ProductId = item.ProductId,
                            SBU = item.SBU,
                            Employee = item.Employee
                        });
                    }
                }
                return recProdList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getRapidSubCampaigns/{sbu}")]
        public IReadOnlyList<SubCampainDtl> GetRapidSubCampaigns(string sbu)
        {
            try
            {
                string qry = "";

                qry = string.Format(@" select sc.Id SubCampId, sc.SubCampaignName,cm.SBU from CampaignDtl cmpDtl
                                        left join SubCampaign sc on sc.Id = cmpDtl.SubCampaignId
                                        left join CampaignMst cm on cm.Id = cmpDtl.MstId where cm.SBU = {0}", sbu);

                List<SubCampainDtl> dsResult = _dbContext.ExecSQL<SubCampainDtl>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #region investmentDoctor
        [HttpPost("IsDoctorInvestmentApprovalPending")]
        public async Task<int> IsDoctorInvestmentApprovalPending(int initId, int doctorId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentDoctor d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE  i.DataStatus=1 AND " +
                // " EXISTS ( " +
                // " SELECT InvestmentInitId " +
                // " FROM InvestmentTargetedGroup IT " +
                // " WHERE IT.InvestmentInitId = I.Id " +
                // " AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentDoctor  " +
                " WHERE InvestmentInitId = " + initId + " " +
                " AND DoctorId = " + doctorId + " " +
                " ) " +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                //" AND i.Id <> " + initId + " " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.DoctorId = " + doctorId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }


        [HttpPost("insertInvestmentDoctor")]
        public async Task<ActionResult<InvestmentDoctorDto>> InsertInvestmentDoctor(InvestmentDoctor investmentDoc)
        {
            try
            {
                //int a = await IsDoctorInvestmentApprovalPending(investmentDoctorDto.InvestmentInitId, investmentDoctorDto.DoctorId);
                // if (await IsDoctorInvestmentApprovalPending(investmentDoc.InvestmentInitId, investmentDoc.DoctorId) > 0)
                // {
                //     return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Doctor!"));
                // }
                var alreadyExistSpec = new InvestmentDoctorSpecification(investmentDoc.InvestmentInitId);
                var alreadyExistInvestmentDoctorList = await _investmentDoctorRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentDoctorList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentDoctorList)
                    {
                        _investmentDoctorRepo.Delete(v);
                        _investmentDoctorRepo.Savechange();
                    }
                }

                var investmentDoctor = new InvestmentDoctor
                {
                    //ReferenceNo = investmentDoctorDto.ReferenceNo,
                    InvestmentInitId = investmentDoc.InvestmentInitId,
                    DoctorId = investmentDoc.DoctorId,
                    InstitutionId = investmentDoc.InstitutionId,
                    DoctorType = investmentDoc.DoctorType,
                    DoctorCategory = investmentDoc.DoctorCategory,
                    PatientsPerDay = investmentDoc.PatientsPerDay,
                    PracticeDayPerMonth = investmentDoc.PracticeDayPerMonth,
                    SetOn = DateTimeOffset.Now
                };
                _investmentDoctorRepo.Add(investmentDoctor);
                _investmentDoctorRepo.Savechange();

                return new InvestmentDoctorDto
                {
                    Id = investmentDoctor.Id,
                    InvestmentInitId = investmentDoc.InvestmentInitId,
                    DoctorId = investmentDoc.DoctorId,
                    InstitutionId = investmentDoc.InstitutionId,
                    DoctorType = investmentDoc.DoctorType,
                    DoctorCategory = investmentDoc.DoctorCategory,
                    PatientsPerDay = investmentDoc.PatientsPerDay,
                    PracticeDayPerMonth = investmentDoc.PracticeDayPerMonth,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentDoctors/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentDoctor>> GetInvestmentDoctors(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentDoctorSpecification(investmentInitId);
                var investmentDoctor = await _investmentDoctorRepo.ListAsync(spec);
                return investmentDoctor;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentDoctor")]
        public async Task<IActionResult> RemoveInvestmentDoctor(InvestmentDoctor investmentDoctorDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentDoctorSpecification(investmentDoctorDto.InvestmentInitId);
                var alreadyExistInvestmentDoctorList = await _investmentDoctorRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentDoctorList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentDoctorList)
                    {
                        _investmentDoctorRepo.Delete(v);
                        _investmentDoctorRepo.Savechange();
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
        #endregion

        #region investmentInstitution

        [HttpPost("IsInstitutionInvestmentApprovalPending")]
        public async Task<int> IsInstitutionInvestmentApprovalPending(int initId, int institutionId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentInstitution d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE   i.DataStatus=1 AND " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE I.Id = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                " AND  NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentInstitution  " +
                " WHERE InvestmentInitId = " + initId + " " +
                " AND InstitutionId = " + institutionId + " " +
                " ) " +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.InstitutionId = " + institutionId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }

        [HttpPost("insertInvestmentInstitution")]
        public async Task<ActionResult<InvestmentInstitutionDto>> InsertInvestmentInstitution(InvestmentInstitution investmentInstitutionDto)
        {
            try
            {
                //if (await IsInstitutionInvestmentApprovalPending(investmentInstitutionDto.InvestmentInitId, investmentInstitutionDto.InstitutionId) > 0)
                //{
                //    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Institution!"));
                //}
                var alreadyExistSpec = new InvestmentInstitutionSpecification(investmentInstitutionDto.InvestmentInitId);
                var alreadyExistInvestmentInstitutionList = await _investmentInstitutionRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentInstitutionList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentInstitutionList)
                    {
                        _investmentInstitutionRepo.Delete(v);
                        _investmentInstitutionRepo.Savechange();
                    }
                }
                var investmentInstitution = new InvestmentInstitution
                {
                    //ReferenceNo = investmentInstitutionDto.ReferenceNo,
                    InvestmentInitId = investmentInstitutionDto.InvestmentInitId,
                    InstitutionId = investmentInstitutionDto.InstitutionId,
                    ResponsibleDoctorId = investmentInstitutionDto.ResponsibleDoctorId,
                    NoOfBed = investmentInstitutionDto.NoOfBed,
                    DepartmentUnit = investmentInstitutionDto.DepartmentUnit,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentInstitutionRepo.Add(investmentInstitution);
                _investmentInstitutionRepo.Savechange();

                return new InvestmentInstitutionDto
                {
                    Id = investmentInstitution.Id,
                    InvestmentInitId = investmentInstitutionDto.InvestmentInitId,
                    InstitutionId = investmentInstitutionDto.InstitutionId,
                    ResponsibleDoctorId = investmentInstitutionDto.ResponsibleDoctorId,
                    NoOfBed = investmentInstitutionDto.NoOfBed,
                    DepartmentUnit = investmentInstitutionDto.DepartmentUnit,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentInstitutions/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentInstitution>> GetInvestmentInstitutions(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentInstitutionSpecification(investmentInitId);
                var investmentInstitution = await _investmentInstitutionRepo.ListAsync(spec);
                return investmentInstitution;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentInstitution")]
        public async Task<IActionResult> RemoveInvestmentInstitution(InvestmentInstitutionDto investmentInstitutionDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentInstitutionSpecification(investmentInstitutionDto.InvestmentInitId);
                var alreadyExistInvestmentInstitutionList = await _investmentInstitutionRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentInstitutionList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentInstitutionList)
                    {
                        _investmentInstitutionRepo.Delete(v);
                        _investmentInstitutionRepo.Savechange();
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

        #endregion

        #region investmentCampaign

        [HttpPost("IsCampaignInvestmentApprovalPending")]
        public async Task<int> IsCampaignInvestmentApprovalPending(int initId, int campaignDtlId, int doctorId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentCampaign d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE   i.DataStatus=1 AND " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE I.Id = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                " AND NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentCampaign  " +
                " WHERE InvestmentInitId = " + initId + " " +
                " AND CampaignDtlId = " + campaignDtlId + " " +
                " ) " +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.DoctorId = " + doctorId + " " +
                " AND d.CampaignDtlId = " + campaignDtlId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }


        [HttpPost("insertInvestmentCampaign")]
        public async Task<ActionResult<InvestmentCampaignDto>> InsertInvestmentCampaign(InvestmentCampaign investmentCampaignDto)
        {
            try
            {
                //if (await IsCampaignInvestmentApprovalPending(investmentCampaignDto.InvestmentInitId, investmentCampaignDto.CampaignDtlId, investmentCampaignDto.DoctorId) > 0)
                //{
                //    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Campaign!"));
                //}
                var alreadyExistSpec = new InvestmentCampaignSpecification(investmentCampaignDto.InvestmentInitId);
                var alreadyExistInvestmentCampaignList = await _investmentCampaignRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentCampaignList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentCampaignList)
                    {
                        _investmentCampaignRepo.Delete(v);
                        _investmentCampaignRepo.Savechange();
                    }
                }

                var investmentCampaign = new InvestmentCampaign
                {
                    //ReferenceNo = investmentCampaignDto.ReferenceNo,
                    InvestmentInitId = investmentCampaignDto.InvestmentInitId,
                    CampaignDtlId = investmentCampaignDto.CampaignDtlId,
                    DoctorId = investmentCampaignDto.DoctorId,
                    InstitutionId = investmentCampaignDto.InstitutionId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentCampaignRepo.Add(investmentCampaign);
                _investmentCampaignRepo.Savechange();

                return new InvestmentCampaignDto
                {
                    Id = investmentCampaign.Id,
                    InvestmentInitId = investmentCampaignDto.InvestmentInitId,
                    CampaignDtlId = investmentCampaignDto.CampaignDtlId,
                    DoctorId = investmentCampaignDto.DoctorId,
                    InstitutionId = investmentCampaignDto.InstitutionId
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentCampaigns/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentCampaign>> GetInvestmentCampaigns(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentCampaignSpecification(investmentInitId);
                var investmentCampaign = await _investmentCampaignRepo.ListAsync(spec);
                return investmentCampaign;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentCampaign")]
        public async Task<IActionResult> RemoveInvestmentCampaign(InvestmentCampaignDto investmentCampaignDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentCampaignSpecification(investmentCampaignDto.InvestmentInitId);
                var alreadyExistInvestmentCampaignList = await _investmentCampaignRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentCampaignList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentCampaignList)
                    {
                        _investmentCampaignRepo.Delete(v);
                        _investmentCampaignRepo.Savechange();
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
        #endregion

        #region investmentBcds

        [HttpPost("IsBcdsInvestmentApprovalPending")]
        public async Task<int> IsBcdsInvestmentApprovalPending(int initId, int bcdsId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);

            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentBcds d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE    i.DataStatus=1 AND " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE I.Id = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                " AND  NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentBcds  " +
                " WHERE InvestmentInitId = " + initId + "  " +
                " ) " +
                " AND BcdsId = " + bcdsId + " " +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.BcdsId = " + bcdsId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }

        [HttpPost("insertInvestmentBcds")]
        public async Task<ActionResult<InvestmentBcdsDto>> InsertInvestmentBcds(InvestmentBcds investmentBcdsDto)
        {
            try
            {
                //if (await IsBcdsInvestmentApprovalPending(investmentBcdsDto.InvestmentInitId, investmentBcdsDto.BcdsId) > 0)
                //{
                //    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Bcds!"));
                //}
                var alreadyExistSpec = new InvestmentBcdsSpecification(investmentBcdsDto.InvestmentInitId);
                var alreadyExistInvestmentBcdsList = await _investmentBcdsRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentBcdsList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentBcdsList)
                    {
                        _investmentBcdsRepo.Delete(v);
                        _investmentBcdsRepo.Savechange();
                    }
                }

                var investmentBcds = new InvestmentBcds
                {
                    //ReferenceNo = investmentBcdsDto.ReferenceNo,
                    InvestmentInitId = investmentBcdsDto.InvestmentInitId,
                    BcdsId = investmentBcdsDto.BcdsId,
                    ResponsibleDoctorId = investmentBcdsDto.ResponsibleDoctorId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentBcdsRepo.Add(investmentBcds);
                _investmentBcdsRepo.Savechange();

                return new InvestmentBcdsDto
                {
                    Id = investmentBcds.Id,
                    InvestmentInitId = investmentBcdsDto.InvestmentInitId,
                    ResponsibleDoctorId = investmentBcdsDto.ResponsibleDoctorId,
                    BcdsId = investmentBcdsDto.BcdsId,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentBcds/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentBcds>> GetInvestmentBcds(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentBcdsSpecification(investmentInitId);
                var investmentBcds = await _investmentBcdsRepo.ListAsync(spec);
                return investmentBcds;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentBcds")]
        public async Task<IActionResult> RemoveInvestmentBcds(InvestmentBcdsDto investmentBcdsDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentBcdsSpecification(investmentBcdsDto.InvestmentInitId);
                var alreadyExistInvestmentBcdsList = await _investmentBcdsRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentBcdsList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentBcdsList)
                    {
                        _investmentBcdsRepo.Delete(v);
                        _investmentBcdsRepo.Savechange();
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
        #endregion

        #region investmentSociety

        [HttpPost("IsSocietyInvestmentApprovalPending")]
        public async Task<int> IsSocietyInvestmentApprovalPending(int initId, int societyId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentSociety d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE   i.DataStatus=1 AND " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE IT.InvestmentInitId = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                " AND  NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentSociety  " +
                " WHERE InvestmentInitId = " + initId + " " +
                " ) " +
                " AND SocietyId = " + societyId + " " +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.SocietyId = " + societyId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }
        [HttpPost("insertInvestmentSociety")]
        public async Task<ActionResult<InvestmentSocietyDto>> InsertInvestmentSociety(InvestmentSociety investmentSocietyDto)
        {
            try
            {
                //if (await IsSocietyInvestmentApprovalPending(investmentSocietyDto.InvestmentInitId, investmentSocietyDto.SocietyId) > 0)
                //{
                //    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Society!"));
                //}
                var alreadyExistSpec = new InvestmentSocietySpecification(investmentSocietyDto.InvestmentInitId);
                var alreadyExistInvestmentSocietyList = await _investmentSocietyRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentSocietyList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentSocietyList)
                    {
                        _investmentSocietyRepo.Delete(v);
                        _investmentSocietyRepo.Savechange();
                    }
                }

                var investmentSociety = new InvestmentSociety
                {
                    //ReferenceNo = investmentSocietyDto.ReferenceNo,
                    InvestmentInitId = investmentSocietyDto.InvestmentInitId,
                    SocietyId = investmentSocietyDto.SocietyId,
                    ResponsibleDoctorId = investmentSocietyDto.ResponsibleDoctorId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentSocietyRepo.Add(investmentSociety);
                _investmentSocietyRepo.Savechange();

                return new InvestmentSocietyDto
                {
                    Id = investmentSociety.Id,
                    InvestmentInitId = investmentSocietyDto.InvestmentInitId,
                    SocietyId = investmentSocietyDto.SocietyId,
                    ResponsibleDoctorId = investmentSocietyDto.ResponsibleDoctorId,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentSociety/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentSociety>> GetInvestmentSociety(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentSocietySpecification(investmentInitId);
                var investmentSociety = await _investmentSocietyRepo.ListAsync(spec);
                return investmentSociety;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentSociety")]
        public async Task<IActionResult> RemoveInvestmentSociety(InvestmentSociety investmentSociety)
        {
            try
            {
                var alreadyExistSpec = new InvestmentSocietySpecification(investmentSociety.InvestmentInitId);
                var alreadyExistInvestmentSocietyList = await _investmentSocietyRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentSocietyList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentSocietyList)
                    {
                        _investmentSocietyRepo.Delete(v);
                        _investmentSocietyRepo.Savechange();
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
        #endregion 
        #region investmentOther

        [HttpPost("IsOtherInvestmentApprovalPending")]
        public async Task<int> IsOtherInvestmentApprovalPending(int initId, int OtherId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentOther d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE   i.DataStatus=1 AND " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE IT.InvestmentInitId = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                " AND  NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentOther  " +
                " WHERE InvestmentInitId = " + initId + " " +
                " ) " +
                " AND OtherId = " + OtherId + " " +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.OtherId = " + OtherId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }
        [HttpPost("insertInvestmentOther")]
        public async Task<ActionResult<InvestmentOther>> InsertInvestmentOther(InvestmentOther investmentOtherDto)
        {
            try
            {
                
                var alreadyExistSpec = new InvestmentOtherSpecification(investmentOtherDto.InvestmentInitId);
                var alreadyExistInvestmentOtherList = await _investmentOtherRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentOtherList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentOtherList)
                    {
                        _investmentOtherRepo.Delete(v);
                        _investmentOtherRepo.Savechange();
                    }
                }

                var investmentOther = new InvestmentOther
                {
                    //ReferenceNo = investmentOtherDto.ReferenceNo,
                    InvestmentInitId = investmentOtherDto.InvestmentInitId,
                    Name = investmentOtherDto.Name,
                    Address = investmentOtherDto.Address,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentOtherRepo.Add(investmentOther);
                _investmentOtherRepo.Savechange();

                return new InvestmentOther
                {
                    Id = investmentOther.Id,
                    InvestmentInitId = investmentOtherDto.InvestmentInitId,
                    Name = investmentOtherDto.Name,
                    Address = investmentOtherDto.Address,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentOther/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentOther>> GetInvestmentOther(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentOtherSpecification(investmentInitId);
                var investmentOther = await _investmentOtherRepo.ListAsync(spec);
                return investmentOther;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentOther")]
        public async Task<IActionResult> RemoveInvestmentOther(InvestmentOther investmentOther)
        {
            try
            {
                var alreadyExistSpec = new InvestmentOtherSpecification(investmentOther.InvestmentInitId);
                var alreadyExistInvestmentOtherList = await _investmentOtherRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentOtherList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentOtherList)
                    {
                        _investmentOtherRepo.Delete(v);
                        _investmentOtherRepo.Savechange();
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
        #endregion


    }
}
