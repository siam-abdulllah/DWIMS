using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
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
        private readonly IGenericRepository<InstitutionMarket> _InstitutionMarketRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        public InstitutionController(IGenericRepository<InstitutionInfo> institutionRepo, IGenericRepository<InstitutionMarket> InstitutionMarketRepo, StoreContext dbContext,
        IMapper mapper)
        {
            _mapper = mapper;
            _institutionRepo = institutionRepo;
            _InstitutionMarketRepo = InstitutionMarketRepo;
            _dbContext = dbContext;
        }



        [HttpGet("institutionsForInvestment/{marketCode}")]
        public ActionResult<IEnumerable<InstitutionInfo>> GetInstitutionsForInvestment(string marketCode)
        {
            try
            {
              //  var institutions = await _institutionRepo.ListAllAsync();
                var institutions = (from d in _dbContext.InstitutionInfo
                               join dm in _dbContext.InstitutionMarket on d.Id equals dm.InstitutionCode
                               where dm.MarketCode == marketCode
                               orderby d.InstitutionName
                               select new InstitutionInfo
                               {
                                   InstitutionName = d.InstitutionName,
                                   InstitutionCode = d.InstitutionCode,
                                   Id = d.Id
                               }
                             ).Distinct().ToList();
                return institutions;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
