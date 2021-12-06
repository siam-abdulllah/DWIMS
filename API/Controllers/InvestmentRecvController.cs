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
       
        [HttpGet("investmentApproved/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentRecvSearch @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);


                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        [HttpGet("investmentReceived/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetinvestmentApproved(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
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

          [HttpPost("InsertRecv/{empID}/{RecvStatus}/{sbu}/{donationId}")]
          public async Task<ActionResult<InvestmentRecvDto>> InsertInvestmentRecv(int empId, string RecvStatus, string sbu, int donationId, InvestmentRecvDto investmentRecvDto)
        {
            try
            {
                if (RecvStatus == "Approved")
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmentRecvDto.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmentRecvDto.ProposedAmount),
                        new SqlParameter("@ASTATUS", RecvStatus),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    // var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentCeilingCheck @SBU,@DID,@EID,@PRAMOUNT,@ASTATUS", parms.ToArray()).ToList();
                   // var result = _dbContext.Database.ExecuteSqlRawAsync("EXECUTE SP_InvestmentCeilingCheck @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheck @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                    //var param=parms[6].Value;
                    if (parms[6].Value.ToString() != "True")
                    //if (result.Result == 0)
                    {
                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                    }
                   
                    
                        var alreadyExistSpec = new InvestmentRecSpecification(investmentRecvDto.InvestmentInitId,empId );
                        var alreadyExistInvestmentRecvList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                        if (alreadyExistInvestmentRecvList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentRecvList)
                            {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                            }
                        }
                    //var invRecv = new InvestmentRecv
                    //{
                    //    //ReferenceNo = investmentInitDto.ReferenceNo,
                    //    InvestmentInitId = investmentRecvDto.InvestmentInitId,
                    //    ProposedAmount = investmentRecvDto.ProposedAmount,
                    //    Purpose = investmentRecvDto.Purpose,
                    //    CommitmentAllSBU = investmentRecvDto.CommitmentAllSBU,
                    //    CommitmentOwnSBU = investmentRecvDto.CommitmentOwnSBU,
                    //    FromDate = investmentRecvDto.FromDate,
                    //    ToDate = investmentRecvDto.ToDate,
                    //    TotalMonth = investmentRecvDto.TotalMonth,
                    //    PaymentMethod = investmentRecvDto.PaymentMethod,
                    //    ChequeTitle = investmentRecvDto.ChequeTitle,
                    //    EmployeeId = empId,
                    //    SetOn = DateTimeOffset.Now
                    //};
                    //_investmentRecvRepo.Add(invRecv);
                    //_investmentRecvRepo.Savechange();
                    //var specAppr = new ApprAuthConfigSpecification(empId, "A");
                    //var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                    var invRecAppr = new InvestmentRec
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
                        EmployeeId = empId,
                       // Priority = apprAuthConfigAppr.ApprovalAuthority.Priority,
                        Priority = 3,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRecAppr);
                    _investmentRecRepo.Savechange();

                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmentRecvDto.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentRecvList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentRecvList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentRecvList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }

                    var donation = await _donationRepo.GetByIdAsync(donationId);
                    if (donation.DonationTypeName == "Honorarium")
                    {
                        DateTimeOffset calcDate = investmentRecvDto.FromDate;
                        for (int i = 0; i < investmentRecvDto.TotalMonth; i++)
                        {
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentRecvDto.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentRecvDto.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentRecvDto.FromDate,
                                ToDate = investmentRecvDto.ToDate,
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
                            InvestmentInitId = investmentRecvDto.InvestmentInitId,
                            DonationId = donationId,
                            ApprovedAmount = investmentRecvDto.ProposedAmount,
                            Month = 0,
                            Year = 0,
                            FromDate = investmentRecvDto.FromDate,
                            ToDate = investmentRecvDto.ToDate,
                            PaidStatus = "Paid",
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentDetailTrackerRepo.Add(invDT);
                        _investmentDetailTrackerRepo.Savechange();
                    }

                    return new InvestmentRecvDto
                    {
                        Id = invRecAppr.Id,
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
                    };
                }

                //var invRecvForRec = new InvestmentRec
                //{
                //    //ReferenceNo = investmentInitDto.ReferenceNo,
                //    InvestmentInitId = investmentRecvDto.InvestmentInitId,
                //    ProposedAmount = investmentRecvDto.ProposedAmount,
                //    Purpose = investmentRecvDto.Purpose,
                //    CommitmentAllSBU = investmentRecvDto.CommitmentAllSBU,
                //    CommitmentOwnSBU = investmentRecvDto.CommitmentOwnSBU,
                //    FromDate = investmentRecvDto.FromDate,
                //    ToDate = investmentRecvDto.ToDate,
                //    TotalMonth = investmentRecvDto.TotalMonth,
                //    PaymentMethod = investmentRecvDto.PaymentMethod,
                //    ChequeTitle = investmentRecvDto.ChequeTitle,
                //    EmployeeId = empId,
                //    SetOn = DateTimeOffset.Now
                //};
                //_investmentRecRepo.Add(invRecvForRec);
                //_investmentRecRepo.Savechange();

                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                var invRec = new InvestmentRec
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
                    EmployeeId = empId,
                    Priority = apprAuthConfig.ApprovalAuthority.Priority,
                    //Priority = 3,
                    CompletionStatus = true,
                    SetOn = DateTimeOffset.Now
                };
                _investmentRecRepo.Add(invRec);
                _investmentRecRepo.Savechange();

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
            //var empData = await _employeeRepo.GetByIdAsync(investmentRecvDto.EmployeeId);
           // var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecvDto.InvestmentInitId);

            // if (investmentInits.SBU == empData.SBU)
            // {
            //     bool isTrue = false;
            //     var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecvDto.InvestmentInitId);
            //     var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            //     var investmentRecvCommentSpec = new InvestmentRecvCommentSpecification((int)investmentRecvDto.InvestmentInitId);
            //     var investmentRecvComments = await _investmentRecvCommentRepo.ListAsync(investmentRecvCommentSpec);
            //     foreach (var v in investmentTargetedGroup)
            //     {
            //         isTrue = false;
            //         foreach (var i in investmentRecvComments)
            //         {
            //             if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
            //             {
            //                 isTrue = true;
            //             }
            //         }
            //         if (!isTrue) { return BadRequest(new ApiResponse(400, "Other recommendations are not completed yet")); }

            //     }
            // }
            //var invRecv = new InvestmentRecvComment
            //{
            //    //ReferenceNo = investmentInitDto.ReferenceNo,
            //    InvestmentInitId = investmentRecvDto.InvestmentInitId,
            //    EmployeeId = investmentRecvDto.EmployeeId,
            //    Comments = investmentRecvDto.Comments,
            //    RecvStatus = investmentRecvDto.RecvStatus,
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
            //_investmentRecvCommentRepo.Add(invRecv);
            //_investmentRecvCommentRepo.Savechange();

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
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            //var empData = await _employeeRepo.GetByIdAsync(investmentRecvDto.EmployeeId);
            //var invRecv = new InvestmentRecvComment
            //{
            //    Id = investmentRecvDto.Id,
            //    InvestmentInitId = investmentRecvDto.InvestmentInitId,
            //    EmployeeId = investmentRecvDto.EmployeeId,
            //    Comments = investmentRecvDto.Comments,
            //    RecvStatus = investmentRecvDto.RecvStatus,
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
            //_investmentRecvCommentRepo.Update(invRecv);
            //_investmentRecvCommentRepo.Savechange();

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
            //try
            //{
            //    foreach (var i in investmentRecvProductDto)
            //    {
            //        var alreadyExistSpec = new InvestmentRecvProductSpecification(i.InvestmentInitId, i.ProductId);
            //        var alreadyExistInvestmentRecvProductList = await _investmentRecvProductRepo.ListAsync(alreadyExistSpec);
            //        if (alreadyExistInvestmentRecvProductList.Count > 0)
            //        {
            //            foreach (var v in alreadyExistInvestmentRecvProductList)
            //            {
            //                _investmentRecvProductRepo.Delete(v);
            //                _investmentRecvProductRepo.Savechange();
            //            }
            //        }
            //    }

            //    foreach (var v in investmentRecvProductDto)
            //    {
            //        var investmentRecvProduct = new InvestmentRecvProducts
            //        {
            //            //ReferenceNo = investmentRecvDto.ReferenceNo,
            //            InvestmentInitId = v.InvestmentInitId,
            //            ProductId = v.ProductId,
            //            EmployeeId = v.EmployeeId,
            //            SBU = v.ProductInfo.SBU,
            //            SetOn = DateTimeOffset.Now,
            //            ModifiedOn = DateTimeOffset.Now
            //        };
            //        _investmentRecvProductRepo.Add(investmentRecvProduct);
            //    }

            //    _investmentRecvProductRepo.Savechange();

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
        [Route("investmentRecvProducts/{investmentInitId}/{sbu}")]
        //public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecvProducts(int investmentInitId, string sbu)
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
        [Route("investmentRecvDetails/{investmentInitId}/{empId}")]
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
        [Route("getInvestmentRecvComment/{investmentInitId}/{empId}")]
        //public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecvComment(int investmentInitId, int empId)
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
        [Route("getInvestmentRecvComments/{investmentInitId}")]
        //public async Task<IReadOnlyList<InvestmentRecvComment>> GetInvestmentRecvComments(int investmentInitId)
        //{
        //    try
        //    {
        //        var spec = new InvestmentRecvCommentSpecification(investmentInitId);
        //        var investmentDetail = await _investmentRecvCommentRepo.ListAsync(spec);
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
