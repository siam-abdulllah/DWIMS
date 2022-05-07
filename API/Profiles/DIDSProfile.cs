using API.Dtos;
using AutoMapper;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class DIDSProfile : Profile
    {
        public DIDSProfile()
        {

            CreateMap<InvestmentRapidDto, InvestmentRapid>()
                .ReverseMap();
        }
    }
}
