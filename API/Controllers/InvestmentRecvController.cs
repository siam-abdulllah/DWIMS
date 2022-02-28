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
    public class InvestmentRecvController : BaseApiController
    {

        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentRecv> _investmentRecvRepo;
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

        public InvestmentRecvController(IGenericRepository<ApprovalCeiling> approvalCeilingRepo,
            IGenericRepository<ApprAuthConfig> apprAuthConfigRepo,
            IGenericRepository<SBUWiseBudget> sbuRepo,
            IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo,
            IGenericRepository<InvestmentInit> investmentInitRepo,
            IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
            IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
            IGenericRepository<InvestmentRecv> investmentRecvRepo,
            IGenericRepository<Employee> employeeRepo,
            IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
            StoreContext dbContext,
            IGenericRepository<InvestmentRec> investmentRecRepo,
            IGenericRepository<InvestmentDetailTracker> investmentDetailTrackerRepo,
            IGenericRepository<Donation> donationRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _investmentRecvRepo = investmentRecvRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentRecProductRepo = investmentRecProductRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
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
        public IReadOnlyList<InvestmentRcvPending> GetInvestmentInits(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Approved")
                    };
                var results = _dbContext.InvestmentRcvPending.FromSqlRaw("EXECUTE SP_InvestmentRecvSearch @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();


                return results;
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [HttpGet("GetinvestmentReceived/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetinvestmentApproved(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_ReceivedInvestment @SBU,@EID", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, results.Count(), data));
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [HttpPost("InsertRecv")]
        public async Task<ActionResult<InvestmentRecvDto>> InsertInvestmentRecv(InvestmentRecvDto investmentRecvDto)
        {
            try
            {
                var spec = new ApprAuthConfigSpecification(investmentRecvDto.EmployeeId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                var empData = await _employeeRepo.GetByIdAsync(investmentRecvDto.EmployeeId);
                var invRec = new InvestmentRecv
                {
                    //ReferenceNo = investmentInitDto.ReferenceNo,
                    InvestmentInitId = investmentRecvDto.InvestmentInitId,
                    ProposedAmount = investmentRecvDto.ProposedAmount,
                    Purpose = investmentRecvDto.Purpose,
                    CommitmentAllSBU = investmentRecvDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentRecvDto.CommitmentOwnSBU,
                    FromDate = investmentRecvDto.FromDate,
                    ToDate = investmentRecvDto.ToDate,
                    TotalMonth = investmentRecvDto.TotalMonth,
                    PaymentMethod = investmentRecvDto.PaymentMethod,
                    ChequeTitle = investmentRecvDto.ChequeTitle,
                    EmployeeId = investmentRecvDto.EmployeeId,
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
                    ReceiveStatus = investmentRecvDto.ReceiveStatus,
                    Comments = investmentRecvDto.Comments,
                    SetOn = DateTimeOffset.Now
                };
                _investmentRecvRepo.Add(invRec);
                _investmentRecvRepo.Savechange();

                return new InvestmentRecvDto
                {
                    Id = invRec.Id,
                    InvestmentInitId = investmentRecvDto.InvestmentInitId,
                    ProposedAmount = investmentRecvDto.ProposedAmount,
                    Purpose = investmentRecvDto.Purpose,
                    CommitmentAllSBU = investmentRecvDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentRecvDto.CommitmentOwnSBU,
                    FromDate = investmentRecvDto.FromDate,
                    ToDate = investmentRecvDto.ToDate,
                    TotalMonth = investmentRecvDto.TotalMonth,
                    PaymentMethod = investmentRecvDto.PaymentMethod,
                    ChequeTitle = investmentRecvDto.ChequeTitle,
                    EmployeeId = investmentRecvDto.EmployeeId,
                    ReceiveStatus = investmentRecvDto.ReceiveStatus,
                    Comments = investmentRecvDto.Comments,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UpdateRecv")]
        public async Task<ActionResult<InvestmentRecvDto>> UpdateInvestmentRecv(InvestmentRecvDto investmentRecvDto)
        {
            try
            {
                var spec = new ApprAuthConfigSpecification(investmentRecvDto.EmployeeId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                var empData = await _employeeRepo.GetByIdAsync(investmentRecvDto.EmployeeId);
                var invRec = new InvestmentRecv
                {
                    Id = investmentRecvDto.Id,
                    InvestmentInitId = investmentRecvDto.InvestmentInitId,
                    ProposedAmount = investmentRecvDto.ProposedAmount,
                    Purpose = investmentRecvDto.Purpose,
                    CommitmentAllSBU = investmentRecvDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentRecvDto.CommitmentOwnSBU,
                    FromDate = investmentRecvDto.FromDate,
                    ToDate = investmentRecvDto.ToDate,
                    TotalMonth = investmentRecvDto.TotalMonth,
                    PaymentMethod = investmentRecvDto.PaymentMethod,
                    ChequeTitle = investmentRecvDto.ChequeTitle,
                    EmployeeId = investmentRecvDto.EmployeeId,
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
                    ReceiveStatus = investmentRecvDto.ReceiveStatus,
                    Comments = investmentRecvDto.Comments,
                    SetOn = DateTimeOffset.Now
                };
                _investmentRecvRepo.Update(invRec);
                _investmentRecvRepo.Savechange();

                return new InvestmentRecvDto
                {
                    Id = invRec.Id,
                    InvestmentInitId = investmentRecvDto.InvestmentInitId,
                    ProposedAmount = investmentRecvDto.ProposedAmount,
                    Purpose = investmentRecvDto.Purpose,
                    CommitmentAllSBU = investmentRecvDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentRecvDto.CommitmentOwnSBU,
                    FromDate = investmentRecvDto.FromDate,
                    ToDate = investmentRecvDto.ToDate,
                    TotalMonth = investmentRecvDto.TotalMonth,
                    PaymentMethod = investmentRecvDto.PaymentMethod,
                    ChequeTitle = investmentRecvDto.ChequeTitle,
                    EmployeeId = investmentRecvDto.EmployeeId,
                    ReceiveStatus = investmentRecvDto.ReceiveStatus,
                    Comments = investmentRecvDto.Comments,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost("InsertRecvCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentRecvComment(InvestmentRecCommentDto investmentRecDto)
        {
            var isComplete = false;
            //var investmentInitSpec = new InvestmentInitSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            if (investmentInits.SBU == empData.SBU)
            {
                bool isTrue = false;
                var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
                var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                isComplete = true;
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

        [HttpPost("UpdateRecvCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentRecvComment(InvestmentRecCommentDto investmentRecDto)
        {
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var isComplete = false;
            if (investmentInits.SBU == empData.SBU)
            {
                bool isTrue = false;
                var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
                var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                isComplete = true;
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

        [HttpPost("InsertRecvProd")]
        public async Task<IActionResult> InsertInvestmentRecvomendationProduct(List<InvestmentRecProductsDto> investmentRecProductDto)
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
                }

                foreach (var v in investmentRecProductDto)
                {
                    var investmentRecProduct = new InvestmentRecProducts
                    {
                        //ReferenceNo = investmentRecDto.ReferenceNo,
                        InvestmentInitId = v.InvestmentInitId,
                        ProductId = v.ProductId,
                        EmployeeId = v.EmployeeId,
                        SBU = v.ProductInfo.SBU,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecProductRepo.Add(investmentRecProduct);
                }

                _investmentRecProductRepo.Savechange();

                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("getInvestmentRecProducts/{investmentInitId}/{sbu}")]
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
        [Route("getInvestmentDetails/{investmentInitId}/{empId}")]
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
        [Route("getInvestmentRecvComment/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentRecv>> GetInvestmentRecvComment(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentRecvSpecification(investmentInitId);
                var investmentDetail = await _investmentRecvRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
