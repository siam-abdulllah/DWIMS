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
using System.Threading.Tasks;

namespace API.Controllers
{
    public class MenuHeadController : BaseApiController
    {
        private readonly IGenericRepository<MenuHead> _menuHeadRepo;
        private readonly IMapper _mapper;
        public MenuHeadController(IGenericRepository<MenuHead> menuHeadRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _menuHeadRepo = menuHeadRepo;
        }
        [HttpPost("insert")]
        public ActionResult<MenuHead> InsertMenuHead(MenuHead menuHead)
        {
            var mh = new MenuHead
            {
                MenuHeadName = menuHead.MenuHeadName,
                MenuHeadSeq = menuHead.MenuHeadSeq,
                SetOn = DateTimeOffset.Now

            };
            _menuHeadRepo.Add(mh);
            _menuHeadRepo.Savechange();

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

            return new MenuHead
            {
                Id = mh.Id,
                MenuHeadName = mh.MenuHeadName,
                MenuHeadSeq = mh.MenuHeadSeq
            };
        }
      
        [HttpPost("update")]
        public ActionResult<MenuHead> UpdateMenuHead(MenuHead menuHead)
        {
            // var user =  _menuHeadRepo.GetByIdAsync(MenuHead.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var mh = new MenuHead
            {
                Id = menuHead.Id,
                MenuHeadName = menuHead.MenuHeadName,
                MenuHeadSeq = menuHead.MenuHeadSeq,
                ModifiedOn = DateTimeOffset.Now

            };
            _menuHeadRepo.Update(mh);
            _menuHeadRepo.Savechange();
            return new MenuHead
            {
                Id = mh.Id,
                MenuHeadName = mh.MenuHeadName,
                MenuHeadSeq = mh.MenuHeadSeq
            };
        }

        [HttpGet("menuHeads")]
        public async Task<ActionResult<Pagination<MenuHead>>> GetMenuHeads(
          [FromQuery] MenuHeadSpecParams menuHeadParrams)
        {
            try
            {
                var spec = new MenuHeadSpecification(menuHeadParrams);

                var countSpec = new MenuHeadWithFiltersForCountSpecificication(menuHeadParrams);

                var totalItems = await _menuHeadRepo.CountAsync(countSpec);

                var menuHead = await _menuHeadRepo.ListAsync(spec);

                //var data = _mapper.Map<IReadOnlyList<MenuHead>, IReadOnlyList<MenuHead>>(menuHead);

                return Ok(new Pagination<MenuHead>(menuHeadParrams.PageIndex, menuHeadParrams.PageSize, totalItems, menuHead));
                 //var data = await _repository.GetDateWiseProformaInfos(searchDto);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        } 
        [HttpGet("menuHeadsForSubMenu")]
        public async Task<IReadOnlyList<MenuHead>> GetMenuHeadsForSubMenu()
        {
            try
            {
              
                var menuHead = await _menuHeadRepo.ListAllAsync();
                return menuHead;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        } 
        [HttpGet("menuHeadsForMenuConfig")]
        public async Task<IReadOnlyList<MenuHead>> GetMenuHeadsForMenuConfig()
        {
            try
            {
              
                var menuHead = await _menuHeadRepo.ListAllAsync();
                return menuHead;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
       
         
    }
}