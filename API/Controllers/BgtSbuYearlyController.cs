using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{

    public class BgtSbuYearlyController : BaseApiController
    {
        private readonly IGenericRepository<BgtSBUTotal> _bgtSbuRepo;
        private readonly IGenericRepository<BgtEmployee> _bgtEmployee;
        private readonly IGenericRepository<SBU> _sbuRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        public BgtSbuYearlyController(IGenericRepository<BgtSBUTotal> bgtSbuRepo, IGenericRepository<BgtEmployee> bgtEmployee, IGenericRepository<SBU> sbuRepo,
        IMapper mapper, StoreContext dbContext)
        {
            _mapper = mapper;
            _bgtSbuRepo = bgtSbuRepo;
            _dbContext = dbContext;
            _sbuRepo = sbuRepo;
            _bgtEmployee = bgtEmployee;
        }



        [HttpGet("getAllSbuBgtList/{deptId}/{compId}/{year}")]
        public List<SBUVM> GetAllSbuBgtListAsync(int deptId, int compId, int year)
        {
            List<SBUVM> dsResult = new List<SBUVM>();
            try
            {
               //string statusQuery = "";
                string ProposeFor = "";
                string allocatedQry = "";
                if (deptId == 1)
                {
                    ProposeFor = "'Others','Sales'";
                    allocatedQry = "(select SUM(Amount) from BgtEmployee where DeptId =1 and SBU = sb.SBUCode and DataStatus = 1) TotalAllowcated ";
                }
                else if (deptId == 2)
                {
                    ProposeFor = "'BrandCampaign','PMD'";
                    allocatedQry = "(SELECT ((select ISNULL(SUM(ISNULL(Amount,0)),0) from BgtEmployee where DeptId =2 and SBU = sb.SBUCode and DataStatus = 1)" +
                                    "+(select ISNULL(SUM(ISNULL(Amount,0)),0) from BgtCampaign where DeptId =2 and SBU = sb.SBUCode and DataStatus = 1))) TotalAllowcated";
                }
                string qry = "";

                //qry = string.Format(@"SELECT sb.*, bs.SBUAmount
                //                    ,bs.Id BgtSbuId,(
                //                    SELECT ISNULL(Round(SUM(ApprovedAmount), 0), 0)
                //                    FROM InvestmentDetailTracker e
                //                    INNER JOIN InvestmentInit c ON c.Id = e.InvestmentInitId
                //                    WHERE c.SBU = SB.SBUCode
                //                    AND C.ProposeFor IN ({3})
                //                    AND e.Year = {0}
                //                    ) Expense,
                //                    (select SUM(Amount) from BgtEmployee where DeptId ={1} and SBU = sb.SBUCode and DataStatus = 1) TotalAllowcated
                //                    FROM SBU sb
                //                    LEFT JOIN BgtSBUTotal bs ON bs.SBU = sb.SbuCode
                //                    AND bs.DeptId = {1}
                //                    AND bs.DataStatus = 1
                //                    AND bs.CompId = {2}
                //                    AND bs.Year = {0}", year, deptId, compId, ProposeFor); 
                qry = string.Format(@"SELECT sb.*, bs.SBUAmount
                                    ,bs.Id BgtSbuId,(
                                    SELECT ISNULL(Round(SUM(ApprovedAmount), 0), 0)
                                    FROM InvestmentDetailTracker e
                                    INNER JOIN InvestmentInit c ON c.Id = e.InvestmentInitId
                                    WHERE c.SBU = SB.SBUCode
                                    AND C.ProposeFor IN ({3})
                                    AND e.Year = {0}
                                    ) Expense,
                                    {4}
                                    FROM SBU sb
                                    LEFT JOIN BgtSBUTotal bs ON bs.SBU = sb.SbuCode
                                    AND bs.DeptId = {1}
                                    AND bs.DataStatus = 1
                                    AND bs.CompId = {2}
                                    AND bs.Year = {0}", year, deptId, compId, ProposeFor, allocatedQry);



                dsResult = _dbContext.ExecSQL<SBUVM>(qry).ToList();
                if (dsResult == null)
                {
                    dsResult = new List<SBUVM>();
                }

                return dsResult;
            }
            catch (System.Exception ex)
            {
                return dsResult;
            }
        }
        [HttpGet("getLayerCountByDept/{deptId}/{compId}/{year}")]
        public int GetLayerCountByDept(int deptId, int compId, int year)
        {

            try
            {
                string qry = "";
                string filterQry = "";
                if (deptId == 1)
                {
                    filterQry = " where aa.Id not in (1,2,9,10,11,13) and aa.DeptId=1";
                }
                else
                {
                    filterQry = " where aa.Id not in (15) and aa.DeptId = 2";

                }


                qry = string.Format(@"SELECT  CAST('1'AS INT) as Id,  1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, COUNT(*) COUNT FROM ApprovalAuthority aa {0}", filterQry);



                var dsResult = _dbContext.ExecSQL<CountInt>(qry).ToList();


                return dsResult[0].Count;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }

        [HttpGet("getAllPipelineExpenseList/{deptId}/{compId}/{year}")]
        public List<PipeLineExpense> GetAllPipeLineExpenseListAsync(int deptId, int compId, int year)
        {
            try
            {
                string statusQuery = "";
                string ProposeFor = "";
                if (deptId == 1)
                {
                    ProposeFor = "Others";
                }
                else if (deptId == 2)
                {
                    ProposeFor = "BrandCampaign";
                }
                string qry = "";

                qry = string.Format(@"SELECT CAST(ROW_NUMBER() OVER (ORDER BY SBUCode) AS INT)  AS Id ,1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,Pipeline,SBUName,SBUCode FROM (
                                        SELECT Round(SUM(ProposedAmount),0) Pipeline ,                               
                                        sb.SBUName                                  
                                        ,sb.SBUCode                               
                                        FROM InvestmentRec A                                    
                                        INNER JOIN InvestmentRecComment B ON A.InvestmentInitId = B.InvestmentInitId                        
                                        AND A.EmployeeId = B.EmployeeId                                   
                                        INNER JOIN InvestmentInit C ON a.InvestmentInitId = C.Id                         
                                        INNER JOIN SBU sb ON C.SBU = sb.SBUCode                           
                                        WHERE C.DataStatus = 1                                  
                                        AND C.ProposeFor='{0}'                                  
                                        AND Year(A.ToDate)={1}                                  
                                        AND RecStatus NOT IN ('Approved' ,'Not Approved','Cancelled') AND A.Id IN (                                
                                        SELECT MAX(ir.Id) AS Expr1                                 
                                        FROM dbo.InvestmentRec AS ir                                   
                                        WHERE (ir.InvestmentInitId = c.Id) )                                
                                        GROUP BY sb.SBUName                                  
                                        ,sb.SBUCode) A", ProposeFor, year);


                var results = _dbContext.PipeLineExpense.FromSqlRaw(qry).ToList();
                List<PipeLineExpense> dsResult = _dbContext.ExecSQL<PipeLineExpense>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("GetAppAuthDetails/{sbuCode}/{deptId}")]
        public List<AppAuthDetailsDto> GetAppAuthDetails(string sbuCode, int deptId)
        {
            try
            {
                string qry = "";
                string filterQry = "";
                if (deptId == 1)
                {
                    //filterQry = " where aa.Id in (3,4,5,6,7,12)";
                    filterQry = " where aa.Id not in (1,2,9,10,11,13) and aa.DeptId=1";
                }
                else
                {
                    //filterQry = " where aa.Id = 8";
                    filterQry = " where aa.Id not in (15) and aa.DeptId = 2";
                }

                qry = string.Format(@"select  CAST(ROW_NUMBER() OVER (ORDER BY Priority) AS INT)  AS Id,aa.Id as AuthId,1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,aa.Priority,
                                     aa.Remarks Authority,0 as Expense,0 as Amount,0 as NewAmount, (select COUNT(*) from ApprAuthConfig ac
                                    left join EmpSbuMapping emp on emp.EmployeeId = ac.EmployeeId
                                    where ac.ApprovalAuthorityId = aa.Id AND ac.Status='A'  and emp.SBU = '{0}' and emp.DataStatus = 1) NoOfEmployee
                                    ,CASE  WHEN aa.Id = 3 THEN ( SELECT COUNT(*) FROM RegionInfo r WHERE r.SBU = '{0}' AND r.DataStatus = 1 )
		                            WHEN aa.Id = 5 THEN (  SELECT COUNT(*) FROM ZoneInfo r WHERE r.SBU = '{0}' AND r.DataStatus = 1 )
		                            ELSE ( SELECT COUNT(*) FROM ApprAuthConfig ac LEFT JOIN EmpSbuMapping emp ON emp.EmployeeId = ac.EmployeeId
				                    WHERE ac.ApprovalAuthorityId = aa.Id AND ac.Status='A' AND emp.SBU = '{0}' AND emp.DataStatus = 1 )
		                            END NoOfLoc
                                    from ApprovalAuthority aa {1}", sbuCode, filterQry);


                //var results = _dbContext.AppAuthDetails.FromSqlRaw(qry).ToList();
                List<AppAuthDetailsDto> dsResult = _dbContext.ExecSQL<AppAuthDetailsDto>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetCampaignDetails/{sbuCode}/{deptId}")]
        public List<CampaignBgtDetailsDto> GetCampaignDetails(string sbuCode, int deptId)
        {
            try
            {
                string qry = "";


                qry = string.Format(@"SELECT  CAST(ROW_NUMBER() OVER (ORDER BY A.SBU) AS INT)  AS Id,1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,
                                    A.SBU,COUNT(Distinct B.SubCampaignId) NoOfCamp,Year(GetDate()) Year,2 DeptId, 
                                    (SELECT ROUND(SUM(ISNULL(invd.ApprovedAmount,0)),0) FROM InvestmentDetailTracker invd INNER JOIN InvestmentInit invi ON invd.InvestmentInitId=invi.Id WHERE invi.SBU=A.SBU AND invi.ProposeFor='BrandCampaign' AND invd.Year=YEAR(GETDATE())) Expense,
                                    0 NewAmount,
                                    (SELECT Amount FROM BgtCampaign WHERE SBU=A.SBU AND YEAR=YEAR(GETDATE()) AND DeptId={0} AND CompId={1} AND DataStatus=1) Amount,
                                    SUM(ISNULL(B.Budget,0)) AllocatedBgt
                                     FROM  CampaignMst A INNER JOIN CampaignDtl B ON A.Id=B.MstId 
                                    WHERE A.SBU='{2}' AND YEAR(B.SubCampEndDate)=YEAR(GETDATE())
                                    GROUP BY A.SBU", deptId, 1000, sbuCode);


                //var results = _dbContext.AppAuthDetails.FromSqlRaw(qry).ToList();
                List<CampaignBgtDetailsDto> dsResult = _dbContext.ExecSQL<CampaignBgtDetailsDto>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("getYearlyBudget/{deptId}/{year}")]
        public BgtYearlyTotal GetBudgetYearly(int deptId, int year)
        {
            BgtYearlyTotal bgt = new BgtYearlyTotal();
            try
            {

                string qry = "";

                qry = string.Format(@" select * from BgtYearlyTotal where DataStatus=1 and DeptId={0} and year={1}", deptId, year);

                bgt = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList().FirstOrDefault();
                if (bgt == null)
                {
                    bgt = new BgtYearlyTotal();
                }
                return bgt;
            }
            catch (System.Exception ex)
            {
                return bgt;
            }
        }

        [HttpGet("getBudgetEmpForSbu/{sbu}/{deptId}/{year}/{compId}")]
        public List<BgtEmployeeVM> GetBudgetEmpForSbu(string sbu, int deptId, int year, int compId)
        {
            BgtYearlyTotal bgt = new BgtYearlyTotal();
            List<BgtEmployeeVM> bgtEmpList = new List<BgtEmployeeVM>();
            try
            {

                string qry = string.Format(@" select be.Id as BgtEmpId,be.[Id]
	                                        ,be.[DataStatus]
	                                        ,be.[SetOn]
	                                        ,be.[ModifiedOn]
	                                        ,be.[CompId]
	                                        ,be.[DeptId]
	                                        ,be.[Year]
	                                        ,be.[SBU]
	                                        ,be.[AuthId]
	                                        ,be.[Amount]
	                                        ,be.[PermEdit]
	                                        ,be.[PermView]
	                                        ,be.[PermAmt]
	                                        ,be.[PermDonation]
	                                        ,be.[Remarks]
	                                        ,be.[EnteredBy]
		                                        ,(SELECT COUNT(*)
		                                        FROM ApprAuthConfig ac
		                                        LEFT JOIN EmpSbuMapping emp ON emp.EmployeeId = ac.EmployeeId
		                                        WHERE ac.ApprovalAuthorityId = aa.Id
                                                    AND ac.Status='A' 
			                                        AND emp.SBU = '{0}'
			                                        AND emp.DataStatus = 1
		                                        ) NoOfEmployee,aa.Remarks Authority,aa.Priority
                                    ,CASE  WHEN aa.Id = 3 THEN ( SELECT COUNT(*) FROM RegionInfo r WHERE r.SBU = '{0}' AND r.DataStatus = 1 )
		                            WHEN aa.Id = 5 THEN (  SELECT COUNT(*) FROM ZoneInfo r WHERE r.SBU = '{0}' AND r.DataStatus = 1 )
		                            ELSE ( SELECT COUNT(*) FROM ApprAuthConfig ac LEFT JOIN EmpSbuMapping emp ON emp.EmployeeId = ac.EmployeeId
				                    WHERE ac.ApprovalAuthorityId = aa.Id AND ac.Status='A'  AND emp.SBU = '{0}' AND emp.DataStatus = 1 )
		                            END NoOfLoc from BgtEmployee be 
									left join ApprovalAuthority aa on aa.Id = be.AuthId
									where be.Sbu = '{0}' and  be.DeptId={1} and be.Year = {2} and be.compId={3} and be.DataStatus = 1", sbu, deptId, year, compId);

                bgtEmpList = _dbContext.ExecSQL<BgtEmployeeVM>(qry).ToList();
                return bgtEmpList;
            }
            catch (System.Exception ex)
            {
                return bgtEmpList;
            }
        }
        [HttpGet("getAllAuthExpenseList/{sbu}/{deptId}/{year}/{compId}")]
        public List<AuthExpense> GetAllAuthExpenseList(string sbu, int deptId, int year, int compId)
        {
            BgtYearlyTotal bgt = new BgtYearlyTotal();
            List<AuthExpense> bgtEmpList = new List<AuthExpense>();
            try
            {
                string ProposeFor = "";
                if (deptId == 1)
                {
                    ProposeFor = "'Others','Sales'";
                }
                else if (deptId == 2)
                {
                    //ProposeFor = "'BrandCampaign','PMD'";
                    ProposeFor = "'PMD'";
                }
                string qry = string.Format(@" SELECT   DISTINCT  CAST(ROW_NUMBER() OVER (ORDER BY  D.Remarks) AS INT)  AS Id,
                                                1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,
                                                ISNULL(Round(SUM(ApprovedAmount), 0), 0) Expense,D.Remarks
                                                FROM InvestmentDetailTracker A
                                                INNER JOIN InvestmentRecComment B ON A.InvestmentInitId = B.InvestmentInitId
                                                AND A.EmployeeId = B.EmployeeId
                                                INNER JOIN InvestmentInit F ON F.Id = A.InvestmentInitId
                                                --INNER JOIN ApprAuthConfig C ON A.EmployeeId = C.EmployeeId
                                                INNER JOIN ApprovalAuthority D ON B.Priority = D.Priority
                                                WHERE B.CompletionStatus = 1                                 
                                                AND F.ProposeFor IN ({4})  
                                                AND B.RecStatus = 'Approved'
                                                AND B.CompletionStatus = 1
                                                AND F.DataStatus = 1
                                                --AND C.STATUS = 'A'

                                                And F.SBU = '{0}'
                                                And D.DeptId = '{1}'
                                                And A.Year = '{2}'
                                                And D.CompId = '{3}'
                                                GROUP BY D.Remarks", sbu, deptId, year, compId, ProposeFor);

                var results = _dbContext.AuthExpense.FromSqlRaw(qry).ToList();
                List<AuthExpense> dsResult = _dbContext.ExecSQL<AuthExpense>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                return bgtEmpList;
            }
        }
        [HttpPost("saveAuthSbuDetails")]
        public async Task<ActionResult<BgtEmployeeDto>> SaveAuthSbuDetails(BgtEmployeeModel model)
        {
            List<BgtEmployee> bgtEmpList = new List<BgtEmployee>();
            BgtEmployee bgtEmp = new BgtEmployee();
            try
            {
                //string qry = string.Format(@" select * from BgtEmployee where  DeptId={0} and Sbu = '{1}' and Year = {2}", model.DeptId,model.SBUCode,model.Year);
                //bgtEmpList = _dbContext.BgtEmployee.FromSqlRaw(qry).ToList();
                if (model.bgtEmpList != null && model.bgtEmpList.Count > 0)
                {
                    foreach (var item in model.bgtEmpList)
                    {
                        if (item.BgtEmpId > 0)
                        {
                            bgtEmp = await _bgtEmployee.GetByIdAsync(item.BgtEmpId);

                            bgtEmp.ModifiedOn = DateTime.Now;
                            bgtEmp.DataStatus = 0;
                            _bgtEmployee.Update(bgtEmp);
                            _bgtEmployee.Savechange();
                        }

                    }
                }

                if (model.bgtEmpList != null && model.bgtEmpList.Count > 0)
                {
                    foreach (var item in model.bgtEmpList)
                    {
                        if (item.NoOfEmployee > 0)
                        {
                            bgtEmp = new BgtEmployee();
                            bgtEmp.Amount = item.NewAmount;
                            bgtEmp.DeptId = model.DeptId;
                            bgtEmp.CompId = model.CompId;
                            bgtEmp.Year = model.Year;
                            bgtEmp.SBU = model.SBUCode;
                            bgtEmp.AuthId = item.AuthId;
                            bgtEmp.NoOfEmployee = item.NoOfEmployee;
                            bgtEmp.ModifiedOn = DateTime.Now;
                            bgtEmp.EnteredBy = item.EnteredBy;
                            bgtEmp.PermView = item.PermView;
                            bgtEmp.PermEdit = item.PermEdit;
                            _bgtEmployee.Add(bgtEmp);
                            _bgtEmployee.Savechange();
                            var Amount = bgtEmp.Amount / bgtEmp.NoOfEmployee;
                            List<SqlParameter> parms = new List<SqlParameter>
                            {
                            new SqlParameter("@DeptId", bgtEmp.DeptId),
                            new SqlParameter("@Year", bgtEmp.Year),
                            new SqlParameter("@SBU ", bgtEmp.SBU),
                            new SqlParameter("@AuthId", bgtEmp.AuthId),
                            new SqlParameter("@Amount", Amount),
                            new SqlParameter("@PermView", bgtEmp.PermView),
                            new SqlParameter("@PermEdit", bgtEmp.PermEdit),
                            new SqlParameter("@EnteredBy", bgtEmp.EnteredBy),
                             };
                            if (item.AuthId == 3)
                            {
                                _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmployeeInsertRSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @PermView, @PermEdit, @EnteredBy", parms.ToArray());
                            }
                            else if (item.AuthId == 5)
                            {
                                _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmployeeInsertDSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @PermView, @PermEdit, @EnteredBy", parms.ToArray());
                            }
                            //else if (bgtEmp.AuthId == 4 || bgtEmp.AuthId == 6 || bgtEmp.AuthId == 8)
                            else
                            {

                                _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmployeeInsert] @DeptId, @Year, @SBU , @AuthId, @Amount, @PermView, @PermEdit, @EnteredBy", parms.ToArray());
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return new BgtEmployeeDto
            {
                Id = bgtEmp.Id,
                DeptId = bgtEmp.DeptId,
                Amount = bgtEmp.Amount,
                Year = bgtEmp.Year,
                CompId = bgtEmp.CompId,
                SBU = bgtEmp.SBU
            };
        }
        [HttpPost("saveCampaignBgtDetails/{empId}")]
        public ActionResult<CampaignBgtDetailsDto> SaveCampaignBgtDetails(int empId, CampaignBgtDetailsDto model)
        {

            try
            {
                List<SqlParameter> parms = new List<SqlParameter>
                        {
                            new SqlParameter("@CompId", 1000),
                            new SqlParameter("@DeptId", model.DeptId),
                            new SqlParameter("@Year", DateTime.Now.Year),
                            new SqlParameter("@SBU ", model.SBU),
                            new SqlParameter("@Amount", model.NewAmount),
                            new SqlParameter("@EnteredBy", empId),
                        };

               var result= _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtCampaignInsert] @CompId,@DeptId, @Year, @SBU , @Amount, @EnteredBy", parms.ToArray());

                return new CampaignBgtDetailsDto
                {
                    Id = model.Id,
                    DeptId = model.DeptId,
                    Amount = model.Amount,
                    Year = model.Year,
                    SBU = model.SBU
                };


            }
            catch (Exception ex)
            {
                return Ok("Failed");
            }


        }

        [HttpPost("SaveSbuBgtYearly")]
        public async Task<ActionResult<BgtSbuYearlyTotalDto>> SaveSbuBgtYearlyAsync(BgtSbuYearlyTotalDto setSbuBgtDto)
        {

            string qry = "";
            BgtSBUTotal sbuTotal = new BgtSBUTotal();
            //qry = string.Format(@" select * from BgtSBUTotal where  DeptId={0}", setSbuBgtDto.DeptId);
            //List<BgtSBUTotal> bgtList = _dbContext.BgtSBUTotal.FromSqlRaw(qry).ToList();
            if (setSbuBgtDto.SbuDetailsList != null && setSbuBgtDto.SbuDetailsList.Count > 0)
            {
                foreach (var item in setSbuBgtDto.SbuDetailsList)
                {
                    if (item.bgtSbuId > 0)
                    {
                        sbuTotal = await _bgtSbuRepo.GetByIdAsync(item.bgtSbuId);

                        sbuTotal.ModifiedOn = DateTime.Now;
                        sbuTotal.DataStatus = 0;
                        _bgtSbuRepo.Update(sbuTotal);
                        _bgtSbuRepo.Savechange();
                    }

                }
            }

            if (setSbuBgtDto.SbuDetailsList != null && setSbuBgtDto.SbuDetailsList.Count > 0)
            {
                foreach (var item in setSbuBgtDto.SbuDetailsList)
                {
                    sbuTotal = new BgtSBUTotal
                    {
                        CompId = setSbuBgtDto.CompId,
                        Year = setSbuBgtDto.Year,
                        SBUAmount = item.NewAmount,
                        Remarks = setSbuBgtDto.Remarks,
                        SBU = item.SBUCode,
                        DeptId = setSbuBgtDto.DeptId,
                        SetOn = DateTimeOffset.Now,
                        ModifiedOn = DateTimeOffset.Now,
                        DataStatus = 1,
                        EnteredBy = setSbuBgtDto.EnteredBy
                    };

                    _bgtSbuRepo.Add(sbuTotal);
                    _bgtSbuRepo.Savechange();

                }
            }
            return new BgtSbuYearlyTotalDto
            {
                Id = sbuTotal.Id,
                DeptId = sbuTotal.DeptId,
                SBUAmount = sbuTotal.SBUAmount,
                Year = setSbuBgtDto.Year,
                CompId = sbuTotal.CompId,
                SBU = setSbuBgtDto.SBU
            };
        }
        [HttpPost("updateSbuBudgetYearly")]
        public async Task<ActionResult<BgtSbuYearlyTotalDto>> UpdateSbuBudgetYearly(BgtSbuYearlyTotalDto setSbuBgtDto)
        {
            BgtSBUTotal sbuTotal = new BgtSBUTotal();
            sbuTotal = await _bgtSbuRepo.GetByIdAsync(setSbuBgtDto.Id);
            if (sbuTotal != null)
            {
                sbuTotal.SBUAmount = setSbuBgtDto.SBUAmount;
                sbuTotal.ModifiedOn = DateTime.Now;
                _bgtSbuRepo.Update(sbuTotal);
                _bgtSbuRepo.Savechange();
            }
            return new BgtSbuYearlyTotalDto
            {
                Id = sbuTotal.Id,
                DeptId = sbuTotal.DeptId,
                SBUAmount = sbuTotal.SBUAmount,
                Year = setSbuBgtDto.Year,
                CompId = sbuTotal.CompId,
                SBU = setSbuBgtDto.SBU
            };

        }

        [HttpPost("updateBgtEmployee")]
        public async Task<ActionResult<BgtEmployeeDto>> UpdateBgtEmployee(BgtEmployeeDto setBgtEmpDto)
        {
            BgtEmployee bgtEmp = new BgtEmployee();
            bgtEmp = await _bgtEmployee.GetByIdAsync(setBgtEmpDto.Id);
            if (bgtEmp != null)
            {
                bgtEmp.Amount = setBgtEmpDto.Amount;
                bgtEmp.PermEdit = setBgtEmpDto.PermEdit;
                bgtEmp.PermView = setBgtEmpDto.PermView;
                bgtEmp.ModifiedOn = DateTime.Now;
                _bgtEmployee.Update(bgtEmp);
                _bgtEmployee.Savechange();

                var Amount = bgtEmp.Amount / bgtEmp.NoOfEmployee;
                List<SqlParameter> parms = new List<SqlParameter>
                        {
                            new SqlParameter("@DeptId", bgtEmp.DeptId),
                            new SqlParameter("@Year", bgtEmp.Year),
                            new SqlParameter("@SBU ", bgtEmp.SBU),
                            new SqlParameter("@AuthId", bgtEmp.AuthId),
                            new SqlParameter("@Amount", Amount),
                            new SqlParameter("@PermView", bgtEmp.PermView),
                            new SqlParameter("@PermEdit", bgtEmp.PermEdit),
                            new SqlParameter("@EnteredBy", bgtEmp.EnteredBy),
                        };
                if (bgtEmp.AuthId == 3)
                {
                    _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmployeeInsertRSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @PermView, @PermEdit, @EnteredBy", parms.ToArray());
                }
                else if (bgtEmp.AuthId == 5)
                {
                    _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmployeeInsertDSM] @DeptId, @Year, @SBU , @AuthId, @Amount, @PermView, @PermEdit, @EnteredBy", parms.ToArray());
                }
                else if (bgtEmp.AuthId == 4 || bgtEmp.AuthId == 6 || bgtEmp.AuthId == 8)
                {
                    _dbContext.Database.ExecuteSqlRaw("EXECUTE [SP_BgtEmployeeInsert] @DeptId, @Year, @SBU , @AuthId, @Amount, @PermView, @PermEdit, @EnteredBy", parms.ToArray());
                }

            }
            return new BgtEmployeeDto
            {
                Id = bgtEmp.Id,
                DeptId = bgtEmp.DeptId,
                Amount = bgtEmp.Amount,
                Year = bgtEmp.Year,
                CompId = bgtEmp.CompId,
                SBU = bgtEmp.SBU
            };

        }

    }
}
