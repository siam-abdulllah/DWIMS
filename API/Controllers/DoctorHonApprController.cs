using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Specifications;
using API.Dtos;
using API.Helpers;

namespace API.Controllers
{
    public class DoctorHonApprController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentDetail> _investmentDetailRepo;
        private readonly IGenericRepository<InvestmentDoctor> _investmentDoctorRepo;
        private readonly IGenericRepository<DoctorHonAppr> _doctorHonApprRepo;
        private readonly IMapper _mapper;
        public DoctorHonApprController(IGenericRepository<DoctorHonAppr> doctorHonApprRepo,IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentDetail> investmentDetailRepo, IGenericRepository<InvestmentDoctor> investmentDoctorRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _doctorHonApprRepo = doctorHonApprRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentDetailRepo = investmentDetailRepo;
            _investmentDoctorRepo = investmentDoctorRepo;
        }

        [HttpGet("GetAllData")]
        // [Authorize(Roles = "Owner,Administrator")]
        // [Authorize(Policy = "DetailUserPolicy")]
        public async Task<ActionResult<IReadOnlyList<DoctorHonAppr>>> GetAllDoctorHonApprInfo([FromQuery] DoctorHonApprSpecParams appParrams, string honMonth)
        {

            var doctorHonApprSpec = new DoctorHonApprSpecification(appParrams);
            var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
            var investmentDetailSpec = new InvestmentDetailSpecification(investmentInitId);
            var investmentDoctorSpec = new InvestmentDoctorSpecification(investmentInitId);

            var doctorHonApprDetail = await _doctorHonApprRepo.ListAsync(doctorHonApprSpec);
            var investmentDetail = await _investmentDetailRepo.ListAsync(spec);
            var investmentDoctor = await _investmentDoctorRepo.ListAsync(spec);

            var countSpec = new DoctorHonApprWithFiltersForCountSpecificication(appParrams);

            var totalItems = await _doctorHonApprRepo.CountAsync(countSpec);

           

            var data = _mapper.Map<IReadOnlyList<DoctorHonAppr>, IReadOnlyList<DoctorHonApprDto>>(posts);

            return Ok(new Pagination<DoctorHonApprDto>(appParrams.PageIndex, appParrams.PageSize, totalItems, data));

        }

    }
}
