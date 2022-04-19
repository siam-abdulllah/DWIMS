using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EmpPostController : BaseApiController
    {
        private readonly IGenericRepository<EmployeePosting> _empRepo;
        private readonly IMapper _mapper;

        public EmpPostController(IGenericRepository<EmployeePosting> empRepo,
  IMapper mapper)
        {
            _mapper = mapper;
            _empRepo = empRepo;
        }


        [HttpGet("GetAllEmpPosting")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<EmployeePostingDto>>> GetAllEmployeePosting([FromQuery] EmpPostSpecParams empParrams)
        {

            var spec = new EmpPostSpecification(empParrams);

            var countSpec = new EmpPostWithFiltersForCountSpecificication(empParrams);

            var totalItems = await _empRepo.CountAsync(countSpec);

            var posts = await _empRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<EmployeePosting>, IReadOnlyList<EmployeePostingDto>>(posts);

            return Ok(new Pagination<EmployeePostingDto>(empParrams.PageIndex, empParrams.PageSize, totalItems, data));

        }
    }
}
