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
    public class InvestmentAprController : BaseApiController
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

        public InvestmentAprController(IGenericRepository<ApprovalCeiling> approvalCeilingRepo,
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
        public ActionResult<IReadOnlyList<InvestmentInitForApr>> GetInvestmentInits(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchNotCamp @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                //var data = _mapper
                //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            orderby r.SetOn descending
                            select new InvestmentInitForApr
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
                                RemainingSBU = null,
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

        [HttpGet("investmentInitsForRSM/{empId}/{sbu}")]
        public ActionResult<IReadOnlyList<InvestmentInitForApr>> GetInvestmentInitsForRSM(int empId, string sbu,
         [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchForRSM @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                //var data = _mapper
                //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            orderby r.SetOn descending
                            select new InvestmentInitForApr
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
                                RemainingSBU = null,
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

        [HttpGet("investmentInitsForGPM/{empId}/{sbu}")]
        public ActionResult<IReadOnlyList<InvestmentInitForApr>> GetInvestmentInitsForGPM(int empId, string sbu,
         [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentAprSearchForGPM @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                //var data = _mapper
                //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            orderby r.SetOn descending
                            select new InvestmentInitForApr
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
                                RemainingSBU = null,
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

        [HttpGet("investmentInitsForSM/{empId}/{sbu}")]
        public ActionResult<IReadOnlyList<InvestmentInitForApr>> GetInvestmentInitsForSM(int empId, string sbu,

        [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended")
                    };
                var results = _dbContext.InvestmentInitForApr.FromSqlRaw<InvestmentInitForApr>("EXECUTE SP_InvestmentAprSearchForMngr @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                //var data = _mapper
                //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            //orderby r.SetOn descending
                            orderby r.RemainingSBU.Length
                            select new InvestmentInitForApr
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
                                RemainingSBU = r.RemainingSBU,
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
        public async Task<ActionResult<IReadOnlyList<InvestmentInitForApr>>> GetinvestmentApproved(int empId, string sbu, string userRole,
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
                                                 orderby r.SetOn descending
                                                 select new InvestmentInitForApr
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
                                                     RemainingSBU = null,
                                                     Donation = d,
                                                     Employee = e
                                                 }
                                                    ).Distinct().OrderByDescending(x => x.SetOn).ToList();
                    //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                    //var totalItems = await _investmentInitRepo.CountAsync(countSpec);
                    //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, investmentInitFormRec.Count(), investmentInitFormRec));
                    return investmentInitFormRec;
                }

                else
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended"),
                        new SqlParameter("@ASTATUS", DBNull.Value)
                    };
                    var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentApprpvedSearchNotCamp @SBU,@EID,@RSTATUS,@ASTATUS", parms.ToArray()).ToList();
                    //var data = _mapper
                    //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                    //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                    //var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                    var data = (from r in results
                                join d in _dbContext.Donation on r.DonationId equals d.Id
                                join e in _dbContext.Employee on r.EmployeeId equals e.Id
                                orderby r.SetOn descending
                                select new InvestmentInitForApr
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
                                    RemainingSBU = null,
                                    Donation = d,
                                    Employee = e
                                }
                          ).Distinct().ToList();
                    return data;
                    //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, results.Count(), data));
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
        [HttpGet("investmentApprovedForRSM/{empId}/{sbu}/{userRole}")]
        public ActionResult<IReadOnlyList<InvestmentInitForApr>> GetinvestmentApprovedForRSM(int empId, string sbu, string userRole,
        [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {

                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended"),
                        new SqlParameter("@ASTATUS", DBNull.Value)
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentApprpvedSearchForRSM @SBU,@EID,@RSTATUS,@ASTATUS", parms.ToArray()).ToList();
                //var data = _mapper
                //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            orderby r.SetOn descending
                            orderby r.SetOn descending
                            select new InvestmentInitForApr
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
                                RemainingSBU = null,
                                Donation = d,
                                Employee = e
                            }
                          ).Distinct().ToList();
                return data;
                //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, results.Count(), data));
            }

            catch (System.Exception e)
            {
                throw e;
            }
        }
        [HttpGet("investmentApprovedForGPM/{empId}/{sbu}/{userRole}")]
        public ActionResult<IReadOnlyList<InvestmentInitForApr>> GetinvestmentApprovedForGPM(int empId, string sbu, string userRole,
        [FromQuery] InvestmentInitSpecParams investmentInitParrams)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                 {
                    new SqlParameter("@SBU", sbu),
                    new SqlParameter("@EID", empId),
                    new SqlParameter("@RSTATUS", "Recommended"),
                    new SqlParameter("@ASTATUS", DBNull.Value)
                 };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentApprpvedSearchForGPM @SBU,@EID,@RSTATUS,@ASTATUS", parms.ToArray()).ToList();
                //var data = _mapper
                //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);
                var data = (from r in results
                            join d in _dbContext.Donation on r.DonationId equals d.Id
                            join e in _dbContext.Employee on r.EmployeeId equals e.Id
                            orderby r.SetOn descending
                            select new InvestmentInitForApr
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
                                RemainingSBU = null,
                                Donation = d,
                                Employee = e
                            }
                          ).Distinct().ToList();
                return data;
                //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, results.Count(), data));
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }


        [HttpPost("insertAprForOwnSBU/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprForOwnSBU(int empId, string sbu, int donationId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (investmentInits.SBU == empData.SBU)
                {
                    bool isTrue = false;
                    var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                    var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                    var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                    var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                    isComplete = true;
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Not Approved")
                    {
                        isComplete = false;
                    }
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
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                    {
                        List<SqlParameter> parms = new List<SqlParameter>
                            {
                            new SqlParameter("@SBU", sbu),
                            new SqlParameter("@DID", donationId),
                            new SqlParameter("@EID", empId),
                            new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                            new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                            new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                            new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                            };
                        var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckNew @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                        if (parms[6].Value.ToString() != "True")
                        {
                            return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                        }
                        var alreadyExistRecSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
                        if (alreadyExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentAprList)
                            {
                                _investmentRecRepo.Delete(v);
                                _investmentRecRepo.Savechange();
                            }
                        }

                        var invRecAppr = new InvestmentRec
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRecAppr);
                        _investmentRecRepo.Savechange();

                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                        {
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                            {
                                DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                                calcDate = calcDate.AddMonths(i);
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(3);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(6);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var alreadyExistRecSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
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
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRec);
                        _investmentRecRepo.Savechange();
                    }
                }


                var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, empId);
                var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
                if (investmentRecCmnts.Count > 0)
                {
                    foreach (var v in investmentRecCmnts)
                    {
                        _investmentRecCommentRepo.Delete(v);
                        _investmentRecCommentRepo.Savechange();
                    }
                }
                var invRecCmnt = new InvestmentRecComment
                {
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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
                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost("updateAprForOwnSBU/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprForOwnSBU(int empId, string sbu, int donationId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (investmentInits.SBU == empData.SBU)
                {
                    bool isTrue = false;
                    var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                    var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                    var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                    var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                    isComplete = true;
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Not Approved")
                    {
                        isComplete = false;
                    }
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
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                    {
                        List<SqlParameter> parms = new List<SqlParameter>
                            {
                            new SqlParameter("@SBU", sbu),
                            new SqlParameter("@DID", donationId),
                            new SqlParameter("@EID", empId),
                            new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                            new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                            new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                            new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                            };
                        var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckNew @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                        if (parms[6].Value.ToString() != "True")
                        {
                            return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                        }
                        var alreadyExistRecSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
                        if (alreadyExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentAprList)
                            {
                                _investmentRecRepo.Delete(v);
                                _investmentRecRepo.Savechange();
                            }
                        }

                        var invRecAppr = new InvestmentRec
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRecAppr);
                        _investmentRecRepo.Savechange();

                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                        {
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                            {
                                DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                                calcDate = calcDate.AddMonths(i);
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(3);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(6);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var alreadyExistRecSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
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
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRec);
                        _investmentRecRepo.Savechange();
                    }
                }






                //var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, empId);
                //var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
                //if (investmentRecCmnts.Count > 0)
                //{
                //foreach (var v in investmentRecCmnts)
                //{
                //    _investmentRecCommentRepo.Delete(v);
                //    _investmentRecCommentRepo.Savechange();
                //}
                //}
                var existsInvestmentRecs = await _investmentRecCommentRepo.GetByIdAsync(investmenAprForOwnSBUInsert.InvestmentRecComment.Id);
                var invRecCmnt = new InvestmentRecComment
                {
                    Id = investmenAprForOwnSBUInsert.InvestmentRecComment.Id,
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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
                    SetOn = existsInvestmentRecs.SetOn,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentRecCommentRepo.Update(invRecCmnt);
                _investmentRecCommentRepo.Savechange();



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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

                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("insertAprForOwnSBURSM/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprForOwnSBURSM(int empId, string sbu, int donationId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (investmentInits.SBU == empData.SBU)
                {
                    bool isTrue = false;
                    var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                    var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                    var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                    var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                    isComplete = true;
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Not Approved")
                    {
                        isComplete = false;
                    }
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
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                    {
                        List<SqlParameter> parms = new List<SqlParameter>
                            {
                            new SqlParameter("@SBU", sbu),
                            new SqlParameter("@DID", donationId),
                            new SqlParameter("@EID", empId),
                            new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                            new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                            new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                            new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                            };
                        var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckNew @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                        if (parms[6].Value.ToString() != "True")
                        {
                            return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                        }
                        var alreadyExistSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                        if (alreadyExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentAprList)
                            {
                                _investmentRecRepo.Delete(v);
                                _investmentRecRepo.Savechange();
                            }
                        }

                        var invRecAppr = new InvestmentRec
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRecAppr);
                        _investmentRecRepo.Savechange();

                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                        {
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                            {
                                DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                                calcDate = calcDate.AddMonths(i);
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(3);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(6);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var alreadyExistSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
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
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRec);
                        _investmentRecRepo.Savechange();
                    }
                    var alreadyExistSpecRecDepot = new InvestmentRecDepotSpecification(investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId, investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode);
                    var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpecRecDepot);
                    if (alreadyExistInvestmentRecDepotList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecDepotList)
                        {
                            _investmentRecDepotRepo.Delete(v);
                            _investmentRecDepotRepo.Savechange();
                        }
                    }
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus != "Not Approved")
                    {
                        var invRecDepot = new InvestmentRecDepot
                        {
                            //ReferenceNo = investmentRecDto.ReferenceNo,
                            InvestmentInitId = investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId,
                            DepotCode = investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode,
                            DepotName = investmenAprForOwnSBUInsert.investmentRecDepot.DepotName,
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecDepotRepo.Add(invRecDepot);
                        _investmentRecDepotRepo.Savechange();
                    }
                }






                var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, empId);
                var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
                if (investmentRecCmnts.Count > 0)
                {
                    foreach (var v in investmentRecCmnts)
                    {
                        _investmentRecCommentRepo.Delete(v);
                        _investmentRecCommentRepo.Savechange();
                    }
                }
                var invRecCmnt = new InvestmentRecComment
                {
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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
                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
     
        [HttpPost("updateAprForOwnSBURSM/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprForOwnSBURSM(int empId, string sbu, int donationId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (investmentInits.SBU == empData.SBU)
                {
                    bool isTrue = false;
                    var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                    var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                    var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                    var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                    isComplete = true;
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Not Approved")
                    {
                        isComplete = false;
                    }
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
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                    {
                        List<SqlParameter> parms = new List<SqlParameter>
                            {
                            new SqlParameter("@SBU", sbu),
                            new SqlParameter("@DID", donationId),
                            new SqlParameter("@EID", empId),
                            new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                            new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                            new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                            new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                            };
                        var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckNew @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                        if (parms[6].Value.ToString() != "True")
                        {
                            return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
                        }
                        var alreadyExistSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                        if (alreadyExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentAprList)
                            {
                                _investmentRecRepo.Delete(v);
                                _investmentRecRepo.Savechange();
                            }
                        }

                        var invRecAppr = new InvestmentRec
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRecAppr);
                        _investmentRecRepo.Savechange();

                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                        {
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                            {
                                DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                                calcDate = calcDate.AddMonths(i);
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(3);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(6);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var alreadyExistSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
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
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRec);
                        _investmentRecRepo.Savechange();
                    }
                    var alreadyExistSpecRecDepot = new InvestmentRecDepotSpecification(investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId, investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode);
                    var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpecRecDepot);
                    if (alreadyExistInvestmentRecDepotList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecDepotList)
                        {
                            _investmentRecDepotRepo.Delete(v);
                            _investmentRecDepotRepo.Savechange();
                        }
                    }
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus != "Not Approved")
                    {
                        var invRecDepot = new InvestmentRecDepot
                        {
                            //ReferenceNo = investmentRecDto.ReferenceNo,
                            InvestmentInitId = investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId,
                            DepotCode = investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode,
                            DepotName = investmenAprForOwnSBUInsert.investmentRecDepot.DepotName,
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecDepotRepo.Add(invRecDepot);
                        _investmentRecDepotRepo.Savechange();
                    }
                }

                var existsInvestmentRecs = await _investmentRecCommentRepo.GetByIdAsync(investmenAprForOwnSBUInsert.InvestmentRecComment.Id);
                var invRecCmnt = new InvestmentRecComment
                {
                    Id = investmenAprForOwnSBUInsert.InvestmentRecComment.Id,
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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
                    SetOn = existsInvestmentRecs.SetOn,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentRecCommentRepo.Update(invRecCmnt);
                _investmentRecCommentRepo.Savechange();



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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

                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("insertAprForOwnSBUCampaign/{empID}/{aprStatus}/{sbu}/{donationId}/{CampaignDtlId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprForOwnSBUCampaign(int empId, string sbu, int donationId, int campaignDtlId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (investmentInits.SBU == empData.SBU)
                {
                    bool isTrue = false;
                    var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                    var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                    var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                    var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                    isComplete = true;
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Not Approved")
                    {
                        isComplete = false;
                    }
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
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                    {
                        List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                        new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                        new SqlParameter("@CDTLID", campaignDtlId),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                        var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckForCampaign @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@CDTLID,@r out", parms.ToArray());
                        if (parms[7].Value.ToString() != "True")
                        {
                            return BadRequest(new ApiResponse(400, parms[7].Value.ToString()));
                        }
                        var alreadyExistRecSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
                        if (alreadyExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentAprList)
                            {
                                _investmentRecRepo.Delete(v);
                                _investmentRecRepo.Savechange();
                            }
                        }

                        var invRecAppr = new InvestmentRec
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRecAppr);
                        _investmentRecRepo.Savechange();

                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                        {
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                            {
                                DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                                calcDate = calcDate.AddMonths(i);
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(3);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(6);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var alreadyExistREcSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistREcSpec);
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
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRec);
                        _investmentRecRepo.Savechange();
                    }

                    var alreadyExistSpec = new InvestmentRecDepotSpecification(investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId, investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode);
                    var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecDepotList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecDepotList)
                        {
                            _investmentRecDepotRepo.Delete(v);
                            _investmentRecDepotRepo.Savechange();
                        }
                    }
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus != "Not Approved")
                    {
                        var invRecDepot = new InvestmentRecDepot
                        {
                            //ReferenceNo = investmentRecDto.ReferenceNo,
                            InvestmentInitId = investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId,
                            DepotCode = investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode,
                            DepotName = investmenAprForOwnSBUInsert.investmentRecDepot.DepotName,
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecDepotRepo.Add(invRecDepot);
                        _investmentRecDepotRepo.Savechange();
                    }
                }






                var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, empId);
                var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
                if (investmentRecCmnts.Count > 0)
                {
                    foreach (var v in investmentRecCmnts)
                    {
                        _investmentRecCommentRepo.Delete(v);
                        _investmentRecCommentRepo.Savechange();
                    }
                }
                var invRecCmnt = new InvestmentRecComment
                {
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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
                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
      
        [HttpPost("updateAprForOwnSBUCampaign/{empID}/{aprStatus}/{sbu}/{donationId}/{CampaignDtlId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprForOwnSBUCampaign(int empId, string sbu, int donationId, int campaignDtlId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (investmentInits.SBU == empData.SBU)
                {
                    bool isTrue = false;
                    var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                    var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
                    var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, apprAuthConfig.ApprovalAuthority.Priority, "true");
                    var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                    isComplete = true;
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Not Approved")
                    {
                        isComplete = false;
                    }
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
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                    {
                        List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                        new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                        new SqlParameter("@CDTLID", campaignDtlId),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                        var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckForCampaign @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@CDTLID,@r out", parms.ToArray());
                        if (parms[7].Value.ToString() != "True")
                        {
                            return BadRequest(new ApiResponse(400, parms[7].Value.ToString()));
                        }
                        var alreadyExistRecSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
                        if (alreadyExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyExistInvestmentAprList)
                            {
                                _investmentRecRepo.Delete(v);
                                _investmentRecRepo.Savechange();
                            }
                        }

                        var invRecAppr = new InvestmentRec
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRecAppr);
                        _investmentRecRepo.Savechange();

                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                        {
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                            {
                                DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                                calcDate = calcDate.AddMonths(i);
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(3);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                            {
                                var invDT = new InvestmentDetailTracker
                                {
                                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                    DonationId = donationId,
                                    ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                    Month = calcDate.Month,
                                    Year = calcDate.Year,
                                    FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                    ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                    PaidStatus = "Paid",
                                    EmployeeId = empId,
                                    SetOn = DateTimeOffset.Now
                                };
                                _investmentDetailTrackerRepo.Add(invDT);
                                calcDate = calcDate.AddMonths(6);
                            }
                            _investmentDetailTrackerRepo.Savechange();
                        }
                        else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = DateTime.Now.Month,
                                Year = DateTime.Now.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                        var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                        var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                        if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                        {
                            foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                            {
                                _investmentDetailTrackerRepo.Delete(v);
                                _investmentDetailTrackerRepo.Savechange();
                            }
                        }
                        var alreadyExistRecSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                        var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistRecSpec);
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
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                            PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                            CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                            CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                            CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                            CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                            TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                            CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                            PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                            ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                            EmployeeId = empId,
                            Priority = apprAuthConfig.ApprovalAuthority.Priority,
                            CompletionStatus = true,
                            SetOn = DateTimeOffset.Now
                        };
                        _investmentRecRepo.Add(invRec);
                        _investmentRecRepo.Savechange();
                    }
                    var alreadyExistSpec = new InvestmentRecDepotSpecification(investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId, investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode);
                    var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecDepotList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentRecDepotList)
                        {
                            _investmentRecDepotRepo.Delete(v);
                            _investmentRecDepotRepo.Savechange();
                        }
                    }
                    if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus != "Not Approved")
                    {
                        var invRecDepot = new InvestmentRecDepot
                        {
                            //ReferenceNo = investmentRecDto.ReferenceNo,
                            InvestmentInitId = investmenAprForOwnSBUInsert.investmentRecDepot.InvestmentInitId,
                            DepotCode = investmenAprForOwnSBUInsert.investmentRecDepot.DepotCode,
                            DepotName = investmenAprForOwnSBUInsert.investmentRecDepot.DepotName,
                            EmployeeId = empId,
                            SetOn = DateTimeOffset.Now,
                            ModifiedOn = DateTimeOffset.Now
                        };
                        _investmentRecDepotRepo.Add(invRecDepot);
                        _investmentRecDepotRepo.Savechange();
                    }
                }






                var existsInvestmentRecs = await _investmentRecCommentRepo.GetByIdAsync(investmenAprForOwnSBUInsert.InvestmentRecComment.Id);

                var invRecCmnt = new InvestmentRecComment
                {
                    Id = investmenAprForOwnSBUInsert.InvestmentRecComment.Id,
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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
                    SetOn = existsInvestmentRecs.SetOn,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentRecCommentRepo.Update(invRecCmnt);
                _investmentRecCommentRepo.Savechange();



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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
                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("insertAprForOwnSBUGPM/{empID}/{aprStatus}/{sbu}/{donationId}/{CampaignDtlId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprForOwnSBUGPM(int empId, string sbu, int donationId, int campaignDtlId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                isComplete = true;
                if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                {
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                        new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                        new SqlParameter("@CDTLID", campaignDtlId),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckForCampaign @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@CDTLID,@r out", parms.ToArray());
                    if (parms[7].Value.ToString() != "True")
                    {
                        return BadRequest(new ApiResponse(400, parms[7].Value.ToString()));
                    }
                    var alreadyExistSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprList)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }

                    var invRecAppr = new InvestmentRec
                    {
                        InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                        Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                        PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                        CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                        ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                        CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRecAppr);
                    _investmentRecRepo.Savechange();

                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }
                    if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                    {
                        for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                    {
                        DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                        for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(3);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                    {
                        DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                        for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(6);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                    {
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            DonationId = donationId,
                            ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Month = DateTime.Now.Month,
                            Year = DateTime.Now.Year,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                    isComplete = false;
                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }
                    var alreadyExistSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
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
                        InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                        Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                        PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                        CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                        ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                        CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRec);
                    _investmentRecRepo.Savechange();
                }


                var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, empId);
                var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
                if (investmentRecCmnts.Count > 0)
                {
                    foreach (var v in investmentRecCmnts)
                    {
                        _investmentRecCommentRepo.Delete(v);
                        _investmentRecCommentRepo.Savechange();
                    }
                }
                var invRecCmnt = new InvestmentRecComment
                {
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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
                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("updateAprForOwnSBUGPM/{empID}/{aprStatus}/{sbu}/{donationId}/{CampaignDtlId}")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprForOwnSBUGPM(int empId, string sbu, int donationId, int campaignDtlId, InvestmenAprForOwnSBUInsert investmenAprForOwnSBUInsert)
        {
            try
            {
                var isComplete = false;
                var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId);
                var empData = await _employeeRepo.GetByIdAsync(empId);
                var spec = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
                if (investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus == "Approved")
                {
                    isComplete = true;
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount),
                        new SqlParameter("@ASTATUS", investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus),
                        new SqlParameter("@CDTLID", campaignDtlId),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                    var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckForCampaign @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@CDTLID,@r out", parms.ToArray());
                    if (parms[7].Value.ToString() != "True")
                    {
                        return BadRequest(new ApiResponse(400, parms[7].Value.ToString()));
                    }
                    var alreadyExistSpec = new InvestmentRecSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
                    var alreadyExistInvestmentAprList = await _investmentRecRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyExistInvestmentAprList)
                        {
                            _investmentRecRepo.Delete(v);
                            _investmentRecRepo.Savechange();
                        }
                    }

                    var invRecAppr = new InvestmentRec
                    {
                        InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                        Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                        PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                        CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                        ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                        CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRecAppr);
                    _investmentRecRepo.Savechange();

                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }
                    if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Monthly")
                    {
                        for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth; i++)
                        {
                            DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                            calcDate = calcDate.AddMonths(i);
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Quarterly")
                    {
                        DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                        for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 3; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(3);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Half Yearly")
                    {
                        DateTimeOffset calcDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate;
                        for (int i = 0; i < investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth / 6; i++)
                        {
                            var invDT = new InvestmentDetailTracker
                            {
                                InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                                DonationId = donationId,
                                ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                                Month = calcDate.Month,
                                Year = calcDate.Year,
                                FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                                ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                                PaidStatus = "Paid",
                                EmployeeId = empId,
                                SetOn = DateTimeOffset.Now
                            };
                            _investmentDetailTrackerRepo.Add(invDT);
                            calcDate = calcDate.AddMonths(6);
                        }
                        _investmentDetailTrackerRepo.Savechange();
                    }
                    else if (investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq == "Yearly")
                    {
                        var invDT = new InvestmentDetailTracker
                        {
                            InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                            DonationId = donationId,
                            ApprovedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                            Month = DateTime.Now.Month,
                            Year = DateTime.Now.Year,
                            FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                            ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
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
                    isComplete = false;
                    var alreadyDetailTrackerExistSpec = new InvestmentDetailTrackerSpecification(investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId);
                    var alreadyDetailTrackerExistInvestmentAprList = await _investmentDetailTrackerRepo.ListAsync(alreadyDetailTrackerExistSpec);
                    if (alreadyDetailTrackerExistInvestmentAprList.Count > 0)
                    {
                        foreach (var v in alreadyDetailTrackerExistInvestmentAprList)
                        {
                            _investmentDetailTrackerRepo.Delete(v);
                            _investmentDetailTrackerRepo.Savechange();
                        }
                    }
                    var alreadyExistSpec = new InvestmentRecSpecification((int)investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId, empId);
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
                        InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentApr.InvestmentInitId,
                        ProposedAmount = investmenAprForOwnSBUInsert.InvestmentApr.ProposedAmount,
                        Purpose = investmenAprForOwnSBUInsert.InvestmentApr.Purpose,
                        PaymentFreq = investmenAprForOwnSBUInsert.InvestmentApr.PaymentFreq,
                        CommitmentAllSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentAllSBU,
                        CommitmentOwnSBU = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentOwnSBU,
                        FromDate = investmenAprForOwnSBUInsert.InvestmentApr.FromDate,
                        ToDate = investmenAprForOwnSBUInsert.InvestmentApr.ToDate,
                        CommitmentFromDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentFromDate,
                        CommitmentToDate = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentToDate,
                        TotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.TotalMonth,
                        CommitmentTotalMonth = investmenAprForOwnSBUInsert.InvestmentApr.CommitmentTotalMonth,
                        PaymentMethod = investmenAprForOwnSBUInsert.InvestmentApr.PaymentMethod,
                        ChequeTitle = investmenAprForOwnSBUInsert.InvestmentApr.ChequeTitle,
                        EmployeeId = empId,
                        Priority = apprAuthConfig.ApprovalAuthority.Priority,
                        CompletionStatus = true,
                        SetOn = DateTimeOffset.Now
                    };
                    _investmentRecRepo.Add(invRec);
                    _investmentRecRepo.Savechange();
                }


                //var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId, empId);
                //var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
                //if (investmentRecCmnts.Count > 0)
                //{
                //    foreach (var v in investmentRecCmnts)
                //    {
                //        _investmentRecCommentRepo.Delete(v);
                //        _investmentRecCommentRepo.Savechange();
                //    }
                //}
                var existsInvestmentRecs = await _investmentRecCommentRepo.GetByIdAsync(investmenAprForOwnSBUInsert.InvestmentRecComment.Id);
                var invRecCmnt = new InvestmentRecComment
                {
                    Id = investmenAprForOwnSBUInsert.InvestmentRecComment.Id,
                    InvestmentInitId = investmenAprForOwnSBUInsert.InvestmentRecComment.InvestmentInitId,
                    EmployeeId = empId,
                    Comments = investmenAprForOwnSBUInsert.InvestmentRecComment.Comments,
                    RecStatus = investmenAprForOwnSBUInsert.InvestmentRecComment.RecStatus,
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
                    SetOn = existsInvestmentRecs.SetOn,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentRecCommentRepo.Update(invRecCmnt);
                _investmentRecCommentRepo.Savechange();



                foreach (var i in investmenAprForOwnSBUInsert.InvestmentRecProducts)
                {
                    var alreadyExistSpec = new InvestmentRecProductSpecification((int)i.InvestmentInitId, i.ProductId);
                    var alreadyExistInvestmentRecProductList = await _investmentRecProductRepo.ListAsync(alreadyExistSpec);
                    if (alreadyExistInvestmentRecProductList.Count > 0)
                    {

                    }
                    else
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
                return new InvestmentRecCommentDto
                {
                    Id = invRecCmnt.Id,
                    InvestmentInitId = invRecCmnt.InvestmentInitId,
                    EmployeeId = invRecCmnt.EmployeeId,
                    Comments = invRecCmnt.Comments,
                    RecStatus = invRecCmnt.RecStatus,
                };

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }









        [HttpPost("insertAprForCampaign/{empID}/{aprStatus}/{sbu}/{donationId}/{CampaignDtlId}")]
        public async Task<ActionResult<InvestmentAprDto>> InsertInvestmentAprForCampaign(int empId, string aprStatus, string sbu, int donationId, int CampaignDtlId, InvestmentAprDto investmentAprDto)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@DID", donationId),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@IID", investmentAprDto.InvestmentInitId),
                        new SqlParameter("@PRAMOUNT", investmentAprDto.ProposedAmount),
                        new SqlParameter("@ASTATUS", aprStatus),
                        new SqlParameter("@CDTLID", CampaignDtlId),
                        new SqlParameter("@r", SqlDbType.VarChar,200){ Direction = ParameterDirection.Output }
                    };
                var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckForCampaign @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@CDTLID,@r out", parms.ToArray());
                if (parms[7].Value.ToString() != "True")
                {
                    return BadRequest(new ApiResponse(400, parms[7].Value.ToString()));
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
                var specAppr = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                var invRecAppr = new InvestmentRec
                {
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
                        Month = DateTime.Now.Month,
                        Year = DateTime.Now.Year,
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("insertApr/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentAprDto>> InsertInvestmentApr(int empId, string aprStatus, string sbu, int donationId, InvestmentAprDto investmentAprDto)
        {
            try
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
                var result = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InvestmentCeilingCheckNew @SBU,@DID,@EID,@IID,@PRAMOUNT,@ASTATUS,@r out", parms.ToArray());
                if (parms[6].Value.ToString() != "True")
                {
                    return BadRequest(new ApiResponse(400, parms[6].Value.ToString()));
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
                var specAppr = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                var invRecAppr = new InvestmentRec
                {
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
                        Month = DateTime.Now.Month,
                        Year = DateTime.Now.Year,
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("insertRec/{empID}/{aprStatus}/{sbu}/{donationId}")]
        public async Task<ActionResult<InvestmentAprDto>> InsertInvestmentRec(int empId, string aprStatus, string sbu, int donationId, InvestmentAprDto investmentAprDto)
        {
            try
            {
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

        [HttpPost("insertAprCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprComment(InvestmentRecCommentDto investmentRecDto)
        {
            //var empData = await _employeeRepo.GetByIdAsync(investmentAprDto.EmployeeId);
            // var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentAprDto.InvestmentInitId);

            // if (investmentInits.SBU == empData.SBU)
            // {
            //     bool isTrue = false;
            //     var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentAprDto.InvestmentInitId);
            //     var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            //     var investmentAprCommentSpec = new InvestmentAprCommentSpecification((int)investmentAprDto.InvestmentInitId);
            //     var investmentAprComments = await _investmentAprCommentRepo.ListAsync(investmentAprCommentSpec);
            //     foreach (var v in investmentTargetedGroup)
            //     {
            //         isTrue = false;
            //         foreach (var i in investmentAprComments)
            //         {
            //             if (v.InvestmentInitId == i.InvestmentInitId && v.SBU == i.SBU)
            //             {
            //                 isTrue = true;
            //             }
            //         }
            //         if (!isTrue) { return BadRequest(new ApiResponse(400, "Other recommendations are not completed yet")); }

            //     }
            // }
            //var invApr = new InvestmentAprComment
            //{
            //    //ReferenceNo = investmentInitDto.ReferenceNo,
            //    InvestmentInitId = investmentAprDto.InvestmentInitId,
            //    EmployeeId = investmentAprDto.EmployeeId,
            //    Comments = investmentAprDto.Comments,
            //    AprStatus = investmentAprDto.AprStatus,
            //    MarketGroupCode = empData.MarketGroupCode,
            //    MarketGroupName = empData.MarketGroupName,
            //    MarketCode = empData.MarketCode,
            //    MarketName = empData.MarketName,
            //    RegionCode = empData.RegionCode,
            //    RegionName = empData.RegionName,
            //    ZoneCode = empData.ZoneCode,
            //    ZoneName = empData.ZoneName,
            //    TerritoryCode = empData.TerritoryCode,
            //    TerritoryName = empData.TerritoryName,
            //    SBUName = empData.SBUName,
            //    SBU = empData.SBU,
            //    SetOn = DateTimeOffset.Now
            //};
            //_investmentAprCommentRepo.Add(invApr);
            //_investmentAprCommentRepo.Savechange();

            var isComplete = false;
            //var investmentInitSpec = new InvestmentInitSpecification((int)investmentRecDto.InvestmentInitId);
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
                isComplete = true;
                if (investmentRecDto.RecStatus == "Not Approved")
                {
                    isComplete = false;
                }
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
            var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, investmentRecDto.EmployeeId);
            var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
            if (investmentRecCmnts.Count > 0)
            {
                foreach (var v in investmentRecCmnts)
                {
                    _investmentRecCommentRepo.Delete(v);
                    _investmentRecCommentRepo.Savechange();
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
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
            };
        }
        [HttpPost("insertAprComForGPM")]
        public async Task<ActionResult<InvestmentRecCommentDto>> InsertInvestmentAprCommentForGPM(InvestmentRecCommentDto investmentRecDto)
        {
            var isComplete = false;
            //var investmentInits = await _investmentInitRepo.GetByIdAsync((int)investmentRecDto.InvestmentInitId);
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            isComplete = true;
            if (investmentRecDto.RecStatus == "Not Approved")
            {
                isComplete = false;
            }
            var investmentRecCmntSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId, investmentRecDto.EmployeeId);
            var investmentRecCmnts = await _investmentRecCommentRepo.ListAsync(investmentRecCmntSpec);
            if (investmentRecCmnts.Count > 0)
            {
                foreach (var v in investmentRecCmnts)
                {
                    _investmentRecCommentRepo.Delete(v);
                    _investmentRecCommentRepo.Savechange();
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
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
            };
        }

        [HttpPost("updateAprCom")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprComment(InvestmentRecCommentDto investmentRecDto)
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
                isComplete = true;
                if (investmentRecDto.RecStatus == "Not Approved")
                {
                    isComplete = false;
                }
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
            var existsInvestmentRecs = await _investmentRecCommentRepo.GetByIdAsync(investmentRecDto.Id);
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
                SetOn = existsInvestmentRecs.SetOn,
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

        [HttpPost("updateAprComForGPM")]
        public async Task<ActionResult<InvestmentRecCommentDto>> UpdateInvestmentAprCommentForGPM(InvestmentRecCommentDto investmentRecDto)
        {


            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var spec = new ApprAuthConfigSpecification(investmentRecDto.EmployeeId, "A");
            var apprAuthConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            var isComplete = true;
            if (investmentRecDto.RecStatus == "Not Approved")
            {
                isComplete = false;
            }
            var existsInvestmentRecs = await _investmentRecCommentRepo.GetByIdAsync(investmentRecDto.Id);
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
                SetOn = existsInvestmentRecs.SetOn,
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

        [HttpPost("insertAprProd")]
        public async Task<IActionResult> InsertInvestmentApromendationProduct(List<InvestmentRecProductsDto> investmentRecProductDto)
        {
            //try
            //{
            //    foreach (var i in investmentAprProductDto)
            //    {
            //        var alreadyExistSpec = new InvestmentAprProductSpecification(i.InvestmentInitId, i.ProductId);
            //        var alreadyExistInvestmentAprProductList = await _investmentAprProductRepo.ListAsync(alreadyExistSpec);
            //        if (alreadyExistInvestmentAprProductList.Count > 0)
            //        {
            //            foreach (var v in alreadyExistInvestmentAprProductList)
            //            {
            //                _investmentAprProductRepo.Delete(v);
            //                _investmentAprProductRepo.Savechange();
            //            }
            //        }
            //    }

            //    foreach (var v in investmentAprProductDto)
            //    {
            //        var investmentAprProduct = new InvestmentAprProducts
            //        {
            //            //ReferenceNo = investmentAprDto.ReferenceNo,
            //            InvestmentInitId = v.InvestmentInitId,
            //            ProductId = v.ProductId,
            //            EmployeeId = v.EmployeeId,
            //            SBU = v.ProductInfo.SBU,
            //            SetOn = DateTimeOffset.Now,
            //            ModifiedOn = DateTimeOffset.Now
            //        };
            //        _investmentAprProductRepo.Add(investmentAprProduct);
            //    }

            //    _investmentAprProductRepo.Savechange();

            //    return Ok("Succsessfuly Saved!!!");
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
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

        [HttpPost("insertInvestmentRecDepot")]
        public async Task<ActionResult<InvestmentRecDepot>> InsertInvestmentRecDepot(InvestmentRecDepot investmentRecDepot)
        {

            try
            {
                var alreadyExistSpec = new InvestmentRecDepotSpecification(investmentRecDepot.InvestmentInitId, investmentRecDepot.DepotCode);
                var alreadyExistInvestmentRecDepotList = await _investmentRecDepotRepo.ListAsync(alreadyExistSpec);
                if (alreadyExistInvestmentRecDepotList.Count > 0)
                {
                    foreach (var v in alreadyExistInvestmentRecDepotList)
                    {
                        _investmentRecDepotRepo.Delete(v);
                        _investmentRecDepotRepo.Savechange();
                    }
                }

                var invRecDepot = new InvestmentRecDepot
                {
                    //ReferenceNo = investmentRecDto.ReferenceNo,
                    InvestmentInitId = investmentRecDepot.InvestmentInitId,
                    DepotCode = investmentRecDepot.DepotCode,
                    DepotName = investmentRecDepot.DepotName,
                    EmployeeId = investmentRecDepot.EmployeeId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now
                };
                _investmentRecDepotRepo.Add(invRecDepot);
                _investmentRecProductRepo.Savechange();

                return invRecDepot;
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
        [HttpGet]
        [Route("investmentRecDetailsForM/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRec>> investmentRecDetailsForM(int investmentInitId, int empId)
        {
            try
            {
                //var specAppr = new ApprAuthConfigSpecification(empId, "A");
                //var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                var initData = await _investmentInitRepo.GetByIdAsync(investmentInitId);
                var spec = new InvestmentRecSpecification(investmentInitId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                string qry = "SELECT CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,  MAX(A.Priority) Count FROM ApprAuthConfig AC INNER JOIN ApprovalAuthority A ON AC.ApprovalAuthorityId = A.Id " +
                    " INNER JOIN Employee E ON Ac.EmployeeId = E.Id WHERE( E.ZoneCode = '" + initData.ZoneCode + "' )";
                var result = _dbContext.CountInt.FromSqlRaw(qry).ToList();
                //return result[0].Count.ToString();

                //return investmentDetail.Where(x => x.Priority == apprAuthConfigAppr.ApprovalAuthority.Priority - 1).ToList();
                return investmentDetail.Where(x => x.Priority == result[0].Count).ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("investmentRecDetailsForGPM/{investmentInitId}/{empId}")]
        public async Task<IReadOnlyList<InvestmentRec>> GetInvestmentRecDetailsForGPM(int investmentInitId, int empId)
        {
            try
            {
                var specAppr = new ApprAuthConfigSpecification(empId, "A");
                var apprAuthConfigAppr = await _apprAuthConfigRepo.GetEntityWithSpec(specAppr);
                var spec = new InvestmentRecSpecification(investmentInitId);
                var investmentDetail = await _investmentRecRepo.ListAsync(spec);
                return investmentDetail.Where(x => x.Priority == 3).ToList();
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

    }
}
