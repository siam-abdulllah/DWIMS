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
    public class InvestmentAprController : BaseApiController
    {

        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductRepo;
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

        public InvestmentAprController(IGenericRepository<ApprovalCeiling> approvalCeilingRepo,
            IGenericRepository<ApprAuthConfig> apprAuthConfigRepo,
            IGenericRepository<SBUWiseBudget> sbuRepo,
            IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo,
            IGenericRepository<InvestmentInit> investmentInitRepo, 
            IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
            IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
            IGenericRepository<InvestmentApr> investmentAprRepo,
            IGenericRepository<InvestmentAprComment> investmentAprCommentRepo,
            IGenericRepository<InvestmentAprProducts> investmentAprProductRepo,
            IGenericRepository<Employee> employeeRepo,
            IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
            StoreContext dbContext,
            IGenericRepository<InvestmentRec> investmentRecRepo,
            IGenericRepository<InvestmentDetailTracker> investmentDetailTrackerRepo,
            IMapper mapper)
        {
            _mapper = mapper;
            _investmentAprRepo = investmentAprRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentRecProductRepo = investmentRecProductRepo;
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
        }
        [HttpGet("investmentInits/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams,
          [FromQuery] InvestmentAprCommentSpecParams investmentAprCommentParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearch @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);


                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        [HttpGet("investmentApproved/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetinvestmentApproved(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams,
          [FromQuery] InvestmentAprCommentSpecParams investmentAprCommentParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended"),
                        new SqlParameter("@ASTATUS", DBNull.Value)
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentApprpvedSearch @SBU,@EID,@RSTATUS,@ASTATUS", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        [HttpPost("InsertApr/{empID}/{aprStatus}/{sbu}/{dType}")]
        public async Task<ActionResult<InvestmentAprDto>> InsertInvestmentApr(int empId, string aprStatus, string sbu, string dType, InvestmentAprDto investmentAprDto)
        {
            try
            {
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (aprStatus == "Approved")
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DTYPE", dType),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmentAprDto.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmentAprDto.ProposedAmount),
                        new SqlParameter("@ASTATUS", aprStatus),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    // var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentCeilingCheck @SBU,@DTYPE,@EID,@PRAMOUNT,@ASTATUS", parms.ToArray()).ToList();
                    var result = _dbContext.Database.ExecuteSqlRawAsync("EXECUTE SP_InvestmentCeilingCheck @SBU,@DTYPE,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                    if (parms[6].Value.ToString() != "True")
                    //if (result.Result == 0)
                    {
                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                    }
                        var alreadyExistSpec = new InvestmentRecSpecification(investmentAprDto.InvestmentInitId,empId );
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                        if (alreadyExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentAprList)
                            {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                            }
                        }
                    //var invApr = new InvestmentApr
                    //{
                    //    //ReferenceNo = investmentInitDto.ReferenceNo,
                    //    InvestmentInitId = investmentAprDto.InvestmentInitId,
                    //    ProposedAmount = investmentAprDto.ProposedAmount,
                    //    Purpose = investmentAprDto.Purpose,
                    //    CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                    //    CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                    //    FromDate = investmentAprDto.FromDate,
                    //    ToDate = investmentAprDto.ToDate,
                    //    TotalMonth = investmentAprDto.TotalMonth,
                    //    PaymentMethod = investmentAprDto.PaymentMethod,
                    //    ChequeTitle = investmentAprDto.ChequeTitle,
                    //    EmployeeId = empId,
                    //    SetOn = DateTimeOffset.Now
                    //};
                    //_investmentAprRepo.Add(invApr);
                    //_investmentAprRepo.Savechange();
                    
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
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
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
                    if (dType == "Honorarium")
                    {
                        DateTimeOffset calcDate = investmentAprDto.FromDate;
                        for (int i = 0; i < investmentAprDto.TotalMonth; i++)
                        {
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentAprDto.InvestmentInitId,
                                DonationType = dType,
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
                    else {
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmentAprDto.InvestmentInitId,
                            DonationType = dType,
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

                //var invAprForRec = new InvestmentRec
                //{
                //    //ReferenceNo = investmentInitDto.ReferenceNo,
                //    InvestmentInitId = investmentAprDto.InvestmentInitId,
                //    ProposedAmount = investmentAprDto.ProposedAmount,
                //    Purpose = investmentAprDto.Purpose,
                //    CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                //    CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                //    FromDate = investmentAprDto.FromDate,
                //    ToDate = investmentAprDto.ToDate,
                //    TotalMonth = investmentAprDto.TotalMonth,
                //    PaymentMethod = investmentAprDto.PaymentMethod,
                //    ChequeTitle = investmentAprDto.ChequeTitle,
                //    EmployeeId = empId,
                //    SetOn = DateTimeOffset.Now
                //};
                //_investmentRecRepo.Add(invAprForRec);
                //_investmentRecRepo.Savechange();

                //var spec = new ApprAuthConfigSpecification(empId, "A");
                //var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
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
            //var empData = await _employeeRepo.GetByIdAsync(investmentAprDto.EmployeeId);
           // var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentAprDto.InvestmentInitId);

            // if (investmentInits.SBU == empData.SBU)
            // {
            //     bool isTrue = false;
            //     var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentAprDto.InvestmentInitId);
            //     var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            //     var investmentAprCommentSpec = new InvestmentAprCommentSpecification((int)investmentAprDto.InvestmentInitId);
            //     var investmentAprComments = await _investmentAprCommentRepo.ListAsync(investmentAprCommentSpec);
            //     foreach (var v in investmentTargetedGroup)
            //     {
            //         isTrue = false;
            //         foreach (var i in investmentAprComments)
            //         {
            //             if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
            //             {
            //                 isTrue = true;
            //             }
            //         }
            //         if (!isTrue) { return BadRequest(new ApiResponse(400, "Other recommendations are not completed yet")); }

            //     }
            // }
            //var invApr = new InvestmentAprComment
            //{
            //    //ReferenceNo = investmentInitDto.ReferenceNo,
            //    InvestmentInitId = investmentAprDto.InvestmentInitId,
            //    EmployeeId = investmentAprDto.EmployeeId,
            //    Comments = investmentAprDto.Comments,
            //    AprStatus = investmentAprDto.AprStatus,
            //    MarketGroupCode = empData.MarketGroupCode,
            //    MarketGroupName = empData.MarketGroupName,
            //    MarketCode = empData.MarketCode,
            //    MarketName = empData.MarketName,
            //    RegionCode = empData.RegionCode,
            //    RegionName = empData.RegionName,
            //    ZoneCode = empData.ZoneCode,
            //    ZoneName = empData.ZoneName,
            //    TerritoryCode = empData.TerritoryCode,
            //    TerritoryName = empData.TerritoryName,
            //    SBUName = empData.SBUName,
            //    SBU = empData.SBU,
            //    SetOn = DateTimeOffset.Now
            //};
            //_investmentAprCommentRepo.Add(invApr);
            //_investmentAprCommentRepo.Savechange();

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

        [HttpPost("UpdateAprCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprComment(InvestmentRecCommentDto investmentRecDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            //var empData = await _employeeRepo.GetByIdAsync(investmentAprDto.EmployeeId);
            //var invApr = new InvestmentAprComment
            //{
            //    Id = investmentAprDto.Id,
            //    InvestmentInitId = investmentAprDto.InvestmentInitId,
            //    EmployeeId = investmentAprDto.EmployeeId,
            //    Comments = investmentAprDto.Comments,
            //    AprStatus = investmentAprDto.AprStatus,
            //    MarketGroupCode = empData.MarketGroupCode,
            //    MarketGroupName = empData.MarketGroupName,
            //    MarketCode = empData.MarketCode,
            //    MarketName = empData.MarketName,
            //    RegionCode = empData.RegionCode,
            //    RegionName = empData.RegionName,
            //    ZoneCode = empData.ZoneCode,
            //    ZoneName = empData.ZoneName,
            //    TerritoryCode = empData.TerritoryCode,
            //    TerritoryName = empData.TerritoryName,
            //    SBUName = empData.SBUName,
            //    SBU = empData.SBU,
            //    ModifiedOn = DateTimeOffset.Now,
            //};
            //_investmentAprCommentRepo.Update(invApr);
            //_investmentAprCommentRepo.Savechange();

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
            };
        }

        [HttpPost("InsertAprProd")]
        public async Task<IActionResult> InsertInvestmentApromendationProduct(List<InvestmentRecProductsDto> investmentRecProductDto)
        {
            //try
            //{
            //    foreach (var i in investmentAprProductDto)
            //    {
            //        var alreadyExistSpec = new InvestmentAprProductSpecification(i.InvestmentInitId, i.ProductId);
            //        var alreadyExistInvestmentAprProductList = await _investmentAprProductRepo.ListAsync(alreadyExistSpec);
            //        if (alreadyExistInvestmentAprProductList.Count > 0)
            //        {
            //            foreach (var v in alreadyExistInvestmentAprProductList)
            //            {
            //                _investmentAprProductRepo.Delete(v);
            //                _investmentAprProductRepo.Savechange();
            //            }
            //        }
            //    }

            //    foreach (var v in investmentAprProductDto)
            //    {
            //        var investmentAprProduct = new InvestmentAprProducts
            //        {
            //            //ReferenceNo = investmentAprDto.ReferenceNo,
            //            InvestmentInitId = v.InvestmentInitId,
            //            ProductId = v.ProductId,
            //            EmployeeId = v.EmployeeId,
            //            SBU = v.ProductInfo.SBU,
            //            SetOn = DateTimeOffset.Now,
            //            ModifiedOn = DateTimeOffset.Now
            //        };
            //        _investmentAprProductRepo.Add(investmentAprProduct);
            //    }

            //    _investmentAprProductRepo.Savechange();

            //    return Ok("Succsessfuly Saved!!!");
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
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
        [Route("investmentAprProducts/{investmentInitId}/{sbu}")]
        //public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentAprProducts(int investmentInitId, string sbu)
        //{
        //    try
        //    {
        //        var spec = new InvestmentRecProductSpecification(investmentInitId, sbu);
        //        var investmentTargetedProd = await _investmentRecProductRepo.ListAsync(spec);
        //        return investmentTargetedProd;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecProducts(int investmentInitId, string sbu)
        {
            try
            {
                var spec = new InvestmentRecProductSpecification(investmentInitId, sbu);
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
        //public async Task<IReadOnlyList<InvestmentRec>> GetInvestmentDetails(int investmentInitId,int empId)
        //{
        //    try
        //    {
        //        var spec = new InvestmentRecSpecification(investmentInitId, empId);
        //        var investmentDetail = await _investmentRecRepo.ListAsync(spec);
        //        return investmentDetail;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
        //public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentAprComment(int investmentInitId, int empId)
        //{
        //    try
        //    {
        //        var spec = new InvestmentRecCommentSpecification(investmentInitId, empId);
        //        var investmentDetail = await _investmentRecCommentRepo.ListAsync(spec);
        //        return investmentDetail;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
        //public async Task<IReadOnlyList<InvestmentAprComment>> GetInvestmentAprComments(int investmentInitId)
        //{
        //    try
        //    {
        //        var spec = new InvestmentAprCommentSpecification(investmentInitId);
        //        var investmentDetail = await _investmentAprCommentRepo.ListAsync(spec);
        //        return investmentDetail;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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

    }
}
