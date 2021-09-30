using System;
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
            try
            {


                var spec = new SBUWiseBudgetSpecificiation(sbuParrams);

                var countSpec = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuParrams);

                var totalItems = await _sbuRepo.CountAsync(countSpec);

                var posts = await _sbuRepo.ListAsync(spec);

                var data = _mapper.Map<IReadOnlyList<SBUWiseBudget>, IReadOnlyList<SBUWiseBudgetDto>>(posts);

                return Ok(new Pagination<SBUWiseBudgetDto>(sbuParrams.PageIndex, sbuParrams.PageSize, totalItems, data));
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("CreateSBUWiseBudget")]
        public async Task<ActionResult<SBUWiseBudget>> SaveSBUBudget(SBUWiseBudget sbuBdgt)
        {
            try
            {
                var alreadyExistSpec = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt);
                var alreadyExistSBUWiseBudgetList = await _sbuRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistSBUWiseBudgetList.Count > 0)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Budget already existed" } });
                }

                var fromDateCheck = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt.FromDate, sbuBdgt.SBU);
                var fromDateCheckList = await _sbuRepo.ListAsync(fromDateCheck);
                var toDateCheck = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt.ToDate, sbuBdgt.SBU);
                var toDateCheckList = await _sbuRepo.ListAsync(toDateCheck);

                if(fromDateCheckList.Count > 0 || toDateCheckList.Count > 0)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Date Range Existed" } });
                }

                var appr = new SBUWiseBudget
                {
                    SBU = sbuBdgt.SBU,
                    SBUName = sbuBdgt.SBUName,
                    FromDate = sbuBdgt.FromDate,
                    ToDate = sbuBdgt.ToDate,
                    Amount = sbuBdgt.Amount,
                    Remarks = sbuBdgt.Remarks,
                    SetOn = DateTimeOffset.Now
                };

                _sbuRepo.Add(appr);
                _sbuRepo.Savechange();

                return new SBUWiseBudget
                {
                    Id = sbuBdgt.Id,
                    SBU = sbuBdgt.SBU,
                    SBUName = sbuBdgt.SBUName,
                    FromDate = sbuBdgt.FromDate,
                    ToDate = sbuBdgt.ToDate,
                    Amount = sbuBdgt.Amount,
                    Remarks = sbuBdgt.Remarks,
                };
            }
            catch (System.Exception)
            {

                throw;
            }
        }


        [HttpPost("ModifySBUWiseBudget")]
        public ActionResult<SBUWiseBudget> UpdateSBUBudget(SBUWiseBudget sbuBdgt)
        {
            var appr = new SBUWiseBudget
            {
                Id = sbuBdgt.Id,
                SBU = sbuBdgt.SBU,
                SBUName = sbuBdgt.SBUName,
                FromDate = sbuBdgt.FromDate,
                ToDate = sbuBdgt.ToDate,
                Amount = sbuBdgt.Amount,
                Remarks = sbuBdgt.Remarks,
                ModifiedOn = DateTimeOffset.Now
            };

            _sbuRepo.Update(appr);
            _sbuRepo.Savechange();

            return new SBUWiseBudget
            {
                Id = sbuBdgt.Id,
                SBU = sbuBdgt.SBU,
                SBUName = sbuBdgt.SBUName,
                FromDate = sbuBdgt.FromDate,
                ToDate = sbuBdgt.ToDate,
                Amount = sbuBdgt.Amount,
                Remarks = sbuBdgt.Remarks,
            };
        }
        [HttpPost("removeSBUWiseBudget")]
        public async Task<IActionResult> RemoveSBUWiseBudget(SBUWiseBudget sbuBdgt)
        {
            try
            {
                var alreadyExistSpec = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt.Id);
                var alreadyExistSubList = await _sbuRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistSubList.Count > 0)
                {
                    foreach (var v in alreadyExistSubList)
                    {
                        _sbuRepo.Delete(v);
                        _sbuRepo.Savechange();
                    }

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
