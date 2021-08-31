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
    public class DoctorController : BaseApiController
    {
        private readonly IGenericRepository<DoctorInfo> _doctorRepo;
        private readonly IMapper _mapper;
        public DoctorController(IGenericRepository<DoctorInfo> doctorRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _doctorRepo = doctorRepo;
        }


        [HttpGet("doctorsForInvestment")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public async Task<IReadOnlyList<DoctorInfo>> GetDoctorsForInvestment()
        {
            try
            {
                var doctors = await _doctorRepo.ListAllAsync();
                return doctors;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
