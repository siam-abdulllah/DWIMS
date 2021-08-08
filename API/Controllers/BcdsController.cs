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
    public class BcdsController : BaseApiController
    {

        private readonly IGenericRepository<Bcds> _bcdsRepo;
        private readonly IMapper _mapper;
        public BcdsController(IGenericRepository<Bcds> bcdsRepo,       
        IMapper mapper)
        {
            _mapper = mapper;
            _bcdsRepo = bcdsRepo;
        }   

        [HttpGet("GetAllBCDS")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<BcdsDto>>> GetAllBcds([FromQuery]BcdsSpecParams bscdParrams)
        {
           
            var spec = new BcdsSpecificiation(bscdParrams);

            var countSpec = new BcdsWithFiltersForCountSpecificication(bscdParrams);

            var totalItems = await _bcdsRepo.CountAsync(countSpec);

            var posts = await _bcdsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Bcds>, IReadOnlyList<BcdsDto>>(posts);

            return Ok(new Pagination<BcdsDto>(bscdParrams.PageIndex, bscdParrams.PageSize, totalItems, data));
     
        }
        [HttpGet("bcdsForInvestment")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<IReadOnlyList<Bcds>> GetBcdsForInvestment()
        {
           
            var spec = new BcdsSpecificiation();

            var data = await _bcdsRepo.ListAsync(spec);

            return data;
     
        }


        [HttpPost("CreateBCDS")]
        public ActionResult<BcdsDto> SaveBcds(BcdsDto setbcdsDto)
        {
            var bcds = new Bcds
            {
                BcdsName = setbcdsDto.BcdsName,
                BcdsAddress = setbcdsDto.BcdsAddress,
                NoOfMember = setbcdsDto.NoOfMember,
                Status = setbcdsDto.Status,
                SetOn = DateTimeOffset.Now
            };

            _bcdsRepo.Add(bcds);
            _bcdsRepo.Savechange();

            return new BcdsDto
            {
                Id = bcds.Id,
                BcdsName = bcds.BcdsName,
                BcdsAddress = bcds.BcdsAddress,
                NoOfMember = bcds.NoOfMember,
                Status = bcds.Status
            };
        }


        [HttpPost("ModifyBCDS")]
        public ActionResult<BcdsDto> UpdateBcds(BcdsDto setbcdsDto)
        {
            //var bcds = await _bcdsRepo.GetByIdAsync(setbcdsDto.Id);
            //if (bcds == null) return Unauthorized(new ApiResponse(401));

            var bcds = new Bcds
            {
                Id = setbcdsDto.Id,
                BcdsName = setbcdsDto.BcdsName,
                BcdsAddress = setbcdsDto.BcdsAddress,
                NoOfMember = setbcdsDto.NoOfMember,
                Status = setbcdsDto.Status,
            };

            _bcdsRepo.Update(bcds);
            _bcdsRepo.Savechange();

            return new BcdsDto
            {
                Id = bcds.Id,
                BcdsName = bcds.BcdsName,
                BcdsAddress = bcds.BcdsAddress,
                NoOfMember = bcds.NoOfMember,
                Status = bcds.Status,
            };
        }
    }
}