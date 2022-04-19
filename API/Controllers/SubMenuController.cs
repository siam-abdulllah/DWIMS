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
    public class SubMenuController : BaseApiController
    {
        private readonly IGenericRepository<SubMenu> _subMenuRepo;
        private readonly IMapper _mapper;
        public SubMenuController(IGenericRepository<SubMenu> SubMenuRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _subMenuRepo = SubMenuRepo;
        }
        [HttpPost("insert")]
        public ActionResult<SubMenu> InsertSubMenu(SubMenu SubMenu)
        {
            var mh = new SubMenu
            {
                SubMenuName = SubMenu.SubMenuName,
                SubMenuSeq = SubMenu.SubMenuSeq,
                Url = SubMenu.Url,
                MenuHeadId = SubMenu.MenuHeadId,
                SetOn = DateTimeOffset.Now

            };
            _subMenuRepo.Add(mh);
            _subMenuRepo.Savechange();

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

            return new SubMenu
            {
                Id = mh.Id,
                SubMenuName = mh.SubMenuName,
                SubMenuSeq = mh.SubMenuSeq
            };
        }
      
        [HttpPost("update")]
        public ActionResult<SubMenu> UpdateSubMenu(SubMenu SubMenu)
        {
            // var user =  _subMenuRepo.GetByIdAsync(SubMenu.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var mh = new SubMenu
            {
                Id = SubMenu.Id,
                SubMenuName = SubMenu.SubMenuName,
                SubMenuSeq = SubMenu.SubMenuSeq,
                Url = SubMenu.Url,
                MenuHeadId = SubMenu.MenuHeadId,
                ModifiedOn = DateTimeOffset.Now

            };
            _subMenuRepo.Update(mh);
            _subMenuRepo.Savechange();
            return new SubMenu
            {
                Id = mh.Id,
                SubMenuName = mh.SubMenuName,
                SubMenuSeq = mh.SubMenuSeq
            };
        }

        [HttpGet("subMenus/{menuHeadId}")]
        public async Task<ActionResult<Pagination<SubMenu>>> GetSubMenus(int menuHeadId,
          [FromQuery] SubMenuSpecParams SubMenuParrams)
        {
            try
            {
                var spec = new SubMenuSpecification(menuHeadId);

                var countSpec = new SubMenuWithFiltersForCountSpecificication(menuHeadId);

                var totalItems = await _subMenuRepo.CountAsync(countSpec);

                var subMenu = await _subMenuRepo.ListAsync(spec);

                //var data = _mapper.Map<IReadOnlyList<SubMenu>, IReadOnlyList<SubMenu>>(SubMenu);

                return Ok(new Pagination<SubMenu>(SubMenuParrams.PageIndex, SubMenuParrams.PageSize, totalItems, subMenu));
                 //var data = await _repository.GetDateWiseProformaInfos(searchDto);
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        } 
      
        [HttpGet("subMenusForMenuConfig/{menuHeadId}")]
        public async Task<IReadOnlyList<SubMenu>> GetsubMenusForMenuConfig(int menuHeadId)
        {
            try
            {
                var spec = new SubMenuSpecification(menuHeadId);
                var subMenu = await _subMenuRepo.ListAsync(spec);
                return subMenu;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        } 
       
         
    }
}