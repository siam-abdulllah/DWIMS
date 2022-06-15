﻿using API.Dtos;
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
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;


        public InvestmentRapidController(IMapper mapper, IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentRapid> investmentRapidRepo,
            IGenericRepository<InvestmentRapidAppr> InvestmentRapidApprRepo, IGenericRepository<InvestmentDetailTracker> investmentDetailTrackerRepo,
        IGenericRepository<MedicineProduct> medicineProductRepo, IGenericRepository<InvestmentRecDepot> investmentRecDepotRepo, IGenericRepository<InvestmentMedicineProd> investmentMedicineProdRepo,
        IGenericRepository<InvestmentCampaign> investmentCampaignRepo, IGenericRepository<InvestmentRecProducts> investmentRecProductsRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
        IGenericRepository<ProductInfo> productInfoRepo, IGenericRepository<ApprAuthConfig> appAuthConfigRepo, IGenericRepository<Employee> employeeRepo,
        IGenericRepository<ApprovalAuthority> approvalAuthorityRepo, IGenericRepository<InvestmentRec> investmentRecRepo, StoreContext dbContext,
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
        }

        [HttpPost("saveInvestmentRapid")]
        public async Task<ActionResult<InvestmentRapidDto>> saveInvestmentRapid(InvestmentRapidDto investmentRapidDto)
        {
            try
            {
                int ApprovedBy = investmentRapidDto.ApprovalAuthId;

                InvestmentInit investmentInit = new InvestmentInit();
                ApprovalAuthority appAuth = new ApprovalAuthority();
                Employee emp = new Employee();
                ApprAuthConfig authConfig = new ApprAuthConfig();
                investmentInit = await _investmentInitRepo.GetByIdAsync(investmentRapidDto.InvestmentInitId);


                InvestmentRapid investmentForm = new InvestmentRapid();
                InvestmentRapidAppr invRapidApr = new InvestmentRapidAppr();
                investmentForm = await _investmentRapidRepo.GetByIdAsync(investmentRapidDto.Id);

                #region Get Auth Id
                //int ApprovedBy = investmentRapidDto.ApprovalAuthId;
                //authConfig = new ApprAuthConfigSpecification(ApprovedBy, "A");
                var spec = new ApprAuthConfigSpecification(ApprovedBy, "A");
                 authConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                //if (authConfig != null)
                //{
                // appAuth = await _approvalAuthorityRepo.GetByIdAsync(authConfig.ApprovalAuthorityId);
                // investmentRapidDto.ApprovalAuthId = appAuth.Id;
                //}
                if (authConfig == null)
                {
                    investmentRapidDto.ApprovalAuthId = 0;
                }
                emp = await _employeeRepo.GetByIdAsync(ApprovedBy);
                #endregion

                #region Investment Rapid Approved
                //InvestmentRapidAppr invRapidApr = new InvestmentRapidAppr();
                if (!string.IsNullOrEmpty(investmentRapidDto.ApprovedStatus))
                {
                    if (investmentRapidDto.ApprovedStatus == "Approved")
                    {
                        if (investmentRapidDto.SubCampaignId == 0)
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
                                    new SqlParameter("@CDTLID", investmentRapidDto.SubCampaignId),
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
                            EmployeeId = ApprovedBy,
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
                    string qry = string.Format(@"select * from InvestmentRapidAppr where InvestmentRapidId = {0}", investmentForm.Id);
                    invRapidApr = _dbContext.InvestmentRapidAppr.FromSqlRaw(qry).ToList().FirstOrDefault();

                    invRapidApr.InvestmentInitId = investmentInit.Id;
                    invRapidApr.InvestmentRapidId = investmentForm.Id;

                    invRapidApr.ApprovalRemarks = investmentRapidDto.Approval;
                    invRapidApr.ApprovedStatus = investmentRapidDto.ApprovedStatus;
                    invRapidApr.ModifiedOn = DateTime.Now;
                    _InvestmentRapidApprRepo.Update(invRapidApr);

                    _InvestmentRapidApprRepo.Savechange();
                }
                #endregion



                #region Save On Investment Init
               
                if (investmentInit != null)
                {
                    //investmentInit.ProposeFor = investmentRapidDto.ProposeFor;
                    //investmentInit.DonationTo = investmentRapidDto.DonationTo;
                    investmentInit.EmployeeId = investmentRapidDto.InitiatorId;
                    investmentInit.SBUName = investmentRapidDto.SbuName;
                    investmentInit.SubmissionDate = DateTime.Now;
                    investmentInit.SBU = investmentRapidDto.SBU;
                    investmentInit.Confirmation = true;
                    investmentInit.SetOn = DateTimeOffset.Now;
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

                    if (investmentRapidDto.SubCampaignId != 0)
                    {

                        var investmentCampaign = new InvestmentCampaign
                        {
                            //ReferenceNo = investmentCampaignDto.ReferenceNo,
                            InvestmentInitId = investmentInit.Id,
                            CampaignDtlId = investmentRapidDto.SubCampaignId,
                            DoctorId = 90000,
                            InstitutionId = 99999,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentCampaignRepo.Add(investmentCampaign);
                        _investmentCampaignRepo.Savechange();

                    }
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


                        invRapidApr.ApprovalAuthId = investmentRapidDto.ApprovalAuthId;
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

                    invRapidApr.ApprovalAuthId = investmentRapidDto.ApprovalAuthId;
                    invRapidApr.SetOn = investmentForm.SetOn;
                    invRapidApr.ModifiedOn = DateTime.Now;
                    _InvestmentRapidApprRepo.Add(invRapidApr);
                    _InvestmentRapidApprRepo.Savechange();

                }


                #endregion
                #region Insert Medicine Product
                InvestmentMedicineProd invMedicineProd = new InvestmentMedicineProd();
                if (investmentRapidDto.investmentMedicineProd != null && investmentRapidDto.investmentMedicineProd.Count > 0)
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
                    foreach (var item in investmentRapidDto.investmentMedicineProd)
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
                if (investmentRapidDto.investmentRecProducts != null && investmentRapidDto.investmentRecProducts.Count > 0)
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
                    foreach (var item in investmentRapidDto.investmentRecProducts)
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

        [HttpGet("getEmployeesforRapidByDpt/{proposeFor}/{sbu}")]
        public IReadOnlyList<Employee> GetEmployeesforRapidByDpt(string proposeFor, string sbu)
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
            if (!string.IsNullOrEmpty(sbu) && sbu!="null")
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
                                        AND esm.DataStatus=1 {0} {1}", deptId, sbuQry);
                empList = _dbContext.Employee.FromSqlRaw(qry).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return empList;
        }
        [HttpGet("getEmployeesforRapidBySBU/{proposeFor}/{sbu}")]
        public IReadOnlyList<Employee> GetEmployeesforRapidBySBU(string proposeFor, string sbu)
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

            if (!string.IsNullOrEmpty(sbu) && sbu!="null")
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
        [HttpGet("getEmployeesforRapidByCamp/{subCampaignId}")]
        public IReadOnlyList<Employee> GetEmployeesforRapidByCamp(string subCampaignId)
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
                string statusQuery = "";
                if (For == "reference")
                {
                    statusQuery = " and IRA.ApprovedStatus is  null and IT.PaymentRefNo is null";
                }
                else if (For == "search")
                {
                    statusQuery = " and IRA.ApprovedStatus is not null";
                }
                string qry = "";
                if (from == "init")
                {
                    qry = string.Format(@" Select  IR.*,IRA.ApprovedBy ApprovalAuthId,d.DonationTypeName,IRA.ApprovedStatus,IRA.ApprovalRemarks as Approval  from InvestmentRapid IR 
                                        left join Donation d on IR.Type = d.Id
                                        left join InvestmentRapidAppr IRA on IR.Id = IRA.InvestmentRapidId
                                        where  IR.InitiatorId={0} and IRA.ApprovedStatus is  null or IRA.ApprovedStatus ='Pending'", employeeId);

                }
                else
                {
                    qry = string.Format(@" 	Select  IR.*,IRA.ApprovedBy ApprovalAuthId,IT.PaymentRefNo,d.DonationTypeName,IRA.ApprovedStatus,IRA.ApprovalRemarks as Approval  from InvestmentRapid IR 
                                        left join Donation d on IR.Type = d.Id
                                        left join InvestmentRapidAppr IRA on IR.Id = IRA.InvestmentRapidId
									    left join InvestmentDetailTracker IT on IT.InvestmentInitId = IR.InvestmentInitId
                                        where  IRA.ApprovedBy={0}{1}", employeeId, statusQuery);
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
    }
}
