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
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentDoctor> _investmentDoctorRepo;
        private readonly IGenericRepository<InvestmentInstitution> _investmentInstitutionRepo;
        private readonly IMapper _mapper;

        public InvestmentController(IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentDoctor> investmentDoctorRepo,
            IGenericRepository<InvestmentInstitution> investmentInstitutionRepo, IMapper mapper)
        {
            _mapper = mapper;
            _investmentInitRepo = investmentInitRepo;
            _investmentDoctorRepo = investmentDoctorRepo;
            _investmentInstitutionRepo = investmentInstitutionRepo;
        }
        [HttpGet("investmentInits")]
        public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetInvestmentInits(
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                var spec = new InvestmentInitSpecification(investmentInitParrams);

                var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                var marketGroup = await _investmentInitRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(marketGroup);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, data));
            }
            catch (System.Exception e)
            {

                throw;
            }
        }
        [HttpPost("insertInit")]
        public async Task<InvestmentInitDto> InsertInvestmentInit(InvestmentInitDto investmentInitDto)
        {
            try
            {
                var data = await _investmentInitRepo.ListAllAsync();
                var referenceNo = "";
                if (data.Count > 0)
                {
                    var investmentInitLastId = (from r in data
                                                orderby r.Id
                                                select new InvestmentInitDto
                                                {
                                                    Id = r.Id,

                                                }
                                     ).Last();
                    referenceNo = DateTimeOffset.Now.ToString("yyyyMM") + (investmentInitLastId.Id + 1).ToString("00000");
                }
                else
                {
                    referenceNo = DateTimeOffset.Now.ToString("yyyyMM") + (0 + 1).ToString("00000");
                }
                var investmentInit = new InvestmentInit
                {
                    //ReferenceNo = investmentInitDto.ReferenceNo,
                    ReferenceNo = referenceNo,
                    ProposeFor = investmentInitDto.ProposeFor,
                    DonationTo = investmentInitDto.DonationTo,
                    DonationType = investmentInitDto.DonationType,
                    EmployeeId = investmentInitDto.EmployeeId,
                    SetOn = DateTimeOffset.Now
                };
                _investmentInitRepo.Add(investmentInit);
                _investmentInitRepo.Savechange();

                return new InvestmentInitDto
                {
                    Id = investmentInit.Id,
                    ReferenceNo = investmentInit.ReferenceNo,
                    ProposeFor = investmentInit.ProposeFor,
                    DonationTo = investmentInit.DonationTo,
                    DonationType = investmentInit.DonationType,
                    EmployeeId = investmentInit.EmployeeId
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost("updateInit")]
        public ActionResult<InvestmentInitDto> UpdateInvestmentInit(InvestmentInitDto investmentInitDto)
        {
            try
            {
                var investmentInit = new InvestmentInit
                {
                    Id = investmentInitDto.Id,
                    ReferenceNo = investmentInitDto.ReferenceNo,
                    ProposeFor = investmentInitDto.ProposeFor,
                    DonationTo = investmentInitDto.DonationTo,
                    DonationType = investmentInitDto.DonationType,
                    EmployeeId = investmentInitDto.EmployeeId,
                    ModifiedOn = DateTimeOffset.Now,
                };
                _investmentInitRepo.Update(investmentInit);
                _investmentInitRepo.Savechange();

                return new InvestmentInitDto
                {
                    Id = investmentInit.Id,
                    ReferenceNo = investmentInit.ReferenceNo,
                    ProposeFor = investmentInit.ProposeFor,
                    DonationTo = investmentInit.DonationTo,
                    DonationType = investmentInit.DonationType,
                    EmployeeId = investmentInit.EmployeeId
                };

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("investmentDoctors/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentDoctor>> GetInvestmentDoctors(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentDoctorSpecification(investmentInitId);
                var investmentDoctor = await _investmentDoctorRepo.ListAsync(spec);
                return investmentDoctor;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        

        [HttpPost("insertInvestmentDoctor")]
        public async Task<InvestmentDoctorDto> InsertInvestmentDoctor(InvestmentDoctorDto investmentDoctorDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentDoctorSpecification(investmentDoctorDto.InvestmentInitId);
                var alreadyExistInvestmentDoctorList = await _investmentDoctorRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentDoctorList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentDoctorList)
                    {
                        _investmentDoctorRepo.Delete(v);
                        _investmentDoctorRepo.Savechange();
                    }
                }
                
                var investmentDoctor = new InvestmentDoctor
                {
                    //ReferenceNo = investmentDoctorDto.ReferenceNo,
                    InvestmentInitId = investmentDoctorDto.InvestmentInitId,
                    DoctorId = investmentDoctorDto.DoctorId,
                    InstitutionId = investmentDoctorDto.InstitutionId,
                    DoctorType = investmentDoctorDto.DoctorType,
                    DoctorCategory = investmentDoctorDto.DoctorCategory,
                    PatientsPerDay = investmentDoctorDto.PatientsPerDay,
                    PracticeDayPerMonth = investmentDoctorDto.PracticeDayPerMonth,
                    SetOn = DateTimeOffset.Now
                };
                _investmentDoctorRepo.Add(investmentDoctor);
                _investmentDoctorRepo.Savechange();

                return new InvestmentDoctorDto
                {
                    Id = investmentDoctor.Id,
                    InvestmentInitId = investmentDoctorDto.InvestmentInitId,
                    DoctorId = investmentDoctorDto.DoctorId,
                    InstitutionId = investmentDoctorDto.InstitutionId,
                    DoctorType = investmentDoctorDto.DoctorType,
                    DoctorCategory = investmentDoctorDto.DoctorCategory,
                    PatientsPerDay = investmentDoctorDto.PatientsPerDay,
                    PracticeDayPerMonth = investmentDoctorDto.PracticeDayPerMonth,
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost("insertInvestmentInstitution")]
        public async Task<InvestmentInstitutionDto> InsertInvestmentInstitution(InvestmentInstitutionDto investmentInstitutionDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentInstitutionSpecification(investmentInstitutionDto.InvestmentInitId);
                var alreadyExistInvestmentInstitutionList = await _investmentInstitutionRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentInstitutionList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentInstitutionList)
                    {
                        _investmentInstitutionRepo.Delete(v);
                        _investmentInstitutionRepo.Savechange();
                    }
                }
                
                var investmentInstitution = new InvestmentInstitution
                {
                    //ReferenceNo = investmentInstitutionDto.ReferenceNo,
                    InvestmentInitId = investmentInstitutionDto.InvestmentInitId,
                    InstitutionId = investmentInstitutionDto.InstitutionId,
                    ResposnsibleDoctorId = investmentInstitutionDto.ResposnsibleDoctorId,
                    NoOfBed = investmentInstitutionDto.NoOfBed,
                    DepartmentUnit = investmentInstitutionDto.DepartmentUnit,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentInstitutionRepo.Add(investmentInstitution);
                _investmentInstitutionRepo.Savechange();

                return new InvestmentInstitutionDto
                {
                    Id = investmentInstitution.Id,
                    InvestmentInitId = investmentInstitutionDto.InvestmentInitId,
                    InstitutionId = investmentInstitutionDto.InstitutionId,
                    ResposnsibleDoctorId = investmentInstitutionDto.ResposnsibleDoctorId,
                    NoOfBed = investmentInstitutionDto.NoOfBed,
                    DepartmentUnit = investmentInstitutionDto.DepartmentUnit,
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("investmentInstitutions/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentInstitution>> GetInvestmentInstitutions(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentInstitutionSpecification(investmentInitId);
                var investmentInstitution = await _investmentInstitutionRepo.ListAsync(spec);
                return investmentInstitution;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("removeInvestmentInstitution")]
        public async Task<IActionResult> RemoveInvestmentInstitution(InvestmentInstitutionDto investmentInstitutionDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentInstitutionSpecification(investmentInstitutionDto.InvestmentInitId);
                var alreadyExistInvestmentInstitutionList = await _investmentInstitutionRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentInstitutionList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentInstitutionList)
                    {
                        _investmentInstitutionRepo.Delete(v);
                        _investmentInstitutionRepo.Savechange();
                    }

                    return Ok();
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } [HttpPost("removeInvestmentDoctor")]
        public async Task<IActionResult> RemoveInvestmentDoctor(InvestmentDoctorDto investmentDoctorDto)
        {
            try
            {
                //var response = new HttpResponseMessage();
                var alreadyExistSpec = new InvestmentDoctorSpecification(investmentDoctorDto.InvestmentInitId);
                var alreadyExistInvestmentDoctorList = await _investmentDoctorRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentDoctorList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentDoctorList)
                    {
                        _investmentDoctorRepo.Delete(v);
                        _investmentDoctorRepo.Savechange();
                    }

                    //response.Headers.Add("Message", "Succsessfuly Deleted!!!");
                    //return response;
                    return Ok("Succsessfuly Deleted!!!");
                }
                //response.Headers.Add("Message", "Failed!!!");
                //return response;
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } 
    }
}
