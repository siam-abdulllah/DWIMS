using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using AutoMapper.Configuration;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericIdentityRepository<AppUser> _userRepo;

        // private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private AppIdentityDbContext _context;
        private readonly StoreContext _db;
        // private readonly IConfiguration _config;
        public AccountController(UserManager<AppUser> userManager,
        // IConfiguration config, 
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService,
        IMapper mapper,
        IGenericIdentityRepository<AppUser> userRepo, StoreContext db,
        AppIdentityDbContext context)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            // _roleManager = roleManager;
            // _config = config;
            _userRepo = userRepo;
            _context = context;
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

            IList<string> roles = await _userManager.GetRolesAsync(user);

            if (roles.Count == 0)
            {
                return BadRequest(new ApiResponse(400, "No Roles assigned to user - " + user.UserName));
            }
            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user, roles),
                DisplayName = user.DisplayName
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet("userNameexists")]
        public async Task<ActionResult<bool>> CheckUserNameExistsAsync([FromQuery] string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }


        [HttpGet("getUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsersToReturnDto>> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return BadRequest(new ApiResponse(400, "User Not Found."));

            UsersToReturnDto data = _mapper.Map<AppUser, UsersToReturnDto>(user);
            if (data == null) return BadRequest(new ApiResponse(400, "Invalid User Requet."));

            IList<string> roles = await _userManager.GetRolesAsync(user);
            data.Roles = roles;
            data.Claims = await _userManager.GetClaimsAsync(user);
            return data;
        }


        [Authorize]
        [HttpPut("update-address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindByUserByClaimsPrincipleWithAddressAsync(HttpContext.User);

            user.Address = _mapper.Map<AddressDto, Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<Address, AddressDto>(user.Address));

            return BadRequest("Problem updating the user");
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            try
            {

                var user = await _userManager.FindByNameAsync(loginDto.UserName);

                if (user == null) return Unauthorized(new ApiResponse(401));

                if (!await _userManager.IsEmailConfirmedAsync(user)) return Unauthorized(new ApiResponse(401, "Check your email to confirm your email address."));

                if (await _userManager.IsLockedOutAsync(user)) return Unauthorized(new ApiResponse(401, "Your user account has locked out for a while"));

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, lockoutOnFailure: true);

                if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

                IList<string> roles = await _userManager.GetRolesAsync(user);

                if (roles.Count == 0)
                {
                    return BadRequest(new ApiResponse(400, "No Roles assigned to user - " + user.UserName));
                }
                var employeesForConfigByEmpIdList = EmployeesForConfigByEmpId(user.EmployeeId);
                if (employeesForConfigByEmpIdList.Count == 0)
                {
                    return BadRequest(new ApiResponse(400, "No Approval Authority assigned to user - " + user.UserName));
                }
                var menuList = MenuConfigsForSecurity(roles[0]);
                return new UserDto
                {
                    EmployeeId = user.EmployeeId,
                    //Email = user.Email,
                    Token = _tokenService.CreateToken(user, roles),
                    DisplayName = user.DisplayName,
                    MenuList = menuList
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("menuConfigsForSecurity")]
        public List<MenuConfigDto> MenuConfigsForSecurity(string roleName)
        {
            try
            {
                var menuConfig = _db.MenuConfig.ToList();
                var subMenu = _db.SubMenu.ToList();
                var roles = _context.Roles.ToList();
                var menuHeads = _db.MenuHead.ToList();
                var menuConfigs = (from mc in menuConfig
                                   join s in subMenu on mc.SubMenuId equals s.Id
                                   join r in roles on mc.RoleId equals r.Id
                                   join m in menuHeads on s.MenuHeadId equals m.Id
                                   orderby m.MenuHeadSeq
                                   where
                                   //s.Url == menuConfigDto.Url 
                                   //&& 
                                   r.Name == roleName
                                   //orderby r.BrandName
                                   select new MenuConfigDto
                                   {
                                       Id = mc.Id,
                                       MenuHeadName = m.MenuHeadName,
                                       MenuHeadId = m.Id,
                                       SubMenuId = s.Id,
                                       SubMenuName = s.SubMenuName,
                                       RoleId = r.Id,
                                       RoleName = r.Name,
                                       Url = s.Url
                                   }
                              ).Distinct().ToList();
                return menuConfigs;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
       
        [HttpGet("employeesForConfigByEmpId")]
        public List<ApprAuthConfigDto> EmployeesForConfigByEmpId(int empId)
        {
            try
            {
                var apprAuthConfig = _db.ApprAuthConfig.ToList();
                var apprAuthConfigs = (from a in apprAuthConfig
                                       where
                                       //s.Url == menuConfigDto.Url 
                                       //&& 
                                       a.EmployeeId == empId && a.Status == "A"
                                       //orderby r.BrandName
                                       select new ApprAuthConfigDto
                                       {
                                           Id = a.Id,

                                       }
                              ).Distinct().ToList();
                return apprAuthConfigs;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(SetRegisterDto setRegDto)
        {

            if (CheckUserNameExistsAsync(setRegDto.UserForm.EmployeeSAPCode).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "User Id is in use" } });
            }

            var user = new AppUser
            {
                EmployeeId = setRegDto.UserForm.EmployeeId,
                EmployeeSAPCode = setRegDto.UserForm.EmployeeSAPCode,
                DisplayName = setRegDto.UserForm.DisplayName,
                Email = setRegDto.UserForm.Email,
                UserName = setRegDto.UserForm.EmployeeSAPCode,
                EmailConfirmed = false,
                PhoneNumber = setRegDto.UserForm.PhoneNumber

            };
            var userObj = await _userManager.CreateAsync(user, setRegDto.UserForm.Password);
            if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));
            // var userEntity = await _userManager.FindByEmailAsync(user.Email);
            //try{
            //      string[] roles = setRegDto.RoleForm.UserRoles
            //                .Select(ob=>ob.Name).ToArray();
            //      var roleObj = await _userManager.AddToRolesAsync(userEntity, roles);
            //      if(!roleObj.Succeeded) return BadRequest(new ApiResponse(400, "User Role Set Faild."));
            //}
            //catch(Exception )
            //{
            //    await _userManager.DeleteAsync(user);               
            //}

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = "",//_tokenService.CreateToken(user),
                Email = user.Email
            };
        } 
        [HttpPost("registerAll")]
        public async Task<ActionResult<bool>> RegisterAll()
        {
           

            var emps =  _db.Employee.ToList();
            foreach (var e in emps)
            {
                if (CheckUserNameExistsAsync(e.EmployeeSAPCode).Result.Value)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "User Id is in use" } });
                }

                var user = new AppUser
                {
                    EmployeeId = e.Id,
                    EmployeeSAPCode = e.EmployeeSAPCode,
                    DisplayName = e.EmployeeName,
                    Email = e.Email,
                    UserName = e.EmployeeSAPCode,
                    EmailConfirmed = true,
                    PhoneNumber = e.Phone

                };
                var userObj = await _userManager.CreateAsync(user, "@Aa123");
                if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));


             

            }

            return true;


        }

        [HttpPost("updateRegisterUser")]
        public async Task<ActionResult<UserDto>> UpdateRegisterUser(SetRegisterDto setRegDto)
        {
            var user = await _userManager.FindByIdAsync(setRegDto.UserForm.Id);
            if (user == null) return Unauthorized(new ApiResponse(401));

            user = new AppUser
            {
                DisplayName = setRegDto.UserForm.DisplayName,
                PhoneNumber = setRegDto.UserForm.PhoneNumber
            };

            var userObj = await _userManager.UpdateAsync(user);

            if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = setRegDto.UserForm.DisplayName,
                PhoneNumber = setRegDto.UserForm.PhoneNumber,
                Email = user.Email
            };
        }

        // [HttpGet("generateEmailConfirmationTokenAsync")]
        // private async Task<string> GenerateEmailConfirmationTokenAsync(string email)
        // {
        //     var userEntity = await _userManager.FindByEmailAsync(email);
        //     var tokenGenerated = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
        //     byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(tokenGenerated);
        //     var codeEncoded = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
        //     var link = _config["Token:Client"]+"/account/user-email-verification/"+userEntity.Email+"/"+codeEncoded; 
        //     return link;
        // }



        [HttpGet("users")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<UsersToReturnDto>>> GetAllUsers([FromQuery] UserSpecParams postParrams)
        {
            var spec = new UserSpecification(postParrams);

            var countSpec = new UserWithFiltersForCountSpecificication(postParrams);

            var totalItems = await _userRepo.CountAsync(countSpec);

            var posts = await _userRepo.ListAsync(spec);

            var data = _mapper
                .Map<IReadOnlyList<AppUser>, IReadOnlyList<UsersToReturnDto>>(posts);

            return Ok(new Pagination<UsersToReturnDto>(postParrams.PageIndex, postParrams.PageSize, totalItems, data));
        }

        //[HttpPost("updateRegApproval")]
        //public Task<ActionResult<UserDto>> UpdateRegApproval(RegApprovalDto regApprovalDto)
        //{

        //    var user = new AppUser
        //    {
        //        Id= regApprovalDto.UserId
        //    };
        //    if (regApprovalDto.ApprovalStatus == "Approved")
        //    {
        //        user.EmailConfirmed = true;
        //    }
        //    else {
        //        user.EmailConfirmed = false;
        //    }
        //    _userManager.UpdateAsync(user);
        //    //_userRepo.Savechange();

        //    return new UserDto
        //    {
        //        DisplayName = user.DisplayName,
        //        PhoneNumber = user.PhoneNumber,
        //        Email = user.Email
        //    };
        //}

        [HttpPost("updateRegApproval")]
        public async Task<ActionResult<UserDto>> UpdateRegisterUserAppr(RegApprovalDto regApprovalDto)
        {
            var user = await _userManager.FindByIdAsync(regApprovalDto.UserId);
            if (user == null) return Unauthorized(new ApiResponse(401));

            //user = new AppUser
            //{
            //};
            if (regApprovalDto.ApprovalStatus == "Approved")
            {
                user.EmailConfirmed = true;
            }
            else
            {
                user.EmailConfirmed = false;
            }
            var userObj = await _userManager.UpdateAsync(user);

            if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));
            //var userEntity = await _userManager.FindByEmailAsync(user.Email);
            try
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);

                if (roles.Count > 0)
                {
                    var removeRoleObj = await _userManager.RemoveFromRolesAsync(user, roles);
                }
                var roleObj = await _userManager.AddToRoleAsync(user, regApprovalDto.Role);
                if (!roleObj.Succeeded) return BadRequest(new ApiResponse(400, "User Role Set Faild."));
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
            }
            return new UserDto
            {
                Email = user.Email
            };
        }

        //[Authorize(Roles = "Importer")]
        [HttpPost("VerifyCurrentPassword")]
        public async Task<ActionResult<bool>> VerifyCurrentPassword(VerifyCrntPassDto verifyCrntPassDto)
        {
            var user = await _userManager.FindByNameAsync(verifyCrntPassDto.EmployeeSAPCode);
            var result = await _userManager.CheckPasswordAsync(user, verifyCrntPassDto.CurrentPassword);
            if (result) return true;
            return false;
        }
        
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(changePasswordDto.EmployeeSAPCode);

                var userObj = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

                if (userObj.Succeeded) return true;
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}