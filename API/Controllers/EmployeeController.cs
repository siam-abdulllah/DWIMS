using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericIdentityRepository<AppUser> _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public EmployeeController(IGenericRepository<Employee> employeeRepo, IGenericIdentityRepository<AppUser> userRepo, UserManager<AppUser> userManager,
        IMapper mapper)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            _userRepo = userRepo;
            _userManager = userManager;
        }
        [HttpGet("employeesForConfig")]
        public async Task<IReadOnlyList<Employee>> GetEmployeesForConfig()
        {
            try
            {
                var employee = await _employeeRepo.ListAllAsync();
                return employee;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("marketForGroup")]
        public async Task<IReadOnlyList<MarketDto>> GetMarketForGroup()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var market = (from r in data
                              orderby r.MarketName
                              select new MarketDto
                              {
                                  MarketCode = r.MarketCode.Trim(),
                                  MarketName = r.MarketName.Trim(),
                                  SBU = r.SBU.Trim()
                              }
                              ).Distinct().ToList();
                //var mappedMarket = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<MarketDto>>(market);
                return market;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getSBU")]
        public async Task<IReadOnlyList<SBUDto>> GetSBU()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                List<Employee> emp = data.GroupBy(p => p.SBU)
                                 .Select(g => g.First())
                                 .ToList();
                var sbu = (from r in emp
                           orderby r.SBU
                           select new SBUDto
                           {
                               SBUName = r.SBU.Trim()
                           }
                              ).ToList();

                return sbu;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("employeeValidateById/{employeeSAPCode}")]
        public async Task<IReadOnlyList<Employee>> GetEmployeeValidateById(string employeeSAPCode)
        {
            try
            {
                var employeeData = await _employeeRepo.ListAllAsync();
                var data = (from e in employeeData
                            where e.EmployeeSAPCode == employeeSAPCode
                            orderby e.EmployeeName
                            select new Employee
                            {
                                Id = e.Id,
                                EmployeeSAPCode = e.EmployeeSAPCode,
                                EmployeeName = e.EmployeeName,
                                DepartmentName = e.DepartmentName,
                                DesignationName = e.DesignationName,
                                Phone = e.Phone,
                                Email = e.Email,
                                MarketName = e.MarketName,
                                RegionName = e.RegionName,
                                ZoneName = e.ZoneName,
                                TerritoryName = e.TerritoryName,
                                DivisionName = e.DivisionName,
                                SBU = e.SBU
                            }
                              ).Distinct().ToList();
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getEmployeeSbuById/{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployeeSbuById(int employeeId)
        {
            try
            {
                var data = await _employeeRepo.GetByIdAsync(employeeId);
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("employeeForApproval")]
        public async Task<IReadOnlyList<RegApprovalDto>> GetEmployeeForApproval()
        {
            try
            {
                var employeeData = await _employeeRepo.ListAllAsync();
                var userData = await _userRepo.ListAllAsync();
                var data = (from e in employeeData
                            join u in userData on e.EmployeeSAPCode equals u.EmployeeSAPCode
                            where u.EmailConfirmed == false
                            orderby e.EmployeeName
                            select new RegApprovalDto
                            {
                                UserId = u.Id,
                                EmployeeId = e.Id,
                                EmployeeSAPCode = e.EmployeeSAPCode,
                                EmployeeName = e.EmployeeName,
                                DepartmentName = e.DepartmentName,
                                DesignationName = e.DesignationName,
                                Phone = e.Phone,
                                Email = e.Email,
                                MarketName = e.MarketName,
                                RegionName = e.RegionName,
                                ZoneName = e.ZoneName,
                                TerritoryName = e.TerritoryName,
                                DivisionName = e.DivisionName,
                                SBU = e.SBU,
                                ApprovalStatus = u.EmailConfirmed == true ? "Approved" : "Not Approved"
                            }
                              ).Distinct().ToList();

                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("employeeApproved")]
        public async Task<IReadOnlyList<RegApprovalDto>> GetEmployeeForApproved()
        {
            try
            {
                var employeeData = await _employeeRepo.ListAllAsync();
                var userData = await _userRepo.ListAllAsync();
                var data = (from e in employeeData
                            join u in userData on e.EmployeeSAPCode equals u.EmployeeSAPCode
                            where u.EmailConfirmed == true
                            orderby e.EmployeeName
                            select new RegApprovalDto
                            {
                                UserId = u.Id,
                                EmployeeId = e.Id,
                                EmployeeSAPCode = e.EmployeeSAPCode,
                                EmployeeName = e.EmployeeName,
                                DepartmentName = e.DepartmentName,
                                DesignationName = e.DesignationName,
                                Phone = e.Phone,
                                Email = e.Email,
                                MarketName = e.MarketName,
                                RegionName = e.RegionName,
                                ZoneName = e.ZoneName,
                                TerritoryName = e.TerritoryName,
                                DivisionName = e.DivisionName,
                                SBU = e.SBU,
                                ApprovalStatus = u.EmailConfirmed == true ? "Approved" : "Not Approved"
                            }
                              ).Distinct().ToList();

                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetMarket")]
        public async Task<IReadOnlyList<MarketLocDto>> GetMarketList()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var market = (from r in data
                              orderby r.MarketName
                              select new MarketLocDto
                              {
                                  MarketCode = r.MarketCode.Trim(),
                                  MarketName = r.MarketName.Trim(),
                              }
                              ).Distinct().ToList();
                //var mappedMarket = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<MarketDto>>(market);
                return market;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("GetTerritory")]
        public async Task<IReadOnlyList<TerrirotyDto>> GetTerritoryList()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var territory = (from r in data
                              orderby r.MarketName
                              select new TerrirotyDto
                              {
                                  TerritoryCode = r.TerritoryCode.Trim(),
                                  TerritoryName = r.TerritoryName.Trim(),
                              }
                              ).Distinct().ToList();
                //var mappedMarket = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<MarketDto>>(market);
                return territory;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("GetZone")]
        public async Task<IReadOnlyList<ZoneDto>> GetZoneList()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var zone = (from r in data
                              orderby r.MarketName
                              select new ZoneDto
                              {
                                  ZoneCode = r.ZoneCode.Trim(),
                                  ZoneName = r.ZoneName.Trim(),
                              }
                              ).Distinct().ToList();
                //var mappedMarket = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<MarketDto>>(market);
                return zone;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetDivision")]
        public async Task<IReadOnlyList<DivisionDto>> GetDivisionList()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var division = (from r in data
                              orderby r.MarketName
                              select new DivisionDto
                              {
                                  DivisionCode = r.DivisionCode.Trim(),
                                  DivisionName = r.DivisionName.Trim(),
                              }
                              ).Distinct().ToList();
                //var mappedMarket = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<MarketDto>>(market);
                return division;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetRegion")]
        public async Task<IReadOnlyList<RegionDto>> GetRegionList()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var region = (from r in data
                              orderby r.RegionName
                              select new RegionDto
                              {
                                  RegionCode = r.RegionCode.Trim(),
                                  RegionName = r.RegionName.Trim(),
                              }
                              ).Distinct().ToList();
                //var mappedMarket = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<MarketDto>>(market);
                return region;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
