using API.Dtos;
using API.Errors;
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
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentDetail> _investmentDetailRepo;
        private readonly IGenericRepository<InvestmentMedicineProd> _investmentMedicineProdRepo;
        private readonly IGenericRepository<MedicineProduct> _medicineProductRepo;
        private readonly IGenericRepository<InvestmentTargetedProd> _investmentTargetedProdRepo;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;
        private readonly IGenericRepository<InvestmentDoctor> _investmentDoctorRepo;
        private readonly IGenericRepository<InvestmentInstitution> _investmentInstitutionRepo;
        private readonly IGenericRepository<InvestmentCampaign> _investmentCampaignRepo;
        private readonly IGenericRepository<InvestmentBcds> _investmentBcdsRepo;
        private readonly IGenericRepository<InvestmentSociety> _investmentSocietyRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<Donation> _donationRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;


        public InvestmentController(IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo, IGenericRepository<InvestmentTargetedProd> investmentTargetedProdRepo,
            IGenericRepository<InvestmentMedicineProd> investmentMedicineProdRepo,IGenericRepository<MedicineProduct> medicineProductRepo,
            IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentDetail> investmentDetailRepo, IGenericRepository<InvestmentDoctor> investmentDoctorRepo,
            IGenericRepository<InvestmentSociety> investmentSocietyRepo, IGenericRepository<InvestmentBcds> investmentBcdsRepo,
            IGenericRepository<InvestmentCampaign> investmentCampaignRepo, IGenericRepository<InvestmentInstitution> investmentInstitutionRepo,
            IGenericRepository<Employee> employeeRepo, IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
            IGenericRepository<InvestmentRecComment> investmentRecCommentRepo, StoreContext dbContext, IGenericRepository<Donation> donationRepo,
            IMapper mapper)
        {
            _mapper = mapper;
            _investmentInitRepo = investmentInitRepo;
            _investmentDetailRepo = investmentDetailRepo;
            _investmentMedicineProdRepo = investmentMedicineProdRepo;
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
            _donationRepo = donationRepo;
            _medicineProductRepo = medicineProductRepo;
        }
        
        #region investmentInit


        [HttpGet("investmentInits/{empId}/{sbu}/{userRole}")]
        //public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetInvestmentInits(int empId, string sbu, string userRole,[FromQuery] InvestmentInitSpecParams investmentInitParrams)
        public async Task<IReadOnlyList<InvestmentInit>> GetInvestmentInits(int empId, string sbu, string userRole,[FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                //var empData = await _employeeRepo.GetByIdAsync(empId);
                //investmentInitParrams.Search = sbu;
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
                //                DonationId = i.DonationId,
                //                DonationTo = i.DonationTo,
                //                EmployeeId = i.EmployeeId
                //            }
                //              ).Distinct().ToList();
                // var results = _dbContext.Query<InvestmentInitDto>().FromSql("EXECUTE dbo.SP_InvestmentInitSearch {0},{1}", sbu,empId).ToList();
                if (userRole == "Administrator")
                {

                    //var spec = new InvestmentInitSpecification(investmentInitParrams);

                    //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                   // var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                   // var investmentInits = await _investmentInitRepo.ListAsync(spec);
                    var investmentInits = await _investmentInitRepo.ListAllAsync();

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
                    //                DonationId = i.DonationId,
                    //                DonationTo = i.DonationTo,
                    //                EmployeeId = i.EmployeeId
                    //            }
                    //              ).Distinct().ToList();
                    //var data = _mapper
                    //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(investmentInits);
                    //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, data));
                    return investmentInits;
                }

                else
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                    var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentInitSearch @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();


                    //var data = _mapper
                    //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                    //  return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, totalItems, results));
                    //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 10, data));
                    return results;
                }
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
                //                DonationId = i.DonationId,
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
                    DonationId = investmentInitDto.DonationId,
                    EmployeeId = investmentInitDto.EmployeeId,
                    MarketGroupCode = empData.MarketGroupCode,
                    MarketGroupName = empData.MarketGroupName,
                    MarketCode = empData.MarketCode,
                    MarketName = empData.MarketName,
                    RegionCode = empData.RegionCode,
                    RegionName = empData.RegionName,
                    ZoneCode = empData.ZoneCode,
                    ZoneName = empData.ZoneName,
                    TerritoryCode = empData.TerritoryCode,
                    TerritoryName = empData.TerritoryName,
                    SBUName = empData.SBUName,
                    SBU = empData.SBU,
                    Confirmation = false,
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
                    DonationId = investmentInit.DonationId,
                    EmployeeId = investmentInit.EmployeeId,
                    MarketCode = empData.MarketCode
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
                var existedInvestmentInit = await _investmentInitRepo.GetByIdAsync(investmentInitDto.Id);
                var investmentInit = new InvestmentInit
                {
                    Id = investmentInitDto.Id,
                    ReferenceNo = investmentInitDto.ReferenceNo,
                    ProposeFor = investmentInitDto.ProposeFor,
                    DonationTo = investmentInitDto.DonationTo,
                    DonationId = investmentInitDto.DonationId,
                    EmployeeId = investmentInitDto.EmployeeId,
                    MarketGroupCode = empData.MarketGroupCode,
                    MarketGroupName = empData.MarketGroupName,
                    MarketCode = empData.MarketCode,
                    MarketName = empData.MarketName,
                    RegionCode = empData.RegionCode,
                    RegionName = empData.RegionName,
                    ZoneCode = empData.ZoneCode,
                    ZoneName = empData.ZoneName,
                    TerritoryCode = empData.TerritoryCode,
                    TerritoryName = empData.TerritoryName,
                    SBUName = empData.SBUName,
                    SBU = empData.SBU,
                    SetOn = existedInvestmentInit.SetOn,
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
                    DonationId = investmentInit.DonationId,
                    EmployeeId = investmentInit.EmployeeId,
                    MarketCode = empData.MarketCode
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        [HttpPost("IsInvestmentDetailExist")]
        public async Task<InvestmentDetail> IsInvestmentDetailExist(int id)
        {
            var spec = new InvestmentDetailSpecification(id);
            var investmentDetail= await _investmentDetailRepo.GetEntityWithSpec(spec);
            return investmentDetail;
        }
        [HttpPost("IsInvestmentDoctorExist")]
        public async Task<InvestmentDoctor> IsInvestmentDoctorExist(int id)
        {
            var spec = new InvestmentDoctorSpecification(id);
            var investmentDoctor = await _investmentDoctorRepo.GetEntityWithSpec(spec);
            return investmentDoctor;
        } 
        [HttpPost("IsInvestmentInstitutionExist")]
        public async Task<InvestmentInstitution> IsInvestmentInstitutionExist(int id)
        {
            var spec = new InvestmentInstitutionSpecification(id);
            var investmentInstitution = await _investmentInstitutionRepo.GetEntityWithSpec(spec);
            return investmentInstitution;
        } 
        [HttpPost("IsInvestmentBcdsExist")]
        public async Task<InvestmentBcds> IsInvestmentBcdsExist(int id)
        {
            var spec = new InvestmentBcdsSpecification(id);
            var investmentBcds = await _investmentBcdsRepo.GetEntityWithSpec(spec);
            return investmentBcds;
        }
        [HttpPost("IsInvestmentSocietyExist")]
        public async Task<InvestmentSociety> IsInvestmentSocietyExist(int id)
        {
            var spec = new InvestmentSocietySpecification(id);
            var investmentSociety = await _investmentSocietyRepo.GetEntityWithSpec(spec);
            return investmentSociety;
        }
        [HttpPost("IsInvestmentCampaignExist")]
        public async Task<InvestmentCampaign> IsInvestmentCampaignExist(int id)
        {
            var spec = new InvestmentCampaignSpecification(id);
            var investmentCampaign = await _investmentCampaignRepo.GetEntityWithSpec(spec);
            return investmentCampaign;
        }[HttpPost("IsInvestmentTargetedGroupValid")]
        public async Task<int> IsInvestmentTargetedGroupValid(int id)
        {
            var spec = new InvestmentTargetedGroupSpecification(id);
            var investmentCampaign = await _investmentTargetedGroupRepo.ListAsync(spec);
            return investmentCampaign.Count;
        }
        
       [HttpPost("submitInvestment")]
        public async Task<ActionResult<InvestmentInitDto>> SubmitInvestmentInit(InvestmentInitDto investmentInitDto)
        {
            try
            {
                if (await IsInvestmentDetailExist(investmentInitDto.Id)==null)
                {
                    return BadRequest(new ApiResponse(0,"Please Insert Detail Data first"));
                }
                if (investmentInitDto.DonationTo == "Doctor")
                {
                    if (await IsInvestmentDoctorExist(investmentInitDto.Id) == null)
                    {
                        return BadRequest(new ApiResponse(0, "Please Insert Doctor Data first"));
                    }
                }  
                else if (investmentInitDto.DonationTo == "Institution")
                {
                    if (await IsInvestmentInstitutionExist(investmentInitDto.Id) == null)
                    {
                        return BadRequest(new ApiResponse(0, "Please Insert Institution Data first"));
                    }
                }
                else if (investmentInitDto.DonationTo == "Bcds")
                {
                    if (await IsInvestmentBcdsExist(investmentInitDto.Id) == null)
                    {
                        return BadRequest(new ApiResponse(0, "Please Insert Bcds Data first"));
                    }
                }
                else if (investmentInitDto.DonationTo == "Society")
                {
                    if (await IsInvestmentSocietyExist(investmentInitDto.Id) == null)
                    {
                        return BadRequest(new ApiResponse(0, "Please Insert Society Data first"));
                    }
                }
                else if (investmentInitDto.DonationTo == "Campaign")
                {
                    if (await IsInvestmentCampaignExist(investmentInitDto.Id) == null)
                    {
                        return BadRequest(new ApiResponse(0, "Please Insert Campaign Data first"));
                    }
                }
                if (await IsInvestmentTargetedGroupValid(investmentInitDto.Id) < 4)
                {
                    return BadRequest(new ApiResponse(0, "Insufficient Targeted Market"));
                }
                var empData = await _employeeRepo.GetByIdAsync(investmentInitDto.EmployeeId);
                var existedInvestmentInit = await _investmentInitRepo.GetByIdAsync(investmentInitDto.Id);
                var investmentInit = new InvestmentInit
                {
                    Id = investmentInitDto.Id,
                    ReferenceNo = investmentInitDto.ReferenceNo,
                    ProposeFor = investmentInitDto.ProposeFor,
                    DonationTo = investmentInitDto.DonationTo,
                    DonationId = investmentInitDto.DonationId,
                    EmployeeId = investmentInitDto.EmployeeId,
                    MarketGroupCode = empData.MarketGroupCode,
                    MarketGroupName = empData.MarketGroupName,
                    MarketCode = empData.MarketCode,
                    MarketName = empData.MarketName,
                    RegionCode = empData.RegionCode,
                    RegionName = empData.RegionName,
                    ZoneCode = empData.ZoneCode,
                    ZoneName = empData.ZoneName,
                    TerritoryCode = empData.TerritoryCode,
                    TerritoryName = empData.TerritoryName,
                    SBUName = empData.SBUName,
                    SBU = empData.SBU,
                    SetOn = existedInvestmentInit.SetOn,
                    ModifiedOn = existedInvestmentInit.ModifiedOn,
                    Confirmation = true,
                    SubmissionDate = DateTimeOffset.Now,
                };
                _investmentInitRepo.Update(investmentInit);
                _investmentInitRepo.Savechange();

                return new InvestmentInitDto
                {
                    Id = investmentInit.Id,
                    ReferenceNo = investmentInit.ReferenceNo,
                    ProposeFor = investmentInit.ProposeFor,
                    DonationTo = investmentInit.DonationTo,
                    DonationId = investmentInit.DonationId,
                    EmployeeId = investmentInit.EmployeeId,
                    MarketCode = empData.MarketCode
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
      
        [HttpPost("updateInitOther/{empId}")]
        public async Task<ActionResult<InvestmentInitDto>> UpdateInvestmentInitOther(InvestmentInitDto investmentInitDto, int empId)
        {
            try
            {
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new InvestmentTargetedGroupSpecification(investmentInitDto.Id, empData.MarketCode);
                var investmentTargetedGroupData = await _investmentTargetedGroupRepo.GetEntityWithSpec(spec);
                //if (investmentTargetedGroupData.Count > 0)
                //{
                //    foreach (var v in investmentTargetedGroupData)
                //    {
                //        //if (v.CompletionStatus == true) return BadRequest(new ApiResponse(400, "Tagged Market Already Submitted Investment Initialization"));
                //        _investmentTargetedGroupRepo.Delete(v);
                //        _investmentTargetedGroupRepo.Savechange();
                //    }
                //}
                //foreach (var v in investmentTargetedGroupData)
                //{
                var investmentTargetedGroup = new InvestmentTargetedGroup
                {
                    Id = investmentTargetedGroupData.Id,
                    DataStatus = 1,
                    InvestmentInitId = investmentTargetedGroupData.InvestmentInitId,
                    MarketCode = investmentTargetedGroupData.MarketCode,
                    MarketName = investmentTargetedGroupData.MarketName,
                    MarketGroupMstId = investmentTargetedGroupData.MarketGroupMstId,
                    SBU = investmentTargetedGroupData.SBU,
                    SBUName = investmentTargetedGroupData.SBUName,
                    MarketGroupCode = empData.MarketGroupCode,
                    MarketGroupName = empData.MarketGroupName,
                    RegionCode = empData.RegionCode,
                    RegionName = empData.RegionName,
                    ZoneCode = empData.ZoneCode,
                    ZoneName = empData.ZoneName,
                    TerritoryCode = empData.TerritoryCode,
                    TerritoryName = empData.TerritoryName,
                    DepotCode = empData.DepotCode,
                    DepotName = empData.DepotName,
                    CompletionStatus = true,
                    SetOn = investmentTargetedGroupData.SetOn,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentTargetedGroupRepo.Update(investmentTargetedGroup);
                //}
                _investmentTargetedGroupRepo.Savechange();
                return new InvestmentInitDto
                {

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
        public async Task<ActionResult<InvestmentDetailDto>> UpdateInvestmentDetail(InvestmentDetailDto investmentDetailDto)
        {
            try
            {

                var existedInvestmentDetail = await _investmentInitRepo.GetByIdAsync(investmentDetailDto.Id);
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
                    SetOn = existedInvestmentDetail.SetOn,
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
                throw ex;
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

        #region investmentMedicineProd
        [HttpPost("insertInvestmentMedicineProd")]
        public async Task<ActionResult<InvestmentMedicineProd>> InsertInvestmentMedicineProd(InvestmentMedicineProd investmentMedicineProd)
        {
            try
            {

                var medicineProd = await _medicineProductRepo.GetByIdAsync(investmentMedicineProd.ProductId);
                var iMedicineProd = new InvestmentMedicineProd
                {
                    //ReferenceNo = investmentMedicineProdDto.ReferenceNo,
                    InvestmentInitId = investmentMedicineProd.InvestmentInitId,
                    ProductId = investmentMedicineProd.ProductId,
                    EmployeeId = investmentMedicineProd.EmployeeId,
                    BoxQuantity = investmentMedicineProd.BoxQuantity,
                    TpVat = (medicineProd.UnitTp+ medicineProd.UnitVat)* investmentMedicineProd.BoxQuantity,
                    SetOn = DateTimeOffset.Now,
                    //ModifiedOn = DateTimeOffset.Now
                };
                _investmentMedicineProdRepo.Add(iMedicineProd);
                _investmentMedicineProdRepo.Savechange();

                return new InvestmentMedicineProd
                {
                    Id = investmentMedicineProd.Id,
                    ProductId = investmentMedicineProd.ProductId,
                    EmployeeId = investmentMedicineProd.EmployeeId,
                    BoxQuantity = investmentMedicineProd.BoxQuantity,
                    TpVat = medicineProd.UnitTp + medicineProd.UnitVat
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentMedicineProds/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentMedicineProd>> GetInvestmentMedicineProds(int investmentInitId)
        {
            try
            {
                ///var spec = new InvestmentMedicineProdSpecification(investmentInitId,sbu);
                var spec = new InvestmentMedicineProdSpecification(investmentInitId);
                var investmentMedicineProd = await _investmentMedicineProdRepo.ListAsync(spec);
                return investmentMedicineProd;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("removeInvestmentMedicineProd")]
        public async Task<IActionResult> RemoveInvestmentMedicineProd(InvestmentMedicineProd investmentMedicineProd)
        {
            try
            {
                //var response = new HttpResponseMessage();
                var alreadyExistSpec = new InvestmentMedicineProdSpecification(investmentMedicineProd.InvestmentInitId, investmentMedicineProd.ProductId);
                var alreadyExistInvestmentMedicineProdList = await _investmentMedicineProdRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentMedicineProdList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentMedicineProdList)
                    {
                        _investmentMedicineProdRepo.Delete(v);
                        _investmentMedicineProdRepo.Savechange();
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
                    EmployeeId = investmentTargetedProdDto.EmployeeId,
                    SBU = investmentTargetedProdDto.SBU,
                    SetOn = DateTimeOffset.Now,
                    //ModifiedOn = DateTimeOffset.Now
                };
                _investmentTargetedProdRepo.Add(investmentTargetedProd);
                _investmentTargetedProdRepo.Savechange();

                return new InvestmentTargetedProdDto
                {
                    Id = investmentTargetedProd.Id,
                    InvestmentInitId = investmentTargetedProdDto.InvestmentInitId,
                    ProductId = investmentTargetedProdDto.ProductId,
                    SBU = investmentTargetedProdDto.SBU,
                    EmployeeId = investmentTargetedProdDto.EmployeeId,
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
                    EmployeeId = investmentTargetedProdDto.EmployeeId,
                    //SetOn = DateTimeOffset.Now,
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
                    EmployeeId = investmentTargetedProdDto.EmployeeId,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentTargetedProds/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentTargetedProd>> GetInvestmentTargetedProds(int investmentInitId, string sbu)
        {
            try
            {
                ///var spec = new InvestmentTargetedProdSpecification(investmentInitId,sbu);
                var spec = new InvestmentTargetedProdSpecification(investmentInitId);
                var investmentTargetedProd = await _investmentTargetedProdRepo.ListAsync(spec);
                return investmentTargetedProd;
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


        // [HttpGet]
        // [Route("investmentTargetedProds/{investmentInitId}")]
        // public async Task<IReadOnlyList<InvestmentTargetedProd>> GetInvestmentTargetedProds(int investmentInitId)
        // {
        //     try
        //     {
        //         var spec = new InvestmentTargetedProdSpecification(investmentInitId);
        //         var investmentTargetedProd = await _investmentTargetedProdRepo.ListAsync(spec);
        //         return investmentTargetedProd;
        //     }
        //     catch (System.Exception ex)
        //     {
        //         throw ex;
        //     }
        // }


        #endregion

        #region investmentTargetedGroup
        [HttpPost("insertInvestmentTargetedGroup/{initId}")]
        public async Task<ActionResult> InsertInvestmentTargetedGroup(List<InvestmentTargetedGroupDto> investmentTargetedGroupDto,int initId)
        {
            try
            {
                var alreadyExistSpec = new InvestmentTargetedGroupSpecification(initId, investmentTargetedGroupDto[0].MarketGroupMstId);
                var alreadyExistInvestmentTargetedGroupList = await _investmentTargetedGroupRepo.ListAsync(alreadyExistSpec); 
                
                if (alreadyExistInvestmentTargetedGroupList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentTargetedGroupList)
                    {
                        if (v.CompletionStatus == true) return BadRequest(new ApiResponse(400, "Tagged Market Already Submitted Investment Initialization"));
                        _investmentTargetedGroupRepo.Delete(v);
                        _investmentTargetedGroupRepo.Savechange();
                    }
                }
                foreach (var v in investmentTargetedGroupDto)
                {
                    var alreadyEmpExistSpec = new EmployeeSpecification(v.MarketCode);
                    var empData = await _employeeRepo.GetEntityWithSpec(alreadyEmpExistSpec);

                    var investmentTargetedGroup = new InvestmentTargetedGroup
                    {
                        InvestmentInitId = initId,
                        MarketGroupCode = empData.MarketGroupCode,
                        MarketGroupName = empData.MarketGroupName,
                        MarketCode = empData.MarketCode,
                        MarketName = empData.MarketName,
                        RegionCode = empData.RegionCode,
                        RegionName = empData.RegionName,
                        ZoneCode = empData.ZoneCode,
                        ZoneName = empData.ZoneName,
                        TerritoryCode = empData.TerritoryCode,
                        TerritoryName = empData.TerritoryName, 
                        DepotCode = empData.DepotCode,
                        DepotName = empData.DepotName,
                        //SBUName = empData.SBUName,
                        //SBU = empData.SBU,
                        MarketGroupMstId = v.MarketGroupMstId,
                        CompletionStatus = false,
                        SBU = v.SBU,
                        SBUName = v.SBUName,
                        SetOn = DateTimeOffset.Now,
                        //ModifiedOn = DateTimeOffset.Now,
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

       
        [HttpPost("removeInvestmentIndTargetedGroup")]
        public async Task<IActionResult> RemoveInvestmentIndTargetedGroup(InvestmentTargetedGroup investmentTargetedGroup)
        {
            try
            {
                //var response = new HttpResponseMessage();
                var alreadyExistSpec = new InvestmentTargetedGroupSpecification(investmentTargetedGroup.InvestmentInitId, investmentTargetedGroup.MarketCode);
                var alreadyExistInvestmentTargetedGroup = await _investmentTargetedGroupRepo.GetEntityWithSpec(alreadyExistSpec);
                if (alreadyExistInvestmentTargetedGroup!=null)
                {
                    //foreach (var v in alreadyExistInvestmentTargetedGroupList)
                    //{
                        _investmentTargetedGroupRepo.Delete(alreadyExistInvestmentTargetedGroup);
                        _investmentTargetedGroupRepo.Savechange();
                    //}

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
        public async Task<ActionResult> RemoveInvestmentTargetedGroup(List<InvestmentTargetedGroupDto> investmentTargetedGroupDto)
        {
            try
            {
                foreach (var a in investmentTargetedGroupDto)
                {
                    //var response = new HttpResponseMessage();
                    var alreadyExistSpec = new InvestmentTargetedGroupSpecification(a.InvestmentInitId, a.MarketCode);
                    var alreadyExistInvestmentTargetedGroup = await _investmentTargetedGroupRepo.GetEntityWithSpec(alreadyExistSpec);
                    if (alreadyExistInvestmentTargetedGroup!=null)
                    {
                        //foreach (var v in alreadyExistInvestmentTargetedGroupList)
                        //{
                            if (alreadyExistInvestmentTargetedGroup.CompletionStatus == true) return BadRequest(new ApiResponse(400, "Tagged Market Already Submitted Investment Initialization"));
                            _investmentTargetedGroupRepo.Delete(alreadyExistInvestmentTargetedGroup);

                        //}
                        _investmentTargetedGroupRepo.Savechange();

                    }
                }
                return Ok("Succsessfuly Deleted!!!");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region investmentDoctor
        [HttpPost("IsDoctorInvestmentApprovalPending")]
        public async Task<int> IsDoctorInvestmentApprovalPending(int initId,int doctorId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentDoctor d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE " +
                // " EXISTS ( " +
                // " SELECT InvestmentInitId " +
                // " FROM InvestmentTargetedGroup IT " +
                // " WHERE IT.InvestmentInitId = I.Id " +
                // " AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                //" )" +
                " AND i.MarketCode = '"+ iInit .MarketCode+ "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.DoctorId = " + doctorId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }


            [HttpPost("insertInvestmentDoctor")]
        public async Task<ActionResult<InvestmentDoctorDto>> InsertInvestmentDoctor(InvestmentDoctorDto investmentDoctorDto)
        {
            try
            {
                //int a = await IsDoctorInvestmentApprovalPending(investmentDoctorDto.InvestmentInitId, investmentDoctorDto.DoctorId);
                if (await IsDoctorInvestmentApprovalPending(investmentDoctorDto.InvestmentInitId, investmentDoctorDto.DoctorId) > 0)
                {
                    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Doctor!"));
                }
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

        [HttpPost("IsInstitutionInvestmentApprovalPending")]
        public async Task<int> IsInstitutionInvestmentApprovalPending(int initId, int institutionId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentInstitution d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE I.Id = I.Id " +
                " AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                //" )" +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.InstitutionId = " + institutionId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }



        [HttpPost("insertInvestmentInstitution")]
        public async Task<ActionResult<InvestmentInstitutionDto>> InsertInvestmentInstitution(InvestmentInstitutionDto investmentInstitutionDto)
        {
            try
            {
                if (await IsInstitutionInvestmentApprovalPending(investmentInstitutionDto.InvestmentInitId, investmentInstitutionDto.InstitutionId) > 0)
                {
                    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Institution!"));
                }
                var investmentInstitution = new InvestmentInstitution
                {
                    //ReferenceNo = investmentInstitutionDto.ReferenceNo,
                    InvestmentInitId = investmentInstitutionDto.InvestmentInitId,
                    InstitutionId = investmentInstitutionDto.InstitutionId,
                    ResponsibleDoctorId = investmentInstitutionDto.ResponsibleDoctorId,
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
                    ResponsibleDoctorId = investmentInstitutionDto.ResponsibleDoctorId,
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

        [HttpPost("IsCampaignInvestmentApprovalPending")]
        public async Task<int> IsCampaignInvestmentApprovalPending(int initId, int campaignDtlId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentCampaign d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE I.Id = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                //" )" +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.CampaignDtlId = " + campaignDtlId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }


        [HttpPost("insertInvestmentCampaign")]
        public async Task<ActionResult<InvestmentCampaignDto>> InsertInvestmentCampaign(InvestmentCampaignDto investmentCampaignDto)
        {
            try
            {
                if (await IsCampaignInvestmentApprovalPending(investmentCampaignDto.InvestmentInitId, investmentCampaignDto.CampaignDtlId) > 0)
                {
                    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Campaign!"));
                }
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

        [HttpPost("IsBcdsInvestmentApprovalPending")]
        public async Task<int> IsBcdsInvestmentApprovalPending(int initId, int bcdsId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentBcds d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE I.Id = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                //" )" +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.BcdsId = " + bcdsId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }

        [HttpPost("insertInvestmentBcds")]
        public async Task<ActionResult<InvestmentBcdsDto>> InsertInvestmentBcds(InvestmentBcdsDto investmentBcdsDto)
        {
            try
            {
                if (await IsBcdsInvestmentApprovalPending(investmentBcdsDto.InvestmentInitId, investmentBcdsDto.BcdsId) > 0)
                {
                    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Bcds!"));
                }
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
                    ResponsibleDoctorId = investmentBcdsDto.ResponsibleDoctorId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentBcdsRepo.Add(investmentBcds);
                _investmentBcdsRepo.Savechange();

                return new InvestmentBcdsDto
                {
                    Id = investmentBcds.Id,
                    InvestmentInitId = investmentBcdsDto.InvestmentInitId,
                    ResponsibleDoctorId = investmentBcdsDto.ResponsibleDoctorId,
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

        [HttpPost("IsSocietyInvestmentApprovalPending")]
        public async Task<int> IsSocietyInvestmentApprovalPending(int initId, int societyId)
        {
            var iInit = await _investmentInitRepo.GetByIdAsync(initId);
            string qry = " SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) Count " +
                " FROM InvestmentSociety d " +
                " JOIN InvestmentInit i ON d.InvestmentInitId = i.Id " +
                " WHERE " +
                //" EXISTS ( " +
                //" SELECT InvestmentInitId " +
                //" FROM InvestmentTargetedGroup IT " +
                //" WHERE IT.InvestmentInitId = I.Id " +
                //" AND " +
                " NOT EXISTS ( " +
                " SELECT InvestmentInitId " +
                " FROM InvestmentRecComment ir " +
                " WHERE ir.InvestmentInitId = I.Id " +
                " AND (ir.RecStatus = 'Approved' OR ir.RecStatus = 'Not Approved' OR ir.RecStatus = 'Cancelled')" +
                " ) " +
                //" )" +
                " AND i.MarketCode = '" + iInit.MarketCode + "' " +
                " AND i.DonationId = '" + iInit.DonationId + "' " +
                " AND d.SocietyId = " + societyId + "";
            var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
            return result[0].Count;
        }
            [HttpPost("insertInvestmentSociety")]
        public async Task<ActionResult<InvestmentSocietyDto>> InsertInvestmentSociety(InvestmentSocietyDto investmentSocietyDto)
        {
            try
            {
                if (await IsSocietyInvestmentApprovalPending(investmentSocietyDto.InvestmentInitId, investmentSocietyDto.SocietyId) > 0)
                {
                    return BadRequest(new ApiResponse(0, "Investment Approval is Pending for this Society!"));
                }
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
                    ResponsibleDoctorId = investmentSocietyDto.ResponsibleDoctorId,
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
                    ResponsibleDoctorId = investmentSocietyDto.ResponsibleDoctorId,
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
        public async Task<IReadOnlyList<ReportInvestmentInfo>> GetLastFiveInvestment(string marketCode, string date)
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

        [HttpGet]
        [Route("getLastFiveInvestmentForDoc/{donationId}/{docId}/{marketCode}/{date}")]
        public IReadOnlyList<LastFiveInvestmentInfo> GetLastFiveInvestmentForDoc(int donationId, int docId, string marketCode, string date)
        {
            //try
            //{
            //    var v = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
            //    // var empData = await _employeeRepo.GetByIdAsync(empId);
            //    var reportInvestmentSpec = new ReportInvestmentSpecification(marketCode);
            //    var reportInvestmentData = await _reportInvestmentInfoRepo.ListAsync(reportInvestmentSpec);
            //    var donation = await _donationRepo.GetByIdAsync(donationId);
            //    // var spec = new ReportInvestmentSpecification(empData.MarketCode);
            //    var data = (from e in reportInvestmentData
            //                where e.DoctorId==docId && e.DonationType== donation.DonationTypeName &&
            //                DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture)
            //                orderby DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) descending
            //                select new ReportInvestmentInfo
            //                {
            //                    InvestmentAmount = e.InvestmentAmount,
            //                    ComtSharePrcnt = e.ComtSharePrcnt,
            //                    ComtSharePrcntAll = e.ComtSharePrcntAll,
            //                    PrescribedSharePrcnt = e.PrescribedSharePrcnt,
            //                    PrescribedSharePrcntAll = e.PrescribedSharePrcntAll
            //                }
            //                 ).Distinct().Take(5).ToList();
            //    return data;
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
            try
            {
                var convertedDate = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
                //List<SqlParameter> parms = new List<SqlParameter>
                //    {
                //        new SqlParameter("@DOCID", docId),
                //        new SqlParameter("@MCODE", marketCode),
                //        new SqlParameter("@FDATE", convertedDate)
                //    };
                //var results = _dbContext.LastFiveInvestmentInfo.FromSqlRaw<LastFiveInvestmentInfo>("EXECUTE SP_LastFiveInvestmentForDocSearch @DOCID,@MCODE,@FDATE", parms.ToArray()).ToList();
                string qry = " SELECT TOP (5) CAST(ROW_NUMBER() OVER (ORDER BY ProposedAmount) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,DonationShortName" +
                            ",ProposedAmount AS InvestmentAmount" +
                            ",CommitmentAllSBU AS  ComtSharePrcntAll" +
                            ",CommitmentOwnSBU AS  ComtSharePrcnt" +
                            // ",(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.DoctorId = '" + docId + "'" +
                            // " AND R.MarketCode = '" + marketCode + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcnt" +
                            // " ,(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.DoctorId = '" + docId + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcntAll" +
                            ",[dbo].[fnGetPrescribedSharePrcntDoc] ('" + docId + "','" + marketCode + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcnt" +
                            ",[dbo].[fnGetPrescribedSharePrcntAllDoc] ('" + docId + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcntAll" +
                            " FROM InvestmentRecComment IRC" +
                            " INNER JOIN InvestmentDoctor ID ON IRC.InvestmentInitId = ID.InvestmentInitId" +
                            " INNER JOIN InvestmentRec IR ON IRC.InvestmentInitId = IR.InvestmentInitId" +
                            " AND IRC.Priority = IR.Priority" +
                            " INNER JOIN InvestmentInit II ON IRC.InvestmentInitId = II.Id" +
                            " INNER JOIN Donation D ON II.DonationId = D.Id" +
                            " WHERE IRC.RecStatus = 'Approved'" +
                            " AND CONVERT(DATE, IR.FromDate) <= Cast('" + convertedDate + "' as date)" +
                            " AND ID.DoctorId = " + docId + "" +
                            " AND II.MarketCode = '" + marketCode + "'" +
                            " ORDER BY IR.FromDate DESC";



                var results = _dbContext.LastFiveInvestmentInfo.FromSqlRaw(qry).ToList();


                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getLastFiveInvestmentForInstitute/{donationId}/{instituteId}/{marketCode}/{date}")]
        public IReadOnlyList<LastFiveInvestmentInfo> GetLastFiveInvestmentForInstitute(int donationId, string instituteId, string marketCode, string date)
        {
            try
            {
                //var v = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
                //// var empData = await _employeeRepo.GetByIdAsync(empId);
                //var reportInvestmentSpec = new ReportInvestmentSpecification(marketCode);
                //var reportInvestmentData = await _reportInvestmentInfoRepo.ListAsync(reportInvestmentSpec);
                //var donation = await _donationRepo.GetByIdAsync(donationId);
                //// var spec = new ReportInvestmentSpecification(empData.MarketCode);
                //var data = (from e in reportInvestmentData
                //            where e.InstituteId == instituteId && e.DonationType == donation.DonationTypeName &&
                //            DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture)
                //            orderby DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) descending
                //            select new ReportInvestmentInfo
                //            {
                //                InvestmentAmount = e.InvestmentAmount,
                //                ComtSharePrcnt = e.ComtSharePrcnt,
                //                ComtSharePrcntAll = e.ComtSharePrcntAll,
                //                PrescribedSharePrcnt = e.PrescribedSharePrcnt,
                //                PrescribedSharePrcntAll = e.PrescribedSharePrcntAll
                //            }
                //             ).Distinct().Take(5).ToList();

                //return data;
                var convertedDate = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
                string qry = " SELECT TOP (5) CAST(ROW_NUMBER() OVER (ORDER BY ProposedAmount) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,DonationShortName" +
                            ",ProposedAmount AS InvestmentAmount" +
                            ",CommitmentAllSBU AS  ComtSharePrcntAll" +
                            ",CommitmentOwnSBU AS  ComtSharePrcnt" +
                            // ",(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.InstituteId = '" + instituteId + "'" +
                            // " AND R.MarketCode = '" + marketCode + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcnt" +
                            // " ,(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.InstituteId = '" + instituteId + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcntAll" +
                            ",[dbo].[fnGetPrescribedSharePrcntIns] ('" + instituteId + "','" + marketCode + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcnt" +
                            ",[dbo].[fnGetPrescribedSharePrcntAllIns] ('" + instituteId + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcntAll" +
                            " FROM InvestmentRecComment IRC" +
                            " INNER JOIN InvestmentInstitution ID ON IRC.InvestmentInitId = ID.InvestmentInitId" +
                            " INNER JOIN InvestmentRec IR ON IRC.InvestmentInitId = IR.InvestmentInitId" +
                            " AND IRC.Priority = IR.Priority" +
                            " INNER JOIN InvestmentInit II ON IRC.InvestmentInitId = II.Id" +
                            " INNER JOIN Donation D ON II.DonationId = D.Id" +
                            " WHERE IRC.RecStatus = 'Approved'" +
                            " AND CONVERT(DATE, IR.FromDate) <= Cast('" + convertedDate + "' as date)" +
                            " AND ID.InstitutionId = " + instituteId + "" +
                            " AND II.MarketCode = '" + marketCode + "'" +
                            " ORDER BY IR.FromDate DESC";



                var results = _dbContext.LastFiveInvestmentInfo.FromSqlRaw(qry).ToList();


                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getLastFiveInvestmentForBcds/{donationId}/{bcdsId}/{marketCode}/{date}")]
        public IReadOnlyList<LastFiveInvestmentInfo> GetLastFiveInvestmentForBcds(int donationId, string bcdsId, string marketCode, string date)
        {
            try
            {
                //var v = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
                //// var empData = await _employeeRepo.GetByIdAsync(empId);
                //var reportInvestmentSpec = new ReportInvestmentSpecification(marketCode);
                //var reportInvestmentData = await _reportInvestmentInfoRepo.ListAsync(reportInvestmentSpec);
                //var donation = await _donationRepo.GetByIdAsync(donationId);
                //// var spec = new ReportInvestmentSpecification(empData.MarketCode);
                //var data = (from e in reportInvestmentData
                //            where e.BcdsId == bcdsId && e.DonationType == donation.DonationTypeName &&
                //            DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture)
                //            orderby DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) descending
                //            select new ReportInvestmentInfo
                //            {
                //                InvestmentAmount = e.InvestmentAmount,
                //                ComtSharePrcnt = e.ComtSharePrcnt,
                //                ComtSharePrcntAll = e.ComtSharePrcntAll,
                //                PrescribedSharePrcnt = e.PrescribedSharePrcnt,
                //                PrescribedSharePrcntAll = e.PrescribedSharePrcntAll
                //            }
                //             ).Distinct().Take(5).ToList();

                //return data;
                var convertedDate = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
                string qry = " SELECT TOP (5) CAST(ROW_NUMBER() OVER (ORDER BY ProposedAmount) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,DonationShortName" +
                            ",ProposedAmount AS InvestmentAmount" +
                            ",CommitmentAllSBU AS  ComtSharePrcntAll" +
                            ",CommitmentOwnSBU AS  ComtSharePrcnt" +
                            // ",(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.BcdsId = '" + bcdsId + "'" +
                            // " AND R.MarketCode = '" + marketCode + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcnt" +
                            // " ,(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.BcdsId = '" + bcdsId + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcntAll" +
                            ",[dbo].[fnGetPrescribedSharePrcntBcds] ('" + bcdsId + "','" + marketCode + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcnt" +
                            ",[dbo].[fnGetPrescribedSharePrcntAllBcds] ('" + bcdsId + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcntAll" +

                            " FROM InvestmentRecComment IRC" +
                            " INNER JOIN InvestmentBcds ID ON IRC.InvestmentInitId = ID.InvestmentInitId" +
                            " INNER JOIN InvestmentRec IR ON IRC.InvestmentInitId = IR.InvestmentInitId" +
                            " AND IRC.Priority = IR.Priority" +
                            " INNER JOIN InvestmentInit II ON IRC.InvestmentInitId = II.Id" +
                            " INNER JOIN Donation D ON II.DonationId = D.Id" +
                            " WHERE IRC.RecStatus = 'Approved'" +
                            " AND CONVERT(DATE, IR.FromDate) <= Cast('" + convertedDate + "' as date)" +
                            " AND ID.BcdsId = " + bcdsId + "" +
                            " AND II.MarketCode = '" + marketCode + "'" +
                            " ORDER BY IR.FromDate DESC";



                var results = _dbContext.LastFiveInvestmentInfo.FromSqlRaw(qry).ToList();


                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getLastFiveInvestmentForSociety/{donationId}/{societyId}/{marketCode}/{date}")]
        public IReadOnlyList<LastFiveInvestmentInfo> GetLastFiveInvestmentForSociety(int donationId, string societyId, string marketCode, string date)
        {
            try
            {
                //var v = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
                //// var empData = await _employeeRepo.GetByIdAsync(empId);
                //var reportInvestmentSpec = new ReportInvestmentSpecification(marketCode);
                //var reportInvestmentData = await _reportInvestmentInfoRepo.ListAsync(reportInvestmentSpec);
                //var donation = await _donationRepo.GetByIdAsync(donationId);
                //// var spec = new ReportInvestmentSpecification(empData.MarketCode);
                //var data = (from e in reportInvestmentData
                //            where e.SocietyId == societyId && e.DonationType == donation.DonationTypeName &&
                //            DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture)
                //            orderby DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) descending
                //            select new ReportInvestmentInfo
                //            {
                //                InvestmentAmount = e.InvestmentAmount,
                //                ComtSharePrcnt = e.ComtSharePrcnt,
                //                ComtSharePrcntAll = e.ComtSharePrcntAll,
                //                PrescribedSharePrcnt = e.PrescribedSharePrcnt,
                //                PrescribedSharePrcntAll = e.PrescribedSharePrcntAll
                //            }
                //             ).Distinct().Take(5).ToList();

                //return data;
                var convertedDate = DateTime.ParseExact(date, "ddMMyyyy", CultureInfo.InvariantCulture);
                string qry = " SELECT TOP (5) CAST(ROW_NUMBER() OVER (ORDER BY ProposedAmount) AS INT)  AS Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,DonationShortName" +
                            ",ProposedAmount AS InvestmentAmount" +
                            ",CommitmentAllSBU AS  ComtSharePrcntAll" +
                            ",CommitmentOwnSBU AS  ComtSharePrcnt" +
                            // ",(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.SocietyId = '" + societyId + "'" +
                            // " AND R.MarketCode = '" + marketCode + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcnt" +
                            // " ,(" +
                            // " SELECT AVG(TRY_CONVERT(FLOAT, PrescribedSharePrcnt))" +
                            // " FROM ReportInvestmentInfo R" +
                            // " WHERE R.SocietyId = '" + societyId + "'" +
                            // " AND R.DonationType = D.DonationTypeName" +
                            // " AND CONVERT(NVARCHAR(6), TRY_CONVERT(DATE, R.FromDate, 103), 112) = CONVERT(NVARCHAR(6), IR.FromDate, 112)" +
                            // " ) AS  PrescribedSharePrcntAll" +
                            ",[dbo].[fnGetPrescribedSharePrcntSoc] ('" + societyId + "','" + marketCode + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcnt" +
                            ",[dbo].[fnGetPrescribedSharePrcntAllSoc] ('" + societyId + "',D.DonationTypeName,IR.FromDate) AS  PrescribedSharePrcntAll" +

                            " FROM InvestmentRecComment IRC" +
                            " INNER JOIN InvestmentSociety ID ON IRC.InvestmentInitId = ID.InvestmentInitId" +
                            " INNER JOIN InvestmentRec IR ON IRC.InvestmentInitId = IR.InvestmentInitId" +
                            " AND IRC.Priority = IR.Priority" +
                            " INNER JOIN InvestmentInit II ON IRC.InvestmentInitId = II.Id" +
                            " INNER JOIN Donation D ON II.DonationId = D.Id" +
                            " WHERE IRC.RecStatus = 'Approved'" +
                            " AND CONVERT(DATE, IR.FromDate) <= Cast('" + convertedDate + "' as date)" +
                            " AND ID.SocietyId = " + societyId + "" +
                            " AND II.MarketCode = '" + marketCode + "'" +
                            " ORDER BY IR.FromDate DESC";
                var results = _dbContext.LastFiveInvestmentInfo.FromSqlRaw(qry).ToList();
                return results;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
