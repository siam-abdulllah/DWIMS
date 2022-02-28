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
    public class InvestmentRecController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductRepo;
        private readonly IGenericRepository<InvestmentAprComment> _investmentAprCommentRepo;
        private readonly IGenericRepository<MedicineProduct> _medicineProductRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IGenericRepository<InvestmentMedicineProd> _investmentMedicineProdRepo;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IGenericRepository<ApprovalAuthority> _approvalAuthorityRepo;
        private readonly IGenericRepository<ApprovalCeiling> _approvalCeilingRepo;
        private readonly IGenericRepository<SBUWiseBudget> _sbuRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;

        public InvestmentRecController(IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo, IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentRec> investmentRecRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo, IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
        IGenericRepository<InvestmentAprComment> investmentAprCommentRepo, IGenericRepository<Employee> employeeRepo,
        IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo, IGenericRepository<InvestmentMedicineProd> investmentMedicineProdRepo,
        IGenericRepository<ApprAuthConfig> apprAuthConfigRepo, IGenericRepository<MedicineProduct> medicineProductRepo,
        IGenericRepository<ApprovalCeiling> approvalCeilingRepo, IGenericRepository<ApprovalAuthority> approvalAuthorityRepo,
        IGenericRepository<SBUWiseBudget> sbuRepo, StoreContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _investmentInitRepo = investmentInitRepo;
            _investmentRecRepo = investmentRecRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _investmentRecProductRepo = investmentRecProductRepo;
            _investmentAprCommentRepo = investmentAprCommentRepo;
            _employeeRepo = employeeRepo;
            _apprAuthConfigRepo = apprAuthConfigRepo;
            _approvalCeilingRepo = approvalCeilingRepo;
            _approvalAuthorityRepo = approvalAuthorityRepo;
            _reportInvestmentInfoRepo = reportInvestmentInfoRepo;
            _sbuRepo = sbuRepo;
            _dbContext = dbContext;
            _investmentTargetedGroupRepo = investmentTargetedGroupRepo;
            _medicineProductRepo = medicineProductRepo;
            _investmentMedicineProdRepo = investmentMedicineProdRepo;
        }
        [HttpGet("investmentInits/{empId}/{sbu}")]
        //public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,[FromQuery] InvestmentInitSpecParams investmentInitParrams)
        public IReadOnlyList<InvestmentInit> GetInvestmentInits(int empId, string sbu, [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId)
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentRecSearch @SBU,@EID", parms.ToArray()).ToList();
                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            orderby r.SetOn descending
                            select new InvestmentInit
                            {
                                Id = r.Id,
                                DataStatus = r.DataStatus,
                                SetOn = r.SetOn,
                                ModifiedOn = r.ModifiedOn,
                                ReferenceNo = r.ReferenceNo,
                                ProposeFor = r.ProposeFor,
                                DonationId = r.DonationId,
                                DonationTo = r.DonationTo,
                                EmployeeId = r.EmployeeId,
                                MarketGroupCode = r.MarketGroupCode,
                                MarketGroupName = r.MarketGroupName,
                                MarketCode = r.MarketCode,
                                MarketName = r.MarketName,
                                RegionCode = r.RegionCode,
                                RegionName = r.RegionName,
                                ZoneCode = r.ZoneCode,
                                ZoneName = r.ZoneName,
                                TerritoryCode = r.TerritoryCode,
                                TerritoryName = r.TerritoryName,
                                SBU = r.SBU,
                                SBUName = r.SBUName,
                                Confirmation = r.Confirmation,
                                SubmissionDate = r.SubmissionDate,
                                Donation = d,
                                Employee = e
                            }
                            ).Distinct().ToList();
                return data;

            }
            catch (System.Exception e)
            {

                throw e;
            }
        }

        [HttpGet("investmentRecommended/{empId}/{sbu}/{userRole}")]
        public async Task<ActionResult<IReadOnlyList<InvestmentInit>>> GetinvestmentRecommended(int empId, string sbu, string userRole,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {

                if (userRole == "Administrator")
                {
                    var investmentInits = await _investmentInitRepo.ListAllAsync();
                    var investmentRecComments = await _investmentRecCommentRepo.ListAllAsync();
                    //var investmentAprComments = await _investmentAprCommentRepo.ListAllAsync();
                    var investmentInitFormRec = (from r in investmentInits
                                                 join rc in investmentRecComments on r.Id equals rc.InvestmentInitId
                                                 join d in _dbContext.Donation on r.DonationId equals d.Id
                                                 join e in _dbContext.Employee on r.EmployeeId equals e.Id
                                                 where rc.RecStatus != "Approved"
                                                 orderby r.SetOn descending
                                                 select new InvestmentInit
                                                 {
                                                     Id = r.Id,
                                                     DataStatus = r.DataStatus,
                                                     SetOn = r.SetOn,
                                                     ModifiedOn = r.ModifiedOn,
                                                     ReferenceNo = r.ReferenceNo,
                                                     ProposeFor = r.ProposeFor,
                                                     DonationId = r.DonationId,
                                                     DonationTo = r.DonationTo,
                                                     EmployeeId = r.EmployeeId,
                                                     MarketGroupCode = r.MarketGroupCode,
                                                     MarketGroupName = r.MarketGroupName,
                                                     MarketCode = r.MarketCode,
                                                     MarketName = r.MarketName,
                                                     RegionCode = r.RegionCode,
                                                     RegionName = r.RegionName,
                                                     ZoneCode = r.ZoneCode,
                                                     ZoneName = r.ZoneName,
                                                     TerritoryCode = r.TerritoryCode,
                                                     TerritoryName = r.TerritoryName,
                                                     SBU = r.SBU,
                                                     SBUName = r.SBUName,
                                                     Confirmation = r.Confirmation,
                                                     SubmissionDate = r.SubmissionDate,
                                                     Donation = d,
                                                     Employee = e
                                                 }
                                  ).Distinct().OrderByDescending(x => x.SetOn).ToList();

                    //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, investmentInitFormRec.Count(), investmentInitFormRec));
                    return investmentInitFormRec;
                }

                else
                {

                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", DBNull.Value),
                        new SqlParameter("@ASTATUS", "Approved")
                    };
                    var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentRecommendedSearch @SBU,@EID,@RSTATUS,@ASTATUS", parms.ToArray()).ToList();

                    var data = (from r in results
                                join d in _dbContext.Donation on r.DonationId equals d.Id
                                join e in _dbContext.Employee on r.EmployeeId equals e.Id
                                orderby r.SetOn descending
                                select new InvestmentInit
                                {
                                    Id = r.Id,
                                    DataStatus = r.DataStatus,
                                    SetOn = r.SetOn,
                                    ModifiedOn = r.ModifiedOn,
                                    ReferenceNo = r.ReferenceNo,
                                    ProposeFor = r.ProposeFor,
                                    DonationId = r.DonationId,
                                    DonationTo = r.DonationTo,
                                    EmployeeId = r.EmployeeId,
                                    MarketGroupCode = r.MarketGroupCode,
                                    MarketGroupName = r.MarketGroupName,
                                    MarketCode = r.MarketCode,
                                    MarketName = r.MarketName,
                                    RegionCode = r.RegionCode,
                                    RegionName = r.RegionName,
                                    ZoneCode = r.ZoneCode,
                                    ZoneName = r.ZoneName,
                                    TerritoryCode = r.TerritoryCode,
                                    TerritoryName = r.TerritoryName,
                                    SBU = r.SBU,
                                    SBUName = r.SBUName,
                                    Confirmation = r.Confirmation,
                                    SubmissionDate = r.SubmissionDate,
                                    Donation = d,
                                    Employee = e
                                }
                            ).Distinct().ToList();
                    return data;
                }
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }


        [HttpPost("insertRecommendForOwnSBU/{empID}/{sbu}")]
        public async Task<ActionResult<InvestmentRecComment>> InsertInvestmentRecommendForOwnSBU(int empId, string sbu, InvestmentRecOwnSBUInsertDto investmentRecOwnSBUInsertDto)
        {
            bool isComplete = false;
            bool isTrue = false;
            var spec = new ApprAuthConfigSpecification(empId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecOwnSBUInsertDto.InvestmentRecComment.InvestmentInitId);
            var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecOwnSBUInsertDto.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
            var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
            if (investmentRecOwnSBUInsertDto.InvestmentRecComment.RecStatus == "Recommended" || investmentRecOwnSBUInsertDto.InvestmentRecComment.RecStatus == "Not Recommended")
            {
                isComplete = true;
                foreach (var v in investmentTargetedGroup)
                {
                    isTrue = false;
                    foreach (var i in investmentRecComments)
                    {
                        if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
                        {
                            isTrue = true;
                        }
                    }
                    if (!isTrue)
                    {
                        return BadRequest(new ApiResponse(400, "Other recommendation has not completed yet"));
                    }
                }
            }

            var alreadyExistSpec = new InvestmentRecSpecification((int)investmentRecOwnSBUInsertDto.InvestmentRec.InvestmentInitId, empId);
            var alreadyExistInvestmentRecList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
            if (alreadyExistInvestmentRecList.Count > 0)
            {
                foreach (var v in alreadyExistInvestmentRecList)
                {
                    _investmentRecRepo.Delete(v);
                    _investmentRecRepo.Savechange();
                }
            }
            
            var invRec = new InvestmentRec
            {
                InvestmentInitId = investmentRecOwnSBUInsertDto.InvestmentRec.InvestmentInitId,
                ProposedAmount = investmentRecOwnSBUInsertDto.InvestmentRec.ProposedAmount,
                PaymentFreq = investmentRecOwnSBUInsertDto.InvestmentRec.PaymentFreq,
                Purpose = investmentRecOwnSBUInsertDto.InvestmentRec.Purpose,
                CommitmentAllSBU = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentOwnSBU,
                FromDate = investmentRecOwnSBUInsertDto.InvestmentRec.FromDate,
                ToDate = investmentRecOwnSBUInsertDto.InvestmentRec.ToDate,
                CommitmentFromDate = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentFromDate,
                CommitmentToDate = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentToDate,
                TotalMonth = investmentRecOwnSBUInsertDto.InvestmentRec.TotalMonth,
                CommitmentTotalMonth = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentTotalMonth,
                PaymentMethod = investmentRecOwnSBUInsertDto.InvestmentRec.PaymentMethod,
                ChequeTitle = investmentRecOwnSBUInsertDto.InvestmentRec.ChequeTitle,
                EmployeeId = empId,
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = true,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecRepo.Add(invRec);
            _investmentRecRepo.Savechange();

            var _investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecOwnSBUInsertDto.InvestmentRecComment.InvestmentInitId, empId);
            var _investmentRecComments = await _investmentRecCommentRepo.ListAsync(_investmentRecCommentSpec);
            if (_investmentRecComments.Count > 0)
            {
                foreach (var v in _investmentRecComments)
                {
                    _investmentRecCommentRepo.Delete(v);
                    _investmentRecCommentRepo.Savechange();
                }
            }
            var empData = await _employeeRepo.GetByIdAsync(empId);
            var invRecCmnt = new InvestmentRecComment
            {
                InvestmentInitId = investmentRecOwnSBUInsertDto.InvestmentRecComment.InvestmentInitId,
                EmployeeId = empId,
                Comments = investmentRecOwnSBUInsertDto.InvestmentRecComment.Comments,
                RecStatus = investmentRecOwnSBUInsertDto.InvestmentRecComment.RecStatus,
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
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = isComplete,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecCommentRepo.Add(invRecCmnt);
            _investmentRecCommentRepo.Savechange();
            foreach (var i in investmentRecOwnSBUInsertDto.InvestmentRecProducts)
            {
                var alreadyExistProdSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistProdSpec);
                if (alreadyExistInvestmentRecProductList.Count < 1)
                {
                    var investmentRecProduct = new InvestmentRecProducts
                    {
                        InvestmentInitId = i.InvestmentInitId,
                        ProductId = i.ProductId,
                        EmployeeId = i.EmployeeId,
                        SBU = i.ProductInfo.SBU,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecProductRepo.Add(investmentRecProduct);
                }
            }
            _investmentRecProductRepo.Savechange();
            return new InvestmentRecComment
            {
                Id = invRecCmnt.Id,
                InvestmentInitId = invRecCmnt.InvestmentInitId,
                EmployeeId = invRec.EmployeeId,
                Comments = invRecCmnt.Comments,
                RecStatus = invRecCmnt.RecStatus,
            };
        }
        [HttpPost("updateRecommendForOwnSBU/{empID}/{sbu}")]
        public async Task<ActionResult<InvestmentRecComment>> UpdateInvestmentRecommendForOwnSBU(int empId, string sbu, InvestmentRecOwnSBUInsertDto investmentRecOwnSBUInsertDto)
        {
            
            var isComplete = false;
            bool isTrue = false;
            var spec = new ApprAuthConfigSpecification(empId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecOwnSBUInsertDto.InvestmentRecComment.InvestmentInitId);
            var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecOwnSBUInsertDto.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
            var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
            if (investmentRecOwnSBUInsertDto.InvestmentRecComment.RecStatus == "Recommended" || investmentRecOwnSBUInsertDto.InvestmentRecComment.RecStatus == "Not Recommended")
            {
                isComplete = true;
                foreach (var v in investmentTargetedGroup)
                {
                    isTrue = false;
                    foreach (var i in investmentRecComments)
                    {
                        if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
                        {
                            isTrue = true;
                        }
                    }
                    if (!isTrue) { return BadRequest(new ApiResponse(400, "Other recommendation not completed yet")); }
                }
            }


            var alreadyExistSpec = new InvestmentRecSpecification((int)investmentRecOwnSBUInsertDto.InvestmentRec.InvestmentInitId, empId);
            var alreadyExistInvestmentRecList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
            if (alreadyExistInvestmentRecList.Count > 0)
            {
                foreach (var v in alreadyExistInvestmentRecList)
                {
                    _investmentRecRepo.Delete(v);
                    _investmentRecRepo.Savechange();
                }
            }
            
            var invRec = new InvestmentRec
            {
                InvestmentInitId = investmentRecOwnSBUInsertDto.InvestmentRec.InvestmentInitId,
                ProposedAmount = investmentRecOwnSBUInsertDto.InvestmentRec.ProposedAmount,
                PaymentFreq = investmentRecOwnSBUInsertDto.InvestmentRec.PaymentFreq,
                Purpose = investmentRecOwnSBUInsertDto.InvestmentRec.Purpose,
                CommitmentAllSBU = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentOwnSBU,
                FromDate = investmentRecOwnSBUInsertDto.InvestmentRec.FromDate,
                ToDate = investmentRecOwnSBUInsertDto.InvestmentRec.ToDate,
                CommitmentFromDate = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentFromDate,
                CommitmentToDate = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentToDate,
                TotalMonth = investmentRecOwnSBUInsertDto.InvestmentRec.TotalMonth,
                CommitmentTotalMonth = investmentRecOwnSBUInsertDto.InvestmentRec.CommitmentTotalMonth,
                PaymentMethod = investmentRecOwnSBUInsertDto.InvestmentRec.PaymentMethod,
                ChequeTitle = investmentRecOwnSBUInsertDto.InvestmentRec.ChequeTitle,
                EmployeeId = empId,
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = true,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecRepo.Add(invRec);
            _investmentRecRepo.Savechange();
            var empData = await _employeeRepo.GetByIdAsync(empId);
            var existsData = await _investmentRecCommentRepo.GetByIdAsync((int)investmentRecOwnSBUInsertDto.InvestmentRecComment.Id);
            var invRecCmnt = new InvestmentRecComment
            {
                Id = investmentRecOwnSBUInsertDto.InvestmentRecComment.Id,
                InvestmentInitId = investmentRecOwnSBUInsertDto.InvestmentRecComment.InvestmentInitId,
                EmployeeId = investmentRecOwnSBUInsertDto.InvestmentRecComment.EmployeeId,
                Comments = investmentRecOwnSBUInsertDto.InvestmentRecComment.Comments,
                RecStatus = investmentRecOwnSBUInsertDto.InvestmentRecComment.RecStatus,
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
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                SetOn = existsData.SetOn,
                CompletionStatus = isComplete,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecCommentRepo.Update(invRecCmnt);
            _investmentRecCommentRepo.Savechange();
            foreach (var i in investmentRecOwnSBUInsertDto.InvestmentRecProducts)
            {
                var alreadyExistProdSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistProdSpec);
                if (alreadyExistInvestmentRecProductList.Count < 1)
                {
                    var investmentRecProduct = new InvestmentRecProducts
                    {
                        InvestmentInitId = i.InvestmentInitId,
                        ProductId = i.ProductId,
                        EmployeeId = i.EmployeeId,
                        SBU = i.ProductInfo.SBU,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecProductRepo.Add(investmentRecProduct);
                }
            }
            _investmentRecProductRepo.Savechange();

            return new InvestmentRecComment
            {
                Id = invRecCmnt.Id,
                InvestmentInitId = invRecCmnt.InvestmentInitId,
                EmployeeId = invRec.EmployeeId,
                Comments = invRecCmnt.Comments,
                RecStatus = invRecCmnt.RecStatus
            };
        }
        [HttpPost("insertRecommendForOtherSBU/{empID}/{sbu}")]
        public async Task<ActionResult<InvestmentRecComment>> InsertInvestmentRecommendForOtherSBU(int empId, string sbu, InvestmentRecOtherSBUInsertDto investmentRecOtherSBUInsertDto)
        {

            var spec = new ApprAuthConfigSpecification(empId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var empData = await _employeeRepo.GetByIdAsync(empId);
            var _investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecOtherSBUInsertDto.InvestmentRecComment.InvestmentInitId, empId);
            var _investmentRecComments = await _investmentRecCommentRepo.ListAsync(_investmentRecCommentSpec);
            if (_investmentRecComments.Count > 0)
            {
                foreach (var v in _investmentRecComments)
                {
                    _investmentRecCommentRepo.Delete(v);
                    _investmentRecCommentRepo.Savechange();
                }
            }
            var invRecCmnt = new InvestmentRecComment
            {
                InvestmentInitId = investmentRecOtherSBUInsertDto.InvestmentRecComment.InvestmentInitId,
                EmployeeId = empId,
                Comments = investmentRecOtherSBUInsertDto.InvestmentRecComment.Comments,
                RecStatus = investmentRecOtherSBUInsertDto.InvestmentRecComment.RecStatus,
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
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = false,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecCommentRepo.Add(invRecCmnt);
            _investmentRecCommentRepo.Savechange();
            foreach (var i in investmentRecOtherSBUInsertDto.InvestmentRecProducts)
            {
                var alreadyExistProdSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistProdSpec);
                if (alreadyExistInvestmentRecProductList.Count < 1)
                {
                    var investmentRecProduct = new InvestmentRecProducts
                    {
                        InvestmentInitId = i.InvestmentInitId,
                        ProductId = i.ProductId,
                        EmployeeId = i.EmployeeId,
                        SBU = i.ProductInfo.SBU,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecProductRepo.Add(investmentRecProduct);
                }
            }
            _investmentRecProductRepo.Savechange();
            return new InvestmentRecComment
            {
                Id = invRecCmnt.Id,
                InvestmentInitId = invRecCmnt.InvestmentInitId,
                EmployeeId = invRecCmnt.EmployeeId,
                Comments = invRecCmnt.Comments,
                RecStatus = invRecCmnt.RecStatus,
            };
        }
        [HttpPost("updateRecommendForOtherSBU/{empID}/{sbu}")]
        public async Task<ActionResult<InvestmentRecComment>> UpdateInvestmentRecommendForOtherSBU(int empId, string sbu, InvestmentRecOtherSBUInsertDto investmentRecOtherSBUInsertDto)
        {

            var spec = new ApprAuthConfigSpecification(empId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var empData = await _employeeRepo.GetByIdAsync(empId);
            var existsData = await _investmentRecCommentRepo.GetByIdAsync((int)investmentRecOtherSBUInsertDto.InvestmentRecComment.Id);
            var invRecCmnt = new InvestmentRecComment
            {
                Id = investmentRecOtherSBUInsertDto.InvestmentRecComment.Id,
                InvestmentInitId = investmentRecOtherSBUInsertDto.InvestmentRecComment.InvestmentInitId,
                EmployeeId = investmentRecOtherSBUInsertDto.InvestmentRecComment.EmployeeId,
                Comments = investmentRecOtherSBUInsertDto.InvestmentRecComment.Comments,
                RecStatus = investmentRecOtherSBUInsertDto.InvestmentRecComment.RecStatus,
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
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                SetOn = existsData.SetOn,
                CompletionStatus = false,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecCommentRepo.Update(invRecCmnt);
            _investmentRecCommentRepo.Savechange();
            foreach (var i in investmentRecOtherSBUInsertDto.InvestmentRecProducts)
            {
                var alreadyExistProdSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistProdSpec);
                if (alreadyExistInvestmentRecProductList.Count < 1)
                {
                    var investmentRecProduct = new InvestmentRecProducts
                    {
                        InvestmentInitId = i.InvestmentInitId,
                        ProductId = i.ProductId,
                        EmployeeId = i.EmployeeId,
                        SBU = i.ProductInfo.SBU,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecProductRepo.Add(investmentRecProduct);
                }
            }
            _investmentRecProductRepo.Savechange();

            return new InvestmentRecComment
            {
                Id = invRecCmnt.Id,
                InvestmentInitId = invRecCmnt.InvestmentInitId,
                EmployeeId = invRecCmnt.EmployeeId,
                Comments = invRecCmnt.Comments,
                RecStatus = invRecCmnt.RecStatus
            };
        }


        [HttpPost("insertRec/{empID}/{recStatus}/{sbu}")]
        public async Task<InvestmentRecDto> InsertInvestmentRecomendation(int empId, string recStatus, string sbu, InvestmentRecDto investmentRecDto)
        {

            var alreadyExistSpec = new InvestmentRecSpecification((int)investmentRecDto.InvestmentInitId, empId);
            var alreadyExistInvestmentRecList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
            if (alreadyExistInvestmentRecList.Count > 0)
            {
                foreach (var v in alreadyExistInvestmentRecList)
                {
                    _investmentRecRepo.Delete(v);
                    _investmentRecRepo.Savechange();
                }
            }
            var spec = new ApprAuthConfigSpecification(empId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var invRec = new InvestmentRec
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProposedAmount = investmentRecDto.ProposedAmount,
                Purpose = investmentRecDto.Purpose,
                CommitmentAllSBU = investmentRecDto.CommitmentAllSBU,
                CommitmentOwnSBU = investmentRecDto.CommitmentOwnSBU,
                FromDate = investmentRecDto.FromDate,
                ToDate = investmentRecDto.ToDate,
                TotalMonth = investmentRecDto.TotalMonth,
                PaymentMethod = investmentRecDto.PaymentMethod,
                ChequeTitle = investmentRecDto.ChequeTitle,
                EmployeeId = empId,
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = true,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecRepo.Add(invRec);
            _investmentRecRepo.Savechange();

            return new InvestmentRecDto
            {
                Id = invRec.Id,
                InvestmentInitId = invRec.InvestmentInitId,
                ProposedAmount = invRec.ProposedAmount,
                Purpose = invRec.Purpose,
                CommitmentAllSBU = invRec.CommitmentAllSBU,
                CommitmentOwnSBU = invRec.CommitmentOwnSBU,
                FromDate = invRec.FromDate,
                ToDate = invRec.ToDate,
                TotalMonth = investmentRecDto.TotalMonth,
                PaymentMethod = invRec.PaymentMethod,
                ChequeTitle = invRec.ChequeTitle,
                EmployeeId = invRec.EmployeeId,
            };
        }

        [HttpPost("insertRecCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentRecomendationComment(InvestmentRecCommentDto investmentRecDto)
        {
            var isComplete = false;
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            if (investmentInits.SBU == empData.SBU)
            {
                bool isTrue = false;
                var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
                var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                if (investmentRecDto.RecStatus == "Recommended" || investmentRecDto.RecStatus == "Not Recommended")
                {
                    isComplete = true;
                    foreach (var v in investmentTargetedGroup)
                    {
                        isTrue = false;
                        foreach (var i in investmentRecComments)
                        {
                            if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
                            {
                                isTrue = true;
                            }
                        }
                        if (!isTrue)
                        {
                            return BadRequest(new ApiResponse(400, "Other recommendation has not completed yet"));
                        }
                    }
                }
                else
                {
                    isComplete = false;
                }
            }

            var invRec = new InvestmentRecComment
            {
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
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
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                CompletionStatus = isComplete,
                SetOn = DateTimeOffset.Now
            };
            _investmentRecCommentRepo.Add(invRec);
            _investmentRecCommentRepo.Savechange();

            return new InvestmentRecCommentDto
            {
                Id = invRec.Id,
                InvestmentInitId = invRec.InvestmentInitId,
                EmployeeId = invRec.EmployeeId,
                Comments = invRec.Comments,
                RecStatus = invRec.RecStatus,
            };
        }

        [HttpPost("updateRecCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentRecomendationComment(InvestmentRecCommentDto investmentRecDto)
        {
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var isComplete = false;
            if (investmentInits.SBU == empData.SBU)
            {
                bool isTrue = false;
                var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
                var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                if (investmentRecDto.RecStatus == "Recommended" || investmentRecDto.RecStatus == "Not Recommended")
                {
                    isComplete = true;
                    foreach (var v in investmentTargetedGroup)
                    {
                        isTrue = false;
                        foreach (var i in investmentRecComments)
                        {
                            if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
                            {
                                isTrue = true;
                            }
                        }
                        if (!isTrue) { return BadRequest(new ApiResponse(400, "Other recommendation not completed yet")); }
                    }
                }
                else
                {
                    isComplete = false;
                }
            }
            var existsData = await _investmentRecCommentRepo.GetByIdAsync(investmentRecDto.Id);
            var invRec = new InvestmentRecComment
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
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
                Priority = apprAuthConfig.ApprovalAuthority.Priority,
                SetOn = existsData.SetOn,
                CompletionStatus = isComplete,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecCommentRepo.Update(invRec);
            _investmentRecCommentRepo.Savechange();

            return new InvestmentRecCommentDto
            {
                Id = invRec.Id,
                InvestmentInitId = invRec.InvestmentInitId,
                EmployeeId = invRec.EmployeeId,
                Comments = invRec.Comments,
                RecStatus = invRec.RecStatus,
            };
        }



        [HttpPost("insertRecProd")]
        public async Task<IActionResult> InsertInvestmentRecomendationProduct(List<InvestmentRecProductsDto> investmentRecProductDto)
        {
            try
            {
                foreach (var i in investmentRecProductDto)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification(i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {
                        //foreach (var v in alreadyExistInvestmentRecProductList)
                        //{
                        //    _investmentRecProductRepo.Delete(v);
                        //    _investmentRecProductRepo.Savechange();
                        //}
                    }
                    else
                    {
                        var investmentRecProduct = new InvestmentRecProducts
                        {
                            //ReferenceNo = investmentRecDto.ReferenceNo,
                            InvestmentInitId = i.InvestmentInitId,
                            ProductId = i.ProductId,
                            EmployeeId = i.EmployeeId,
                            SBU = i.ProductInfo.SBU,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecProductRepo.Add(investmentRecProduct);
                    }
                }
                //foreach (var v in investmentRecProductDto)
                //{
                //    var investmentRecProduct = new InvestmentRecProducts
                //    {
                //        //ReferenceNo = investmentRecDto.ReferenceNo,
                //        InvestmentInitId = v.InvestmentInitId,
                //        ProductId = v.ProductId,
                //        EmployeeId = v.EmployeeId,
                //        SBU = v.ProductInfo.SBU,
                //        SetOn = DateTimeOffset.Now,
                //        ModifiedOn = DateTimeOffset.Now
                //    };
                //    _investmentRecProductRepo.Add(investmentRecProduct);
                //}
                _investmentRecProductRepo.Savechange();
                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost("removeInvestmentTargetedProd")]
        public async Task<IActionResult> RemoveInvestmentTargetedProd(InvestmentTargetedProd investmentTargetedProd)
        {
            try
            {
                var alreadyExistSpec = new InvestmentRecProductSpecification(investmentTargetedProd.InvestmentInitId, investmentTargetedProd.ProductId);
                var alreadyExistInvestmentTargetedProdList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentTargetedProdList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentTargetedProdList)
                    {
                        _investmentRecProductRepo.Delete(v);
                        _investmentRecProductRepo.Savechange();
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


        [HttpGet]
        [Route("investmentRecProducts/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecProducts(int investmentInitId, string sbu)
        {
            try
            {
                var spec = new InvestmentRecProductSpecification(investmentInitId);
                var investmentTargetedProd = await _investmentRecProductRepo.ListAsync(spec);
                return investmentTargetedProd;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentTargetedGroups/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentTargetGroupStatusDto>> GetInvestmentTargetedGroups(int investmentInitId, int empId)
        {
            try
            {
                var spec = new InvestmentTargetedGroupSpecification(investmentInitId);
                var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(spec);



                var spec3 = new ApprAuthConfigSpecification(empId, "A");
                var approAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec3);

                // var empSpec = new Emplp(investmentInitId);
                // var empList = await _investmentRecCommentRepo.ListAsync(spec2);

                //string sts = "Active";

                var spec4 = new ApprovalAuthoritySpecification(approAuthConfig.ApprovalAuthorityId);
                var aprAuthority = await _approvalAuthorityRepo.GetEntityWithSpec(spec4);

                var spec2 = new InvestmentRecCommentSpecification(investmentInitId, aprAuthority.Priority, "Recommended");
                var investrecComment = await _investmentRecCommentRepo.ListAsync(spec2);

                var stsResult = (from t in investmentTargetedGroup
                                 join u in investrecComment on t.InvestmentInitId equals u.InvestmentInitId into ut
                                 from p in ut.Where(f => f.SBU == t.SBU).DefaultIfEmpty()
                                 where
                                 // u.EmployeeId == empId
                                 t.InvestmentInitId == investmentInitId
                                 //&& p.Priority == aprAuthority.Priority

                                 select new InvestmentTargetGroupStatusDto
                                 {
                                     InvestmentInitId = t.InvestmentInitId,
                                     SBU = t.SBU,
                                     SBUName = t.SBUName,
                                     MarketCode = t.MarketCode,
                                     MarketName = t.MarketName,
                                     //MarketGroupName = t.MarketGroupMst.GroupName,
                                     RecStatus = p == null ? "Pending" : p.RecStatus,
                                     //ApprovalAuthorityName = aprAuthority.ApprovalAuthorityName
                                     ApprovalAuthorityName = aprAuthority.Remarks
                                 }).OrderBy(x=>x.MarketName).ToList();

                return stsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentRecDetails/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentRec>> GetInvestmentDetails(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentRecSpecification(investmentInitId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getInvestmentRecComment/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecComment(int investmentInitId, int empId)
        {
            try
            {
                var spec = new InvestmentRecCommentSpecification(investmentInitId, empId);
                var investmentCmnt = await _investmentRecCommentRepo.ListAsync(spec);
                return investmentCmnt;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getInvestmentRecComments/{investmentInitId}")]
        public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecComments(int investmentInitId)
        {
            try
            {
                var spec = new InvestmentRecCommentSpecification(investmentInitId);
                var investmentDetail = await _investmentRecCommentRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

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
                    TpVat = medicineProd.UnitTp + medicineProd.UnitVat,
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

        [HttpGet]
        [Route("getLastFiveInvestment/{marketCode}/{date}")]
        public async Task<IReadOnlyList<ReportInvestmentInfo>> GetLastFiveInvestment(string marketCode, string date)
        {
            try
            {
                //var empData = await _employeeRepo.GetByIdAsync(empId);
                var reportInvestmentSpec = new ReportInvestmentSpecification(marketCode);
                var reportInvestmentData = await _reportInvestmentInfoRepo.ListAsync(reportInvestmentSpec);
                // var spec = new ReportInvestmentSpecification(empData.MarketCode);
                var data = (from e in reportInvestmentData
                            where DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture)
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
