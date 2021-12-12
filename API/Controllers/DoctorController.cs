using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class DoctorController : BaseApiController
    {
        private readonly IGenericRepository<DoctorInfo> _doctorRepo;
        private readonly IGenericRepository<DoctorMarket> _doctorMarketRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        public DoctorController(IGenericRepository<DoctorInfo> doctorRepo,IGenericRepository<DoctorMarket> doctorMarketRepo, StoreContext dbContext,
        IMapper mapper)
        {
            _mapper = mapper;
            _doctorRepo = doctorRepo;
            _doctorMarketRepo = doctorMarketRepo;
            _dbContext = dbContext;
        }


        [HttpGet("doctorsForInvestment/{marketCode}")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult<IEnumerable<DoctorInfo>> GetDoctorsForInvestment(string marketCode)
        {
            try
            {
                //var doctorMarketSpec = new DoctorMarketSpecification(marketCode);

                //var doctorMarketData = await _doctorMarketRepo.ListAsync(doctorMarketSpec);
                //var doctors = await _doctorRepo.ListAllAsync();
                //var data = await _productRepo.ListAllAsync();
                var doctors = (from d in _dbContext.DoctorInfo
                             join dm in _dbContext.DoctorMarket on d.Id equals dm.DoctorCode
                             where dm.MarketCode == marketCode
                              orderby d.DoctorName
                             select new DoctorInfo
                             {
                                 DoctorName = d.DoctorName,
                                 DoctorCode = d.DoctorCode,
                                 Degree = d.Degree,
                                 Designation= d.Designation,
                                 Id=d.Id
                             }
                              ).Distinct().ToList();
                //return doctors.OrderBy(x=>x.DoctorName);
                return doctors;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
