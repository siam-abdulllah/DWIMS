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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentAprController : BaseApiController
    {

        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentApr> _investmentAprRepo;
        private readonly IGenericRepository<InvestmentAprComment> _investmentAprCommentRepo;
        private readonly IGenericRepository<InvestmentAprProducts> _investmentAprProductRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;

        public InvestmentAprController(IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
            IGenericRepository<InvestmentApr> investmentAprRepo, IGenericRepository<InvestmentAprComment> investmentAprCommentRepo,
            IGenericRepository<InvestmentAprProducts> investmentAprProductRepo, IGenericRepository<Employee> employeeRepo,
            IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo, StoreContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _investmentAprRepo = investmentAprRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _investmentAprCommentRepo = investmentAprCommentRepo;
            _investmentAprProductRepo = investmentAprProductRepo;
            _employeeRepo = employeeRepo;
            _dbContext = dbContext;
        }
        [HttpGet("investmentInits/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams,
          [FromQuery] InvestmentAprCommentSpecParams investmentAprCommentParrams)
        {
            try
            {
                //investmentInitParrams.Search = sbu;
                //investmentRecCommentParrams.Search = sbu;
                //investmentAprCommentParrams.Search = sbu;

                //var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                //var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);
                //var investmentAprCommentSpec = new InvestmentAprCommentSpecification(investmentAprCommentParrams);



                //var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);
                //var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                //var investmentAprComments = await _investmentAprCommentRepo.ListAsync(investmentAprCommentSpec);

                //var investmentInitFormAppr = (from i in investmentInits
                //                              join rc in investmentRecComments on i.Id equals rc.InvestmentInitId
                //                              where rc.RecStatus== "Recommended" && !(from ac in investmentAprComments
                //                                      select ac.InvestmentInitId).Contains(i.Id)
                //                              orderby i.ReferenceNo
                //                              select new InvestmentInitDto
                //                              {
                //                                  Id = i.Id,
                //                                  ReferenceNo = i.ReferenceNo.Trim(),
                //                                  ProposeFor = i.ProposeFor.Trim(),
                //                                  DonationType = i.DonationType.Trim(),
                //                                  DonationTo = i.DonationTo.Trim(),
                //                                  EmployeeId = i.EmployeeId,
                //                              }
                //              ).Distinct().ToList();

                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended"),
                        new SqlParameter("@ASTATUS", DBNull.Value)
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentRecSearch @SBU,@EID,@RSTATUS,@ASTATUS", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);


                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {

                throw;
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
                //investmentInitParrams.Search = sbu;
                //investmentRecCommentParrams.Search = sbu;
                //investmentAprCommentParrams.Search = sbu;
                //var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                //var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);
                //var investmentAprCommentSpec = new InvestmentAprCommentSpecification(investmentAprCommentParrams);



                //var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);
                //var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                //var investmentAprComments = await _investmentAprCommentRepo.ListAsync(investmentAprCommentSpec);

                //var investmentInitForAppr = (from i in investmentInits
                //                             where (from rc in investmentRecComments
                //                                    join ac in investmentAprComments on rc.InvestmentInitId equals ac.InvestmentInitId
                //                                    select ac.InvestmentInitId).Contains(i.Id)
                //                             orderby i.ReferenceNo
                //                             select new InvestmentInitDto
                //                             {
                //                                 Id = i.Id,
                //                                 ReferenceNo = i.ReferenceNo.Trim(),
                //                                 ProposeFor = i.ProposeFor.Trim(),
                //                                 DonationType = i.DonationType.Trim(),
                //                                 DonationTo = i.DonationTo.Trim(),
                //                                 EmployeeId = i.EmployeeId,
                //                             }
                //              ).Distinct().ToList();

                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended"),
                        new SqlParameter("@ASTATUS", DBNull.Value)
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentInitSearch @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {

                throw;
            }
        }

        [HttpPost("InsertApr")]
        public async Task<InvestmentAprDto> InsertInvestmentApr(InvestmentAprDto investmentAprDto)
        {
            var alreadyExistSpec = new InvestmentAprSpecification(investmentAprDto.InvestmentInitId);
            var alreadyExistInvestmentAprList = await _investmentAprRepo.ListAsync(alreadyExistSpec);
            if (alreadyExistInvestmentAprList.Count > 0)
            {
                foreach (var v in alreadyExistInvestmentAprList)
                {
                    _investmentAprRepo.Delete(v);
                    _investmentAprRepo.Savechange();
                }
            }
            var invApr = new InvestmentApr
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
                SetOn = DateTimeOffset.Now
            };
            _investmentAprRepo.Add(invApr);
            _investmentAprRepo.Savechange();

            return new InvestmentAprDto
            {
                Id = invApr.Id,
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
            };
        }



        [HttpPost("InsertAprCom")]
        public async Task<ActionResult<InvestmentAprCommentDto>> InsertInvestmentAprComment(InvestmentAprCommentDto investmentAprDto)
        {
            var empData = await _employeeRepo.GetByIdAsync(investmentAprDto.EmployeeId);
            var invApr = new InvestmentAprComment
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentInitId = investmentAprDto.InvestmentInitId,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                AprStatus = investmentAprDto.AprStatus,
                PostingType = empData.PostingType,
                MarketCode = empData.MarketCode,
                MarketName = empData.MarketName,
                RegionCode = empData.RegionCode,
                RegionName = empData.RegionName,
                ZoneCode = empData.ZoneCode,
                ZoneName = empData.ZoneName,
                TerritoryCode = empData.TerritoryCode,
                TerritoryName = empData.TerritoryName,
                DivisionCode = empData.DivisionCode,
                DivisionName = empData.DivisionName,
                SBU = empData.SBU,
                SetOn = DateTimeOffset.Now
            };
            _investmentAprCommentRepo.Add(invApr);
            _investmentAprCommentRepo.Savechange();

            return new InvestmentAprCommentDto
            {
                Id = invApr.Id,
                InvestmentInitId = investmentAprDto.InvestmentInitId,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                AprStatus = investmentAprDto.AprStatus,
            };
        }

        [HttpPost("UpdateAprCom")]
        public async Task<ActionResult<InvestmentAprCommentDto>> UpdateInvestmentApromendationComment(InvestmentAprCommentDto investmentAprDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var empData = await _employeeRepo.GetByIdAsync(investmentAprDto.EmployeeId);
            var invApr = new InvestmentAprComment
            {
                Id = investmentAprDto.Id,
                InvestmentInitId = investmentAprDto.InvestmentInitId,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                AprStatus = investmentAprDto.AprStatus,
                PostingType = empData.PostingType,
                MarketCode = empData.MarketCode,
                MarketName = empData.MarketName,
                RegionCode = empData.RegionCode,
                RegionName = empData.RegionName,
                ZoneCode = empData.ZoneCode,
                ZoneName = empData.ZoneName,
                TerritoryCode = empData.TerritoryCode,
                TerritoryName = empData.TerritoryName,
                DivisionCode = empData.DivisionCode,
                DivisionName = empData.DivisionName,
                SBU = empData.SBU,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentAprCommentRepo.Update(invApr);
            _investmentAprCommentRepo.Savechange();

            return new InvestmentAprCommentDto
            {
                Id = investmentAprDto.Id,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                AprStatus = investmentAprDto.AprStatus,
            };
        }

        [HttpPost("InsertAprProd")]
        public async Task<IActionResult> InsertInvestmentApromendationProduct(List<InvestmentAprProductsDto> investmentAprProductDto)
        {
            try
            {
                foreach (var i in investmentAprProductDto)
                {
                    var alreadyExistSpec = new InvestmentAprProductSpecification(i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentAprProductList = await _investmentAprProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentAprProductList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprProductList)
                        {
                            _investmentAprProductRepo.Delete(v);
                            _investmentAprProductRepo.Savechange();
                        }
                    }
                }

                foreach (var v in investmentAprProductDto)
                {
                    var investmentAprProduct = new InvestmentAprProducts
                    {
                        //ReferenceNo = investmentAprDto.ReferenceNo,
                        InvestmentInitId = v.InvestmentInitId,
                        ProductId = v.ProductId,
                        SBU = v.SBU,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentAprProductRepo.Add(investmentAprProduct);
                }

                _investmentAprProductRepo.Savechange();

                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpGet]
        [Route("investmentAprProducts/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentAprProducts>> GetInvestmentAprProducts(int investmentInitId, string sbu)
        {
            try
            {
                var spec = new InvestmentAprProductSpecification(investmentInitId, sbu);
                var investmentTargetedProd = await _investmentAprProductRepo.ListAsync(spec);
                return investmentTargetedProd;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentAprDetails/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentApr>> GetInvestmentDetails(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentAprSpecification(investmentInitId);
                var investmentDetail = await _investmentAprRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("getInvestmentAprComment/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentAprComment>> GetInvestmentAprComment(int investmentInitId, int empId)
        {
            try
            {
                var spec = new InvestmentAprCommentSpecification(investmentInitId, empId);
                var investmentDetail = await _investmentAprCommentRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("getInvestmentAprComments/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentAprComment>> GetInvestmentAprComments(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentAprCommentSpecification(investmentInitId);
                var investmentDetail = await _investmentAprCommentRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
