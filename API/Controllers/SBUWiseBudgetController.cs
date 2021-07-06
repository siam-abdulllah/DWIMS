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
    public class SBUWiseBudgetController : BaseApiController
    {
        private readonly IGenericRepository<SBUWiseBudget> _sbuRepo;
        private readonly IMapper _mapper;
        public SBUWiseBudgetController(IGenericRepository<SBUWiseBudget> sbuRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _sbuRepo = sbuRepo;
        }

        [HttpGet("GetAllSBUBudget")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<SBUWiseBudgetDto>>> GetAllSBUWiseBudget([FromQuery] SBUWiseBudgetSpecParams sbuParrams)
        {

            var spec = new SBUWiseBudgetSpecificiation(sbuParrams);

            var countSpec = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuParrams);

            var totalItems = await _sbuRepo.CountAsync(countSpec);

            var posts = await _sbuRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<SBUWiseBudget>, IReadOnlyList<SBUWiseBudgetDto>>(posts);

            return Ok(new Pagination<SBUWiseBudgetDto>(sbuParrams.PageIndex, sbuParrams.PageSize, totalItems, data));
        }

        [HttpPost("CreateSBUWiseBudget")]
        public async Task<ActionResult<SBUWiseBudget>> SaveSBUBudget(SBUWiseBudget sbuBdgt)
        {
            var appr = new SBUWiseBudget
            {
                SBUId = sbuBdgt.SBUId,
                FromDate = sbuBdgt.FromDate,
                ToDate = sbuBdgt.ToDate,
                Amount = sbuBdgt.Amount,
                Remarks = sbuBdgt.Remarks,
            };

            _sbuRepo.Add(appr);
            _sbuRepo.Savechange();

            return new SBUWiseBudget
            {
                Id = sbuBdgt.Id,
                SBUId = sbuBdgt.SBUId,
                FromDate = sbuBdgt.FromDate,
                ToDate = sbuBdgt.ToDate,
                Amount = sbuBdgt.Amount,
                Remarks = sbuBdgt.Remarks,
            };
        }


        [HttpPost("ModifySBUWiseBudget")]
        public async Task<ActionResult<SBUWiseBudget>> UpdateSBUBudget(SBUWiseBudget sbuBdgt)
        {
            var appr = new SBUWiseBudget
            {
                Id = sbuBdgt.Id,
                SBUId = sbuBdgt.SBUId,
                FromDate = sbuBdgt.FromDate,
                ToDate = sbuBdgt.ToDate,
                Amount = sbuBdgt.Amount,
                Remarks = sbuBdgt.Remarks,
            };

            _sbuRepo.Update(appr);
            _sbuRepo.Savechange();

            return new SBUWiseBudget
            {
                Id = sbuBdgt.Id,
                SBUId = sbuBdgt.SBUId,
                FromDate = sbuBdgt.FromDate,
                ToDate = sbuBdgt.ToDate,
                Amount = sbuBdgt.Amount,
                Remarks = sbuBdgt.Remarks,
            };
        }
    }
}
