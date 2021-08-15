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
        [HttpGet("investmentInits")]
        public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetInvestmentInits(
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecSpecParams investmentRecParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams)
        {
            try
            {
                var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                var investmentRecSpec = new InvestmentRecSpecification(investmentRecParrams);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);

               

                var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);
                var investmentRecs = await _investmentRecRepo.ListAsync(investmentRecSpec);
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                
                var market = (from i in investmentInits
                    where ! (from  rc in investmentRecComments 
                              select rc.InvestmentInitId).Contains(i.Id)
                              orderby i.ReferenceNo
                              select new InvestmentInitDto
                              {
                                  ReferenceNo = i.ReferenceNo.Trim(),
                                  ProposeFor = i.ProposeFor.Trim(),
                                  DonationType = i.DonationType.Trim(),
                                  DonationTo = i.DonationTo.Trim(),
                                  EmployeeId = i.EmployeeId,
                              }
                              ).Distinct().ToList();

                var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(investmentInits);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, data));
            }
            catch (System.Exception e)
            {

                throw;
            }
        }






        [HttpPost("insertRec")]
        public async Task<InvestmentRecDto> InsertInvestmentRecomendation(InvestmentRecDto investmentRecDto)
        {
            var invRec = new InvestmentRec
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProposedAmt = investmentRecDto.ProposedAmt,
                InvestmentPurpose = investmentRecDto.InvestmentPurpose,
                CommitmentAllSBU = investmentRecDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecDto.CommitmentOwnSBU,
                FromDate = investmentRecDto.FromDate,
                ToDate = investmentRecDto.ToDate,
                PaymentMethod = investmentRecDto.PaymentMethod,
                ChequeTitle = investmentRecDto.ChequeTitle,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecRepo.Add(invRec);
            _investmentRecRepo.Savechange();

            return new InvestmentRecDto
            {
                Id = invRec.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProposedAmt = investmentRecDto.ProposedAmt,
                InvestmentPurpose = investmentRecDto.InvestmentPurpose,
                CommitmentAllSBU = investmentRecDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecDto.CommitmentOwnSBU,
                FromDate = investmentRecDto.FromDate,
                ToDate = investmentRecDto.ToDate,
                PaymentMethod = investmentRecDto.PaymentMethod,
                ChequeTitle = investmentRecDto.ChequeTitle,
            };
        }

        [HttpPost("updateRec")]
        public ActionResult<InvestmentRecDto> UpdateInvestmentRecomendation(InvestmentRecDto investmentRecDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invRec = new InvestmentRec
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProposedAmt = investmentRecDto.ProposedAmt,
                InvestmentPurpose = investmentRecDto.InvestmentPurpose,
                CommitmentAllSBU = investmentRecDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecDto.CommitmentOwnSBU,
                FromDate = investmentRecDto.FromDate,
                ToDate = investmentRecDto.ToDate,
                PaymentMethod = investmentRecDto.PaymentMethod,
                ChequeTitle = investmentRecDto.ChequeTitle,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecRepo.Update(invRec);
            _investmentRecRepo.Savechange();

            return new InvestmentRecDto
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProposedAmt = investmentRecDto.ProposedAmt,
                InvestmentPurpose = investmentRecDto.InvestmentPurpose,
                CommitmentAllSBU = investmentRecDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecDto.CommitmentOwnSBU,
                FromDate = investmentRecDto.FromDate,
                ToDate = investmentRecDto.ToDate,
                PaymentMethod = investmentRecDto.PaymentMethod,
                ChequeTitle = investmentRecDto.ChequeTitle,
            };
        }

        [HttpPost("insertRecCom")]
        public async Task<InvestmentRecCommentDto> InsertInvestmentRecomendationComment(InvestmentRecCommentDto investmentRecDto)
        {
            var invRec = new InvestmentRecComment
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                //InvestmentInitId = investmentRecDto.InvestmenRecId,
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
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
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
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
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



        [HttpPost("insertRecProd")]
        public async Task<InvestmentRecProductsDto> InsertInvestmentRecomendationProduct(InvestmentRecProductsDto investmentRecDto)
        {
            var invRec = new InvestmentRecProducts
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProductId = investmentRecDto.ProductId,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecProductRepo.Add(invRec);
            _investmentRecProductRepo.Savechange();

            return new InvestmentRecProductsDto
            {
                Id = invRec.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProductId = investmentRecDto.ProductId,
            };
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
    }
}
