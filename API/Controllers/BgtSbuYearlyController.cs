using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IGenericRepository<SBU> _sbuRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        public BgtSbuYearlyController(IGenericRepository<BgtSBUTotal> bgtSbuRepo, IGenericRepository<SBU> sbuRepo,
        IMapper mapper, StoreContext dbContext)
        {
            _mapper = mapper;
            _bgtSbuRepo = bgtSbuRepo;
            _dbContext = dbContext;
            _sbuRepo = sbuRepo;
        }

     

        [HttpGet("getAllSbuBgtList/{deptId}/{compId}/{year}")]
        public async Task<List<SBUVM>> GetAllSbuBgtListAsync(int deptId,int compId,int year)
        {
            List<SBUVM> dsResult = new List<SBUVM>();
            try
            {
                string statusQuery = "";
                string ProposeFor = "";
                if(deptId == 1)
                {
                    ProposeFor = "Others";
                }
                else if(deptId == 2)
                {
                    ProposeFor = "BrandCampaign";
                }
                string qry = "";
              
                qry = string.Format(@"SELECT sb.*
                                    ,bs.SBUAmount
                                    ,bs.Id BgtSbuId
                                    ,(
                                    SELECT ISNULL(Round(SUM(ApprovedAmount),0), 0)
                                    FROM InvestmentDetailTracker e
                                    INNER JOIN InvestmentInit c ON c.Id = e.InvestmentInitId
                                    WHERE c.SBU = SB.SBUCode AND C.ProposeFor='{3}'
                                    AND e.Year={0}
                                    ) Expense
                                    FROM SBU sb
                                    LEFT JOIN BgtSBUTotal bs ON bs.SBU = sb.SbuCode
                                    AND bs.DeptId = {1}
                                    AND bs.DataStatus = 1
                                    AND bs.CompId = {2}
                                    AND bs.Year = {0}",year, deptId, compId,ProposeFor);
                


                dsResult = _dbContext.ExecSQL<SBUVM>(qry).ToList();
                if(dsResult == null)
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

        [HttpGet("getAllPipelineExpenseList/{deptId}/{compId}/{year}")]
        public async Task<List<PipeLineExpense>> GetAllPipeLineExpenseListAsync(int deptId, int compId, int year)
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

        [HttpGet("GetAppAuthDetails/{sbuName}")]
        public async Task<List<AppAuthDetails>> GetAppAuthDetails(string sbuName)
        {
            try
            {
                string qry = "";

                qry = string.Format(@"select CAST(ROW_NUMBER() OVER (ORDER BY Priority) AS INT)  AS Id ,1 AS DataStatus,SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn,aa.Priority,
                                     aa.Remarks,0 as Expense,0 as TotalAmount,0 as NewAmount, (select COUNT(*) from ApprAuthConfig ac
                                    left join EmpSbuMapping emp on emp.Id = ac.EmployeeId
                                    where ac.ApprovalAuthorityId = aa.Priority and emp.SBUName = '{0}' and emp.DataStatus = 1) TotalPerson
                                    from ApprovalAuthority aa where Priority in (3,4,5,6,7)", sbuName);


                var results = _dbContext.AppAuthDetails.FromSqlRaw(qry).ToList();
                List<AppAuthDetails> dsResult = _dbContext.ExecSQL<AppAuthDetails>(qry).ToList();
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getYearlyBudget/{deptId}/{year}")]
        public BgtYearlyTotal GetBudgetYearly(int deptId,int year)
        {
            BgtYearlyTotal bgt = new BgtYearlyTotal();
            try
            {
               
                string qry = "";

                qry = string.Format(@" select * from BgtYearlyTotal where DataStatus=1 and DeptId={0} and year={1}",deptId,year);

                 bgt = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList().FirstOrDefault();
                if(bgt == null)
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
        [HttpPost("SaveSbuBgtYearly")]
        public async Task<ActionResult<BgtSbuYearlyTotalDto>> SaveSbuBgtYearlyAsync(BgtSbuYearlyTotalDto setSbuBgtDto)
        {

            string qry = "";
            BgtSBUTotal sbuTotal = new BgtSBUTotal();
            qry = string.Format(@" select * from BgtSBUTotal where  DeptId={0}", setSbuBgtDto.DeptId);
            List<BgtSBUTotal> bgtList = _dbContext.BgtSBUTotal.FromSqlRaw(qry).ToList();
            if(bgtList != null && bgtList.Count > 0)
            {
                foreach(var item in bgtList)
                {
                    item.DataStatus = 0;
                    _bgtSbuRepo.Update(item);
                    _bgtSbuRepo.Savechange();
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
            if(sbuTotal != null)
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
        
    }
}
