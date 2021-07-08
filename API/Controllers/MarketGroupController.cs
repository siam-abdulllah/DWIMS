using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    
    public class MarketGroupController : BaseApiController
    {
        private readonly IGenericRepository<MarketGroupMst> _marketGroupMstRepo;
        private readonly IMapper _mapper;
        public MarketGroupController(IGenericRepository<MarketGroupMst> bcdsRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _marketGroupMstRepo = bcdsRepo;
        }

       
        
    }
}
