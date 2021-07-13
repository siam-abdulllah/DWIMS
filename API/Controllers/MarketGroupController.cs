using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
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
        private readonly IGenericRepository<MarketGroupDtl> _marketGroupDtlRepo;
        private readonly IMapper _mapper;
        public MarketGroupController(IGenericRepository<MarketGroupMst> marketGroupMstRepo, IGenericRepository<MarketGroupDtl> marketGroupDtlRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _marketGroupMstRepo = marketGroupMstRepo;
            _marketGroupDtlRepo = marketGroupDtlRepo;
        }
        [HttpPost("insertMst")]
        public ActionResult<MarketGroupMstDto> InsertMarketGroupMst(MarketGroupMstDto marketGroupMstDto)
        {
            var employeeId = 1;
            var marketGroupMsts = new MarketGroupMst
            {
                GroupName = marketGroupMstDto.GroupName,
                EmployeeId = employeeId,
                Status = marketGroupMstDto.Status,
                SetOn = DateTimeOffset.Now
            };
            _marketGroupMstRepo.Add(marketGroupMsts);
            _marketGroupMstRepo.Savechange();

            return new MarketGroupMstDto
            {
                Id = marketGroupMsts.Id,
                GroupName = marketGroupMsts.GroupName,
                EmployeeId = marketGroupMsts.EmployeeId,
                Status = marketGroupMsts.Status
            };
        }
        [HttpPost("insertDtl")]
        public ActionResult<MarketGroupDtlDto> InsertMarketGroupDtl(MarketGroupDtlDto marketGroupDtlDto)
        {
            try
            {
                var marketGroupDtls = new MarketGroupDtl
                {
                    MarketCode = marketGroupDtlDto.MarketCode,
                    MarketName = marketGroupDtlDto.MarketName,
                    MstId = marketGroupDtlDto.MstId,
                    Status = "A",
                    SetOn = DateTimeOffset.Now
                };
                _marketGroupDtlRepo.Add(marketGroupDtls);
                _marketGroupDtlRepo.Savechange();

                return new MarketGroupDtlDto
                {
                    Id = marketGroupDtls.Id,
                    MarketCode = marketGroupDtlDto.MarketCode,
                    MarketName = marketGroupDtlDto.MarketName,
                    MstId = marketGroupDtlDto.MstId,
                    Status = marketGroupDtlDto.Status
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost("updateMst")]
        public ActionResult<MarketGroupMstDto> UpdateMarketGroupMst(MarketGroupMstDto marketGroupMstDto)
        {
            var marketGroupMsts = new MarketGroupMst
            {
                Id = marketGroupMstDto.Id,
                GroupName = marketGroupMstDto.GroupName,
                Status = marketGroupMstDto.Status,
                ModifiedOn = DateTimeOffset.Now

            };
            _marketGroupMstRepo.Update(marketGroupMsts);
            _marketGroupMstRepo.Savechange();

            return new MarketGroupMstDto
            {
                Id = marketGroupMstDto.Id,
                GroupName = marketGroupMstDto.GroupName,
                Status = marketGroupMstDto.Status
            };
        }
        [HttpPost("updateDtl")]
        public ActionResult<MarketGroupDtlDto> UpdateMarketGroupDtl(MarketGroupDtlDto marketGroupDtlDto)
        {
            var marketGroupDtls = new MarketGroupDtl
            {
                Id = marketGroupDtlDto.Id,
                MarketCode = marketGroupDtlDto.MarketCode,
                MarketName = marketGroupDtlDto.MarketName,
                MstId = marketGroupDtlDto.MstId,
                Status = "I",
                ModifiedOn = DateTimeOffset.Now

            };
            _marketGroupDtlRepo.Update(marketGroupDtls);
            _marketGroupDtlRepo.Savechange();

            return new MarketGroupDtlDto
            {
                Id = marketGroupDtlDto.Id,
                MarketCode = marketGroupDtlDto.MarketCode,
                MarketName = marketGroupDtlDto.MarketName,
                MstId = marketGroupDtlDto.MstId,
                Status = "A"
            };
        }

        [HttpGet("marketGroupMsts")]
        public async Task<ActionResult<Pagination<MarketGroupMstDto>>> GetMarketGroupMsts(
          [FromQuery] MarketGroupSpecParams marketGroupParrams)
        {
            try
            {
                var spec = new MarketGroupSpecification(marketGroupParrams);

                var countSpec = new MarketGroupWithFiltersForCountSpecificication(marketGroupParrams);

                var totalItems = await _marketGroupMstRepo.CountAsync(countSpec);

                var marketGroup = await _marketGroupMstRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<MarketGroupMst>, IReadOnlyList<MarketGroupMstDto>>(marketGroup);

                return Ok(new Pagination<MarketGroupMstDto>(marketGroupParrams.PageIndex, marketGroupParrams.PageSize, totalItems, data));
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
        [HttpGet("marketGroupDtls")]
        public async Task<ActionResult<Pagination<MarketGroupDtlDto>>> GetMarketGroupDtls(
        [FromQuery] MarketGroupSpecParams marketGroupParrams, int mstId)
        {
            try
            {
                var spec = new MarketGroupSpecification(mstId);

                var countSpec = new MarketGroupWithFiltersForCountSpecificication(mstId);

                var totalItems = await _marketGroupMstRepo.CountAsync(countSpec);

                var marketGroup = await _marketGroupMstRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<MarketGroupMst>, IReadOnlyList<MarketGroupDtlDto>>(marketGroup);

                return Ok(new Pagination<MarketGroupDtlDto>(marketGroupParrams.PageIndex, marketGroupParrams.PageSize, totalItems, data));
            }
            catch (System.Exception)
            {

                throw;
            }
        }


    }
}
