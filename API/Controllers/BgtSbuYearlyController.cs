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
                                    ,(
                                    SELECT ISNULL(Round(SUM(ApprovedAmount),0), 0)
                                    FROM InvestmentDetailTracker e
                                    INNER JOIN InvestmentInit c ON c.Id = e.InvestmentInitId
                                    WHERE c.SBU = SB.SBUCode AND C.ProposeFor='BrandCampaign'
                                    AND e.Year={0}
                                    ) Expense
                                    FROM SBU sb
                                    LEFT JOIN BgtSBUTotal bs ON bs.SBU = sb.SbuCode
                                    AND bs.DeptId = {1}
                                    AND bs.DataStatus = 1
                                    WHERE bs.CompId = {2}",year, deptId, compId);
                


                List<SBUVM> dsResult = _dbContext.ExecSQL<SBUVM>(qry).ToList();
           
            
                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getAllPipelineExpenseList/{deptId}/{compId}/{year}")]
        public async Task<List<InvestmentRecVM>> GetAllPipeLineExpenseListAsync(int deptId, int compId, int year)
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

                qry = string.Format(@"SELECT SUM(ProposedAmount) Pipeline
                                    ,sb.SBUName
                                    ,sb.SBUCode
                                    FROM InvestmentRec A
                                    INNER JOIN InvestmentRecComment B ON A.InvestmentInitId = B.InvestmentInitId
                                    AND A.EmployeeId = B.EmployeeId
                                    INNER JOIN InvestmentInit C ON a.InvestmentInitId = C.Id
                                    INNER JOIN SBU sb ON C.SBU = sb.SBUCode

                                    WHERE C.DataStatus = 1
                                    AND C.ProposeFor='{0}'
                                    AND Year(A.ToDate)={1}
                                    AND RecStatus NOT IN (
                                    'Approved'
                                    ,'Not Approved'
                                    ,'Cancelled'
                                    )
                                    AND A.Id IN (
                                    SELECT MAX(ir.Id) AS Expr1
                                    FROM dbo.InvestmentRec AS ir
                                    WHERE (ir.InvestmentInitId = c.Id)
                                    )
                                    GROUP BY sb.SBUName
                                    ,sb.SBUCode", ProposeFor, year);



                List<InvestmentRecVM> dsResult = _dbContext.ExecSQL<InvestmentRecVM>(qry).ToList();


                return dsResult;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getYearlyBudget/{deptId}")]
        public BgtYearlyTotal GetBudgetYearly(int deptId)
        {
            try
            {
                string qry = "";

                qry = string.Format(@" select * from BgtYearlyTotal where DataStatus=1 and DeptId={0}",deptId);

               BgtYearlyTotal bgt = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList().FirstOrDefault();
                return bgt;
            }
            catch (System.Exception ex)
            {
                throw ex;
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

    }
}
