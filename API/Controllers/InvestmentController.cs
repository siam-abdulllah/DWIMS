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
        private readonly IGenericRepository<InvestmentInit> _invRepo;
        private readonly IMapper _mapper;

        public InvestmentController(IGenericRepository<InvestmentInit> invRepo,
       IMapper mapper)
        {
            _mapper = mapper;
            _invRepo = invRepo;
        }
    }
}
