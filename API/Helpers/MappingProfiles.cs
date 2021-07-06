using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();         
       
            CreateMap<IdentityRole, RoleDto>().ReverseMap();  
             CreateMap<AppUser, UsersToReturnDto>().ReverseMap();  
            CreateMap<AppUser, UserDto>().ReverseMap();  

            CreateMap<PostComments, CommentToReturnDto>().ReverseMap();
            CreateMap<Post, PostToReturnDto>()
            .ForMember(d => d.Comments, o => o.MapFrom(s => s.PostComments))
            .ReverseMap();
            CreateMap<Donation, DonationToReturnDto>().ReverseMap();
            CreateMap<SubCampaign, SubCampaignToReturnDto>().ReverseMap();
            CreateMap<ApprovalAuthority, ApprovalAuthorityToReturnDto>().ReverseMap();
            CreateMap<ApprAuthConfig, ApprAuthConfigDto>().ReverseMap();
            CreateMap<Employee, AuthEmployeeDto>().ReverseMap();
            CreateMap<ApprovalTimeLimit, ApprovalTimeLimitDto>().ReverseMap();
            CreateMap<ApprovalCeiling, ApprovalCeilingDto>().ReverseMap();
            CreateMap<ApprovalAuthority, ApprovalAuthorityToReturnDto>().ReverseMap();
            CreateMap<InvestmentType, InvestmentTypeDto>().ReverseMap();
            CreateMap<SBU, SBUDto>().ReverseMap();
            CreateMap<SBUWiseBudget, SBUWiseBudgetDto>().ReverseMap();
        }
    }
}