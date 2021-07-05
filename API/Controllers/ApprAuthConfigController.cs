using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ApprAuthConfigController : BaseApiController
    {
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IMapper _mapper;
        public ApprAuthConfigController(IGenericRepository<ApprAuthConfig> apprAuthConfigRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _apprAuthConfigRepo = apprAuthConfigRepo;
        }
        //[HttpGet("apprAuthConfigs")]
        //public async Task<ActionResult<Pagination<ApprAuthConfigDto>>> GetApprAuthConfigs(
        //  [FromQuery] ApprAuthConfigSpecParams apprAuthConfigParrams)
        //{
        //    try
        //    {
        //        var spec = new ApprAuthConfigSpecification(apprAuthConfigParrams);

        //        var countSpec = new ApprAuthConfigWithFiltersForCountSpecificication(apprAuthConfigParrams);

        //        var totalItems = await _apprAuthConfigRepo.CountAsync(countSpec);

        //        var apprAuthConfigs = await _apprAuthConfigRepo.ListAsync(spec);

        //        var data = _mapper.Map<IReadOnlyList<ApprAuthConfig>, IReadOnlyList<ApprAuthConfigDto>>(apprAuthConfigs);

        //        return Ok(new Pagination<ApprAuthConfigDto>(apprAuthConfigParrams.PageIndex, apprAuthConfigParrams.PageSize, totalItems, data));
        //        //var data = await _repository.GetDateWiseProformaInfos(searchDto);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPost("insert")]
        public async Task<ActionResult<ApprAuthConfigDto>> InsertApprAuthConfig(ApprAuthConfig apprAuthConfig)
        {

            try
            {
                var alreadyExistSpec=new ApprAuthConfigSpecification(apprAuthConfig.EmployeeId, apprAuthConfig.ApprovalAuthorityId,"A");
                var alreadyExistApprAuthConfigList = await _apprAuthConfigRepo.ListAsync(alreadyExistSpec);
                if(alreadyExistApprAuthConfigList.Count>0)
                {
                    return BadRequest();
                }
                var spec = new ApprAuthConfigSpecification(apprAuthConfig.EmployeeId, "A");
                var apprAuthConfigList = await _apprAuthConfigRepo.ListAsync(spec);
                foreach (var v in apprAuthConfigList)
                {
                    
                    v.Status = "I";
                    v.ModifiedOn = DateTimeOffset.Now;
                    _apprAuthConfigRepo.Update(v);
                    _apprAuthConfigRepo.Savechange();
                }

                var apprAuthConfigMst = new ApprAuthConfig
                {
                    ApprovalAuthorityId = apprAuthConfig.ApprovalAuthorityId,
                    EmployeeId = apprAuthConfig.EmployeeId,
                    Status = "A",
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _apprAuthConfigRepo.Add(apprAuthConfigMst);
                _apprAuthConfigRepo.Savechange();

                return new ApprAuthConfigDto
                {
                    Id = apprAuthConfig.Id,
                    ApprovalAuthorityId = apprAuthConfig.ApprovalAuthorityId,
                    EmployeeId = apprAuthConfig.EmployeeId,
                    Status = apprAuthConfig.Status
                };
            }
            catch (Exception e)
            {

                throw e;

            }
        }

        [HttpGet]
         [Route("employeesForConfigByAuthId/{id}")]
        public async Task<IReadOnlyList<ApprAuthConfig>> GetEmployees(int id)
        {
            try
            {
                var spec = new ApprAuthConfigSpecification(id);
                var employee = await _apprAuthConfigRepo.ListAsync(spec);
                return employee;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}