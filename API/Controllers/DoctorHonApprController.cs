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
using System.Globalization;

namespace API.Controllers
{
    public class DoctorHonApprController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentApr> _investmentAprRepo;
        private readonly IGenericRepository<InvestmentDoctor> _investmentDoctorRepo;
        private readonly IGenericRepository<DoctorHonAppr> _doctorHonApprRepo;
        private readonly IMapper _mapper;
        public DoctorHonApprController(IGenericRepository<DoctorHonAppr> doctorHonApprRepo, IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentApr> investmentAprRepo, IGenericRepository<InvestmentDoctor> investmentDoctorRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _doctorHonApprRepo = doctorHonApprRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentAprRepo = investmentAprRepo;
            _investmentDoctorRepo = investmentDoctorRepo;
        }

        [HttpGet("GetAllData/{honMonth}")]
        public async Task<ActionResult<List<DoctorHonAppr>>> GetAllDoctorHonApprInfo(string honMonth, [FromQuery] DoctorHonApprSpecParams appParrams)
        {
            try
            {
                var doctorHonApprRec=new List<DoctorHonAppr>();
                DateTime now = DateTime.ParseExact(honMonth, "MMyyyy", CultureInfo.InvariantCulture);
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var doctorHonApprSpec = new DoctorHonApprSpecification(honMonth);
                //var investmentInitSpec = new InvestmentInitSpecification("Honorarium");
                var investmentAprSpec = new InvestmentAprSpecification("Honorarium");
                var investmentDoctorSpec = new InvestmentDoctorSpecification("Honorarium");

                var doctorHonApprApr = await _doctorHonApprRepo.ListAsync(doctorHonApprSpec);
                var investmentApr = await _investmentAprRepo.ListAsync(investmentAprSpec);
                var investmentDoctor = await _investmentDoctorRepo.ListAsync(investmentDoctorSpec);
                if (doctorHonApprApr.Count > 0)
                {
                     doctorHonApprRec = (from i in investmentApr
                                            join d in investmentDoctor on i.InvestmentInitId equals d.InvestmentInitId
                                            join h in doctorHonApprApr on d.InvestmentInitId equals h.InvestmentInitId into dh
                                            from p in dh.DefaultIfEmpty()
                                            where i.FromDate<=startDate && i.ToDate>=endDate
                                            orderby i.FromDate
                                            select new DoctorHonAppr
                                            {
                                                Id= p == null ? 0 : p.Id,
                                                InvestmentInitId = i.InvestmentInitId,
                                                DoctorId = d.DoctorId,
                                                DoctorInfo = d.DoctorInfo,
                                                HonAmount = i.ProposedAmount / i.TotalMonth,
                                                HonMonth = p.HonMonth,
                                                Status = p == null ? "Pending" : p.Status,
                                            }
                                      ).Distinct().ToList();
                }
                else {
                    doctorHonApprRec = (from i in investmentApr
                                        join d in investmentDoctor on i.InvestmentInitId equals d.InvestmentInitId
                                        where i.FromDate <= startDate && i.ToDate >= endDate
                                        orderby i.FromDate
                                        select new DoctorHonAppr
                                        {   Id=0,
                                            InvestmentInitId = i.InvestmentInitId,
                                            DoctorId = d.DoctorId,
                                            DoctorInfo = d.DoctorInfo,
                                            HonAmount = i.ProposedAmount / i.TotalMonth,
                                            HonMonth = honMonth,
                                            Status ="Pending",
                                        }
                                         ).Distinct().ToList();
                }

                var countSpec = new DoctorHonApprWithFiltersForCountSpecificication(appParrams);

                var totalItems = await _doctorHonApprRepo.CountAsync(countSpec);



                //var data = _mapper.Map<IReadOnlyList<DoctorHonAppr>, IReadOnlyList<DoctorHonApprDto>>(doctorHonApprApr);

                return Ok(new Pagination<DoctorHonAppr>(appParrams.PageIndex, appParrams.PageSize, totalItems, doctorHonApprRec));
            }
            catch (Exception ex)
            {

                throw  ex;
            }
        }

        [HttpPost("insertDocHonAppr")]
        public ActionResult<DoctorHonApprDto> InsertDoctorHonAppr(DoctorHonApprDto doctorHonApprDto)
        {
            
            var doctorHonApr = new DoctorHonAppr
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentInitId = doctorHonApprDto.InvestmentInitId,
                DoctorId = doctorHonApprDto.DoctorId,
                HonAmount = doctorHonApprDto.HonAmount,
                HonMonth = doctorHonApprDto.HonMonth,
                Status = doctorHonApprDto.Status,
                SetOn = DateTimeOffset.Now
            };
            _doctorHonApprRepo.Add(doctorHonApr);
            _doctorHonApprRepo.Savechange();

            return new DoctorHonApprDto
            {
                Id = doctorHonApr.Id,
                InvestmentInitId = doctorHonApprDto.InvestmentInitId,
                DoctorId = doctorHonApprDto.DoctorId,
                HonAmount = doctorHonApprDto.HonAmount,
                HonMonth = doctorHonApprDto.HonMonth,
                Status = doctorHonApprDto.Status,
            };
        }
         [HttpPost("updateDocHonAppr")]
        public ActionResult<DoctorHonApprDto> UpdateDoctorHonAppr(DoctorHonApprDto doctorHonApprDto)
        {
            
            var doctorHonApr = new DoctorHonAppr
            {
                Id = doctorHonApprDto.Id,
                InvestmentInitId = doctorHonApprDto.InvestmentInitId,
                DoctorId = doctorHonApprDto.DoctorId,
                HonAmount = doctorHonApprDto.HonAmount,
                HonMonth = doctorHonApprDto.HonMonth,
                Status = doctorHonApprDto.Status,
                SetOn = DateTimeOffset.Now
            };
            _doctorHonApprRepo.Update(doctorHonApr);
            _doctorHonApprRepo.Savechange();

            return new DoctorHonApprDto
            {
                Id = doctorHonApr.Id,
                InvestmentInitId = doctorHonApprDto.InvestmentInitId,
                DoctorId = doctorHonApprDto.DoctorId,
                HonAmount = doctorHonApprDto.HonAmount,
                HonMonth = doctorHonApprDto.HonMonth,
                Status = doctorHonApprDto.Status,
            };
        }

    }
}
