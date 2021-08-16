using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace API.Controllers
{
    public class InvestmentRecController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductRepo;
        private readonly IMapper _mapper;

        public InvestmentRecController(IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentRec> investmentRecRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo, IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _investmentInitRepo = investmentInitRepo;
            _investmentRecRepo = investmentRecRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _investmentRecProductRepo = investmentRecProductRepo;
        }
        [HttpGet("investmentInits/{sbu}")]
        public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetInvestmentInits(string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams)
        {
            try
            {
                investmentInitParrams.Search = sbu;
                investmentRecCommentParrams.Search = sbu;
                var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);



                var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);

                var investmentInitFormRec = (from i in investmentInits
                                             where !(from rc in investmentRecComments
                                                     select rc.InvestmentInitId).Contains(i.Id)
                                             orderby i.ReferenceNo
                                             select new InvestmentInitDto
                                             {
                                                 Id = i.Id,
                                                 ReferenceNo = i.ReferenceNo.Trim(),
                                                 ProposeFor = i.ProposeFor.Trim(),
                                                 DonationType = i.DonationType.Trim(),
                                                 DonationTo = i.DonationTo.Trim(),
                                                 EmployeeId = i.EmployeeId,
                                             }
                              ).Distinct().ToList();

                var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                var totalItems = await _investmentInitRepo.CountAsync(countSpec);



                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, investmentInitFormRec));
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
        [HttpGet("investmentRecommended/{sbu}")]
        public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetinvestmentRecommended(string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams)
        {
            try
            {
                investmentInitParrams.Search = sbu;
                investmentRecCommentParrams.Search = sbu;
                var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);



                var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);

                var investmentInitFormRec = (from i in investmentInits
                                             where (from rc in investmentRecComments
                                                    select rc.InvestmentInitId).Contains(i.Id)
                                             orderby i.ReferenceNo
                                             select new InvestmentInitDto
                                             {
                                                 Id = i.Id,
                                                 ReferenceNo = i.ReferenceNo.Trim(),
                                                 ProposeFor = i.ProposeFor.Trim(),
                                                 DonationType = i.DonationType.Trim(),
                                                 DonationTo = i.DonationTo.Trim(),
                                                 EmployeeId = i.EmployeeId,
                                             }
                              ).Distinct().ToList();

                var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                var totalItems = await _investmentInitRepo.CountAsync(countSpec);



                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, investmentInitFormRec));
            }
            catch (System.Exception e)
            {

                throw;
            }
        }






        [HttpPost("insertRec")]
        public async Task<InvestmentRecDto> InsertInvestmentRecomendation(InvestmentRecDto investmentRecDto)
        {
            var alreadyExistSpec = new InvestmentRecSpecification(investmentRecDto.InvestmentInitId);
            var alreadyExistInvestmentRecList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
            if (alreadyExistInvestmentRecList.Count > 0)
            {
                foreach (var v in alreadyExistInvestmentRecList)
                {
                    _investmentRecRepo.Delete(v);
                    _investmentRecRepo.Savechange();
                }
            }
            var invRec = new InvestmentRec
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProposedAmount = investmentRecDto.ProposedAmount,
                Purpose = investmentRecDto.Purpose,
                CommitmentAllSBU = investmentRecDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecDto.CommitmentOwnSBU,
                FromDate = investmentRecDto.FromDate,
                ToDate = investmentRecDto.ToDate,
                TotalMonth = investmentRecDto.TotalMonth,
                PaymentMethod = investmentRecDto.PaymentMethod,
                ChequeTitle = investmentRecDto.ChequeTitle,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecRepo.Add(invRec);
            _investmentRecRepo.Savechange();

            return new InvestmentRecDto
            {
                Id = invRec.Id,
                InvestmentInitId = invRec.InvestmentInitId,
                ProposedAmount = invRec.ProposedAmount,
                Purpose = invRec.Purpose,
                CommitmentAllSBU = invRec.CommitmentAllSBU,
                CommitmentOwnSBU = invRec.CommitmentOwnSBU,
                FromDate = invRec.FromDate,
                ToDate = invRec.ToDate,
                TotalMonth = investmentRecDto.TotalMonth,
                PaymentMethod = invRec.PaymentMethod,
                ChequeTitle = invRec.ChequeTitle,
            };
        }



        [HttpPost("insertRecCom")]
        public async Task<InvestmentRecCommentDto> InsertInvestmentRecomendationComment(InvestmentRecCommentDto investmentRecDto)
        {
            var invRec = new InvestmentRecComment
            {
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecCommentRepo.Add(invRec);
            _investmentRecCommentRepo.Savechange();

            return new InvestmentRecCommentDto
            {
                Id = invRec.Id,
                InvestmentInitId = invRec.InvestmentInitId,
                EmployeeId = invRec.EmployeeId,
                Comments = invRec.Comments,
                RecStatus = invRec.RecStatus,
            };
        }

        [HttpPost("updateRecCom")]
        public ActionResult<InvestmentRecCommentDto> UpdateInvestmentRecomendationComment(InvestmentRecCommentDto investmentRecDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invRec = new InvestmentRecComment
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecCommentRepo.Update(invRec);
            _investmentRecCommentRepo.Savechange();

            return new InvestmentRecCommentDto
            {
                Id = invRec.Id,
                InvestmentInitId = invRec.InvestmentInitId,
                EmployeeId = invRec.EmployeeId,
                Comments = invRec.Comments,
                RecStatus = invRec.RecStatus,
            };
        }



        [HttpPost("insertRecProd")]
        public async Task<IActionResult> InsertInvestmentRecomendationProduct(List<InvestmentRecProductsDto> investmentRecProductDto)
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

                throw;
            }
        }

        [HttpPost("updateRecProd")]
        public ActionResult<InvestmentRecProductsDto> UpdateInvestmentRecomendationProduct(InvestmentRecProductsDto investmentRecDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invRec = new InvestmentRecProducts
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProductId = investmentRecDto.ProductId,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecProductRepo.Update(invRec);
            _investmentRecProductRepo.Savechange();

            return new InvestmentRecProductsDto
            {
                Id = invRec.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProductId = investmentRecDto.ProductId,
            };
        }

        [HttpGet]
        [Route("investmentRecProducts/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecProducts(int investmentInitId, string sbu)
        {
            try
            {
                var spec = new InvestmentRecProductSpecification(investmentInitId,sbu);
                var investmentTargetedProd = await _investmentRecProductRepo.ListAsync(spec);
                return investmentTargetedProd;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentRecDetails/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentRec>> GetInvestmentDetails(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentRecSpecification(investmentInitId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("getInvestmentRecComment/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecComment(int investmentInitId,int empId)
        {
            try
            {
                var spec = new InvestmentRecCommentSpecification(investmentInitId,empId);
                var investmentDetail = await _investmentRecCommentRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("getInvestmentRecComments/{investmentInitId}")]
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
