using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Core.Specifications;

namespace API.Controllers
{
    public class EmpSbuMapController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<EmpSbuMapping> _empSbuMappingRepo;
        private readonly IGenericIdentityRepository<AppUser> _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly StoreContext _db;
        private readonly IGenericRepository<ApprAuthConfig> _apprAuthConfigRepo;
        public EmpSbuMapController(IGenericRepository<Employee> employeeRepo, IGenericIdentityRepository<AppUser> userRepo, UserManager<AppUser> userManager, StoreContext db,
        IMapper mapper, IGenericRepository<EmpSbuMapping> empSbuMappingRepo,
            IGenericRepository<ApprAuthConfig> apprAuthConfigRepo)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            _userRepo = userRepo;
            _userManager = userManager;
            _db = db;
            _empSbuMappingRepo = empSbuMappingRepo;
            _apprAuthConfigRepo = apprAuthConfigRepo;
        }
      
        [HttpPost("SaveEmpSbuMapping")]
        public async Task<ActionResult<EmpSbuMappingDto>> SaveEmpSbuMapping(EmpSbuMappingDto empSbuMappingDto)
        {
            var qry = string.Format(@"select * from EmpSbuMapping where EmployeeId = {0}
                                    and SBU = {1} and Serial = {1} and DataStatus=1", 
                                    empSbuMappingDto.EmployeeId, empSbuMappingDto.SBU, empSbuMappingDto.Serial);

            List<EmpSbuMapping> existingRecord = _db.ExecSQL<EmpSbuMapping>(qry).ToList();
            var empMapping = new EmpSbuMapping();
            var spec = new ApprAuthConfigSpecification(empSbuMappingDto.EmployeeId, "A");
            ApprAuthConfig authConfig = await _apprAuthConfigRepo.GetEntityWithSpec(spec);
            if (existingRecord == null || existingRecord.Count == 0)
            {
                #region Insert New Budget
                empMapping = new EmpSbuMapping
                {
                    CompId = empSbuMappingDto.CompId,
                    SBU = empSbuMappingDto.SBU,
                    SBUName = empSbuMappingDto.SBUName,
                    DeptId = empSbuMappingDto.DeptId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now,
                    DataStatus = 1,
                    EmployeeId = empSbuMappingDto.EmployeeId,
                    ApprovalAuthorityId = authConfig.ApprovalAuthorityId,
                    TagCode = empSbuMappingDto.SBUName + empSbuMappingDto.Serial,
                    Serial = empSbuMappingDto.Serial,

                };

                _empSbuMappingRepo.Add(empMapping);
                _empSbuMappingRepo.Savechange();
                #endregion
          
            }
            return new EmpSbuMappingDto
            {
                Id = empMapping.Id,
                DeptId = empMapping.DeptId,
                EmployeeId = empMapping.EmployeeId,
                SBU = empMapping.SBU,
                CompId = empMapping.CompId

            };
        }
        [HttpPost("removeEmpSbuMapping")]
        public async Task<IActionResult> RemoveEmpSbuMappingAsync(EmpSbuMappingDto empSbuMappingDto)
        {
            #region Updating existing Entry
            try
            {
                var empSbuMapping = await _empSbuMappingRepo.GetByIdAsync(empSbuMappingDto.Id);
                if (empSbuMapping != null)
                {
                    empSbuMapping.DataStatus = 0;
                    _empSbuMappingRepo.Update(empSbuMapping);
                    _empSbuMappingRepo.Savechange();
                    return Ok("Succsessfuly Deleted!!!");
                }
                return NotFound();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            #endregion
     
        }
       
        [HttpGet]
        [Route("getEmpSbuMappingList/{deptID}/{sbu}")]
        public IReadOnlyList<EmpSbuMappingVM> GetEmpSbuMappingList(int deptID,string sbu)
        {
            try
            {
                 var qry = string.Format(@" select em.*,emp.EmployeeName from EmpSbuMapping em
                                            left join employee emp on emp.Id = em.EmployeeId
                                            where em.DeptId={0} and em.SBU={1} and em.DataStatus=1", deptID,sbu);

                List<EmpSbuMappingVM> dsResult = _db.ExecSQL<EmpSbuMappingVM>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } [HttpGet]
        [Route("getEmpSbuMappingListByEmp/{empID}/{authId}/{sbu}/{deptID}")]
        public IReadOnlyList<EmpSbuMappingVM> getEmpSbuMappingListByEmp(int empID,int authId, string sbu, int deptID)
        {
            try
            {
                 var qry = string.Format(@" select em.*,emp.EmployeeName from EmpSbuMapping em
                                            left join employee emp on emp.Id = em.EmployeeId
                                            where em.DeptId={0} and em.SBU={1} and em.ApprovalAuthorityId={2} and em.DataStatus=1", deptID,sbu, deptID);

                List<EmpSbuMappingVM> dsResult = _db.ExecSQL<EmpSbuMappingVM>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
       
        [HttpGet]
        [Route("getEmpSbuMappingListBySbu/{sbu}")]
        public IReadOnlyList<EmpSbuMappingVM> GetEmpSbuMappingListBySbu(string sbu)
        {
            try
            {
                var qry = string.Format(@" select em.*,emp.EmployeeName from EmpSbuMapping em
                                            left join employee emp on emp.Id = em.EmployeeId
                                            where em.SBU={0} and em.DataStatus=1", sbu);

                List<EmpSbuMappingVM> dsResult = _db.ExecSQL<EmpSbuMappingVM>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("getEmpSbuMappingListByDept/{deptID}")]
        public IReadOnlyList<EmpSbuMappingVM> GetEmpSbuMappingListByDept(int deptID)
        {
            try
            {
                 var qry = string.Format(@" select em.*,emp.EmployeeName from EmpSbuMapping em
                                            left join employee emp on emp.Id = em.EmployeeId
                                            where em.DeptId={0} and em.DataStatus=1", deptID);

                List<EmpSbuMappingVM> dsResult = _db.ExecSQL<EmpSbuMappingVM>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
