using API.Dtos;
using API.Errors;
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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentAprNoSbuController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductRepo;
        private readonly IGenericRepository<InvestmentRecDepot> _investmentRecDepotRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentApr> _investmentAprRepo;
        private readonly IGenericRepository<InvestmentAprComment> _investmentAprCommentRepo;
        private readonly IGenericRepository<InvestmentAprProducts> _investmentAprProductRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;
        private readonly IGenericRepository<SBUWiseBudget> _sbuRepo;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IGenericRepository<ApprovalCeiling> _approvalCeilingRepo;
        private readonly IGenericRepository<InvestmentDetailTracker> _investmentDetailTrackerRepo;
        private readonly IGenericRepository<Donation> _donationRepo;

        public InvestmentAprNoSbuController(IGenericRepository<ApprovalCeiling> approvalCeilingRepo,
            IGenericRepository<ApprAuthConfig> apprAuthConfigRepo,
            IGenericRepository<SBUWiseBudget> sbuRepo,
            IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo,
            IGenericRepository<InvestmentInit> investmentInitRepo,
            IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
            IGenericRepository<InvestmentRecDepot> investmentRecDepotRepo,
            IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
            IGenericRepository<InvestmentApr> investmentAprRepo,
            IGenericRepository<InvestmentAprComment> investmentAprCommentRepo,
            IGenericRepository<InvestmentAprProducts> investmentAprProductRepo,
            IGenericRepository<Employee> employeeRepo,
            IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
            StoreContext dbContext,
            IGenericRepository<InvestmentRec> investmentRecRepo,
            IGenericRepository<InvestmentDetailTracker> investmentDetailTrackerRepo,
            IGenericRepository<Donation> donationRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _investmentAprRepo = investmentAprRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentRecProductRepo = investmentRecProductRepo;
            _investmentRecDepotRepo = investmentRecDepotRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _investmentAprCommentRepo = investmentAprCommentRepo;
            _investmentAprProductRepo = investmentAprProductRepo;
            _reportInvestmentInfoRepo = reportInvestmentInfoRepo;
            _employeeRepo = employeeRepo;
            _dbContext = dbContext;
            _investmentTargetedGroupRepo = investmentTargetedGroupRepo;
            _sbuRepo = sbuRepo;
            _apprAuthConfigRepo = apprAuthConfigRepo;
            _approvalCeilingRepo = approvalCeilingRepo;
            _investmentRecRepo = investmentRecRepo;
            _investmentDetailTrackerRepo = investmentDetailTrackerRepo;
            _donationRepo = donationRepo;
        }
        [HttpGet("investmentInits/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,
        [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@EID", empId),
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchNoSbu @EID", parms.ToArray()).ToList();
                var data = _mapper.Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [HttpGet("investmentApproved/{empId}/{sbu}/{userRole}")]
        public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetinvestmentApproved(int empId, string sbu, string userRole,
        [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                if (userRole == "Administrator")
                {

                    var investmentInits = await _investmentInitRepo.ListAllAsync();
                    var investmentRecComments = await _investmentRecCommentRepo.ListAllAsync();
                    //var investmentAprComments = await _investmentAprCommentRepo.ListAllAsync();
                    var investmentInitFormRec = (from i in investmentInits
                                                 join rc in investmentRecComments on i.Id equals rc.InvestmentInitId
                                                 where rc.RecStatus == "Approved"
                                                 orderby i.ReferenceNo
                                                 select new InvestmentInitDto
                                                 {
                                                     Id = i.Id,
                                                     ReferenceNo = i.ReferenceNo,
                                                     ProposeFor = i.ProposeFor,
                                                     DonationId = i.DonationId,
                                                     MarketCode = i.MarketCode,
                                                     MarketName = i.MarketName,
                                                     DonationTo = i.DonationTo,
                                                     EmployeeId = i.EmployeeId,
                                                     SetOn = i.SetOn
                                                 }
                                                    ).Distinct().OrderByDescending(x => x.SetOn).ToList();
                    return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, investmentInitFormRec.Count(), investmentInitFormRec));
                }

                else
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        //new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                      //  new SqlParameter("@RSTATUS", "Recommended"),
                        //new SqlParameter("@ASTATUS", DBNull.Value)
                    };
                    var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentApprpvedSearchNoSbu @EID", parms.ToArray()).ToList();
                    var data = _mapper
                        .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                    var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                    var totalItems = await _investmentInitRepo.CountAsync(countSpec);
                    return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, results.Count(), data));
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [HttpPost("InsertApr/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentAprDto>> InsertInvestmentApr(int empId, string aprStatus, string sbu, int donationId, InvestmentAprDto investmentAprDto)
        {
            try
            {
                if (aprStatus == "Approved")
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmentAprDto.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmentAprDto.ProposedAmount),
                        new SqlParameter("@ASTATUS", aprStatus),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheck @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                    //var param=parms[6].Value;
                    if (parms[6].Value.ToString() != "True")
                    //if (result.Result == 0)
                    {
                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                    }


                    var alreadyExistSpec = new InvestmentRecSpecification(investmentAprDto.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprList)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }
                    var specAppr = new ApprAuthConfigSpecification(empId, "A");
                    var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                    var invRecAppr = new InvestmentRec
                    {
                        //ReferenceNo = investmentInitDto.ReferenceNo,
                        InvestmentInitId = investmentAprDto.InvestmentInitId,
                        ProposedAmount = investmentAprDto.ProposedAmount,
                        Purpose = investmentAprDto.Purpose,
                        CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                        FromDate = investmentAprDto.FromDate,
                        ToDate = investmentAprDto.ToDate,
                        TotalMonth = investmentAprDto.TotalMonth,
                        PaymentMethod = investmentAprDto.PaymentMethod,
                        ChequeTitle = investmentAprDto.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfigAppr.ApprovalAuthority.Priority,
                        //Priority = 3,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRecAppr);
                    _investmentRecRepo.Savechange();

                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmentAprDto.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }

                    var donation = await _donationRepo.GetByIdAsync(donationId);
                    if (donation.DonationTypeName == "Honorarium")
                    {
                        DateTimeOffset calcDate = investmentAprDto.FromDate;
                        for (int i = 0; i < investmentAprDto.TotalMonth; i++)
                        {
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentAprDto.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentAprDto.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentAprDto.FromDate,
                                ToDate = investmentAprDto.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else
                    {
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmentAprDto.InvestmentInitId,
                            DonationId = donationId,
                            ApprovedAmount = investmentAprDto.ProposedAmount,
                            Month = 0,
                            Year = 0,
                            FromDate = investmentAprDto.FromDate,
                            ToDate = investmentAprDto.ToDate,
                            PaidStatus = "Paid",
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentDetailTrackerRepo.Add(invDT);
                        _investmentDetailTrackerRepo.Savechange();
                    }

                    return new InvestmentAprDto
                    {
                        Id = invRecAppr.Id,
                        InvestmentInitId = investmentAprDto.InvestmentInitId,
                        ProposedAmount = investmentAprDto.ProposedAmount,
                        Purpose = investmentAprDto.Purpose,
                        CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                        FromDate = investmentAprDto.FromDate,
                        ToDate = investmentAprDto.ToDate,
                        TotalMonth = investmentAprDto.TotalMonth,
                        PaymentMethod = investmentAprDto.PaymentMethod,
                        ChequeTitle = investmentAprDto.ChequeTitle,
                        EmployeeId = investmentAprDto.EmployeeId,
                    };
                }
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                var invRec = new InvestmentRec
                {
                    //ReferenceNo = investmentInitDto.ReferenceNo,
                    InvestmentInitId = investmentAprDto.InvestmentInitId,
                    ProposedAmount = investmentAprDto.ProposedAmount,
                    Purpose = investmentAprDto.Purpose,
                    CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                    FromDate = investmentAprDto.FromDate,
                    ToDate = investmentAprDto.ToDate,
                    TotalMonth = investmentAprDto.TotalMonth,
                    PaymentMethod = investmentAprDto.PaymentMethod,
                    ChequeTitle = investmentAprDto.ChequeTitle,
                    EmployeeId = empId,
                    Priority = apprAuthConfig.ApprovalAuthority.Priority,
                    CompletionStatus = true,
                    SetOn = DateTimeOffset.Now
                };
                _investmentRecRepo.Add(invRec);
                _investmentRecRepo.Savechange();

                return new InvestmentAprDto
                {
                    Id = invRec.Id,
                    InvestmentInitId = investmentAprDto.InvestmentInitId,
                    ProposedAmount = investmentAprDto.ProposedAmount,
                    Purpose = investmentAprDto.Purpose,
                    CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                    FromDate = investmentAprDto.FromDate,
                    ToDate = investmentAprDto.ToDate,
                    TotalMonth = investmentAprDto.TotalMonth,
                    PaymentMethod = investmentAprDto.PaymentMethod,
                    ChequeTitle = investmentAprDto.ChequeTitle,
                    EmployeeId = investmentAprDto.EmployeeId,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("InsertAprCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprComment(InvestmentRecCommentDto investmentRecDto)
        {
            var isComplete = false;
            //var investmentInitSpec = new InvestmentInitSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            bool isTrue = false;
            var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
            var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
            isComplete = true;
            if (investmentRecDto.RecStatus == "Not Approved")
            {
                isComplete = false;
            }
            foreach (var v in investmentTargetedGroup)
            {
                isTrue = false;
                foreach (var i in investmentRecComments)
                {
                    if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
                    {
                        isTrue = true;
                    }
                }
                if (!isTrue)
                {
                    return BadRequest(new ApiResponse(400, "Other recommendation has not completed yet"));
                }
            }
            var invRec = new InvestmentRecComment
            {
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
                MarketGroupCode = empData.MarketGroupCode,
                MarketGroupName = empData.MarketGroupName,
                MarketCode = empData.MarketCode,
                MarketName = empData.MarketName,
                RegionCode = empData.RegionCode,
                RegionName = empData.RegionName,
                ZoneCode = empData.ZoneCode,
                ZoneName = empData.ZoneName,
                TerritoryCode = empData.TerritoryCode,
                TerritoryName = empData.TerritoryName,
                SBUName = empData.SBUName,
                SBU = empData.SBU,
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = isComplete,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecCommentRepo.Add(invRec);
            _investmentRecCommentRepo.Savechange();

            return new InvestmentRecCommentDto
            {
                Id = invRec.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
            };
        }

        [HttpPost("UpdateAprCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprComment(InvestmentRecCommentDto investmentRecDto)
        {
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var isComplete = false;
            bool isTrue = false;
            var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
            var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
            isComplete = true;
            if (investmentRecDto.RecStatus == "Not Approved")
            {
                isComplete = false;
            }
            foreach (var v in investmentTargetedGroup)
            {
                isTrue = false;
                foreach (var i in investmentRecComments)
                {
                    if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
                    {
                        isTrue = true;
                    }
                }
                if (!isTrue) { return BadRequest(new ApiResponse(400, "Other recommendation not completed yet")); }
            }


            var invRec = new InvestmentRecComment
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
                MarketGroupCode = empData.MarketGroupCode,
                MarketGroupName = empData.MarketGroupName,
                MarketCode = empData.MarketCode,
                MarketName = empData.MarketName,
                RegionCode = empData.RegionCode,
                RegionName = empData.RegionName,
                ZoneCode = empData.ZoneCode,
                ZoneName = empData.ZoneName,
                TerritoryCode = empData.TerritoryCode,
                TerritoryName = empData.TerritoryName,
                SBUName = empData.SBUName,
                SBU = empData.SBU,
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = isComplete,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecCommentRepo.Update(invRec);
            _investmentRecCommentRepo.Savechange();

            return new InvestmentRecCommentDto
            {
                Id = investmentRecDto.Id,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
            };
        }

        [HttpPost("InsertAprProd")]
        public async Task<IActionResult> InsertInvestmentApromendationProduct(List<InvestmentRecProductsDto> investmentRecProductDto)
        {
            try
            {
                foreach (var i in investmentRecProductDto)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification(i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecProductList)
                        {
                            _investmentRecProductRepo.Delete(v);
                            _investmentRecProductRepo.Savechange();
                        }
                    }
                    else
                    {
                        var investmentRecProduct = new InvestmentRecProducts
                        {
                            //ReferenceNo = investmentRecDto.ReferenceNo,
                            InvestmentInitId = i.InvestmentInitId,
                            ProductId = i.ProductId,
                            EmployeeId = i.EmployeeId,
                            SBU = i.ProductInfo.SBU,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecProductRepo.Add(investmentRecProduct);
                    }
                }
                _investmentRecProductRepo.Savechange();
                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("insertInvestmentRecDepot")]
        public async Task<ActionResult<InvestmentRecDepot>> InsertInvestmentRecDepot(InvestmentRecDepot investmentRecDepot)
        {
            try
            {
                var alreadyExistSpec = new InvestmentRecDepotSpecification(investmentRecDepot.InvestmentInitId, investmentRecDepot.DepotCode);
                var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentRecDepotList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentRecDepotList)
                    {
                        _investmentRecDepotRepo.Delete(v);
                        _investmentRecDepotRepo.Savechange();
                    }
                }
                var invRecDepot = new InvestmentRecDepot
                {
                    //ReferenceNo = investmentRecDto.ReferenceNo,
                    InvestmentInitId = investmentRecDepot.InvestmentInitId,
                    DepotCode = investmentRecDepot.DepotCode,
                    DepotName = investmentRecDepot.DepotName,
                    EmployeeId = investmentRecDepot.EmployeeId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentRecDepotRepo.Add(invRecDepot);
                _investmentRecProductRepo.Savechange();

                return invRecDepot;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("getInvestmentRecDepot/{initId}")]
        public async Task<ActionResult<InvestmentRecDepot>> GetInvestmentRecDepot(int initId)
        {
            try
            {
                var alreadyExistSpec = new InvestmentRecDepotSpecification(initId);
                var alreadyExistInvestmentRecDepot = await _investmentRecDepotRepo.GetEntityWithSpec(alreadyExistSpec);

                return alreadyExistInvestmentRecDepot;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentTargetedProd")]
        public async Task<IActionResult> RemoveInvestmentTargetedProd(InvestmentTargetedProd investmentTargetedProd)
        {
            try
            {
                //var response = new HttpResponseMessage();
                var alreadyExistSpec = new InvestmentRecProductSpecification(investmentTargetedProd.InvestmentInitId, investmentTargetedProd.ProductId);
                var alreadyExistInvestmentTargetedProdList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentTargetedProdList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentTargetedProdList)
                    {
                        _investmentRecProductRepo.Delete(v);
                        _investmentRecProductRepo.Savechange();
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

        [HttpGet]
        [Route("investmentAprProducts/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecProducts(int investmentInitId, string sbu)
        {
            try
            {
                //var spec = new InvestmentRecProductSpecification(investmentInitId, sbu);
                var spec = new InvestmentRecProductSpecification(investmentInitId);
                var investmentTargetedProd = await _investmentRecProductRepo.ListAsync(spec);
                return investmentTargetedProd;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentAprDetails/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRec>> GetInvestmentDetails(int investmentInitId, int empId)
        {
            try
            {
                var spec = new InvestmentRecSpecification(investmentInitId, empId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getInvestmentAprComment/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecComment(int investmentInitId, int empId)
        {
            try
            {
                var spec = new InvestmentRecCommentSpecification(investmentInitId, empId);
                var investmentDetail = await _investmentRecCommentRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getInvestmentAprComments/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecComments(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentRecCommentSpecification(investmentInitId);
                var investmentDetail = await _investmentRecCommentRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [Route("getEmpMarket/{investmentInitId}")]
        public async Task<object> GetEmpLocation(int investmentInitId)
        {
            try
            {
                string qry= " select CAST(a.EmployeeId AS INT) AS Id, a.SetOn, a.ModifiedOn, a.DataStatus, a.MarketCode,  a.MarketName, " +
                " a.TerritoryCode, a.TerritoryName, a.RegionCode, a.RegionName, a.ZoneCode, a.ZoneName, dbo.fnGetEmpNamedesig(a.EmployeeId) EmployeeName, a.[Priority]  " +
                " from InvestmentRecComment a inner join InvestmentInit b on b.Id = a.InvestmentInitId " +
                " where InvestmentInitId = '"+ investmentInitId +"' " +
                " UNION " +
                " select CAST(a.EmployeeId AS INT) AS Id, a.SetOn, a.ModifiedOn, a.DataStatus, a.MarketCode,  a.MarketName,  " +
                " a.TerritoryCode, a.TerritoryName, a.RegionCode, a.RegionName, a.ZoneCode, a.ZoneName, dbo.fnGetEmpNamedesig(a.EmployeeId) EmployeeName, '1' " +
                " From InvestmentInit a where a.Id = '"+ investmentInitId +"' AND a.Confirmation = 1 " +
                " UNION " +
                " select CAST(a.Id AS INT) AS Id, a.SetOn, a.ModifiedOn, a.DataStatus, a.MarketCode,  a.MarketName,  a.TerritoryCode, a.TerritoryName, a.RegionCode, a.RegionName, a.ZoneCode, a.ZoneName, dbo.fnGetEmpNamedesigByMarket(a.MarketCode) EmployeeName, '1' "+
                " from InvestmentTargetedGroup a " +
                " where a.Id = '"+ investmentInitId +"' "+
                " order by a.[Priority] desc ";

                var spec = await _dbContext.EmployeeLocation.FromSqlRaw(qry).ToListAsync();               
                return spec;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}