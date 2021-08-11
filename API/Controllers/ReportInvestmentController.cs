using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ReportInvestmentController : BaseApiController
    {
        private readonly IGenericRepository<ReportInvestmentInfo> _investRepo;
        private readonly IMapper _mapper;
        public ReportInvestmentController(IGenericRepository<ReportInvestmentInfo> investRepo, IMapper mapper)
        {
            _mapper = mapper;
            _investRepo = investRepo;
        }
    }
}
