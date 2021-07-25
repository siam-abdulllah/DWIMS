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




    }
}
