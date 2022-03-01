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
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentAprNoSbuController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IGenericRepository<InvestmentRec> _investmentRecRepo;
        private readonly IGenericRepository<InvestmentRecProducts> _investmentRecProductRepo;
        private readonly IGenericRepository<InvestmentRecDepot> _investmentRecDepotRepo;
        private readonly IGenericRepository<InvestmentRecComment> _investmentRecCommentRepo;
        private readonly IGenericRepository<InvestmentApr> _investmentAprRepo;
        private readonly IGenericRepository<InvestmentAprComment> _investmentAprCommentRepo;
        private readonly IGenericRepository<InvestmentAprProducts> _investmentAprProductRepo;
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;
        private readonly IGenericRepository<SBUWiseBudget> _sbuRepo;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IGenericRepository<ApprovalCeiling> _approvalCeilingRepo;
        private readonly IGenericRepository<InvestmentDetailTracker> _investmentDetailTrackerRepo;
        private readonly IGenericRepository<Donation> _donationRepo;

        public InvestmentAprNoSbuController(IGenericRepository<ApprovalCeiling> approvalCeilingRepo,
            IGenericRepository<ApprAuthConfig> apprAuthConfigRepo,
            IGenericRepository<SBUWiseBudget> sbuRepo,
            IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo,
            IGenericRepository<InvestmentInit> investmentInitRepo,
            IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
            IGenericRepository<InvestmentRecDepot> investmentRecDepotRepo,
            IGenericRepository<InvestmentRecComment> investmentRecCommentRepo,
            IGenericRepository<InvestmentApr> investmentAprRepo,
            IGenericRepository<InvestmentAprComment> investmentAprCommentRepo,
            IGenericRepository<InvestmentAprProducts> investmentAprProductRepo,
            IGenericRepository<Employee> employeeRepo,
            IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
            StoreContext dbContext,
            IGenericRepository<InvestmentRec> investmentRecRepo,
            IGenericRepository<InvestmentDetailTracker> investmentDetailTrackerRepo,
            IGenericRepository<Donation> donationRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _investmentAprRepo = investmentAprRepo;
            _investmentInitRepo = investmentInitRepo;
            _investmentRecProductRepo = investmentRecProductRepo;
            _investmentRecDepotRepo = investmentRecDepotRepo;
            _investmentRecCommentRepo = investmentRecCommentRepo;
            _investmentAprCommentRepo = investmentAprCommentRepo;
            _investmentAprProductRepo = investmentAprProductRepo;
            _reportInvestmentInfoRepo = reportInvestmentInfoRepo;
            _employeeRepo = employeeRepo;
            _dbContext = dbContext;
            _investmentTargetedGroupRepo = investmentTargetedGroupRepo;
            _sbuRepo = sbuRepo;
            _apprAuthConfigRepo = apprAuthConfigRepo;
            _approvalCeilingRepo = approvalCeilingRepo;
            _investmentRecRepo = investmentRecRepo;
            _investmentDetailTrackerRepo = investmentDetailTrackerRepo;
            _donationRepo = donationRepo;
        }

        [HttpGet("investmentInits/{empId}/{sbu}")]
        public ActionResult<IReadOnlyList<InvestmentInit>> GetInvestmentInits(int empId, string sbu,
        [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@EID", empId),
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchNoSbu @EID", parms.ToArray()).ToList();
                //var data = _mapper.Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            orderby r.SetOn
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
                //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        [HttpGet("investmentApproved/{empId}/{sbu}/{userRole}")]
        public async Task<ActionResult<IReadOnlyList<InvestmentInit>>> GetinvestmentApproved(int empId, string sbu, string userRole,
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
                                                 where rc.RecStatus == "Approved"
                                                 orderby r.SetOn
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
                        //new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                      //  new SqlParameter("@RSTATUS", "Recommended"),
                        //new SqlParameter("@ASTATUS", DBNull.Value)
                    };
                    var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentApprpvedSearchNoSbu @EID", parms.ToArray()).ToList();
                    //var data = _mapper
                    //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                    //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                    //var totalItems = await _investmentInitRepo.CountAsync(countSpec);
                    //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, results.Count(), data));
                    var data = (from r in results
                                join d in _dbContext.Donation on r.DonationId equals d.Id
                                join e in _dbContext.Employee on r.EmployeeId equals e.Id
                                orderby r.SetOn
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

        [HttpPost("insertInvestAprNoSBU/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentRecComment>> InsertInvestmentAprNoSBU(int empId, string aprStatus, string sbu, int donationId, InvestmentNoSBUAprInsertDto investmentNoSBUAprInsertDto)
        {
            try
            {
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (aprStatus == "Approved")
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount),
                        new SqlParameter("@ASTATUS", aprStatus),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheck @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                    if (parms[6].Value.ToString() != "True")
                    {
                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                    }
                    var alreadyExistSpecs = new InvestmentRecSpecification((int)investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprLists = await _investmentRecRepo.ListAsync(alreadyExistSpecs);
                    if (alreadyExistInvestmentAprLists.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprLists)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }
                    //var specAppr = new ApprAuthConfigSpecification(empId, "A");
                    //var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                    var invRecAppr = new InvestmentRec
                    {
                        InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                        PaymentFreq = investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq,
                        Purpose = investmentNoSBUAprInsertDto.InvestmentApr.Purpose,
                        CommitmentAllSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                        ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                        CommitmentFromDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmentNoSBUAprInsertDto.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmentNoSBUAprInsertDto.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRecAppr);
                    _investmentRecRepo.Savechange();

                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification((int)investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }
                    //var donation = await _donationRepo.GetByIdAsync(donationId);
                    if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Monthly")
                    {
                        for (int i = 0; i < investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth; i++)
                        {
                            DateTimeOffset calcDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate;
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                                ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Quarterly")
                    {
                        DateTimeOffset calcDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate;
                        for (int i = 0; i < investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth/3; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                                ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(3);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    } 
                    else if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Half Yearly")
                    {
                        DateTimeOffset calcDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate;
                        for (int i = 0; i < investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth/6; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                                ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(6);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Yearly")
                    {
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                            DonationId = donationId,
                            ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                            Month = DateTime.Now.Month,
                            Year = DateTime.Now.Year,
                            FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                            ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                            PaidStatus = "Paid",
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentDetailTrackerRepo.Add(invDT);
                        _investmentDetailTrackerRepo.Savechange();
                    }
                }
                else
                {
                    var alreadyExistSpec = new InvestmentRecSpecification((int)investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprList)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }
                    //var spec = new ApprAuthConfigSpecification(empId, "A");
                    //var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                    var invRec = new InvestmentRec
                    {
                        InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                        Purpose = investmentNoSBUAprInsertDto.InvestmentApr.Purpose,
                        PaymentFreq = investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq,
                        CommitmentAllSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                        ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                        CommitmentFromDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmentNoSBUAprInsertDto.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmentNoSBUAprInsertDto.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRec);
                    _investmentRecRepo.Savechange();
                }
                //---------------------------------------------------------------------------------------------------------------------

                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);

                var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId, empId);
                var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                if (investmentRecComments.Count > 0)
                {
                    foreach (var v in investmentRecComments)
                    {
                        _investmentRecCommentRepo.Delete(v);
                        _investmentRecCommentRepo.Savechange();
                    }
                }
                isComplete = true;
                if (investmentNoSBUAprInsertDto.InvestmentRecComment.RecStatus == "Not Approved")
                {
                    isComplete = false;
                }
                var invRecCmnt = new InvestmentRecComment
                {
                    InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmentNoSBUAprInsertDto.InvestmentRecComment.Comments,
                    RecStatus = investmentNoSBUAprInsertDto.InvestmentRecComment.RecStatus,
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

                foreach (var i in investmentNoSBUAprInsertDto.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
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
                _investmentRecProductRepo.Savechange();

                return new InvestmentRecComment
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmentNoSBUAprInsertDto.InvestmentRecComment.Comments,
                    RecStatus = investmentNoSBUAprInsertDto.InvestmentRecComment.RecStatus,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("UpdateInvestAprNoSBU/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentRecComment>> UpdateInvestmentAprNoSBU(int empId, string aprStatus, string sbu, int donationId, InvestmentNoSBUAprInsertDto investmentNoSBUAprInsertDto)
        {

            try
            {
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (aprStatus == "Approved")
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount),
                        new SqlParameter("@ASTATUS", aprStatus),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheck @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                    if (parms[6].Value.ToString() != "True")
                    {
                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                    }
                    var alreadyExistSpecs = new InvestmentRecSpecification((int)investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprLists = await _investmentRecRepo.ListAsync(alreadyExistSpecs);
                    if (alreadyExistInvestmentAprLists.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprLists)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }
                    var invRecAppr = new InvestmentRec
                    {
                        InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                        Purpose = investmentNoSBUAprInsertDto.InvestmentApr.Purpose,
                        PaymentFreq = investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq,
                        CommitmentAllSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                        ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                        CommitmentFromDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmentNoSBUAprInsertDto.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmentNoSBUAprInsertDto.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRecAppr);
                    _investmentRecRepo.Savechange();

                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification((int)investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }
                    //var donation = await _donationRepo.GetByIdAsync(donationId);
                    if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Monthly")
                    {
                        for (int i = 0; i < investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth; i++)
                        {
                            DateTimeOffset calcDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate;
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                                ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Quarterly")
                    {
                        DateTimeOffset calcDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate;
                        for (int i = 0; i < investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth / 3; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                                ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(3);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Half Yearly")
                    {
                        DateTimeOffset calcDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate;
                        for (int i = 0; i < investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth / 6; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                                ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(6);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq == "Yearly")
                    {
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                            DonationId = donationId,
                            ApprovedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                            Month = DateTime.Now.Month,
                            Year = DateTime.Now.Year,
                            FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                            ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                            PaidStatus = "Paid",
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentDetailTrackerRepo.Add(invDT);
                        _investmentDetailTrackerRepo.Savechange();
                    }
                }
                else
                {
                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification((int)investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }
                    var alreadyExistSpec = new InvestmentRecSpecification((int)investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprList)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }
                    var invRec = new InvestmentRec
                    {
                        InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmentNoSBUAprInsertDto.InvestmentApr.ProposedAmount,
                        Purpose = investmentNoSBUAprInsertDto.InvestmentApr.Purpose,
                        PaymentFreq = investmentNoSBUAprInsertDto.InvestmentApr.PaymentFreq,
                        CommitmentAllSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmentNoSBUAprInsertDto.InvestmentApr.FromDate,
                        ToDate = investmentNoSBUAprInsertDto.InvestmentApr.ToDate,
                        CommitmentFromDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmentNoSBUAprInsertDto.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmentNoSBUAprInsertDto.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmentNoSBUAprInsertDto.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRec);
                    _investmentRecRepo.Savechange();
                }
                //---------------------------------------------------------------------------------------------------------------------

                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                //var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId);
                //var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId, empId);
                var investmentRecComments = await _investmentRecCommentRepo.GetEntityWithSpec(investmentRecCommentSpec);
                //if (investmentRecComments.Count > 0)
                //{
                //    foreach (var v in investmentRecComments)
                //    {
                //        _investmentRecCommentRepo.Delete(v);
                //        _investmentRecCommentRepo.Savechange();
                //    }
                //}
                isComplete = true;
                if (investmentNoSBUAprInsertDto.InvestmentRecComment.RecStatus == "Not Approved")
                {
                    isComplete = false;
                }
                var invRecCmnt = new InvestmentRecComment
                {
                    Id = investmentNoSBUAprInsertDto.InvestmentRecComment.Id,
                    InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmentNoSBUAprInsertDto.InvestmentRecComment.Comments,
                    RecStatus = investmentNoSBUAprInsertDto.InvestmentRecComment.RecStatus,
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
                    ModifiedOn = DateTimeOffset.Now,
                    SetOn = investmentRecComments.SetOn
                };
                _investmentRecCommentRepo.Update(invRecCmnt);
                _investmentRecCommentRepo.Savechange();

                foreach (var i in investmentNoSBUAprInsertDto.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
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
                _investmentRecProductRepo.Savechange();

                return new InvestmentRecComment
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = investmentNoSBUAprInsertDto.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmentNoSBUAprInsertDto.InvestmentRecComment.Comments,
                    RecStatus = investmentNoSBUAprInsertDto.InvestmentRecComment.RecStatus,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("InsertApr/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentAprDto>> InsertInvestmentApr(int empId, string aprStatus, string sbu, int donationId, InvestmentAprDto investmentAprDto)
        {
            try
            {
                if (aprStatus == "Approved")
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmentAprDto.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmentAprDto.ProposedAmount),
                        new SqlParameter("@ASTATUS", aprStatus),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheck @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                    //var param=parms[6].Value;
                    if (parms[6].Value.ToString() != "True")
                    //if (result.Result == 0)
                    {
                        return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                    }


                    var alreadyExistSpecs = new InvestmentRecSpecification(investmentAprDto.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprLists = await _investmentRecRepo.ListAsync(alreadyExistSpecs);
                    if (alreadyExistInvestmentAprLists.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprLists)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }
                    var specAppr = new ApprAuthConfigSpecification(empId, "A");
                    var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                    var invRecAppr = new InvestmentRec
                    {
                        //ReferenceNo = investmentInitDto.ReferenceNo,
                        InvestmentInitId = investmentAprDto.InvestmentInitId,
                        ProposedAmount = investmentAprDto.ProposedAmount,
                        Purpose = investmentAprDto.Purpose,
                        CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                        FromDate = investmentAprDto.FromDate,
                        ToDate = investmentAprDto.ToDate,
                        TotalMonth = investmentAprDto.TotalMonth,
                        PaymentMethod = investmentAprDto.PaymentMethod,
                        ChequeTitle = investmentAprDto.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfigAppr.ApprovalAuthority.Priority,
                        //Priority = 3,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRecAppr);
                    _investmentRecRepo.Savechange();

                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmentAprDto.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }

                    var donation = await _donationRepo.GetByIdAsync(donationId);
                    if (donation.DonationTypeName == "Honorarium")
                    {

                        for (int i = 0; i < investmentAprDto.TotalMonth; i++)
                        {
                            DateTimeOffset calcDate = investmentAprDto.FromDate;
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmentAprDto.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmentAprDto.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmentAprDto.FromDate,
                                ToDate = investmentAprDto.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else
                    {
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmentAprDto.InvestmentInitId,
                            DonationId = donationId,
                            ApprovedAmount = investmentAprDto.ProposedAmount,
                            Month = 0,
                            Year = 0,
                            FromDate = investmentAprDto.FromDate,
                            ToDate = investmentAprDto.ToDate,
                            PaidStatus = "Paid",
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentDetailTrackerRepo.Add(invDT);
                        _investmentDetailTrackerRepo.Savechange();
                    }

                    return new InvestmentAprDto
                    {
                        Id = invRecAppr.Id,
                        InvestmentInitId = investmentAprDto.InvestmentInitId,
                        ProposedAmount = investmentAprDto.ProposedAmount,
                        Purpose = investmentAprDto.Purpose,
                        CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                        CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                        FromDate = investmentAprDto.FromDate,
                        ToDate = investmentAprDto.ToDate,
                        TotalMonth = investmentAprDto.TotalMonth,
                        PaymentMethod = investmentAprDto.PaymentMethod,
                        ChequeTitle = investmentAprDto.ChequeTitle,
                        EmployeeId = investmentAprDto.EmployeeId,
                    };
                }

                var alreadyExistSpec = new InvestmentRecSpecification(investmentAprDto.InvestmentInitId, empId);
                var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentAprList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentAprList)
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
                    InvestmentInitId = investmentAprDto.InvestmentInitId,
                    ProposedAmount = investmentAprDto.ProposedAmount,
                    Purpose = investmentAprDto.Purpose,
                    CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                    FromDate = investmentAprDto.FromDate,
                    ToDate = investmentAprDto.ToDate,
                    TotalMonth = investmentAprDto.TotalMonth,
                    PaymentMethod = investmentAprDto.PaymentMethod,
                    ChequeTitle = investmentAprDto.ChequeTitle,
                    EmployeeId = empId,
                    Priority = apprAuthConfig.ApprovalAuthority.Priority,
                    CompletionStatus = true,
                    SetOn = DateTimeOffset.Now
                };
                _investmentRecRepo.Add(invRec);
                _investmentRecRepo.Savechange();

                return new InvestmentAprDto
                {
                    Id = invRec.Id,
                    InvestmentInitId = investmentAprDto.InvestmentInitId,
                    ProposedAmount = investmentAprDto.ProposedAmount,
                    Purpose = investmentAprDto.Purpose,
                    CommitmentAllSBU = investmentAprDto.CommitmentAllSBU,
                    CommitmentOwnSBU = investmentAprDto.CommitmentOwnSBU,
                    FromDate = investmentAprDto.FromDate,
                    ToDate = investmentAprDto.ToDate,
                    TotalMonth = investmentAprDto.TotalMonth,
                    PaymentMethod = investmentAprDto.PaymentMethod,
                    ChequeTitle = investmentAprDto.ChequeTitle,
                    EmployeeId = investmentAprDto.EmployeeId,
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("InsertAprCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprComment(InvestmentRecCommentDto investmentRecDto)
        {
            var isComplete = false;
            //var investmentInitSpec = new InvestmentInitSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            //bool isTrue = false;
            var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
            var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
            isComplete = true;
            if (investmentRecDto.RecStatus == "Not Approved")
            {
                isComplete = false;
            }
            // foreach (var v in investmentTargetedGroup)
            // {
            //     isTrue = false;
            //     foreach (var i in investmentRecComments)
            //     {
            //         if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
            //         {
            //             isTrue = true;
            //         }
            //     }
            //     if (!isTrue)
            //     {
            //         return BadRequest(new ApiResponse(400, "Other recommendation has not completed yet"));
            //     }
            // }
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
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
            };
        }

        [HttpPost("UpdateAprCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprComment(InvestmentRecCommentDto investmentRecDto)
        {
            var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var isComplete = false;
            //bool isTrue = false;
            var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
            var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
            isComplete = true;
            if (investmentRecDto.RecStatus == "Not Approved")
            {
                isComplete = false;
            }
            // foreach (var v in investmentTargetedGroup)
            // {
            //     isTrue = false;
            //     foreach (var i in investmentRecComments)
            //     {
            //         if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
            //         {
            //             isTrue = true;
            //         }
            //     }
            //     if (!isTrue) { return BadRequest(new ApiResponse(400, "Other recommendation not completed yet")); }
            // }


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
                CompletionStatus = isComplete,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecCommentRepo.Update(invRec);
            _investmentRecCommentRepo.Savechange();

            return new InvestmentRecCommentDto
            {
                Id = investmentRecDto.Id,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
            };
        }

        [HttpPost("InsertAprProd")]
        public async Task<IActionResult> InsertInvestmentApromendationProduct(List<InvestmentRecProductsDto> investmentRecProductDto)
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
                _investmentRecProductRepo.Savechange();
                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpGet("getInvestmentRecDepot/{initId}")]
        public async Task<ActionResult<InvestmentRecDepot>> GetInvestmentRecDepot(int initId)
        {
            try
            {
                var alreadyExistSpec = new InvestmentRecDepotSpecification(initId);
                var alreadyExistInvestmentRecDepot = await _investmentRecDepotRepo.GetEntityWithSpec(alreadyExistSpec);

                return alreadyExistInvestmentRecDepot;
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
                //var response = new HttpResponseMessage();
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
        [Route("investmentAprProducts/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecProducts(int investmentInitId, string sbu)
        {
            try
            {
                //var spec = new InvestmentRecProductSpecification(investmentInitId, sbu);
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
        [Route("investmentAprDetails/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRec>> GetInvestmentDetails(int investmentInitId, int empId)
        {
            try
            {
                var spec = new InvestmentRecSpecification(investmentInitId, empId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getInvestmentAprComment/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecComment(int investmentInitId, int empId)
        {
            try
            {
                var spec = new InvestmentRecCommentSpecification(investmentInitId, empId);
                var investmentDetail = await _investmentRecCommentRepo.ListAsync(spec);
                return investmentDetail;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getInvestmentAprComments/{investmentInitId}")]
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

        [HttpGet]
        [Route("getEmpMarket/{investmentInitId}")]
        public async Task<object> GetEmpLocation(int investmentInitId)
        {
            try
            {
                string qry = " select CAST(a.EmployeeId AS INT) AS Id, a.SetOn, a.ModifiedOn, a.DataStatus, a.MarketCode,  a.MarketName, " +
                " a.TerritoryCode, a.TerritoryName, a.RegionCode, a.RegionName, a.ZoneCode, a.ZoneName, dbo.fnGetEmpNamedesig(a.EmployeeId) EmployeeName, a.[Priority],a.RecStatus, a.Comments " +
                " from InvestmentRecComment a inner join InvestmentInit b on b.Id = a.InvestmentInitId " +
                " where InvestmentInitId = '" + investmentInitId + "' " +
                " UNION " +
                " select CAST(a.EmployeeId AS INT) AS Id, a.SetOn, a.ModifiedOn, a.DataStatus, a.MarketCode,  a.MarketName,  " +
                " a.TerritoryCode, a.TerritoryName, a.RegionCode, a.RegionName, a.ZoneCode, a.ZoneName, dbo.fnGetEmpNamedesig(a.EmployeeId) EmployeeName, '1','Inititator', '' Comments " +
                " From InvestmentInit a where a.Id = '" + investmentInitId + "' AND a.Confirmation = 1 " +
                " UNION " +
                " select CAST(a.Id AS INT) AS Id, a.SetOn, a.ModifiedOn, a.DataStatus, a.MarketCode,  a.MarketName,  a.TerritoryCode, a.TerritoryName, a.RegionCode, a.RegionName, a.ZoneCode, a.ZoneName, dbo.fnGetEmpNamedesigByMarket(a.MarketCode) EmployeeName, '1'," +
                " RecStatus =CASE CompletionStatus WHEN 1 THEN 'Recommended' ELSE 'Not Recommended' END,'' Comments " +
                " from InvestmentTargetedGroup a " +
                " where a.InvestmentInitId = '" + investmentInitId + "' " +
                " order by a.[Priority] desc ";

                var spec = await _dbContext.EmployeeLocation.FromSqlRaw(qry).ToListAsync();
                return spec;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("investmentRecDetails/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRec>> GetInvestmentRecDetails(int investmentInitId, int empId)
        {
            try
            {
                var specAppr = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                var spec = new InvestmentRecSpecification(investmentInitId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                return investmentDetail.Where(x => x.Priority == apprAuthConfigAppr.ApprovalAuthority.Priority - 1).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}