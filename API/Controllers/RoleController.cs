using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Specifications;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class RoleController : BaseApiController
    {
        private readonly IMapper _mapper;
        private AppIdentityDbContext _context;
        
        public RoleController(IMapper mapper,        
        AppIdentityDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("getRoles")]
        // [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagination<RoleDto>>> getRolesAsync([FromQuery] GenericSpecificationParams roleParams )
        { 
            var roles= await _context.Roles.ToListAsync();
            var data = _mapper
                .Map<IReadOnlyList<RoleDto>>(roles);            
            return Ok(new Pagination<RoleDto>(roleParams.PageIndex, roleParams.PageSize, data.Count, data));
        }
    }
}