using API.Dtos;
using API.Errors;
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
    public class SubCampaignController : BaseApiController
    {
        private readonly IGenericRepository<SubCampaign> _subCampaignRepo;
        private readonly IMapper _mapper;
        public SubCampaignController(IGenericRepository<SubCampaign> subCampaignRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _subCampaignRepo = subCampaignRepo;
        }
        [HttpPost("insert")]
        public ActionResult<SubCampaignToReturnDto> InsertSubCampaign(SubCampaignToReturnDto subCampaignToReturnDto)
        {

            var subCampaign = new SubCampaign
            {
                SubCampaignName = subCampaignToReturnDto.SubCampaignName,
                Remarks = subCampaignToReturnDto.Remarks,
                Status = subCampaignToReturnDto.Status,
                SetOn = DateTimeOffset.Now
            };
            _subCampaignRepo.Add(subCampaign);
            _subCampaignRepo.Savechange();

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

            return new SubCampaignToReturnDto
            {
                Id = subCampaignToReturnDto.Id,
                SubCampaignName = subCampaignToReturnDto.SubCampaignName,
                Remarks = subCampaignToReturnDto.Remarks
            };
        }
     
        [HttpPost("update")]
        public ActionResult<SubCampaignToReturnDto> UpdateSubCampaign(SubCampaignToReturnDto subCampaignToReturnDto)
        {
            // var user =  _subCampaignRepo.GetByIdAsync(subCampaignToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var subCampaign = new SubCampaign
            {
                Id = subCampaignToReturnDto.Id,
                SubCampaignName = subCampaignToReturnDto.SubCampaignName,
                Remarks = subCampaignToReturnDto.Remarks,
                Status = subCampaignToReturnDto.Status,
                ModifiedOn = DateTimeOffset.Now

            };
            _subCampaignRepo.Update(subCampaign);
            _subCampaignRepo.Savechange();
            return new SubCampaignToReturnDto
            {
                Id = subCampaignToReturnDto.Id,
                SubCampaignName = subCampaignToReturnDto.SubCampaignName,
                Remarks = subCampaignToReturnDto.Remarks,
                Status = subCampaignToReturnDto.Status
            };
        }

        [HttpGet("subCampaigns")]
        public async Task<ActionResult<Pagination<SubCampaignToReturnDto>>> GetSubCampaigns(
          [FromQuery] SubCampaignSpecParams subCampaignParrams)
        {
            try
            {
                var spec = new SubCampaignWithCommentsSpecification(subCampaignParrams);

                var countSpec = new SubCampaignWithFiltersForCountSpecificication(subCampaignParrams);

                var totalItems = await _subCampaignRepo.CountAsync(countSpec);

                var subCampaign = await _subCampaignRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<SubCampaign>, IReadOnlyList<SubCampaignToReturnDto>>(subCampaign);

                return Ok(new Pagination<SubCampaignToReturnDto>(subCampaignParrams.PageIndex, subCampaignParrams.PageSize, totalItems, data));
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        [HttpGet("subCampaignsForCamp")]
        public async Task<IReadOnlyList<SubCampaignToReturnDto>> GetSubCampaign()
        {
            try
            {

                var data = await _subCampaignRepo.ListAllAsync();
                var subCampaign = (from r in data
                                   where r.Status == "Active"
                                   orderby r.SubCampaignName
                                   select new SubCampaignToReturnDto
                                   {
                                       SubCampaignName = r.SubCampaignName.Trim(),
                                       Id = r.Id
                                   }
                              ).Distinct().ToList();
                return subCampaign;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}