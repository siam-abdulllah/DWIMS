﻿using API.Dtos;
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
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _reportInvestmentInfoRepo;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        private readonly IGenericRepository<ApprovalAuthority> _approvalAuthorityRepo;
        private readonly IGenericRepository<ApprovalCeiling> _approvalCeilingRepo;
        private readonly IGenericRepository<SBUWiseBudget> _sbuRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IGenericRepository<InvestmentTargetedGroup> _investmentTargetedGroupRepo;

        public InvestmentRecController(IGenericRepository<InvestmentTargetedGroup> investmentTargetedGroupRepo, IGenericRepository<InvestmentInit> investmentInitRepo, IGenericRepository<InvestmentRec> investmentRecRepo, IGenericRepository<InvestmentRecComment> investmentRecCommentRepo, IGenericRepository<InvestmentRecProducts> investmentRecProductRepo,
        IGenericRepository<InvestmentAprComment> investmentAprCommentRepo, IGenericRepository<Employee> employeeRepo,
        IGenericRepository<ReportInvestmentInfo> reportInvestmentInfoRepo,
        IGenericRepository<ApprAuthConfig> apprAuthConfigRepo,
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
        }
        [HttpGet("investmentInits/{empId}/{sbu}")]
        //public ActionResult<Pagination<InvestmentInitDto>> GetInvestmentInits(int empId, string sbu,[FromQuery] InvestmentInitSpecParams investmentInitParrams)
        public IReadOnlyList<InvestmentInit> GetInvestmentInits(int empId, string sbu,[FromQuery] InvestmentInitSpecParams investmentInitParrams)
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
                        new SqlParameter("@EID", empId)
                    };
                var results = _dbContext.InvestmentInit.FromSqlRaw<InvestmentInit>("EXECUTE SP_InvestmentRecSearch @SBU,@EID", parms.ToArray()).ToList();
                //var data = _mapper
                //    .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                //return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, 50, data));
                return results;
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }
   
        [HttpGet("investmentRecommended/{empId}/{sbu}/{userRole}")]
        public async Task<ActionResult<Pagination<InvestmentInitDto>>> GetinvestmentRecommended(int empId, string sbu, string userRole,
          [FromQuery] InvestmentInitSpecParams investmentInitParrams)
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
                if (userRole == "Administrator")
                {

                    var investmentInits = await _investmentInitRepo.ListAllAsync();
                    var investmentRecComments = await _investmentRecCommentRepo.ListAllAsync();
                    //var investmentAprComments = await _investmentAprCommentRepo.ListAllAsync();
                    var investmentInitFormRec = (from i in investmentInits
                                                 join rc in investmentRecComments on i.Id equals rc.InvestmentInitId
                                                 where rc.RecStatus != "Approved"
                                                 orderby i.ReferenceNo
                                                 select new InvestmentInitDto
                                                 {
                                                     Id = i.Id,
                                                     ReferenceNo = i.ReferenceNo,
                                                     ProposeFor = i.ProposeFor,
                                                     DonationId = i.DonationId,
                                                     MarketCode = i.MarketCode,
                                                     MarketName = i.MarketName,
                                                     DonationTo = i.DonationTo,
                                                     EmployeeId = i.EmployeeId,
                                                     SetOn = i.SetOn
                                                 }
                                  ).Distinct().OrderByDescending(x => x.SetOn).ToList();

                    return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, investmentInitFormRec.Count(), investmentInitFormRec));
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
                    var data = _mapper
                        .Map<IReadOnlyList<InvestmentInit>, IReadOnlyList<InvestmentInitDto>>(results);
                    var countSpec = new InvestmentInitWithFiltersForCountSpecificication(investmentInitParrams);
                    var totalItems = await _investmentInitRepo.CountAsync(countSpec);
                    return Ok(new Pagination<InvestmentInitDto>(investmentInitParrams.PageIndex, investmentInitParrams.PageSize, results.Count(), data));
                }
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }


        [HttpPost("insertRec/{empID}/{recStatus}/{sbu}")]
        public async Task<InvestmentRecDto> InsertInvestmentRecomendation(int empId, string recStatus, string sbu, InvestmentRecDto investmentRecDto)
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
                else {
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

        //[HttpPost("updateRecProd")]
        //public ActionResult<InvestmentRecProductsDto> UpdateInvestmentRecomendationProduct(InvestmentRecProductsDto investmentRecDto)
        //{
        //    // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
        //    // if (user == null) return Unauthorized(new ApiResponse(401));
        //    var invRec = new InvestmentRecProducts
        //    {
        //        Id = investmentRecDto.Id,
        //        InvestmentInitId = investmentRecDto.InvestmentInitId,
        //        ProductId = investmentRecDto.ProductId,
        //        SBU = investmentRecDto.SBU,
        //        EmployeeId = investmentRecDto.EmployeeId,
        //        ModifiedOn = DateTimeOffset.Now,
        //    };
        //    _investmentRecProductRepo.Update(invRec);
        //    _investmentRecProductRepo.Savechange();

        //    return new InvestmentRecProductsDto
        //    {
        //        Id = invRec.Id,
        //        InvestmentInitId = investmentRecDto.InvestmentInitId,
        //        ProductId = investmentRecDto.ProductId,
        //    };
        //}

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
        [Route("investmentRecProducts/{investmentInitId}/{sbu}")]
        public async Task<IReadOnlyList<InvestmentRecProducts>> GetInvestmentRecProducts(int investmentInitId, string sbu)
        {
            try
            {
                var spec = new InvestmentRecProductSpecification(investmentInitId);
                //var spec = new InvestmentRecProductSpecification(investmentInitId, sbu);
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
                                     ApprovalAuthorityName = aprAuthority.ApprovalAuthorityName
                                 }).ToList();

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
