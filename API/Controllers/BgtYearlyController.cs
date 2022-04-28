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
            #region Updating existing Entry
            string qry = "";
            qry = string.Format(@" select * from BgtYearlyTotal where DeptId={0}",setBgtDto.DeptId);
            List<BgtYearlyTotal> bgtList = _dbContext.BgtYearlyTotal.FromSqlRaw(qry).ToList();
            if(bgtList != null && bgtList.Count>0)
            {
                foreach(var item in bgtList)
                {
                    item.DataStatus = 0;
                    _dbContext.BgtYearlyTotal.Update(item);
                    _dbContext.SaveChanges();
                }
            }
            #endregion
            #region Insert New Budget
            //string bgtYear = setBgtDto.Year.ToString("yyyy");
            var bgt = new BgtYearlyTotal
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

        [HttpGet("getTotalExpense")]
        public long GetTotalExpense()
        {
            try
            {
                string qry = "";

                qry = string.Format(@"SELECT e.*,ISNULL(Round(SUM(e.ApprovedAmount),0), 0) TotalExpense

                                        FROM InvestmentDetailTracker e

                                        INNER JOIN InvestmentInit c ON c.Id = e.InvestmentInitId

                                        WHERE  C.ProposeFor='Others'

                                        AND e.Year=2022");
                TotalExpense dsResult = _dbContext.ExecSQL<TotalExpense>(qry).ToList().FirstOrDefault();
             
                return 0;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
