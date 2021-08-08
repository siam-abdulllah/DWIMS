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
    public class SocietyController : BaseApiController
    {
        private readonly IGenericRepository<Society> _societyRepo;
        private readonly IMapper _mapper;
        public SocietyController(IGenericRepository<Society> societyRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _societyRepo = societyRepo;
        }


        [HttpGet("GetAllSociety")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<SocietyDto>>> GetAllSociety([FromQuery] SocietySpecParams societyParrams)
        {

            var spec = new SocietySpecification(societyParrams);

            var countSpec = new SocietyWithFiltersForCountSpecificication(societyParrams);

            var totalItems = await _societyRepo.CountAsync(countSpec);

            var posts = await _societyRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Society>, IReadOnlyList<SocietyDto>>(posts);

            return Ok(new Pagination<SocietyDto>(societyParrams.PageIndex, societyParrams.PageSize, totalItems, data));

        }[HttpGet("societyForInvestment")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<IReadOnlyList<SocietyDto>> GetSocietyForInvestment()
        {
            try
            {

          
            var spec = new SocietySpecification();
            var posts = await _societyRepo.ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<Society>, IReadOnlyList<SocietyDto>>(posts);

            return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        [HttpPost("CreateSociety")]
        public ActionResult<SocietyDto> SaveSociety(SocietyDto setsocietyDto)
        {
            var society = new Society
            {
                SocietyName = setsocietyDto.SocietyName,
                SocietyAddress = setsocietyDto.SocietyAddress,
                NoOfMember = setsocietyDto.NoOfMember,
                Status = setsocietyDto.Status,
                SetOn = DateTimeOffset.Now
        };

            _societyRepo.Add(society);
            _societyRepo.Savechange();

            return new SocietyDto
            {
                Id = society.Id,
                SocietyName = society.SocietyName,
                SocietyAddress = society.SocietyAddress,
                NoOfMember = society.NoOfMember,
                Status = society.Status
            };
        }

        [HttpPost("ModifySociety")]
        public ActionResult<SocietyDto> UpdateSociety(SocietyDto setsocietyDto)
        {
            var society = new Society
            {
                Id = setsocietyDto.Id,
                SocietyName = setsocietyDto.SocietyName,
                SocietyAddress = setsocietyDto.SocietyAddress,
                NoOfMember = setsocietyDto.NoOfMember,
                Status = setsocietyDto.Status,
                ModifiedOn = DateTimeOffset.Now
            };

            _societyRepo.Update(society);
            _societyRepo.Savechange();

            return new SocietyDto
            {
                Id = society.Id,
                SocietyName = society.SocietyName,
                SocietyAddress = society.SocietyAddress,
                NoOfMember = society.NoOfMember,
                Status = society.Status,
            };
        }

    }
}
