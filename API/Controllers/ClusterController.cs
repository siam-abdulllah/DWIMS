using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ClusterController : BaseApiController
    {
        private readonly IGenericRepository<ClusterMst> _clusterMstRepo;
        private readonly IGenericRepository<ClusterDtl> _clusterDtlRepo;
        private readonly IMapper _mapper;
        public ClusterController(IGenericRepository<ClusterMst> clusterMstRepo, IGenericRepository<ClusterDtl> clusterDtlRepo,
       IMapper mapper)
        {
            _mapper = mapper;
            _clusterMstRepo = clusterMstRepo;
            _clusterDtlRepo = clusterDtlRepo;
        }
        [HttpPost("insertMst")]
        public ActionResult<ClusterMstDto> InsertClusterMst(ClusterMstDto clusterMstDto)
        {
            var clusterMsts = new ClusterMst
            {
                ClusterCode = clusterMstDto.ClusterCode,
                ClusterName = clusterMstDto.ClusterName,
                Status = clusterMstDto.Status,
                SetOn = DateTimeOffset.Now
            };
            _clusterMstRepo.Add(clusterMsts);
            _clusterMstRepo.Savechange();

            return new ClusterMstDto
            {
                Id = clusterMsts.Id,
                ClusterCode = clusterMstDto.ClusterCode,
                ClusterName = clusterMstDto.ClusterName,
                Status = clusterMstDto.Status,
            };
        }
       
        [HttpPost("insertDtl")]
        public ActionResult<ClusterDtlDto> InsertClusterDtl(ClusterDtlDto clusterDtlDto)
        {
            try
            {
                var clusterDtls = new ClusterDtl
                {
                    MstId = clusterDtlDto.MstId,
                    RegionCode = clusterDtlDto.RegionCode,
                    RegionName = clusterDtlDto.RegionName,
                    Status = clusterDtlDto.Status,
                    SetOn = DateTimeOffset.Now
                };
                _clusterDtlRepo.Add(clusterDtls);
                _clusterDtlRepo.Savechange();

                return new ClusterDtlDto
                {
                    Id = clusterDtls.Id,
                    MstId = clusterDtlDto.MstId,
                    RegionCode = clusterDtlDto.RegionCode,
                    RegionName = clusterDtlDto.RegionName,
                    Status = clusterDtlDto.Status,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    
        [HttpPost("updateMst")]
        public ActionResult<ClusterMstDto> UpdateClusterMst(ClusterMstDto clusterMstDto)
        {
            var clusterMsts = new ClusterMst
            {
                Id = clusterMstDto.Id,
                ClusterCode = clusterMstDto.ClusterCode,
                ClusterName = clusterMstDto.ClusterName,
                Status = clusterMstDto.Status,
                ModifiedOn = DateTimeOffset.Now

            };
            _clusterMstRepo.Update(clusterMsts);
            _clusterMstRepo.Savechange();

            return new ClusterMstDto
            {
                Id = clusterMstDto.Id,
                ClusterCode = clusterMstDto.ClusterCode,
                ClusterName = clusterMstDto.ClusterName,
                Status = clusterMstDto.Status,
            };
        }
    
        [HttpPost("updateDtl")]
        public ActionResult<ClusterDtlDto> UpdateClusterDtl(ClusterDtlDto clusterDtlDto)
        {
            var clusterDtls = new ClusterDtl
            {
                Id = clusterDtlDto.Id,
                MstId = clusterDtlDto.MstId,
                RegionCode = clusterDtlDto.RegionCode,
                RegionName = clusterDtlDto.RegionName,
                Status = clusterDtlDto.Status,
                ModifiedOn = DateTimeOffset.Now

            };
            _clusterDtlRepo.Update(clusterDtls);
            _clusterDtlRepo.Savechange();

            return new ClusterDtlDto
            {
                Id = clusterDtlDto.Id,
                MstId = clusterDtlDto.MstId,
                RegionCode = clusterDtlDto.RegionCode,
                RegionName = clusterDtlDto.RegionName,
                Status = clusterDtlDto.Status,
            };
        }
     
        [HttpGet("clusterMsts")]
        public async Task<ActionResult<Pagination<ClusterMstDto>>> GetClusterMsts(
              [FromQuery] ClusterMstSpecParams clusterMstParrams)
        {
            try
            {
                var spec = new ClusterMstSpecification(clusterMstParrams);

                var countSpec = new ClusterMstWithFiltersForCountSpecificication(clusterMstParrams);

                var totalItems = await _clusterMstRepo.CountAsync(countSpec);

                var clusterMst = await _clusterMstRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<ClusterMst>, IReadOnlyList<ClusterMstDto>>(clusterMst);

                return Ok(new Pagination<ClusterMstDto>(clusterMstParrams.PageIndex, clusterMstParrams.PageSize, totalItems, data));
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
     
        [HttpGet("clusterDtls/{mstId}")]
        public async Task<ActionResult<Pagination<ClusterDtlDto>>> GetClusterDtls(
        [FromQuery] ClusterDtlSpecParams clusterDtlParrams, int mstId)
        {
            try
            {
                var spec = new ClusterDtlSpecification(mstId);

                var countSpec = new ClusterDtlWithFiltersForCountSpecificication(mstId);

                var totalItems = await _clusterDtlRepo.CountAsync(countSpec);

                var clusterDtl = await _clusterDtlRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<ClusterDtl>, IReadOnlyList<ClusterDtlDto>>(clusterDtl);

                return Ok(new Pagination<ClusterDtlDto>(clusterDtlParrams.PageIndex, clusterDtlParrams.PageSize, totalItems, data));
            }
            catch (System.Exception)
            {

                throw;
            }
        }



    }
}
