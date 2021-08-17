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

        [HttpGet("GetInsSocietyBCDSWiseInvestment")]
        //[Authorize(Roles = "Owner,Administrator")]
        //[Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<ReportInvestmentDto>>> GetInstituteInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams)
        {
    

            var spec = new ReportInvestmentInfoSpecification(rptParrams);

            var countSpec = new ReportInvestmentInfoSpecParamsWithFiltersForCountSpecificication(rptParrams);

            var totalItems = await _investRepo.CountAsync(countSpec);

            var posts = await _investRepo.ListAsync(spec);

            //var t = from x in posts


            var data = _mapper.Map<IReadOnlyList<ReportInvestmentInfo>, IReadOnlyList<ReportInvestmentDto>>(posts);

            return Ok(new Pagination<ReportInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, totalItems, data));
        }



        [HttpGet("GetDoctorWiseLeadership")]
        //[Authorize(Roles = "Owner,Administrator")]
        //[Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<ReportInvestmentDto>>> GetDoctorWiseLeadership([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        {
            var spec = new ReportInvestmentInfoSpecification(search.FromDate.ToString(), search.ToDate.ToString());

            var countSpec = new ReportInvestmentInfoSpecParamsWithFiltersForCountSpecificication(rptParrams);

            var totalItems = await _investRepo.CountAsync(countSpec);

            var posts = await _investRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ReportInvestmentInfo>, IReadOnlyList<ReportInvestmentDto>>(posts);

            return Ok(new Pagination<ReportInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, totalItems, data));
        }
    }
}
