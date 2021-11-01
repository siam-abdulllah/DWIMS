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
    public class InstitutionController : BaseApiController
    {
        private readonly IGenericRepository<InstitutionInfo> _institutionRepo;
        private readonly IMapper _mapper;
        public InstitutionController(IGenericRepository<InstitutionInfo> institutionRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _institutionRepo = institutionRepo;
        }



        [HttpGet("institutionsForInvestment")]
        public async Task<IEnumerable<InstitutionInfo>> GetInstitutionsForInvestment()
        {
            try
            {
                var institutions = await _institutionRepo.ListAllAsync();
                return institutions.OrderBy(x=>x.InstitutionName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
