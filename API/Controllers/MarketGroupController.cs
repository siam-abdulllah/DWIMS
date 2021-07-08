using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    
    public class MarketGroupController : BaseApiController
    {
        private readonly IGenericRepository<MarketGroupMst> _marketGroupMstRepo;
        private readonly IMapper _mapper;
        public MarketGroupController(IGenericRepository<MarketGroupMst> bcdsRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _marketGroupMstRepo = bcdsRepo;
        }
        [HttpPost("insert")]
        public ActionResult<MarketGroupMstDto> InsertMarketGroup(MarketGroupMstDto marketGroupMstDto)
        {
            var marketGroupMsts = new MarketGroupMst
            {
                GroupName = marketGroupMstDto.GroupName,
                EmployeeId = marketGroupMstDto.EmployeeId,
                Status = marketGroupMstDto.Status,
                SetOn = DateTimeOffset.Now
            };
            _marketGroupMstRepo.Add(marketGroupMsts);
            _marketGroupMstRepo.Savechange();

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

            return new MarketGroupMstDto
            {
                Id = marketGroupMsts.Id,
                GroupName = marketGroupMsts.GroupName,
                EmployeeId = marketGroupMsts.EmployeeId,
                Status = marketGroupMsts.Status
            };
        }
        [HttpPost("update")]
        public ActionResult<MarketGroupMstDto> UpdateMarketGroup(MarketGroupMstDto marketGroupMstDto)
        {
            // var user =  _marketGroupMstRepo.GetByIdAsync(marketGroupMstDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var donation = new MarketGroupMst
            {
                Id = marketGroupMstDto.Id,
                GroupName = marketGroupMstDto.GroupName,
                EmployeeId = marketGroupMstDto.EmployeeId,
                Status = marketGroupMstDto.Status,
                ModifiedOn = DateTimeOffset.Now

            };
            _marketGroupMstRepo.Update(donation);
            _marketGroupMstRepo.Savechange();

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

            return new MarketGroupMstDto
            {
                Id = marketGroupMstDto.Id,
                GroupName = marketGroupMstDto.GroupName,
                EmployeeId = marketGroupMstDto.EmployeeId,
                Status = marketGroupMstDto.Status
            };
        }

        [HttpGet("donations")]
        public async Task<ActionResult<Pagination<MarketGroupMstDto>>> GetMarketGroups(
          [FromQuery] MarketGroupSpecParams marketGroupParrams)
        {
            try
            {
                var spec = new MarketGroupSpecification(marketGroupParrams);

                var countSpec = new MarketGroupWithFiltersForCountSpecificication(marketGroupParrams);

                var totalItems = await _marketGroupMstRepo.CountAsync(countSpec);

                var marketGroup = await _marketGroupMstRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<MarketGroupMst>, IReadOnlyList<MarketGroupMstDto>>(marketGroup);

                return Ok(new Pagination<MarketGroupMstDto>(marketGroupParrams.PageIndex, marketGroupParrams.PageSize, totalItems, data));
            }
            catch (System.Exception)
            {

                throw;
            }
        }


    }
}
