using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace API.Controllers
{
    public class InvestmentRecController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductRepo;
        private readonly IMapper _mapper;

        public InvestmentRecController(IGenericRepository<InvestmentRec> investmentRecRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo, IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _investmentRecRepo = investmentRecRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _investmentRecProductRepo = investmentRecProductRepo;
        }


        [HttpPost("InsertRec")]
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

        [HttpPost("UpdateRec")]
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

        [HttpPost("InsertRecCom")]
        public async Task<InvestmentRecCommentDto> InsertInvestmentRecomendationComment(InvestmentRecCommentDto investmentRecDto)
        {
            var invRec = new InvestmentRecComment
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmenRecId = investmentRecDto.InvestmenRecId,
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
                InvestmenRecId = investmentRecDto.InvestmenRecId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
            };
        }

        [HttpPost("UpdateRecCom")]
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



        [HttpPost("InsertRecProd")]
        public async Task<InvestmentRecProductsDto> InsertInvestmentRecomendationProduct(InvestmentRecProductsDto investmentRecDto)
        {
            var invRec = new InvestmentRecProducts
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmenRecId = investmentRecDto.InvestmenRecId,
                ProductId = investmentRecDto.ProductId,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecProductRepo.Add(invRec);
            _investmentRecProductRepo.Savechange();

            return new InvestmentRecProductsDto
            {
                Id = invRec.Id,
                InvestmenRecId = investmentRecDto.InvestmenRecId,
                ProductId = investmentRecDto.ProductId,
            };
        }

        [HttpPost("UpdateRecProd")]
        public ActionResult<InvestmentRecProductsDto> UpdateInvestmentRecomendationProduct(InvestmentRecProductsDto investmentRecDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invRec = new InvestmentRecProducts
            {
                Id = investmentRecDto.Id,
                InvestmenRecId = investmentRecDto.InvestmenRecId,
                ProductId = investmentRecDto.ProductId,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecProductRepo.Update(invRec);
            _investmentRecProductRepo.Savechange();

            return new InvestmentRecProductsDto
            {
                Id = invRec.Id,
                InvestmenRecId = investmentRecDto.InvestmenRecId,
                ProductId = investmentRecDto.ProductId,
            };
        }
    }
}
