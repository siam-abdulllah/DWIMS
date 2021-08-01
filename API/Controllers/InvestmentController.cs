using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class InvestmentController : BaseApiController
    {
        private readonly IGenericRepository<InvestmentInit> _investmentInitRepo;
        private readonly IMapper _mapper;

        public InvestmentController(IGenericRepository<InvestmentInit> investmentInitRepo,
       IMapper mapper)
        {
            _mapper = mapper;
            _investmentInitRepo = investmentInitRepo;
        }

        [HttpPost("insertInit")]
        public async Task<InvestmentInitDto> InsertInvestmentInit(InvestmentInitDto investmentInitDto)
        {
            var data =  await _investmentInitRepo.ListAllAsync();
            var investmentInitLastId = (from r in data
                          orderby r.Id
                          select new InvestmentInitDto
                          {
                              Id = r.Id,
                              
                          }
                              ).Last();
            var investmentInit = new InvestmentInit
            {
                //ReferenceNo = investmentInitDto.ReferenceNo,
                ReferenceNo = DateTimeOffset.Now.ToString("yyyyMM")+ (investmentInitLastId.Id+1).ToString("00000"),
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

        [HttpPost("updateInit")]
        public ActionResult<InvestmentInitDto> UpdateInvestmentInit(InvestmentInitDto investmentInitDto)
        {
            // var user =  _approvalAuthorityRepo.GetByIdAsync(ApprovalAuthorityToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
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

    }
}
