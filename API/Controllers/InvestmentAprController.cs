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
    public class InvestmentAprController :  BaseApiController
    {


        private readonly IGenericRepository<InvestmentApr> _investmentAprRepo;
        private readonly IGenericRepository<InvestmentAprComment> _investmentAprCommentRepo;
        private readonly IGenericRepository<InvestmentAprProducts> _investmentAprProductRepo;
        private readonly IMapper _mapper;

        public InvestmentAprController(IGenericRepository<InvestmentApr> investmentAprRepo, IGenericRepository<InvestmentAprComment> investmentAprCommentRepo, IGenericRepository<InvestmentAprProducts> investmentAprProductRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _investmentAprRepo = investmentAprRepo;
            _investmentAprCommentRepo = investmentAprCommentRepo;
            _investmentAprProductRepo = investmentAprProductRepo;
        }


        [HttpPost("InsertApr")]
        public async Task<InvestmentAprDto> InsertInvestmentApromendation(InvestmentAprDto investmentAprDto)
        {
            var invApr = new InvestmentApr
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentRecId = investmentAprDto.InvestmentRecId,
                ProposedAmt = investmentAprDto.ProposedAmt,
                InvestmentPurpose = investmentAprDto.InvestmentPurpose,
                CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                FromDate = investmentAprDto.FromDate,
                ToDate = investmentAprDto.ToDate,
                PaymentMethod = investmentAprDto.PaymentMethod,
                ChequeTitle = investmentAprDto.ChequeTitle,
                SetOn = DateTimeOffset.Now
            };
            _investmentAprRepo.Add(invApr);
            _investmentAprRepo.Savechange();

            return new InvestmentAprDto
            {
                Id = invApr.Id,
                InvestmentRecId = investmentAprDto.InvestmentRecId,
                ProposedAmt = investmentAprDto.ProposedAmt,
                InvestmentPurpose = investmentAprDto.InvestmentPurpose,
                CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                FromDate = investmentAprDto.FromDate,
                ToDate = investmentAprDto.ToDate,
                PaymentMethod = investmentAprDto.PaymentMethod,
                ChequeTitle = investmentAprDto.ChequeTitle,
            };
        }

        [HttpPost("UpdateApr")]
        public ActionResult<InvestmentAprDto> UpdateInvestmentApromendation(InvestmentAprDto investmentAprDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invApr = new InvestmentApr
            {
                Id = investmentAprDto.Id,
                InvestmentRecId = investmentAprDto.InvestmentRecId,
                ProposedAmt = investmentAprDto.ProposedAmt,
                InvestmentPurpose = investmentAprDto.InvestmentPurpose,
                CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                FromDate = investmentAprDto.FromDate,
                ToDate = investmentAprDto.ToDate,
                PaymentMethod = investmentAprDto.PaymentMethod,
                ChequeTitle = investmentAprDto.ChequeTitle,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentAprRepo.Update(invApr);
            _investmentAprRepo.Savechange();

            return new InvestmentAprDto
            {
                Id = investmentAprDto.Id,
                InvestmentRecId = investmentAprDto.InvestmentRecId,
                ProposedAmt = investmentAprDto.ProposedAmt,
                InvestmentPurpose = investmentAprDto.InvestmentPurpose,
                CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                FromDate = investmentAprDto.FromDate,
                ToDate = investmentAprDto.ToDate,
                PaymentMethod = investmentAprDto.PaymentMethod,
                ChequeTitle = investmentAprDto.ChequeTitle,
            };
        }

        [HttpPost("InsertAprCom")]
        public async Task<InvestmentAprCommentDto> InsertInvestmentApromendationComment(InvestmentAprCommentDto investmentAprDto)
        {
            var invApr = new InvestmentAprComment
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentAprId = investmentAprDto.InvestmentAprId,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                RecStatus = investmentAprDto.RecStatus,
                SetOn = DateTimeOffset.Now
            };
            _investmentAprCommentRepo.Add(invApr);
            _investmentAprCommentRepo.Savechange();

            return new InvestmentAprCommentDto
            {
                Id = invApr.Id,
                InvestmentAprId = investmentAprDto.InvestmentAprId,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                RecStatus = investmentAprDto.RecStatus,
            };
        }

        [HttpPost("UpdateAprCom")]
        public ActionResult<InvestmentAprCommentDto> UpdateInvestmentApromendationComment(InvestmentAprCommentDto investmentAprDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invApr = new InvestmentAprComment
            {
                Id = investmentAprDto.Id,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                RecStatus = investmentAprDto.RecStatus,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentAprCommentRepo.Update(invApr);
            _investmentAprCommentRepo.Savechange();

            return new InvestmentAprCommentDto
            {
                Id = investmentAprDto.Id,
                EmployeeId = investmentAprDto.EmployeeId,
                Comments = investmentAprDto.Comments,
                RecStatus = investmentAprDto.RecStatus,
            };
        }

        [HttpPost("InsertAprProd")]
        public async Task<InvestmentAprProductsDto> InsertInvestmentApromendationProduct(InvestmentAprProductsDto investmentAprDto)
        {
            var invApr = new InvestmentAprProducts
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentAprId = investmentAprDto.InvestmentAprId,
                ProductId = investmentAprDto.ProductId,
                SetOn = DateTimeOffset.Now
            };
            _investmentAprProductRepo.Add(invApr);
            _investmentAprProductRepo.Savechange();

            return new InvestmentAprProductsDto
            {
                Id = invApr.Id,
                InvestmentAprId = investmentAprDto.InvestmentAprId,
                ProductId = investmentAprDto.ProductId,
            };
        }

        [HttpPost("UpdateAprProd")]
        public ActionResult<InvestmentAprProductsDto> UpdateInvestmentApromendationProduct(InvestmentAprProductsDto investmentAprDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invApr = new InvestmentAprProducts
            {
                Id = investmentAprDto.Id,
                InvestmentAprId = investmentAprDto.InvestmentAprId,
                ProductId = investmentAprDto.ProductId,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentAprProductRepo.Update(invApr);
            _investmentAprProductRepo.Savechange();

            return new InvestmentAprProductsDto
            {
                Id = invApr.Id,
                InvestmentAprId = investmentAprDto.InvestmentAprId,
                ProductId = investmentAprDto.ProductId,
            };
        }

    }
}
