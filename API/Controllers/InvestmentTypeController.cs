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
    public class InvestmentTypeController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentType> _invRepo;
        private readonly IMapper _mapper;

        public InvestmentTypeController(IGenericRepository<InvestmentType> invRepo,
       IMapper mapper)
        {
            _mapper = mapper;
            _invRepo = invRepo;
        }


        [HttpGet("GetAllInvestType")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<InvestmentTypeDto>>> GetAllInvestType([FromQuery] InvestmentTypeSpecParams bscdParrams)
        {

            var spec = new InvestmentTypeSpecification(bscdParrams);

            var countSpec = new InvestmentTypeWithFiltersForCountSpecificication(bscdParrams);

            var totalItems = await _invRepo.CountAsync(countSpec);

            var posts = await _invRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<InvestmentType>, IReadOnlyList<InvestmentTypeDto>>(posts);

            return Ok(new Pagination<InvestmentTypeDto>(bscdParrams.PageIndex, bscdParrams.PageSize, totalItems, data));

        }
    }
}
