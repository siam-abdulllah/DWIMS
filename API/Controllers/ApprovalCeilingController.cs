using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ApprovalCeilingController : BaseApiController
    {
        private readonly IGenericRepository<ApprovalCeiling> _aptimeRepo;
        private readonly IMapper _mapper;
        public ApprovalCeilingController(IGenericRepository<ApprovalCeiling> aptimeRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _aptimeRepo = aptimeRepo;
        }


        [HttpGet("GetAllData")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<ApprovalCeiling>>> GetAllApprovalCeilingInfo([FromQuery] ApprovalCeilingSpecParams appParrams)
        {
            try
            {
                var spec = new ApprovalCeilingSpecification(appParrams);

                var countSpec = new ApprovalCeilingWithFiltersForCountSpecificication(appParrams);

                var totalItems = await _aptimeRepo.CountAsync(countSpec);

                var posts = await _aptimeRepo.ListAsync(spec);

                var data = _mapper.Map<IReadOnlyList<ApprovalCeiling>, IReadOnlyList<ApprovalCeilingDto>>(posts);

                return Ok(new Pagination<ApprovalCeilingDto>(appParrams.PageIndex, appParrams.PageSize, totalItems, data));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("CreateApprovalCeiling")]

        public async Task<ActionResult<ApprovalCeiling>> SaveApprovalCeiling(ApprovalCeiling apprclngDto)
        {
            try
            {
                var fromDateCheck = new ApprovalCeilingWithFiltersForCountSpecificication(apprclngDto.ApprovalAuthorityId, apprclngDto.DonationType, apprclngDto.InvestmentFrom);
                var fromDateCheckList = await _aptimeRepo.ListAsync(fromDateCheck);
                var toDateCheck = new ApprovalCeilingWithFiltersForCountSpecificication(apprclngDto.ApprovalAuthorityId, apprclngDto.DonationType, apprclngDto.InvestmentTo);
                var toDateCheckList = await _aptimeRepo.ListAsync(toDateCheck);

                if (fromDateCheckList.Count > 0 || toDateCheckList.Count > 0)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Date Range Existed" } });
                }


                var appr = new ApprovalCeiling
                {
                    DonationType = apprclngDto.DonationType,
                    InvestmentFrom = apprclngDto.InvestmentFrom,
                    InvestmentTo = apprclngDto.InvestmentTo,
                    AmountPerMonth = apprclngDto.AmountPerMonth,
                    AmountPerTransacion = apprclngDto.AmountPerTransacion,
                    ApprovalAuthorityId = apprclngDto.ApprovalAuthorityId,
                    Additional = apprclngDto.Additional,
                    Remarks = apprclngDto.Remarks,
                    Status = "A",
                    SetOn = DateTimeOffset.Now
                };

                _aptimeRepo.Add(appr);
                _aptimeRepo.Savechange();

                return new ApprovalCeiling
                {
                    Id = appr.Id,
                    DonationType = appr.DonationType,
                    InvestmentFrom = appr.InvestmentFrom,
                    InvestmentTo = appr.InvestmentTo,
                    AmountPerMonth = appr.AmountPerMonth,
                    AmountPerTransacion = appr.AmountPerTransacion,
                    ApprovalAuthorityId = appr.ApprovalAuthorityId,
                    Additional = appr.Additional,
                    Remarks = appr.Remarks,
                    Status = appr.Remarks,
                };
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        [HttpPost("ModifyApprovalCeiling")]
        public async Task<ActionResult<ApprovalCeiling>> UpdateApprovalCeiling(ApprovalCeiling apprclngDto)
        {
            var fromDateCheck = new ApprovalCeilingWithFiltersForCountSpecificication(apprclngDto.Id, apprclngDto.ApprovalAuthorityId, apprclngDto.DonationType, apprclngDto.InvestmentFrom);
            var fromDateCheckList = await _aptimeRepo.ListAsync(fromDateCheck);
            var toDateCheck = new ApprovalCeilingWithFiltersForCountSpecificication(apprclngDto.Id, apprclngDto.ApprovalAuthorityId, apprclngDto.DonationType, apprclngDto.InvestmentTo);
            var toDateCheckList = await _aptimeRepo.ListAsync(toDateCheck);

            if (fromDateCheckList.Count > 0 || toDateCheckList.Count > 0)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Date Range Existed" } });
            }
            var appr = new ApprovalCeiling
            {
                Id = apprclngDto.Id,
                DonationType = apprclngDto.DonationType,
                InvestmentFrom = apprclngDto.InvestmentFrom,
                InvestmentTo = apprclngDto.InvestmentTo,
                AmountPerMonth = apprclngDto.AmountPerMonth,
                AmountPerTransacion = apprclngDto.AmountPerTransacion,
                ApprovalAuthorityId = apprclngDto.ApprovalAuthorityId,
                Additional = apprclngDto.Additional,
                Remarks = apprclngDto.Remarks,
                Status = "A",
                ModifiedOn = DateTimeOffset.Now
            };

            _aptimeRepo.Update(appr);
            _aptimeRepo.Savechange();

            return new ApprovalCeiling
            {
                Id = appr.Id,
                DonationType = appr.DonationType,
                InvestmentFrom = appr.InvestmentFrom,
                InvestmentTo = appr.InvestmentTo,
                AmountPerMonth = appr.AmountPerMonth,
                AmountPerTransacion = appr.AmountPerTransacion,
                ApprovalAuthorityId = appr.ApprovalAuthorityId,
                Additional = appr.Additional,
                Remarks = appr.Remarks,
                Status = "A",
            };
        }

    }
}
