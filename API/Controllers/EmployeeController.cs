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

namespace API.Controllers
{
    public class EmployeeController : BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;
        private readonly IGenericRepository<EmpSbuMapping> _empSbuMappingRepo;
        private readonly IGenericIdentityRepository<AppUser> _userRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly StoreContext _db;
        public EmployeeController(IGenericRepository<Employee> employeeRepo, IGenericIdentityRepository<AppUser> userRepo, UserManager<AppUser> userManager, StoreContext db,
        IMapper mapper, IGenericRepository<EmpSbuMapping> empSbuMappingRepo)
        {
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            _userRepo = userRepo;
            _userManager = userManager;
            _db = db;
            _empSbuMappingRepo = empSbuMappingRepo;
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
        [HttpGet("employeesForSbuMapping")]
        public async Task<IReadOnlyList<Employee>> GetEmployeesForSbuMapping()
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
        [HttpGet("getEmployeesCampaign")]
        public async Task<IReadOnlyList<Employee>> GetgetEmployeesCampaign()
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
  
        [HttpGet("marketForGroup/{empId}")]
        public async Task<IReadOnlyList<MarketDto>> GetMarketForGroup(int empId)
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var market = (from r in data
                              where r.MarketCode != null && r.Id != empId && r.MarketCode!="" && r.DataStatus==1
                              orderby r.MarketName
                              select new MarketDto
                              {
                                  MarketCode = r.MarketCode,
                                  MarketName = r.MarketName,
                                  SBU = r.SBU,
                                  SBUName = r.SBUName,
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
                           where r.SBU !=null
                           orderby r.SBUName
                           select new SBUDto
                           {
                               SBUCode = r.SBU,
                               SBUName = r.SBUName
                           }
                              ).ToList();

                return sbu;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } 
    
        [HttpGet("depotForInvestment")]
        public async Task<IReadOnlyList<DepotDto>> GetDepot()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                List<Employee> emp = data.GroupBy(p => p.DepotCode)
                                 .Select(g => g.First()).Where(x=>x.DepotCode!=null)
                                 .ToList();
                var depot = (from r in emp
                           orderby r.SBUName
                           select new DepotDto
                           {
                               DepotCode = r.DepotCode,
                               DepotName = r.DepotName
                           }
                              ).OrderBy(x=>x.DepotName).Distinct().ToList();

                return depot;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // [HttpGet("employeeValidateById/{employeeId}")]
        // public async Task<ActionResult<Employee>> GetEmployeeValidateById(int employeeId)
        // {
        //     try
        //     {
        //         var data = await _employeeRepo.GetByIdAsync(employeeId);
        //         return data;
        //     }
        //     catch (System.Exception ex)
        //     {
        //         throw ex;
        //     }
        // }



        [HttpGet("getEmpDepot/{Id}")]
        public object GetEmpDepot(int id)
        {
            try
            {
               var emp= _db.Employee.Where(x=> x.Id == id).FirstOrDefault();
                return emp;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        } 

        [HttpGet("employeeValidateById/{employeeSapCode}")]
        public async Task<IReadOnlyList<Employee>> GetEmployeeValidateById(string employeeSapCode)
        {
            try
            {
                var empData = await _employeeRepo.ListAllAsync();
                var data = (from e in empData
                              where e.EmployeeSAPCode == employeeSapCode
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
                                  MarketGroupName = e.MarketGroupName,
                                  SBU = e.SBU,

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
                            join u in userData on e.Id equals u.EmployeeId
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
                                MarketGroupName = e.MarketGroupName,
                                SBU = e.SBU,
                                SBUName = e.SBUName,
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
                            join u in userData on e.Id equals u.EmployeeId
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
                                MarketGroupName = e.MarketGroupName,
                                SBU = e.SBU,
                                SBUName = e.SBUName,
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
                              where r.MarketName != null
                              orderby r.MarketName
                              select new MarketLocDto
                              {
                                  MarketCode = r.MarketCode,
                                  MarketName = r.MarketName,
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
                                 where r.TerritoryName != null
                                 orderby r.TerritoryName
                                 select new TerrirotyDto
                              {
                                  TerritoryCode = r.TerritoryCode,
                                  TerritoryName = r.TerritoryName,
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
                            where r.ZoneName != null
                            orderby r.ZoneName
                            select new ZoneDto
                              {
                                  ZoneCode = r.ZoneCode,
                                  ZoneName = r.ZoneName,
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

        //[HttpGet("GetDivision")]
        //public async Task<IReadOnlyList<DivisionDto>> GetDivisionList()
        //{
        //    try
        //    {
        //        var data = await _employeeRepo.ListAllAsync();
        //        //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
        //        var division = (from r in data
        //                        where r.DivisionName != null
        //                        orderby r.DivisionName
        //                        select new DivisionDto
        //                      {
        //                          DivisionCode = r.DivisionCode,
        //                          DivisionName = r.DivisionName,
        //                      }
        //                      ).Distinct().ToList();
        //        //var mappedMarket = _mapper.Map<IReadOnlyList<Employee>, IReadOnlyList<MarketDto>>(market);
        //        return division;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpGet("getRegion")]
        public async Task<IReadOnlyList<RegionDto>> GetRegionList()
        {
            try
            {
                var data = await _employeeRepo.ListAllAsync();
                //var market = data.GroupBy(p => p.MarketCode).Select(g => g.First()).ToList();
                var region = (from r in data
                              where r.RegionName != null
                              orderby r.RegionName
                              select new RegionDto
                              {
                                  RegionCode = r.RegionCode,
                                  RegionName = r.RegionName,
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

        [HttpPost("SaveEmpSbuMapping")]
        public ActionResult<EmpSbuMappingDto> SaveEmpSbuMapping(EmpSbuMappingDto empSbuMappingDto)
        {
            //#region Updating existing Entry
            var qry = string.Format(@"select * from EmpSbuMapping where EmployeeId = {0}
                                    and SBU = {1} and Serial = {1} and DataStatus=1", 
                                    empSbuMappingDto.EmployeeId, empSbuMappingDto.SBU, empSbuMappingDto.Serial);

            List<EmpSbuMapping> existingRecord = _db.ExecSQL<EmpSbuMapping>(qry).ToList();
            var empMapping = new EmpSbuMapping();
            if (existingRecord == null || existingRecord.Count == 0)
            {
                #region Insert New Budget
                //string bgtYear = setBgtDto.Year.ToString("yyyy");
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
