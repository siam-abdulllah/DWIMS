using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ApprovalAuthorityController : BaseApiController
    {
        private readonly IGenericRepository<ApprovalAuthority> _approvalAuthorityRepo;
        private readonly IMapper _mapper;
        public ApprovalAuthorityController(IGenericRepository<ApprovalAuthority> approvalAuthorityRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _approvalAuthorityRepo = approvalAuthorityRepo;
        }
        [HttpPost("insert")]
        public ActionResult<ApprovalAuthorityToReturnDto> InsertApprovalAuthority(ApprovalAuthorityToReturnDto ApprovalAuthorityToReturnDto)
        {
            var ApprovalAuthority = new ApprovalAuthority
            {
                ApprovalAuthorityName = ApprovalAuthorityToReturnDto.ApprovalAuthorityName,
                Priority = ApprovalAuthorityToReturnDto.Priority,
                Remarks = ApprovalAuthorityToReturnDto.Remarks,
                Status = ApprovalAuthorityToReturnDto.Status
            };
            _approvalAuthorityRepo.Add(ApprovalAuthority);
            _approvalAuthorityRepo.Savechange();

            //if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));
            //var userEntity = await _userManager.FindByEmailAsync(user.Email);
            //try
            //{
            //    string[] roles = setRegDto.RoleForm.UserRoles
            //              .Select(ob => ob.Name).ToArray();
            //    var roleObj = await _userManager.AddToRolesAsync(userEntity, roles);
            //    if (!roleObj.Succeeded) return BadRequest(new ApiResponse(400, "User Role Set Faild."));
            //}
            //catch (Exception ex)
            //{
            //    await _userManager.DeleteAsync(user);
            //}

            return new ApprovalAuthorityToReturnDto
            {
                Id = ApprovalAuthorityToReturnDto.Id,
                ApprovalAuthorityName = ApprovalAuthorityToReturnDto.ApprovalAuthorityName,
                Remarks = ApprovalAuthorityToReturnDto.Remarks
            };
        }
      
        [HttpPost("update")]
        public ActionResult<ApprovalAuthorityToReturnDto> UpdateApprovalAuthority(ApprovalAuthorityToReturnDto ApprovalAuthorityToReturnDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var ApprovalAuthority = new ApprovalAuthority
            {
                Id = ApprovalAuthorityToReturnDto.Id,
                ApprovalAuthorityName = ApprovalAuthorityToReturnDto.ApprovalAuthorityName,
                Priority = ApprovalAuthorityToReturnDto.Priority,
                Remarks = ApprovalAuthorityToReturnDto.Remarks,
                Status = ApprovalAuthorityToReturnDto.Status

            };
            _approvalAuthorityRepo.Update(ApprovalAuthority);
            _approvalAuthorityRepo.Savechange();

            //if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));
            //var userEntity = await _userManager.FindByEmailAsync(user.Email);
            //try
            //{
            //    string[] roles = setRegDto.RoleForm.UserRoles
            //              .Select(ob => ob.Name).ToArray();
            //    var roleObj = await _userManager.AddToRolesAsync(userEntity, roles);
            //    if (!roleObj.Succeeded) return BadRequest(new ApiResponse(400, "User Role Set Faild."));
            //}
            //catch (Exception ex)
            //{
            //    await _userManager.DeleteAsync(user);
            //}

            return new ApprovalAuthorityToReturnDto
            {
                Id = ApprovalAuthorityToReturnDto.Id,
                ApprovalAuthorityName = ApprovalAuthorityToReturnDto.ApprovalAuthorityName,
                Remarks = ApprovalAuthorityToReturnDto.Remarks,
                Status = ApprovalAuthorityToReturnDto.Status
            };
        }

        [HttpGet("approvalAuthorities")]
        public async Task<ActionResult<Pagination<ApprovalAuthorityToReturnDto>>> GetApprovalAuthorities(
          [FromQuery] ApprovalAuthoritySpecParams approvalAuthorityParrams)
        {
            try
            {
                var spec = new ApprovalAuthoritySpecification(approvalAuthorityParrams);

                var countSpec = new ApprovalAuthorityWithFiltersForCountSpecificication(approvalAuthorityParrams);

                var totalItems = await _approvalAuthorityRepo.CountAsync(countSpec);

                var approvalAuthority = await _approvalAuthorityRepo.ListAsync(spec);

                var data = _mapper.Map<IReadOnlyList<ApprovalAuthority>, IReadOnlyList<ApprovalAuthorityToReturnDto>>(approvalAuthority);

                return Ok(new Pagination<ApprovalAuthorityToReturnDto>(approvalAuthorityParrams.PageIndex, approvalAuthorityParrams.PageSize, totalItems, data));
                 //var data = await _repository.GetDateWiseProformaInfos(searchDto);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
       
        [HttpGet("approvalAuthoritiesForConfig")]
        public async Task<IReadOnlyList<ApprovalAuthority>> GetApprovalAuthorities()
        {
            try
            {
                var spec = new ApprovalAuthoritySpecification("Active");
                var totalAmtValidityDto = await _approvalAuthorityRepo.ListAsync(spec);
                return totalAmtValidityDto;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}