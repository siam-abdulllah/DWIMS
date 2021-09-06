using API.Dtos;
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
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IGenericRepository<ApprovalCeiling> _approvalCeilingRepo;
        private readonly IGenericRepository<SBUWiseBudget> _sbuRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;

        public InvestmentRecController(IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo, IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentRec> investmentRecRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo, IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
        IGenericRepository<InvestmentAprComment> investmentAprCommentRepo, IGenericRepository<Employee> employeeRepo,
        IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
        IGenericRepository<ApprAuthConfig> apprAuthConfigRepo,
        IGenericRepository<ApprovalCeiling> approvalCeilingRepo,
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
            _reportInvestmentInfoRepo = reportInvestmentInfoRepo;
            _sbuRepo = sbuRepo;
            _dbContext = dbContext;
            _investmentTargetedGroupRepo = investmentTargetedGroupRepo;
        }
        [HttpGet("investmentInits/{empId}/{sbu}")]
        public  ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams)
        {
            try
            {

                //investmentInitParrams.Search = sbu;
                //investmentRecCommentParrams.Search = sbu;
                //var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                //var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);



                //var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);
                //var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);

                //var investmentInitFormRec = (from i in investmentInits
                //                             where !(from rc in investmentRecComments
                //                                     select rc.InvestmentInitId).Contains(i.Id)
                //                             orderby i.ReferenceNo
                //                             select new InvestmentInitDto
                //                             {
                //                                 Id = i.Id,
                //                                 ReferenceNo = i.ReferenceNo.Trim(),
                //                                 ProposeFor = i.ProposeFor.Trim(),
                //                                 DonationType = i.DonationType.Trim(),
                //                                 DonationTo = i.DonationTo.Trim(),
                //                                 EmployeeId = i.EmployeeId,
                //                             }
                //              ).Distinct().ToList();

                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);
                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", DBNull.Value)
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentInitSearch @SBU,@EID,@RSTATUS", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);



                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
        [HttpGet("investmentRecommended/{empId}/{sbu}")]
        public ActionResult<Pagination<InvestmentInitDto>> GetinvestmentRecommended(int empId, string sbu,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams,
          [FromQuery] InvestmentRecCommentSpecParams investmentRecCommentParrams,
          [FromQuery] InvestmentAprCommentSpecParams investmentAprCommentParrams)
        {
            try
            {
                //investmentInitParrams.Search = sbu;
                //investmentRecCommentParrams.Search = sbu;
                //investmentAprCommentParrams.Search = sbu;

                //var investmentInitSpec = new InvestmentInitSpecification(investmentInitParrams);
                //var investmentRecCommentSpec = new InvestmentRecCommentSpecification(investmentRecCommentParrams);
                //var investmentAprCommentSpec = new InvestmentAprCommentSpecification(investmentAprCommentParrams);

                //var investmentInits = await _investmentInitRepo.ListAsync(investmentInitSpec);
                //var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
                //var investmentAprComments = await _investmentAprCommentRepo.ListAsync(investmentAprCommentSpec);

                //var investmentInitFormRec = (from i in investmentInits
                //                             join rc in investmentRecComments on i.Id equals rc.InvestmentInitId
                //                             where !(from ac in investmentAprComments where ac.AprStatus=="Approved"
                //                                     select ac.InvestmentInitId).Contains(i.Id)
                //                             orderby i.ReferenceNo
                //                             select new InvestmentInitDto
                //                             {
                //                                 Id = i.Id,
                //                                 ReferenceNo = i.ReferenceNo.Trim(),
                //                                 ProposeFor = i.ProposeFor.Trim(),
                //                                 DonationType = i.DonationType.Trim(),
                //                                 DonationTo = i.DonationTo.Trim(),
                //                                 EmployeeId = i.EmployeeId,
                //                             }
                //              ).Distinct().ToList();

                //var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);

                //var totalItems = await _investmentInitRepo.CountAsync(countSpec);

                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@SBU", sbu),
                        new SqlParameter("@EID", empId),
                        new SqlParameter("@RSTATUS", "Recommended"),
                        new SqlParameter("@ASTATUS", "Approved")
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentRecSearch @SBU,@EID,@RSTATUS,@ASTATUS", parms.ToArray()).ToList();
                var data = _mapper
                    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);

                return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }


        [HttpPost("insertRec/{empID}/{recStatus}/{sbu}")]
        public async Task<InvestmentRecDto> InsertInvestmentRecomendation(int empId,string recStatus,string sbu,InvestmentRecDto investmentRecDto)
        {
            // if (recStatus == "Approved")
            // {
            //     var sbuWiseBudgetSpec = new SBUWiseBudgetSpecificiation(sbu, DateTime.Now.ToString("dd/MM/yyyy"));
            //     var sbuWiseBudgetData = await _sbuRepo.ListAsync(sbuWiseBudgetSpec);
            //     var sbuWiseInvestmentRecSpec = new InvestmentRecSpecification(empId, recStatus);

            //     var apprAuthConfigSpec = new ApprAuthConfigSpecification(empId, "A");
            //     var apprAuthConfigList = await _apprAuthConfigRepo.ListAsync(apprAuthConfigSpec);
            //     var approvalCeilingSpec = new ApprovalCeilingSpecification(apprAuthConfigList[0].ApprovalAuthorityId,"A", DateTime.Now.ToString("dd/MM/yyyy"));
            //     var approvalAuthorityCeilingData = await _approvalCeilingRepo.ListAsync(approvalCeilingSpec);
               
            //     //int v2 = investmentRecDto.InvestmentInitId ?? default(int);
            //     // var recCommentRepo= _investmentRecCommentRepo.GetByIdAsync(investmentRecDto.InvestmentInitId ?? default);

            // }

            var alreadyExistSpec = new InvestmentRecSpecification(investmentRecDto.InvestmentInitId);
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
            var investmentTargetedGroupSpec = new InvestmentTargetedGroupSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentTargetedGroup = await _investmentTargetedGroupRepo.ListAsync(investmentTargetedGroupSpec);
            var investmentRecCommentSpec = new InvestmentRecCommentSpecification((int)investmentRecDto.InvestmentInitId);
            var investmentRecComments = await _investmentRecCommentRepo.ListAsync(investmentRecCommentSpec);
            foreach (var i in investmentRecComments)
            {
                foreach (var v in investmentTargetedGroup)
                {
                    if (v.InvestmentInitId == i.InvestmentInitId) { }
                }
            }

                var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var invRec = new InvestmentRecComment
            {
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
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
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var empData = await _employeeRepo.GetByIdAsync(investmentRecDto.EmployeeId);
            var invRec = new InvestmentRecComment
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                EmployeeId = investmentRecDto.EmployeeId,
                Comments = investmentRecDto.Comments,
                RecStatus = investmentRecDto.RecStatus,
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
                        foreach (var v in alreadyExistInvestmentRecProductList)
                        {
                            _investmentRecProductRepo.Delete(v);
                            _investmentRecProductRepo.Savechange();
                        }
                    }
                }
                    
                foreach (var v in investmentRecProductDto)
                {
                    var investmentRecProduct = new InvestmentRecProducts
                    {
                        //ReferenceNo = investmentRecDto.ReferenceNo,
                        InvestmentInitId = v.InvestmentInitId,
                        ProductId = v.ProductId,
                        SBU = v.ProductInfo.SBU,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now
                    };
                    _investmentRecProductRepo.Add(investmentRecProduct);
                }

                _investmentRecProductRepo.Savechange();

                return Ok("Succsessfuly Saved!!!");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("updateRecProd")]
        public ActionResult<InvestmentRecProductsDto> UpdateInvestmentRecomendationProduct(InvestmentRecProductsDto investmentRecDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var invRec = new InvestmentRecProducts
            {
                Id = investmentRecDto.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProductId = investmentRecDto.ProductId,
                SBU = investmentRecDto.SBU,
                ModifiedOn = DateTimeOffset.Now,
            };
            _investmentRecProductRepo.Update(invRec);
            _investmentRecProductRepo.Savechange();

            return new InvestmentRecProductsDto
            {
                Id = invRec.Id,
                InvestmentInitId = investmentRecDto.InvestmentInitId,
                ProductId = investmentRecDto.ProductId,
            };
        }

        [HttpGet]
        [Route("investmentRecProducts/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecProducts(int investmentInitId, string sbu)
        {
            try
            {
                var spec = new InvestmentRecProductSpecification(investmentInitId,sbu);
                var investmentTargetedProd = await _investmentRecProductRepo.ListAsync(spec);
                return investmentTargetedProd;
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
        public async Task<IReadOnlyList<InvestmentRecComment>> GetInvestmentRecComment(int investmentInitId,int empId)
        {
            try
            {
                var spec = new InvestmentRecCommentSpecification(investmentInitId,empId);
                var investmentDetail = await _investmentRecCommentRepo.ListAsync(spec);
                return investmentDetail;
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
                            where DateTime.ParseExact(e.FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)>= DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture)
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
