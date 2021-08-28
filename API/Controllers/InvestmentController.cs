﻿using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentDetail> _investmentDetailRepo;
        private readonly IGenericRepository<InvestmentTargetedProd> _investmentTargetedProdRepo;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;
        private readonly IGenericRepository<InvestmentDoctor> _investmentDoctorRepo;
        private readonly IGenericRepository<InvestmentInstitution> _investmentInstitutionRepo;
        private readonly IGenericRepository<InvestmentCampaign> _investmentCampaignRepo;
        private readonly IGenericRepository<InvestmentBcds> _investmentBcdsRepo;
        private readonly IGenericRepository<InvestmentSociety> _investmentSocietyRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;


        public InvestmentController(IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo, IGenericRepository<InvestmentTargetedProd> investmentTargetedProdRepo,
            IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentDetail> investmentDetailRepo, IGenericRepository<InvestmentDoctor> investmentDoctorRepo,
            IGenericRepository<InvestmentSociety> investmentSocietyRepo, IGenericRepository<InvestmentBcds> investmentBcdsRepo,
            IGenericRepository<InvestmentCampaign> investmentCampaignRepo, IGenericRepository<InvestmentInstitution> investmentInstitutionRepo,
            IGenericRepository<Employee> employeeRepo, IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
            IGenericRepository<InvestmentRecComment> investmentRecCommentRepo, StoreContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _investmentInitRepo = investmentInitRepo;
            _investmentDetailRepo = investmentDetailRepo;
            _investmentTargetedProdRepo = investmentTargetedProdRepo;
            _investmentTargetedGroupRepo = investmentTargetedGroupRepo;
            _investmentDoctorRepo = investmentDoctorRepo;
            _investmentInstitutionRepo = investmentInstitutionRepo;
            _investmentCampaignRepo = investmentCampaignRepo;
            _investmentBcdsRepo = investmentBcdsRepo;
            _investmentSocietyRepo = investmentSocietyRepo;
            _employeeRepo = employeeRepo;
            _reportInvestmentInfoRepo = reportInvestmentInfoRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _dbContext = dbContext;
        }
        #region investmentInit

      
        [HttpGet("investmentInits/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams)
        {
            try
            {
                //var empData = await _employeeRepo.GetByIdAsync(empId);
                investmentInitParrams.Search = sbu;
                //var spec = new InvestmentInitSpecification(investmentInitParrams);

                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                //var investmentInits = await _investmentInitRepo.ListAsync(spec);

                //investmentRecCommentParrams.Search = sbu;
                //var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);
                //var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);

                //var data = (from i in investmentInits
                //            where i.MarketCode == empData.MarketCode && i.SBU==sbu
                //            && !(from rc in investmentRecComments
                //                 where rc.RecStatus == "Recommended"
                //                 select rc.InvestmentInitId).Contains(i.Id)
                //            orderby i.SetOn descending
                //            select new InvestmentInitDto
                //            {
                //                Id = i.Id,
                //                ReferenceNo = i.ReferenceNo,
                //                ProposeFor = i.ProposeFor,
                //                DonationType = i.DonationType,
                //                DonationTo = i.DonationTo,
                //                EmployeeId = i.EmployeeId
                //            }
                //              ).Distinct().ToList();
                // var results = _dbContext.Query<InvestmentInitDto>().FromSql("EXECUTE dbo.SP_InvestmentInitSearch {0},{1}", sbu,empId).ToList();
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentInitSearch @SBU,@EID", parms.ToArray()).ToList();
               
                 
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

              //  return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, results));
                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 10, data));
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [HttpGet("investmentInitsOthers/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInitsOthers(int empId, string sbu,
        [FromQuery] InvestmentInitSpecParams investmentInitParrams, [FromQuery] InvestmentTargetedGroupSpecParams investmentTargetedGroupParrams,
        [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams)
        {
            try
            {
                //investmentInitParrams.Search = sbu;
                //var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                //var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);

                //var empData = await _employeeRepo.GetByIdAsync(empId);

                //investmentTargetedGroupParrams.Search = empData.MarketCode;
                //var spec = new InvestmentTargetedGroupSpecification(investmentTargetedGroupParrams);
                //var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(spec);

                //investmentRecCommentParrams.Search = empData.SBU;
                //var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);
                //var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);

                //var data = (from i in investmentInits
                //            join t in investmentTargetedGroup on i.Id equals t.InvestmentInitId
                //            where t.MarketCode == empData.MarketCode && !(from rc in investmentRecComments
                //                                                          select rc.InvestmentInitId).Contains(i.Id)
                //            orderby t.MarketName
                //            select new InvestmentInitDto
                //            {
                //                Id = i.Id,
                //                ReferenceNo = i.ReferenceNo,
                //                ProposeFor = i.ProposeFor,
                //                DonationType = i.DonationType,
                //                DonationTo = i.DonationTo,
                //                EmployeeId = i.EmployeeId
                //            }
                //              ).Distinct().ToList();


                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);



                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentInitSearch @SBU,@EID", parms.ToArray()).ToList();


                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {

                throw e;
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
                var empData = await _employeeRepo.GetByIdAsync(investmentInitDto.EmployeeId);
                var investmentInit = new InvestmentInit
                {
                    //ReferenceNo = investmentInitDto.ReferenceNo,
                    ReferenceNo = referenceNo,
                    ProposeFor = investmentInitDto.ProposeFor,
                    DonationTo = investmentInitDto.DonationTo,
                    DonationType = investmentInitDto.DonationType,
                    EmployeeId = investmentInitDto.EmployeeId,
                    PostingType = empData.PostingType,
                    MarketCode = empData.MarketCode,
                    MarketName = empData.MarketName,
                    RegionCode = empData.RegionCode,
                    RegionName = empData.RegionName,
                    ZoneCode = empData.ZoneCode,
                    ZoneName = empData.ZoneName,
                    TerritoryCode = empData.TerritoryCode,
                    TerritoryName = empData.TerritoryName,
                    DivisionCode = empData.DivisionCode,
                    DivisionName = empData.DivisionName,
                    SBU = empData.SBU,
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

                throw ex;
            }
        }

        [HttpPost("updateInit")]
        public async Task<ActionResult<InvestmentInitDto>> UpdateInvestmentInit(InvestmentInitDto investmentInitDto)
        {
            try
            {
                var empData = await _employeeRepo.GetByIdAsync(investmentInitDto.EmployeeId);
                var investmentInit = new InvestmentInit
                {
                    Id = investmentInitDto.Id,
                    ReferenceNo = investmentInitDto.ReferenceNo,
                    ProposeFor = investmentInitDto.ProposeFor,
                    DonationTo = investmentInitDto.DonationTo,
                    DonationType = investmentInitDto.DonationType,
                    EmployeeId = investmentInitDto.EmployeeId,
                    PostingType = empData.PostingType,
                    MarketCode = empData.MarketCode,
                    MarketName = empData.MarketName,
                    RegionCode = empData.RegionCode,
                    RegionName = empData.RegionName,
                    ZoneCode = empData.ZoneCode,
                    ZoneName = empData.ZoneName,
                    TerritoryCode = empData.TerritoryCode,
                    TerritoryName = empData.TerritoryName,
                    DivisionCode = empData.DivisionCode,
                    DivisionName = empData.DivisionName,
                    SBU = empData.SBU,
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

                throw ex;
            }
        }
        #endregion
        #region investmentDetail

        [HttpPost("insertDetail")]
        public ActionResult<InvestmentDetailDto> InsertInvestmentDetail(InvestmentDetailDto investmentDetailDto)
        {
            try
            {
                var investmentDetail = new InvestmentDetail
                {
                    ChequeTitle = investmentDetailDto.ChequeTitle,
                    PaymentMethod = investmentDetailDto.PaymentMethod,
                    CommitmentAllSBU = investmentDetailDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentDetailDto.CommitmentOwnSBU,
                    FromDate = investmentDetailDto.FromDate,
                    ToDate = investmentDetailDto.ToDate,
                    TotalMonth = investmentDetailDto.TotalMonth,
                    ProposedAmount = investmentDetailDto.ProposedAmount,
                    Purpose = investmentDetailDto.Purpose,
                    InvestmentInitId = investmentDetailDto.InvestmentInitId,
                    SetOn = DateTimeOffset.Now
                };
                _investmentDetailRepo.Add(investmentDetail);
                _investmentDetailRepo.Savechange();

                return new InvestmentDetailDto
                {
                    Id = investmentDetail.Id,
                    ChequeTitle = investmentDetailDto.ChequeTitle,
                    PaymentMethod = investmentDetailDto.PaymentMethod,
                    CommitmentAllSBU = investmentDetailDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentDetailDto.CommitmentOwnSBU,
                    FromDate = investmentDetailDto.FromDate,
                    ToDate = investmentDetailDto.ToDate,
                    TotalMonth = investmentDetailDto.TotalMonth,
                    ProposedAmount = investmentDetailDto.ProposedAmount,
                    Purpose = investmentDetailDto.Purpose,
                    InvestmentInitId = investmentDetailDto.InvestmentInitId,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("updateDetail")]
        public ActionResult<InvestmentDetailDto> UpdateInvestmentDetail(InvestmentDetailDto investmentDetailDto)
        {
            try
            {
                var investmentDetail = new InvestmentDetail
                {
                    Id = investmentDetailDto.Id,
                    ChequeTitle = investmentDetailDto.ChequeTitle,
                    PaymentMethod = investmentDetailDto.PaymentMethod,
                    CommitmentAllSBU = investmentDetailDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentDetailDto.CommitmentOwnSBU,
                    FromDate = investmentDetailDto.FromDate,
                    ToDate = investmentDetailDto.ToDate,
                    TotalMonth = investmentDetailDto.TotalMonth,
                    ProposedAmount = investmentDetailDto.ProposedAmount,
                    Purpose = investmentDetailDto.Purpose,
                    InvestmentInitId = investmentDetailDto.InvestmentInitId,
                    ModifiedOn = DateTimeOffset.Now,
                };
                _investmentDetailRepo.Update(investmentDetail);
                _investmentDetailRepo.Savechange();

                return new InvestmentDetailDto
                {
                    Id = investmentDetail.Id,
                    ChequeTitle = investmentDetailDto.ChequeTitle,
                    PaymentMethod = investmentDetailDto.PaymentMethod,
                    CommitmentAllSBU = investmentDetailDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentDetailDto.CommitmentOwnSBU,
                    FromDate = investmentDetailDto.FromDate,
                    ToDate = investmentDetailDto.ToDate,
                    TotalMonth = investmentDetailDto.TotalMonth,
                    ProposedAmount = investmentDetailDto.ProposedAmount,
                    Purpose = investmentDetailDto.Purpose,
                    InvestmentInitId = investmentDetailDto.InvestmentInitId,
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("investmentDetails/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentDetail>> GetInvestmentDetails(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentDetailSpecification(investmentInitId);
                var investmentDetail = await _investmentDetailRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        
        #region investmentTargetedProd
        [HttpPost("insertInvestmentTargetedProd")]
        public ActionResult<InvestmentTargetedProdDto> InsertInvestmentTargetedProd(InvestmentTargetedProdDto investmentTargetedProdDto)
        {
            try
            {
                //var alreadyExistSpec = new InvestmentTargetedProdSpecification(investmentTargetedProdDto.InvestmentInitId);
                //var alreadyExistInvestmentTargetedProdList = await _investmentTargetedProdRepo.ListAsync(alreadyExistSpec);
                //if (alreadyExistInvestmentTargetedProdList.Count > 0)
                //{
                //    foreach (var v in alreadyExistInvestmentTargetedProdList)
                //    {
                //        _investmentTargetedProdRepo.Delete(v);
                //        _investmentTargetedProdRepo.Savechange();
                //    }
                //}

                var investmentTargetedProd = new InvestmentTargetedProd
                {
                    //ReferenceNo = investmentTargetedProdDto.ReferenceNo,
                    InvestmentInitId = investmentTargetedProdDto.InvestmentInitId,
                    ProductId = investmentTargetedProdDto.ProductId,
                    SBU = investmentTargetedProdDto.SBU,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentTargetedProdRepo.Add(investmentTargetedProd);
                _investmentTargetedProdRepo.Savechange();

                return new InvestmentTargetedProdDto
                {
                    Id = investmentTargetedProd.Id,
                    InvestmentInitId = investmentTargetedProdDto.InvestmentInitId,
                    ProductId = investmentTargetedProdDto.ProductId,
                    SBU = investmentTargetedProdDto.SBU,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("updateInvestmentTargetedProd")]
        public ActionResult<InvestmentTargetedProdDto> UpdateInvestmentTargetedProd(InvestmentTargetedProdDto investmentTargetedProdDto)
        {
            try
            {
                var investmentTargetedProd = new InvestmentTargetedProd
                {
                    Id = investmentTargetedProdDto.Id,
                    InvestmentInitId = investmentTargetedProdDto.InvestmentInitId,
                    ProductId = investmentTargetedProdDto.ProductId,
                    SBU = investmentTargetedProdDto.SBU,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentTargetedProdRepo.Update(investmentTargetedProd);
                _investmentTargetedProdRepo.Savechange();

                return new InvestmentTargetedProdDto
                {
                    Id = investmentTargetedProd.Id,
                    InvestmentInitId = investmentTargetedProdDto.InvestmentInitId,
                    ProductId = investmentTargetedProdDto.ProductId,
                    SBU = investmentTargetedProdDto.SBU,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        [HttpGet]
        [Route("investmentTargetedProdsForRec/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentTargetedProd>> GetInvestmentTargetedProds(int investmentInitId, string ssbu)
        {
            try
            {
                var spec = new InvestmentTargetedProdSpecification(investmentInitId);
                var investmentTargetedProd = await _investmentTargetedProdRepo.ListAsync(spec);
                return investmentTargetedProd;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpGet]
        [Route("investmentTargetedProds/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentTargetedProd>> GetInvestmentTargetedProds(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentTargetedProdSpecification(investmentInitId);
                var investmentTargetedProd = await _investmentTargetedProdRepo.ListAsync(spec);
                return investmentTargetedProd;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region investmentTargetedGroup
        [HttpPost("insertInvestmentTargetedGroup")]
        public async Task<IActionResult> InsertInvestmentTargetedGroup(List<InvestmentTargetedGroupDto> investmentTargetedGroupDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentTargetedGroupSpecification(investmentTargetedGroupDto[0].InvestmentInitId, investmentTargetedGroupDto[0].MarketGroupMstId);
                var alreadyExistInvestmentTargetedGroupList = await _investmentTargetedGroupRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentTargetedGroupList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentTargetedGroupList)
                    {
                        _investmentTargetedGroupRepo.Delete(v);
                        _investmentTargetedGroupRepo.Savechange();
                    }
                }
                foreach (var v in investmentTargetedGroupDto)
                {
                    var investmentTargetedGroup = new InvestmentTargetedGroup
                    {
                        InvestmentInitId = v.InvestmentInitId,
                        MarketCode = v.MarketCode,
                        MarketName = v.MarketName,
                        MarketGroupMstId = v.MarketGroupMstId,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now,
                        DataStatus = 0
                    };
                    _investmentTargetedGroupRepo.Add(investmentTargetedGroup);
                }

                _investmentTargetedGroupRepo.Savechange();

                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
      
        [HttpGet]
        [Route("investmentTargetedGroups/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentTargetedGroup>> GetInvestmentTargetedGroups(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentTargetedGroupSpecification(investmentInitId);
                var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(spec);
                return investmentTargetedGroup;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost("removeInvestmentTargetedProd")]
        public async Task<IActionResult> RemoveInvestmentTargetedProd(InvestmentTargetedProd investmentTargetedProd)
        {
            try
            {
                //var response = new HttpResponseMessage();
                var alreadyExistSpec = new InvestmentTargetedProdSpecification(investmentTargetedProd.InvestmentInitId, investmentTargetedProd.ProductId);
                var alreadyExistInvestmentTargetedProdList = await _investmentTargetedProdRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentTargetedProdList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentTargetedProdList)
                    {
                        _investmentTargetedProdRepo.Delete(v);
                        _investmentTargetedProdRepo.Savechange();
                    }

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
       
        [HttpPost("removeInvestmentTargetedGroup")]
        public async Task<IActionResult> RemoveInvestmentTargetedGroup(List<InvestmentTargetedGroupDto> investmentTargetedGroupDto)
        {
            try
            {
                //var response = new HttpResponseMessage();
                var alreadyExistSpec = new InvestmentTargetedGroupSpecification(investmentTargetedGroupDto[0].InvestmentInitId, investmentTargetedGroupDto[0].MarketGroupMstId);
                var alreadyExistInvestmentTargetedGroupList = await _investmentTargetedGroupRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentTargetedGroupList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentTargetedGroupList)
                    {
                        _investmentTargetedGroupRepo.Delete(v);
                        _investmentTargetedGroupRepo.Savechange();
                    }

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        #region investmentDoctor
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

                throw ex;
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

        [HttpPost("removeInvestmentDoctor")]
        public async Task<IActionResult> RemoveInvestmentDoctor(InvestmentDoctorDto investmentDoctorDto)
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

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region investmentInstitution
        [HttpPost("insertInvestmentInstitution")]
        public ActionResult<InvestmentInstitutionDto> InsertInvestmentInstitution(InvestmentInstitutionDto investmentInstitutionDto)
        {
            try
            {
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

                throw ex;
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

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region investmentCampaign
        [HttpPost("insertInvestmentCampaign")]
        public async Task<InvestmentCampaignDto> InsertInvestmentCampaign(InvestmentCampaignDto investmentCampaignDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentCampaignSpecification(investmentCampaignDto.InvestmentInitId);
                var alreadyExistInvestmentCampaignList = await _investmentCampaignRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentCampaignList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentCampaignList)
                    {
                        _investmentCampaignRepo.Delete(v);
                        _investmentCampaignRepo.Savechange();
                    }
                }

                var investmentCampaign = new InvestmentCampaign
                {
                    //ReferenceNo = investmentCampaignDto.ReferenceNo,
                    InvestmentInitId = investmentCampaignDto.InvestmentInitId,
                    CampaignDtlId = investmentCampaignDto.CampaignDtlId,
                    DoctorId = investmentCampaignDto.DoctorId,
                    InstitutionId = investmentCampaignDto.InstitutionId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentCampaignRepo.Add(investmentCampaign);
                _investmentCampaignRepo.Savechange();

                return new InvestmentCampaignDto
                {
                    Id = investmentCampaign.Id,
                    InvestmentInitId = investmentCampaignDto.InvestmentInitId,
                    CampaignDtlId = investmentCampaignDto.CampaignDtlId,
                    DoctorId = investmentCampaignDto.DoctorId,
                    InstitutionId = investmentCampaignDto.InstitutionId
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
        [HttpGet]
        [Route("investmentCampaigns/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentCampaign>> GetInvestmentCampaigns(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentCampaignSpecification(investmentInitId);
                var investmentCampaign = await _investmentCampaignRepo.ListAsync(spec);
                return investmentCampaign;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
       
        [HttpPost("removeInvestmentCampaign")]
        public async Task<IActionResult> RemoveInvestmentCampaign(InvestmentCampaignDto investmentCampaignDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentCampaignSpecification(investmentCampaignDto.InvestmentInitId);
                var alreadyExistInvestmentCampaignList = await _investmentCampaignRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentCampaignList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentCampaignList)
                    {
                        _investmentCampaignRepo.Delete(v);
                        _investmentCampaignRepo.Savechange();
                    }

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region investmentBcds
        [HttpPost("insertInvestmentBcds")]
        public async Task<InvestmentBcdsDto> InsertInvestmentBcds(InvestmentBcdsDto investmentBcdsDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentBcdsSpecification(investmentBcdsDto.InvestmentInitId);
                var alreadyExistInvestmentBcdsList = await _investmentBcdsRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentBcdsList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentBcdsList)
                    {
                        _investmentBcdsRepo.Delete(v);
                        _investmentBcdsRepo.Savechange();
                    }
                }

                var investmentBcds = new InvestmentBcds
                {
                    //ReferenceNo = investmentBcdsDto.ReferenceNo,
                    InvestmentInitId = investmentBcdsDto.InvestmentInitId,
                    BcdsId = investmentBcdsDto.BcdsId,

                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentBcdsRepo.Add(investmentBcds);
                _investmentBcdsRepo.Savechange();

                return new InvestmentBcdsDto
                {
                    Id = investmentBcds.Id,
                    InvestmentInitId = investmentBcdsDto.InvestmentInitId,
                    BcdsId = investmentBcdsDto.BcdsId,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       
        [HttpGet]
        [Route("investmentBcds/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentBcds>> GetInvestmentBcds(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentBcdsSpecification(investmentInitId);
                var investmentBcds = await _investmentBcdsRepo.ListAsync(spec);
                return investmentBcds;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
       
        [HttpPost("removeInvestmentBcds")]
        public async Task<IActionResult> RemoveInvestmentBcds(InvestmentBcdsDto investmentBcdsDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentBcdsSpecification(investmentBcdsDto.InvestmentInitId);
                var alreadyExistInvestmentBcdsList = await _investmentBcdsRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentBcdsList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentBcdsList)
                    {
                        _investmentBcdsRepo.Delete(v);
                        _investmentBcdsRepo.Savechange();
                    }

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region investmentSociety
        [HttpPost("insertInvestmentSociety")]
        public async Task<InvestmentSocietyDto> InsertInvestmentSociety(InvestmentSocietyDto investmentSocietyDto)
        {
            try
            {
                var alreadyExistSpec = new InvestmentSocietySpecification(investmentSocietyDto.InvestmentInitId);
                var alreadyExistInvestmentSocietyList = await _investmentSocietyRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentSocietyList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentSocietyList)
                    {
                        _investmentSocietyRepo.Delete(v);
                        _investmentSocietyRepo.Savechange();
                    }
                }

                var investmentSociety = new InvestmentSociety
                {
                    //ReferenceNo = investmentSocietyDto.ReferenceNo,
                    InvestmentInitId = investmentSocietyDto.InvestmentInitId,
                    SocietyId = investmentSocietyDto.SocietyId,

                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentSocietyRepo.Add(investmentSociety);
                _investmentSocietyRepo.Savechange();

                return new InvestmentSocietyDto
                {
                    Id = investmentSociety.Id,
                    InvestmentInitId = investmentSocietyDto.InvestmentInitId,
                    SocietyId = investmentSocietyDto.SocietyId,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentSociety/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentSociety>> GetInvestmentSociety(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentSocietySpecification(investmentInitId);
                var investmentSociety = await _investmentSocietyRepo.ListAsync(spec);
                return investmentSociety;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
     
        [HttpPost("removeInvestmentSociety")]
        public async Task<IActionResult> RemoveInvestmentSociety(InvestmentSociety investmentSociety)
        {
            try
            {
                var alreadyExistSpec = new InvestmentSocietySpecification(investmentSociety.InvestmentInitId);
                var alreadyExistInvestmentSocietyList = await _investmentSocietyRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentSocietyList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentSocietyList)
                    {
                        _investmentSocietyRepo.Delete(v);
                        _investmentSocietyRepo.Savechange();
                    }

                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion













        [HttpGet]
        [Route("getLastFiveInvestment/{marketCode}/{date}")]
        public async Task<IReadOnlyList<ReportInvestmentInfo>> GetLastFiveInvestment(int marketCode, string date)
        {
            try
            {
                var v = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
               // var empData = await _employeeRepo.GetByIdAsync(empId);
                var reportInvestmentSpec = new ReportInvestmentSpecification(marketCode);
                var reportInvestmentData = await _reportInvestmentInfoRepo.ListAsync(reportInvestmentSpec);
                // var spec = new ReportInvestmentSpecification(empData.MarketCode);
                var data = (from e in reportInvestmentData
                            where DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture)
                            orderby DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) descending
                            select new ReportInvestmentInfo
                            {
                                InvestmentAmount = e.InvestmentAmount,
                                ComtSharePrcnt = e.ComtSharePrcnt,
                                ComtSharePrcntAll = e.ComtSharePrcntAll,
                                PrescribedSharePrcnt = e.PrescribedSharePrcnt,
                                PrescribedSharePrcntAll = e.PrescribedSharePrcntAll
                            }
                             ).Distinct().Take(5).ToList();

                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


       
       

       
        

        
        

       
    }
}
