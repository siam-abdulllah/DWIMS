using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IGenericRepository<YearlyBudget> _yearlyBudgetRepo;
        private readonly IMapper _mapper;
        public SBUWiseBudgetController(IGenericRepository<SBUWiseBudget> sbuRepo, IGenericRepository<YearlyBudget> yearlyBudgetRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _sbuRepo = sbuRepo;
            _yearlyBudgetRepo = yearlyBudgetRepo;
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

        [HttpPost("CreateSBUWiseBudget/{year}/{amount}")]
        public async Task<ActionResult<SBUWiseBudget>> SaveSBUBudget(SBUWiseBudget sbuBdgt,string year,string amount)
        {
            try
            {
                var alreadyExistYearlyBudgetSpec = new YearlyBudgetSpecificiation(year);
                var alreadyExistYearlyBudget = await _yearlyBudgetRepo.GetEntityWithSpec(alreadyExistYearlyBudgetSpec);
                if (alreadyExistYearlyBudget != null)
                {
                    var yearlyBUdget = new YearlyBudget
                    {
                        Id= alreadyExistYearlyBudget.Id,
                        Year = alreadyExistYearlyBudget.Year,
                        Amount = Convert.ToInt32(amount),
                        FromDate = alreadyExistYearlyBudget.FromDate,
                        ToDate = alreadyExistYearlyBudget.ToDate,
                        SetOn = alreadyExistYearlyBudget.SetOn,
                        ModifiedOn = DateTimeOffset.Now
                    };

                    _yearlyBudgetRepo.Update(yearlyBUdget);
                    _yearlyBudgetRepo.Savechange();
                }
                else {
                    var yearlyBUdget = new YearlyBudget
                    {
                        Year = year,
                        Amount = Convert.ToInt32(amount),
                        FromDate = new DateTime(Convert.ToInt16(year), 1, 1),
                        ToDate = new DateTime(Convert.ToInt16(year), 12, 31),
                        SetOn = DateTimeOffset.Now
                    };

                    _yearlyBudgetRepo.Add(yearlyBUdget);
                    _yearlyBudgetRepo.Savechange();
                }
                var alreadyExistSpec = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt);
                var alreadyExistSBUWiseBudgetList = await _sbuRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistSBUWiseBudgetList.Count > 0)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Budget Already Existed" } });
                }
                var fromDateCheck = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt.FromDate, sbuBdgt.SBU,sbuBdgt.DonationId);
                var fromDateCheckList = await _sbuRepo.ListAsync(fromDateCheck);
                var toDateCheck = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt.ToDate, sbuBdgt.SBU, sbuBdgt.DonationId);
                var toDateCheckList = await _sbuRepo.ListAsync(toDateCheck);
                if(fromDateCheckList.Count > 0 || toDateCheckList.Count > 0)
                {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Date Range Existed" } });
                }

                var appr = new SBUWiseBudget
                {
                    SBU = sbuBdgt.SBU,
                    SBUName = sbuBdgt.SBUName,
                    DonationId = sbuBdgt.DonationId,
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
                    DonationId = sbuBdgt.DonationId,
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


        [HttpPost("ModifySBUWiseBudget/{year}/{amount}")]
        public async Task<ActionResult<SBUWiseBudget>> UpdateSBUBudget(SBUWiseBudget sbuBdgt, string year, string amount)
        {
            var alreadyExistYearlyBudgetSpec = new YearlyBudgetSpecificiation(year);
            var alreadyExistYearlyBudget = await _yearlyBudgetRepo.GetEntityWithSpec(alreadyExistYearlyBudgetSpec);
            if (alreadyExistYearlyBudget != null)
            {
                var yearlyBUdget = new YearlyBudget
                {
                    Id = alreadyExistYearlyBudget.Id,
                    Year = alreadyExistYearlyBudget.Year,
                    Amount = Convert.ToInt32(amount),
                    FromDate = alreadyExistYearlyBudget.FromDate,
                    ToDate = alreadyExistYearlyBudget.ToDate,
                    SetOn = alreadyExistYearlyBudget.SetOn,
                    ModifiedOn = DateTimeOffset.Now
                };

                _yearlyBudgetRepo.Update(yearlyBUdget);
                _yearlyBudgetRepo.Savechange();
            }
            else
            {
                var yearlyBUdget = new YearlyBudget
                {
                    Year = year,
                    Amount = Convert.ToInt32(amount),
                    FromDate = new DateTime(Convert.ToInt16(year), 1, 1),
                    ToDate = new DateTime(Convert.ToInt16(year), 12, 31),
                    SetOn = DateTimeOffset.Now
                };

                _yearlyBudgetRepo.Add(yearlyBUdget);
                _yearlyBudgetRepo.Savechange();
            }
            var fromDateCheck = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt.Id, sbuBdgt.FromDate, sbuBdgt.SBU, sbuBdgt.DonationId);
            var fromDateCheckList = await _sbuRepo.ListAsync(fromDateCheck);
            var toDateCheck = new SBUWiseBudgetWithFiltersForCountSpecificication(sbuBdgt.Id, sbuBdgt.ToDate, sbuBdgt.SBU, sbuBdgt.DonationId);
            var toDateCheckList = await _sbuRepo.ListAsync(toDateCheck);

            if (fromDateCheckList.Count > 0 || toDateCheckList.Count > 0)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Date Range Existed" } });
            }
            var appr = new SBUWiseBudget
            {
                Id = sbuBdgt.Id,
                SBU = sbuBdgt.SBU,
                SBUName = sbuBdgt.SBUName,
                DonationId = sbuBdgt.DonationId,
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
                DonationId = sbuBdgt.DonationId,
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
        [HttpGet("getYearlyTotalAmount/{year}")]
        public async Task<ActionResult<YearlyBudget>> GetYearlyTotalAmount(string year)
        {
            try
            {
                var alreadyExistSpec = new YearlyBudgetSpecificiation(year);
                var alreadyExistYearlyBudget = await _yearlyBudgetRepo.GetEntityWithSpec(alreadyExistSpec);
                
                return alreadyExistYearlyBudget;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }  
        [HttpGet("getYearlyTotalExpense/{year}")]
        public async Task<ActionResult<string>> GetYearlyTotalExpense(string year)
        {
            try
            {
                var allData = await _sbuRepo.ListAllAsync();
                string totalExpense = "";
                //totalExpense=allData.AsEnumerable().Where(x=>x.FromDate >= new DateTime(year, 1, 1) && x.ToDate <= new DateTime(year, 12, 31)).Sum(o => o.Amount).ToString();
                var items = (from o in allData
                                   where o.FromDate.Value.Date >= DateTime.ParseExact("01/01/"+year, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                                   && o.ToDate.Value.Date <= DateTime.ParseExact("31/12/" + year, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                             // && o.ToDate <= new DateTime(year, 12, 31)
                             select new { o.Amount,o.FromDate,o.ToDate,o.Id}
                  ).ToList();
                totalExpense = items.Select(c=>c.Amount).Sum().ToString();
                //var market = (from r in allData
                //              where r.FromDate!=null
                //              select new MarketDto
                //              {
                //                  MarketCode = r.MarketCode,
                //                  MarketName = r.MarketName,
                //                  SBU = r.SBU,
                //                  SBUName = r.SBUName,
                //              }
                //              ).First();
                return totalExpense;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
