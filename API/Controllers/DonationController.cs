using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class DonationController : BaseApiController
    {
        private readonly IGenericRepository<Donation> _donationRepo;
        private readonly IMapper _mapper;
        public DonationController(IGenericRepository<Donation> donationRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _donationRepo = donationRepo;
        }

        [HttpPost("insert")]
        public ActionResult<DonationToReturnDto> InsertDonation(DonationToReturnDto donationToReturnDto)
        {
            var donation = new Donation
            {
                DonationTypeName = donationToReturnDto.DonationTypeName,
                DonationShortName = donationToReturnDto.DonationShortName,
                Remarks = donationToReturnDto.Remarks,
                Status = donationToReturnDto.Status,
                SetOn = DateTimeOffset.Now
        };
            _donationRepo.Add(donation);
            _donationRepo.Savechange();

            //if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));
            //var userEntity = await _userManager.FindByEmailAsync(user.Email);
            //try
            //{
            //    string[] roles = setRegDto.RoleForm.UserRoles
            //              .Select(ob => ob.Name).ToArray();
            //    var roleObj = await _userManager.AddToRolesAsync(userEntity, roles);
            //    if (!roleObj.Succeeded) return BadRequest(new ApiResponse(400, "User Role Set Faild."));
            //}
            //catch (Exception ex)
            //{
            //    await _userManager.DeleteAsync(user);
            //}

            return new DonationToReturnDto
            {
                Id = donationToReturnDto.Id,
                DonationTypeName = donationToReturnDto.DonationTypeName,
                Remarks = donationToReturnDto.Remarks
            };
        }
     
        [HttpPost("update")]
        public async Task<ActionResult<DonationToReturnDto>> UpdateDonationAsync(DonationToReturnDto donationToReturnDto)
        {
            // var user =  _donationRepo.GetByIdAsync(donationToReturnDto.Id);
            // if (user == null) return Unauthorized(new ApiResponse(401));
            var donations = await _donationRepo.GetByIdAsync(donationToReturnDto.Id);
            var donation = new Donation
            {
                Id = donationToReturnDto.Id,
                DonationTypeName = donationToReturnDto.DonationTypeName,
                DonationShortName = donationToReturnDto.DonationShortName,
                Remarks = donationToReturnDto.Remarks,
                Status = donationToReturnDto.Status,
                SetOn = donations.SetOn,
                ModifiedOn = DateTimeOffset.Now

        };
            _donationRepo.Update(donation);
            _donationRepo.Savechange();

            //if (!userObj.Succeeded) return BadRequest(new ApiResponse(400));
            //var userEntity = await _userManager.FindByEmailAsync(user.Email);
            //try
            //{
            //    string[] roles = setRegDto.RoleForm.UserRoles
            //              .Select(ob => ob.Name).ToArray();
            //    var roleObj = await _userManager.AddToRolesAsync(userEntity, roles);
            //    if (!roleObj.Succeeded) return BadRequest(new ApiResponse(400, "User Role Set Faild."));
            //}
            //catch (Exception ex)
            //{
            //    await _userManager.DeleteAsync(user);
            //}

            return new DonationToReturnDto
            {
                Id = donationToReturnDto.Id,
                DonationTypeName = donationToReturnDto.DonationTypeName,
                Remarks = donationToReturnDto.Remarks,
                Status = donationToReturnDto.Status
            };
        }

        [HttpGet("donations")]
        public async Task<ActionResult<Pagination<DonationToReturnDto>>> GetDonations(
          [FromQuery] DonationSpecParams donationParrams)
        {
            try
            {
                var spec = new DonationWithCommentsSpecification(donationParrams);

                var countSpec = new DonationWithFiltersForCountSpecificication(donationParrams);

                var totalItems = await _donationRepo.CountAsync(countSpec);

                var donation = await _donationRepo.ListAsync(spec);

                var data = _mapper
                    .Map<IReadOnlyList<Donation>, IReadOnlyList<DonationToReturnDto>>(donation);

                return Ok(new Pagination<DonationToReturnDto>(donationParrams.PageIndex, donationParrams.PageSize, totalItems, data));
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [HttpGet("donationsForInvestment")]
        public async Task<IReadOnlyList<Donation>> GetEmployeesForConfig()
        {
            try
            {
                var data = new DonationWithCommentsSpecification("Active");
                var donation = await _donationRepo.ListAsync(data);
                return donation;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}