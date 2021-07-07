using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    
    public class MarketGroupController : BaseApiController
    {
        private readonly IGenericRepository<MarketGroupMst> _marketGroupMstRepo;
        private readonly IMapper _mapper;
        public MarketGroupController(IGenericRepository<MarketGroupMst> bcdsRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _marketGroupMstRepo = bcdsRepo;
        }

        [HttpGet("GetAllBCDS")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<BcdsDto>>> GetAllBcds([FromQuery] BcdsSpecParams bscdParrams)
        {

            var spec = new BcdsSpecificiation(bscdParrams);

            var countSpec = new BcdsWithFiltersForCountSpecificication(bscdParrams);

            var totalItems = await _marketGroupMstRepo.CountAsync(countSpec);

            var posts = await _marketGroupMstRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<MarketGroupMst>, IReadOnlyList<BcdsDto>>(posts);

            return Ok(new Pagination<BcdsDto>(bscdParrams.PageIndex, bscdParrams.PageSize, totalItems, data));

        }


        [HttpPost("CreateBCDS")]
        public ActionResult<BcdsDto> SaveBcds(BcdsDto setbcdsDto)
        {
            var bcds = new MarketGroupMst
            {
                BcdsName = setbcdsDto.BcdsName,
                BcdsAddress = setbcdsDto.BcdsAddress,
                NoOfMember = setbcdsDto.NoOfMember,
                Status = setbcdsDto.Status,
                SetOn = DateTimeOffset.Now
            };

            _marketGroupMstRepo.Add(bcds);
            _marketGroupMstRepo.Savechange();

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
            //var bcds = await _marketGroupMstRepo.GetByIdAsync(setbcdsDto.Id);
            //if (bcds == null) return Unauthorized(new ApiResponse(401));

            var bcds = new MarketGroupMst
            {
                Id = setbcdsDto.Id,
                BcdsName = setbcdsDto.BcdsName,
                BcdsAddress = setbcdsDto.BcdsAddress,
                NoOfMember = setbcdsDto.NoOfMember,
                Status = setbcdsDto.Status,
            };

            _marketGroupMstRepo.Update(bcds);
            _marketGroupMstRepo.Savechange();

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
