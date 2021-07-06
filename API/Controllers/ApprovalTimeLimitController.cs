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
    public class ApprovalTimeLimitController : BaseApiController
    {

        private readonly IGenericRepository<ApprovalTimeLimit> _aptimeRepo;
        private readonly IMapper _mapper;
        public ApprovalTimeLimitController(IGenericRepository<ApprovalTimeLimit> aptimeRepo,       
        IMapper mapper)
        {
            _mapper = mapper;
            _aptimeRepo = aptimeRepo;
        }


        [HttpGet("GetAllApprovalTime")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<ApprovalTimeLimitDto>>> GetAllApprovalTime([FromQuery] ApprovalTimeSpecParams approvalParrams)
        {

            var spec = new ApprovalTimeSpecification(approvalParrams);

            var countSpec = new ApprovalTimeWithFiltersForCountSpecificication(approvalParrams);

            var totalItems = await _aptimeRepo.CountAsync(countSpec);

            var posts = await _aptimeRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ApprovalTimeLimit>, IReadOnlyList<ApprovalTimeLimitDto>>(posts);

            return Ok(new Pagination<ApprovalTimeLimitDto>(approvalParrams.PageIndex, approvalParrams.PageSize, totalItems, data));

        }


        [HttpPost("CreateApprovalTimeLimit")]
        public async Task<ActionResult<ApprovalTimeLimitDto>> SaveApprovalTimeLimit(ApprovalTimeLimitDto setApprovalDto)
        {
            var approvaltime = new ApprovalTimeLimit
            {
                ApprovalAuthorityId = setApprovalDto.ApprovalAuthorityId,
                Remarks = setApprovalDto.Remarks,
                TimeLimit = setApprovalDto.TimeLimit,
                Status = setApprovalDto.Status,
            };

            _aptimeRepo.Add(approvaltime);
            _aptimeRepo.Savechange();

            return new ApprovalTimeLimitDto
            {
                Id = approvaltime.Id,
                ApprovalAuthorityId = approvaltime.ApprovalAuthorityId,
                Remarks = approvaltime.Remarks,
                TimeLimit = approvaltime.TimeLimit,
                Status = approvaltime.Status,
            };
        }


        [HttpPost("ModifyApprovalTimeLimit")]
        public async Task<ActionResult<ApprovalTimeLimitDto>> UpdateApprovalTimeLimit(ApprovalTimeLimitDto setApprovalDto)
        {
            //var bcds = await _bcdsRepo.GetByIdAsync(setbcdsDto.Id);
            //if (bcds == null) return Unauthorized(new ApiResponse(401));

            var approvaltime = new ApprovalTimeLimit
            {
                ApprovalAuthorityId = setApprovalDto.ApprovalAuthorityId,
                Remarks = setApprovalDto.Remarks,
                TimeLimit = setApprovalDto.TimeLimit,
                Status = setApprovalDto.Status,
            };

            _aptimeRepo.Add(approvaltime);
            _aptimeRepo.Savechange();

            return new ApprovalTimeLimitDto
            {
                Id = approvaltime.Id,
                ApprovalAuthorityId = approvaltime.ApprovalAuthorityId,
                Remarks = approvaltime.Remarks,
                TimeLimit = approvaltime.TimeLimit,
                Status = approvaltime.Status,
            };
        }
    }
}