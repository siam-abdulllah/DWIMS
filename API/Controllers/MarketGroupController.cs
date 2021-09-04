using API.Dtos;
using API.Errors;
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
            
            var marketGroupMsts = new MarketGroupMst
            {
                GroupName = marketGroupMstDto.GroupName,
                EmployeeId = marketGroupMstDto.EmployeeId,
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
                    SBU = marketGroupDtlDto.SBU,
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
                    SBU = marketGroupDtlDto.SBU,
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
                EmployeeId = marketGroupMstDto.EmployeeId,
                Status = marketGroupMstDto.Status,
                ModifiedOn = DateTimeOffset.Now

            };
            _marketGroupMstRepo.Update(marketGroupMsts);
            _marketGroupMstRepo.Savechange();

            return new MarketGroupMstDto
            {
                Id = marketGroupMstDto.Id,
                GroupName = marketGroupMstDto.GroupName,
                EmployeeId = marketGroupMsts.EmployeeId,
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
                SBU = marketGroupDtlDto.SBU,
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

        [HttpGet("marketGroupMsts/{empId}")]
        public async Task<ActionResult<Pagination<MarketGroupMstDto>>> GetMarketGroupMsts(int empId,
          [FromQuery] MarketGroupMstSpecParams marketGroupMstParrams)
        {
            try
            {
                var spec = new MarketGroupMstSpecification(empId);

                var countSpec = new MarketGroupMstWithFiltersForCountSpecificication(marketGroupMstParrams);

                var totalItems = await _marketGroupMstRepo.CountAsync(countSpec);

                var marketGroup = await _marketGroupMstRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<MarketGroupMst>, IReadOnlyList<MarketGroupMstDto>>(marketGroup);

                return Ok(new Pagination<MarketGroupMstDto>(marketGroupMstParrams.PageIndex, marketGroupMstParrams.PageSize, totalItems, data));
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        [HttpGet("getMarketGroupMstsForInvestment")]
        public async Task<IReadOnlyList<MarketGroupMst>> GetMarketGroupMstsForInvestment()
        {
            try
            {
                var spec = new MarketGroupMstSpecification();


                var marketGroup = await _marketGroupMstRepo.ListAsync(spec);

                //var data = _mapper
                //    .Map<IReadOnlyList<MarketGroupMst>, IReadOnlyList<MarketGroupMstDto>>(marketGroup);
                return marketGroup;
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        [HttpGet("getMarketGroupMstsForInvestment/{empId}")]
        public async Task<IReadOnlyList<MarketGroupMst>> GetMarketGroupMstsForInvestment(int empId)
        {
            try
            {
                var spec = new MarketGroupMstSpecification(empId);


                var marketGroup = await _marketGroupMstRepo.ListAsync(spec);

                //var data = _mapper
                //    .Map<IReadOnlyList<MarketGroupMst>, IReadOnlyList<MarketGroupMstDto>>(marketGroup);
                return marketGroup;
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        [HttpGet]
        [Route("marketGroupDtls/{mstId}")]
        public async Task<ActionResult<Pagination<MarketGroupDtlDto>>> GetMarketGroupDtls(
        [FromQuery] MarketGroupDtlSpecParams marketGroupDtlParrams, int mstId)
        {
            try
            {
                var spec = new MarketGroupDtlSpecification(mstId);

                var countSpec = new MarketGroupDtlWithFiltersForCountSpecificication(mstId);

                var totalItems = await _marketGroupDtlRepo.CountAsync(countSpec);

                var marketGroup = await _marketGroupDtlRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<MarketGroupDtl>, IReadOnlyList<MarketGroupDtlDto>>(marketGroup);

                return Ok(new Pagination<MarketGroupDtlDto>(marketGroupDtlParrams.PageIndex, marketGroupDtlParrams.PageSize, totalItems, data));
            }
            catch (System.Exception)
            {

                throw;
            }
        }


    }
}
