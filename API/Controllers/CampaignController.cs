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
    public class CampaignController : BaseApiController
    {
        private readonly IGenericRepository<CampaignMst> _campaignMstRepo;
        private readonly IGenericRepository<CampaignDtl> _campaignDtlRepo;
        private readonly IGenericRepository<CampaignDtlProduct> _campaignDtlProductRepo;
        private readonly IMapper _mapper;
        public CampaignController(IGenericRepository<CampaignMst> campaignMstRepo, IGenericRepository<CampaignDtlProduct> campaignDtlProductRepo, IGenericRepository<CampaignDtl> campaignDtlRepo,
       IMapper mapper)
        {
            _mapper = mapper;
            _campaignMstRepo = campaignMstRepo;
            _campaignDtlRepo = campaignDtlRepo;
            _campaignDtlProductRepo = campaignDtlProductRepo;
        }
        [HttpPost("insertMst")]
        public ActionResult<CampaignMstDto> InsertCampaignMst(CampaignMstDto campaignMstDto)
        {
            var campaignMsts = new CampaignMst
            {
                CampaignNo = campaignMstDto.CampaignNo,
                CampaignName = campaignMstDto.CampaignName,
                SBU = campaignMstDto.SBU,
                BrandCode = campaignMstDto.BrandCode,
                SetOn = DateTimeOffset.Now
            };
            _campaignMstRepo.Add(campaignMsts);
            _campaignMstRepo.Savechange();

            return new CampaignMstDto
            {
                Id = campaignMsts.Id,
                CampaignNo = campaignMsts.CampaignNo,
                CampaignName = campaignMsts.CampaignName,
                SBU = campaignMsts.SBU,
                BrandCode = campaignMsts.BrandCode
            };
        }
       
        [HttpPost("insertDtl")]
        public ActionResult<CampaignDtlDto> InsertCampaignDtl(CampaignDtlDto campaignDtlDto)
        {
            try
            {
                var campaignDtls = new CampaignDtl
                {
                    MstId = campaignDtlDto.MstId,
                    SubCampaignId = campaignDtlDto.SubCampaignId,
                    Budget = campaignDtlDto.Budget,
                    SubCampStartDate = campaignDtlDto.SubCampStartDate,
                    SubCampEndDate = campaignDtlDto.SubCampEndDate,
                    SetOn = DateTimeOffset.Now
                };
                _campaignDtlRepo.Add(campaignDtls);
                _campaignDtlRepo.Savechange();

                return new CampaignDtlDto
                {
                    Id = campaignDtls.Id,
                    MstId = campaignDtlDto.MstId,
                    SubCampaignId = campaignDtlDto.SubCampaignId,
                    Budget = campaignDtlDto.Budget,
                    SubCampStartDate = campaignDtlDto.SubCampStartDate,
                    SubCampEndDate = campaignDtlDto.SubCampEndDate,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
     
        [HttpPost("insertDtlProduct")]
        public ActionResult<CampaignDtlProductDto> InsertCampaignDtlProduct(CampaignDtlProductDto campaignDtlProductDto)
        {
            try
            {
                var campaignDtlProducts = new CampaignDtlProduct
                {
                    //Id = campaignDtlProductDto.Id,
                    DtlId = campaignDtlProductDto.DtlId,
                    ProductId = campaignDtlProductDto.ProductId,
                    SetOn = DateTimeOffset.Now
                };
                _campaignDtlProductRepo.Add(campaignDtlProducts);
                _campaignDtlProductRepo.Savechange();

                return new CampaignDtlProductDto
                {
                    Id = campaignDtlProductDto.Id,
                    DtlId = campaignDtlProductDto.DtlId,
                    ProductId = campaignDtlProductDto.ProductId,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
      
        [HttpPost("updateMst")]
        public ActionResult<CampaignMstDto> UpdateCampaignMst(CampaignMstDto campaignMstDto)
        {
            var campaignMsts = new CampaignMst
            {
                Id = campaignMstDto.Id,
                CampaignNo = campaignMstDto.CampaignNo,
                CampaignName = campaignMstDto.CampaignName,
                SBU = campaignMstDto.SBU,
                BrandCode = campaignMstDto.BrandCode,
                ModifiedOn = DateTimeOffset.Now

            };
            _campaignMstRepo.Update(campaignMsts);
            _campaignMstRepo.Savechange();

            return new CampaignMstDto
            {
                Id = campaignMstDto.Id,
                CampaignNo = campaignMstDto.CampaignNo,
                CampaignName = campaignMstDto.CampaignName,
                SBU = campaignMstDto.SBU,
                BrandCode = campaignMstDto.BrandCode
            };
        }
     
        [HttpPost("updateDtl")]
        public ActionResult<CampaignDtlDto> UpdateCampaignDtl(CampaignDtlDto campaignDtlDto)
        {
            var campaignDtls = new CampaignDtl
            {
                Id = campaignDtlDto.Id,
                MstId = campaignDtlDto.MstId,
                SubCampaignId = campaignDtlDto.SubCampaignId,
                Budget = campaignDtlDto.Budget,
                SubCampStartDate = campaignDtlDto.SubCampStartDate,
                SubCampEndDate = campaignDtlDto.SubCampEndDate,
                ModifiedOn = DateTimeOffset.Now

            };
            _campaignDtlRepo.Update(campaignDtls);
            _campaignDtlRepo.Savechange();

            return new CampaignDtlDto
            {
                Id = campaignDtlDto.Id,
                MstId = campaignDtlDto.MstId,
                SubCampaignId = campaignDtlDto.SubCampaignId,
                Budget = campaignDtlDto.Budget,
                SubCampStartDate = campaignDtlDto.SubCampStartDate,
                SubCampEndDate = campaignDtlDto.SubCampEndDate,
            };
        }
      
        [HttpPost("updateDtlProduct")]
        public ActionResult<CampaignDtlProductDto> UpdateCampaignDtlProduct(CampaignDtlProductDto campaignDtlProductDto)
        {
            var campaignDtlProducts = new CampaignDtlProduct
            {
                Id = campaignDtlProductDto.Id,
                DtlId = campaignDtlProductDto.DtlId,
                ProductId = campaignDtlProductDto.ProductId,
                ModifiedOn = DateTimeOffset.Now

            };
            _campaignDtlProductRepo.Update(campaignDtlProducts);
            _campaignDtlProductRepo.Savechange();

            return new CampaignDtlProductDto
            {
                Id = campaignDtlProductDto.Id,
                DtlId = campaignDtlProductDto.DtlId,
                ProductId = campaignDtlProductDto.ProductId,
            };
        }
     
        [HttpGet("campaignMsts")]
        public async Task<ActionResult<Pagination<CampaignMstDto>>> GetCampaignMsts(
          [FromQuery] CampaignMstSpecParams campaignMstParrams)
        {
            try
            {
                var spec = new CampaignMstSpecification(campaignMstParrams);

                var countSpec = new CampaignMstWithFiltersForCountSpecificication(campaignMstParrams);

                var totalItems = await _campaignMstRepo.CountAsync(countSpec);

                var campaignMst = await _campaignMstRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<CampaignMst>, IReadOnlyList<CampaignMstDto>>(campaignMst);

                return Ok(new Pagination<CampaignMstDto>(campaignMstParrams.PageIndex, campaignMstParrams.PageSize, totalItems, data));
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
      
        [HttpGet("campaignDtls/{mstId}")]
        public async Task<ActionResult<Pagination<CampaignDtlDto>>> GetCampaignDtls(
        [FromQuery] CampaignDtlSpecParams campaignDtlParrams, int mstId)
        {
            try
            {
                var spec = new CampaignDtlSpecification(campaignDtlParrams,mstId);

                var countSpec = new CampaignDtlWithFiltersForCountSpecificication(mstId);

                var totalItems = await _campaignDtlRepo.CountAsync(countSpec);

                var campaignDtl = await _campaignDtlRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<CampaignDtl>, IReadOnlyList<CampaignDtlDto>>(campaignDtl);

                return Ok(new Pagination<CampaignDtlDto>(campaignDtlParrams.PageIndex, campaignDtlParrams.PageSize, totalItems, data));
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    
        [HttpGet("campaignDtlProducts/{dtlId}")]
        public async Task<ActionResult<Pagination<CampaignDtlProductDto>>> GetCampaignDtlProducts(
        [FromQuery] CampaignDtlProductSpecParams campaignDtlProductParrams, int dtlId)
        {
            try
            {
                var spec = new CampaignDtlProductSpecification(campaignDtlProductParrams, dtlId);

                var countSpec = new CampaignDtlProductWithFiltersForCountSpecificication(dtlId);

                var totalItems = await _campaignDtlProductRepo.CountAsync(countSpec);

                var campaignDtlProduct = await _campaignDtlProductRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<CampaignDtlProduct>, IReadOnlyList<CampaignDtlProductDto>>(campaignDtlProduct);

                return Ok(new Pagination<CampaignDtlProductDto>(campaignDtlProductParrams.PageIndex, campaignDtlProductParrams.PageSize, totalItems, data));
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        } 
      
        [HttpPost("removeDtlProduct")]
        public  void RemoveDtlProduct(CampaignDtlProduct campaignDtlProduct)
        {
            try
            {
                _campaignDtlProductRepo.Delete(campaignDtlProduct);
                _campaignDtlProductRepo.Savechange();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet("campaignMstsForInvestment")]
        public async Task<IReadOnlyList<CampaignMstDto>> GetCampaignMstsForInvestment()
        {
            try
            {
                var mstData = await _campaignMstRepo.ListAllAsync();
                var dtlData = await _campaignDtlRepo.ListAllAsync();
                var data = (from m in mstData
                              join d in dtlData on m.Id equals d.MstId
                              where d.SubCampStartDate<=DateTime.Now && d.SubCampEndDate>=DateTime.Now
                              orderby m.CampaignName
                              select new CampaignMstDto
                              {
                                  CampaignName = m.CampaignName.Trim(),
                                   Id= m.Id
                              }
                              ).Distinct().ToList();
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
     
        [HttpGet("campaignDtlsForInvestment/{mstId}")]
        public async Task<IReadOnlyList<CampaignDtlDto>> GetSubCampaignDtlsForInvestment(int mstId)
        {
            try
            {

                var spec = new CampaignDtlSpecification(mstId);

                var campaignDtl = await _campaignDtlRepo.ListAsync(spec);

                var data = _mapper.Map<IReadOnlyList<CampaignDtl>, IReadOnlyList<CampaignDtlDto>>(campaignDtl);
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } 
        
        [HttpGet("campaignDtlProductsForInvestment/{dtlId}")]
        
        public async Task<IReadOnlyList<CampaignDtlProductDto>> GetSubCampaignDtlProductsForInvestment(int dtlId)
        {
            try
            {

                var spec = new CampaignDtlProductSpecification(dtlId);

                var campaignDtlProduct = await _campaignDtlProductRepo.ListAsync(spec);

                var data = _mapper.Map<IReadOnlyList<CampaignDtlProduct>, IReadOnlyList<CampaignDtlProductDto>>(campaignDtlProduct);
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getCampaignForReport")]
        public async Task<IReadOnlyList<CampaignMstDto>> GetCampaignForReport()
        {
            try
            {
                var mstData = await _campaignMstRepo.ListAllAsync();
                var data = (from m in mstData
                            orderby m.CampaignName
                            select new CampaignMstDto
                            {
                                CampaignName = m.CampaignName,
                                Id = m.Id
                            }).Distinct().ToList();
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
