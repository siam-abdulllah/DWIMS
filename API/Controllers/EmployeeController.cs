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
    public class EmployeeController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IMapper _mapper;
        public EmployeeController(IGenericRepository<Employee> employeeRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
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
                var sbu = (from r in data
                           orderby r.SBU
                           select new SBUDto
                           {
                               SBUName = r.SBU.Trim()
                           }
                              ).Distinct().ToList();
                return sbu;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpGet("employeeValidateById/{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployeeValidateById(int employeeId)
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



    }
}
