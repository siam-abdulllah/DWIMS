using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
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
        private readonly IMapper _mapper;
        public CampaignController(IGenericRepository<CampaignMst> campaignMstRepo, IGenericRepository<CampaignDtl> campaignDtlRepo,
       IMapper mapper)
        {
            _mapper = mapper;
            _campaignMstRepo = campaignMstRepo;
            _campaignDtlRepo = campaignDtlRepo;
        }
        [HttpPost("insertMst")]
        public ActionResult<CampaignMstDto> InsertCampaignMst(CampaignMstDto campaignMstDto)
        {
            var campaignMsts = new CampaignMst
            {
                CampaignNo = campaignMstDto.CampaignNo,
                CampaignName = campaignMstDto.CampaignName,
                SBU = campaignMstDto.SBU,
                BrandId = campaignMstDto.BrandId,
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
                BrandId = campaignMsts.BrandId
            };
        }
        //[HttpPost("insertDtl")]
        //public ActionResult<MarketGroupDtlDto> InsertMarketGroupDtl(MarketGroupDtlDto marketGroupDtlDto)
        //{
        //    try
        //    {
        //        var marketGroupDtls = new MarketGroupDtl
        //        {
        //            MarketCode = marketGroupDtlDto.MarketCode,
        //            MarketName = marketGroupDtlDto.MarketName,
        //            SBU = marketGroupDtlDto.SBU,
        //            MstId = marketGroupDtlDto.MstId,
        //            Status = "A",
        //            SetOn = DateTimeOffset.Now
        //        };
        //        _marketGroupDtlRepo.Add(marketGroupDtls);
        //        _marketGroupDtlRepo.Savechange();

        //        return new MarketGroupDtlDto
        //        {
        //            Id = marketGroupDtls.Id,
        //            MarketCode = marketGroupDtlDto.MarketCode,
        //            MarketName = marketGroupDtlDto.MarketName,
        //            SBU = marketGroupDtlDto.SBU,
        //            MstId = marketGroupDtlDto.MstId,
        //            Status = marketGroupDtlDto.Status
        //        };
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        [HttpPost("updateMst")]
        public ActionResult<CampaignMstDto> UpdateCampaignMst(CampaignMstDto campaignMstDto)
        {
            var campaignMsts = new CampaignMst
            {
                Id = campaignMstDto.Id,
                CampaignNo = campaignMstDto.CampaignNo,
                CampaignName = campaignMstDto.CampaignName,
                SBU = campaignMstDto.SBU,
                BrandId = campaignMstDto.BrandId,
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
                BrandId = campaignMstDto.BrandId,
            };
        }
        //[HttpPost("updateDtl")]
        //public ActionResult<MarketGroupDtlDto> UpdateMarketGroupDtl(MarketGroupDtlDto marketGroupDtlDto)
        //{
        //    var marketGroupDtls = new MarketGroupDtl
        //    {
        //        Id = marketGroupDtlDto.Id,
        //        MarketCode = marketGroupDtlDto.MarketCode,
        //        MarketName = marketGroupDtlDto.MarketName,
        //        SBU = marketGroupDtlDto.SBU,
        //        MstId = marketGroupDtlDto.MstId,
        //        Status = "I",
        //        ModifiedOn = DateTimeOffset.Now

        //    };
        //    _marketGroupDtlRepo.Update(marketGroupDtls);
        //    _marketGroupDtlRepo.Savechange();

        //    return new MarketGroupDtlDto
        //    {
        //        Id = marketGroupDtlDto.Id,
        //        MarketCode = marketGroupDtlDto.MarketCode,
        //        MarketName = marketGroupDtlDto.MarketName,
        //        MstId = marketGroupDtlDto.MstId,
        //        Status = "A"
        //    };
        //}




    }
}
