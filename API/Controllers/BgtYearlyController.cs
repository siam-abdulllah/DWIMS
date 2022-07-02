using API.Dtos;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    public class BgtYearlyController : BaseApiController
    {
        private readonly IGenericRepository<BgtYearlyTotal> _bgtYearlyRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        public BgtYearlyController(IGenericRepository<BgtYearlyTotal> bgtYearlyRepo,
        IMapper mapper,StoreContext dbContext)
        {
            _mapper = mapper;
            _bgtYearlyRepo = bgtYearlyRepo;
            _dbContext = dbContext;
        }
      

        [HttpPost("SaveBgtYearly")]
        public ActionResult<BgtYearlyTotalDto> SaveBgtYearly(BgtYearlyTotalDto setBgtDto)
        {
            BgtYearlyTotal bgt = new BgtYearlyTotal();
            if (setBgtDto.AddAmount > 0)
            {
                #region Update Existing Amount
                string qry = "";
                qry = string.Format(@" select * from BgtYearlyTotal where DeptId={0} and DataStatus = 1", setBgtDto.DeptId);
                bgt = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList().FirstOrDefault();
                if(bgt != null)
                {
                    bgt.TotalAmount = bgt.TotalAmount + setBgtDto.AddAmount;
                    _dbContext.BgtYearlyTotal.Update(bgt);
                    _dbContext.SaveChanges();
                }
                #endregion
            }
            else
            {
                #region Updating existing Entry
                string qry = "";
                qry = string.Format(@" select * from BgtYearlyTotal where DeptId={0}", setBgtDto.DeptId);
                List<BgtYearlyTotal> bgtList = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList();
                if (bgtList != null && bgtList.Count > 0)
                {
                    foreach (var item in bgtList)
                    {
                        item.DataStatus = 0;
                        _dbContext.BgtYearlyTotal.Update(item);
                        _dbContext.SaveChanges();
                    }
                }
                #endregion
                #region Insert New Budget
                //string bgtYear = setBgtDto.Year.ToString("yyyy");
                bgt = new BgtYearlyTotal
                {
                    CompId = setBgtDto.CompId,
                    Year = setBgtDto.Year,
                    TotalAmount = setBgtDto.NewAmount,
                    DeptId = setBgtDto.DeptId,
                    SetOn = DateTimeOffset.Now,
                    ModifiedOn = DateTimeOffset.Now,
                    DataStatus = 1,
                    EnteredBy = setBgtDto.EnteredBy

                };

                _bgtYearlyRepo.Add(bgt);
                _bgtYearlyRepo.Savechange();
                #endregion
            }

            return new BgtYearlyTotalDto
            {
                Id = bgt.Id,
                DeptId = bgt.DeptId,
                TotalAmount = bgt.TotalAmount,
                Year = setBgtDto.Year,
                CompId = bgt.CompId,
                DataStatus = bgt.DataStatus
            };
        }

        [HttpGet("getBudgetYearly")]
        public  List<BgtYearlyTotal> GetBudgetYearly()
        {
            try
            {
                string qry = "";

                qry = string.Format(@" select * from BgtYearlyTotal where DataStatus=1");

                List<BgtYearlyTotal> bgtList = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList();
                return bgtList;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getTotalExpense/{deptId}/{year}")]
        public double GetTotalExpense(int deptId,int year)
        {
            string ProposeFor = "";
            if (deptId == 1)
            {
                ProposeFor = "'Others','Sales'";
            }
            else if (deptId == 2)
            {
                ProposeFor = "'BrandCampaign','PMD'";
            }
            try
            {
                string qry = "";

                qry = string.Format(@"SELECT ISNULL(Round(SUM(e.ApprovedAmount),0), 0) Expense

                                        FROM InvestmentDetailTracker e

                                        INNER JOIN InvestmentInit c ON c.Id = e.InvestmentInitId

                                        WHERE  C.ProposeFor IN ({0})

                                        AND e.Year={1}", ProposeFor,year);
    
                DataTable dt = _dbContext.DataTable(qry);
                TotalExpense expense = new TotalExpense();
                
                   expense = (from DataRow row in dt.Rows
                    select new TotalExpense
                    {
                        Expense = row["Expense"]!= null ? Convert.ToInt64(row["Expense"]):0,
                      
                    }).ToList().FirstOrDefault();
                return expense.Expense;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getTotalPipeLine/{deptId}/{year}")]
        public double GetTotalPipeLine(int deptId,int year)
        {
            string ProposeFor = "";
            if (deptId == 1)
            {
                ProposeFor = "Others";
            }
            else if (deptId == 2)
            {
                ProposeFor = "BrandCampaign";
            }
            try
            {
                string qry = "";

                qry = string.Format(@"SELECT Pipeline FROM (

                                    SELECT Round(SUM(ProposedAmount),0) Pipeline 
                                    FROM InvestmentRec A                                   

                                    INNER JOIN InvestmentRecComment B ON A.InvestmentInitId = B.InvestmentInitId                       

                                    AND A.EmployeeId = B.EmployeeId                                  

                                    INNER JOIN InvestmentInit C ON a.InvestmentInitId = C.Id                        

                                    WHERE C.DataStatus = 1                                 

                                    AND C.ProposeFor='{0}'                                 

                                    AND Year(A.ToDate)={1}                                 

                                    AND RecStatus NOT IN ('Approved' ,'Not Approved','Cancelled') AND A.Id IN (                               

                                    SELECT MAX(ir.Id) AS Expr1                                

                                    FROM dbo.InvestmentRec AS ir                                  

                                    WHERE (ir.InvestmentInitId = c.Id) )                               

                                    ) A

                                    ", ProposeFor,year);

                DataTable dt = _dbContext.DataTable(qry);
                TotalPipeLine pipeLine = new TotalPipeLine();
                
                pipeLine = (from DataRow row in dt.Rows
                           select new TotalPipeLine
                           {
                               PipeLine = row["PipeLine"] != null ? Convert.ToInt64(row["PipeLine"]) : 0,

                           }).ToList().FirstOrDefault();
                return pipeLine.PipeLine;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        [HttpGet("getBudgetAmount/{deptId}/{year}")]
        public double GetBudgetAmount(int deptId, int year)
        {
          
            try
            {
                string qry = "";

                qry = string.Format(@"select * from BgtYearlyTotal where DataStatus = 1 and  DeptId = {0} and Year = {1}", deptId, year);

                BgtYearlyTotal bgtyearly = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList().FirstOrDefault();
                long TotalBudget = 0;
                if(bgtyearly != null)
                {
                    TotalBudget = bgtyearly.TotalAmount;
                }
                return TotalBudget;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getTotalAllocated/{deptId}/{year}")]
        public double GetTotalAllocated(int deptId, int year)
        {
          
            try
            {
                string qry = "";

                qry = string.Format(@"select CAST('1' AS INT) AS Id ,1 AS DataStatus , SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, ISNULL(ROUND(SUM(SBUAmount),0),0) Count  from BgtSBUTotal where DataStatus = 1 and  DeptId = {0} and Year = {1}  ", deptId, year);

                var result = _dbContext.CountLong.FromSqlRaw(qry).ToList().FirstOrDefault();
          
               
                return result.Count;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
