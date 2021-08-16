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
    public class ReportInvestmentController : BaseApiController
    {
        private readonly IGenericRepository<ReportInvestmentInfo> _investRepo;
        private readonly IMapper _mapper;
        public ReportInvestmentController(IGenericRepository<ReportInvestmentInfo> investRepo, IMapper mapper)
        {
            _mapper = mapper;
            _investRepo = investRepo;
        }

        [HttpGet("GetInstituteInvestment")]
        //[Authorize(Roles = "Owner,Administrator")]
        //[Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<ReportInvestmentDto>>> GetInstituteInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams)
        {
            string from = "", to = "";

            var spec = new ReportInvestmentInfoSpecification(from, to);

            var countSpec = new ReportInvestmentInfoSpecParamsWithFiltersForCountSpecificication(rptParrams);

            var totalItems = await _investRepo.CountAsync(countSpec);

            var posts = await _investRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ReportInvestmentInfo>, IReadOnlyList<ReportInvestmentDto>>(posts);

            return Ok(new Pagination<ReportInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, totalItems, data));
        }
    }
}
