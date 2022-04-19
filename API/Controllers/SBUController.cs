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
    public class SBUController : BaseApiController
    {
        private readonly IGenericRepository<SBU> _sbuRepo;
        private readonly IMapper _mapper;
        public SBUController(IGenericRepository<SBU> sbuRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _sbuRepo = sbuRepo;
        }

        [HttpGet("GetAllSBU")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<SBUDto>>> GetAllSBU([FromQuery] SBUSpecParams sbuParrams)
        {

            var spec = new SBUSpecificiation(sbuParrams);

            var countSpec = new SBUWithFiltersForCountSpecificication(sbuParrams);

            var totalItems = await _sbuRepo.CountAsync(countSpec);

            var posts = await _sbuRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<SBU>, IReadOnlyList<SBUDto>>(posts);

            return Ok(new Pagination<SBUDto>(sbuParrams.PageIndex, sbuParrams.PageSize, totalItems, data));

        }


    }
}
