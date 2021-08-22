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
        private readonly IGenericRepository<ApprovalAuthority> _approvalAuthorityRepo;
        private readonly IMapper _mapper;
        public ApprovalTimeLimitController(IGenericRepository<ApprovalTimeLimit> aptimeRepo, IGenericRepository<ApprovalAuthority> approvalAuthorityRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _aptimeRepo = aptimeRepo;
            _approvalAuthorityRepo = approvalAuthorityRepo;
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
            var approvalAuthority = await _approvalAuthorityRepo.ListAllAsync();
            //var approvalAuthority = await _approvalAuthorityRepo.ListAllAsync();
            var approvalAuthorityData = _mapper.Map<IReadOnlyList<ApprovalAuthority>, IReadOnlyList<ApprovalAuthorityToReturnDto>>(approvalAuthority);
            var data=(from p in posts
                      join a in approvalAuthorityData on p.ApprovalAuthorityId equals a.Id
                      orderby a.ApprovalAuthorityName
                              select new ApprovalTimeLimitDto
                              {
                                  Id = p.Id,
                                  ApprovalAuthorityId = p.ApprovalAuthorityId,
                                  TimeLimit = p.TimeLimit,
                                  Remarks=p.Remarks,
                                  Status=p.Status,
                                  ApprovalAuthority=a
                              }
                              ).Distinct().ToList();

            //var data = _mapper.Map<IReadOnlyList<ApprovalTimeLimit>, IReadOnlyList<ApprovalTimeLimitDto>>(posts);

            return Ok(new Pagination<ApprovalTimeLimitDto>(approvalParrams.PageIndex, approvalParrams.PageSize, totalItems, data));

        }


        [HttpPost("CreateApprovalTimeLimit")]
        public async Task<ActionResult<ApprovalTimeLimitDto>> SaveApprovalTimeLimit(ApprovalTimeLimitDto setApprovalDto)
        {
            var alreadyExistSpec = new ApprovalTimeWithFiltersForCountSpecificication(setApprovalDto.ApprovalAuthorityId);
            var alreadyExistApprovalTimeLimitList = await _aptimeRepo.ListAsync(alreadyExistSpec);
            if (alreadyExistApprovalTimeLimitList.Count > 0)
            {
                foreach (var v in alreadyExistApprovalTimeLimitList)
                {
                    _aptimeRepo.Delete(v);
                    _aptimeRepo.Savechange();
                }
            }
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

            _aptimeRepo.Update(approvaltime);
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