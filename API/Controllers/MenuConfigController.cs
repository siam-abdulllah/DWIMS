using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class MenuConfigController : BaseApiController
    {
        private readonly IGenericRepository<MenuConfig> _menuConfigRepo;
        private readonly IGenericRepository<SubMenu> _subMenuRepo; 
        private readonly IGenericRepository<MenuHead> _menuHeadRepo;
        private AppIdentityDbContext _context;
        private readonly IMapper _mapper;
        public MenuConfigController(IGenericRepository<MenuHead> menuHeadRepo,IGenericRepository<MenuConfig> menuConfigRepo, IGenericRepository<SubMenu> SubMenuRepo,
        IMapper mapper,
        AppIdentityDbContext context)
        {
            _mapper = mapper;
            _menuConfigRepo = menuConfigRepo;
            _subMenuRepo = SubMenuRepo;
            _menuHeadRepo = menuHeadRepo;
            _context = context;
        }
        [HttpPost("insert")]
        public ActionResult<MenuConfig> InsertMenuConfig(MenuConfig menuConfig)
        {
            var mh = new MenuConfig
            {
                SubMenuId = menuConfig.SubMenuId,
                RoleId = menuConfig.RoleId,
                SetOn = DateTimeOffset.Now

            };
            _menuConfigRepo.Add(mh);
            _menuConfigRepo.Savechange();

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

            return new MenuConfig
            {
                Id = mh.Id,
                SubMenuId = menuConfig.SubMenuId,
                RoleId = menuConfig.RoleId,
            };
        }
      
        [HttpPost("update")]
        public ActionResult<MenuConfig> UpdateMenuConfig(MenuConfig menuConfig)
        {
            // var user =  _menuConfigRepo.GetByIdAsync(MenuConfig.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var mh = new MenuConfig
            {
                Id = menuConfig.Id,
                SubMenuId = menuConfig.SubMenuId,
                RoleId = menuConfig.RoleId,
                ModifiedOn = DateTimeOffset.Now

            };
            _menuConfigRepo.Update(mh);
            _menuConfigRepo.Savechange();
            return new MenuConfig
            {
                Id = mh.Id,
                SubMenuId = menuConfig.SubMenuId,
                RoleId = menuConfig.RoleId,
            };
        }

        [HttpGet("menuConfigs/{menuHeadId}/{roleId}")]
        public async Task<IReadOnlyList<MenuConfigDto>> GetMenuConfigs(int menuHeadId, string roleId)
        {
            try
            {
                var menuConfig = await _menuConfigRepo.ListAllAsync();
                var subMenu = await _subMenuRepo.ListAllAsync();
                var roles = await _context.Roles.ToListAsync();
                var menuHeads = await _menuHeadRepo.ListAllAsync();
                var menuConfigs = (from mc in menuConfig
                             join s in subMenu on mc.SubMenuId equals s.Id
                             join r in roles on mc.RoleId equals r.Id
                             join m in menuHeads on s.MenuHeadId equals m.Id
                             where s.MenuHeadId == menuHeadId && mc.RoleId == roleId
                             //orderby r.BrandName
                             select new MenuConfigDto
                             {
                                 Id=mc.Id,
                                 MenuHeadName=m.MenuHeadName,
                                 MenuHeadId=m.Id,
                                 SubMenuId=s.Id,
                                 SubMenuName=s.SubMenuName,
                                 RoleId=r.Id,
                                 RoleName=r.Name
                             }
                              ).Distinct().ToList();
                return menuConfigs;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        } 
        [HttpPost("menuConfigsForSecurity")]
        public async Task<IReadOnlyList<MenuConfigDto>> menuConfigsForSecurity(MenuConfigDto menuConfigDto)
        {
            try
            {
                var menuConfig = await _menuConfigRepo.ListAllAsync();
                var subMenu = await _subMenuRepo.ListAllAsync();
                var roles = await _context.Roles.ToListAsync();
                var menuHeads = await _menuHeadRepo.ListAllAsync();
                var menuConfigs = (from mc in menuConfig
                             join s in subMenu on mc.SubMenuId equals s.Id
                             join r in roles on mc.RoleId equals r.Id
                             join m in menuHeads on s.MenuHeadId equals m.Id
                             where s.Url == menuConfigDto.Url && r.Name == menuConfigDto.RoleName
                                   //orderby r.BrandName
                                   select new MenuConfigDto
                             {
                                 Id=mc.Id,
                                 MenuHeadName=m.MenuHeadName,
                                 MenuHeadId=m.Id,
                                 SubMenuId=s.Id,
                                 SubMenuName=s.SubMenuName,
                                 RoleId=r.Id,
                                 RoleName=r.Name
                             }
                              ).Distinct().ToList();
                return menuConfigs;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        } 
        //[HttpGet("menuConfigsForSubMenu")]
        //public async Task<IReadOnlyList<MenuConfig>> GetMenuConfigsForSubMenu()
        //{
        //    try
        //    {
              
        //        var menuConfig = await _menuConfigRepo.ListAllAsync();
        //        return menuConfig;
        //    }
        //    catch (System.Exception ex)
        //    {

        //        throw ex;
        //    }
        //} 
        //[HttpGet("menuConfigsForMenuConfig")]
        //public async Task<IReadOnlyList<MenuConfig>> GetMenuConfigsForMenuConfig()
        //{
        //    try
        //    {
              
        //        var menuConfig = await _menuConfigRepo.ListAllAsync();
        //        return menuConfig;
        //    }
        //    catch (System.Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
       
         
    }
}