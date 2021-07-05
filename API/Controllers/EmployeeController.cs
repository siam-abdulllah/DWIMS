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
        
    }
}
