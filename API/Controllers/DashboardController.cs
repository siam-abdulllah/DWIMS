using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;

        public DashboardController(IMapper mapper, StoreContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("totalApproved/{role}/{empCode}")]
        public object GetTotalApproved(string role, string empCode)
        {
            try
            {
                DateTime fd = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime td = DateTime.Today;

            string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empCode +"' ";
            var empData = _dbContext.Employee.FromSqlRaw(empQry).ToList();
    
            //string qry = " select CAST(a.Id AS INT) as Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, d.DonationTypeName, a.DonationTo, b.ProposedAmount, b.FromDate, b.ToDate, dbo.fnGetInvestmentStatus(a.Id) InvStatus, e.EmployeeName,dbo.fnGetInvestmentApprovedBy(a.Id) ApprovedBy,e.MarketName, ISNULL(rcv.ReceiveStatus, 'Not Completed') ReceiveStatus, ISNULL(rcvBy.EmployeeName, 'N/A') ReceiveBy " +
                string qry = " select COUNT(a.Id) " +
                " from InvestmentInit a " +
                " left join InvestmentDetail b on a.Id = b.InvestmentInitId " +
                " left join InvestmentRecv rcv on a.Id = rcv.InvestmentInitId " +
                " inner join Donation d on d.Id = a.DonationId " +
                " left join Employee e on e.Id = a.EmployeeId " +
                " left join Employee rcvBy on rcvBy.Id = rcv.EmployeeId " +
                " Where 1 = 1 AND dbo.fnGetInvestmentStatus(a.Id) = 'Approved' " +
                //" AND(CONVERT(date, b.FromDate) >= CAST('" + fd + "' as Date) AND CAST('" + td + "' as Date) >= CONVERT(date, b.ToDate)) ";
                " AND rcv.ReceiveStatus IS NULL ";
            if (role != "Administrator")
            {
                qry = qry + " AND (" +
                            " e.MarketGroupCode = COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.MarketCode = COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.TerritoryCode = COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.RegionCode = COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.ZoneCode = COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All') = 'All'" +
                            " )";
            }

            var results = _dbContext.CountInt.FromSqlRaw(qry);
            return results.ToString();

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }


        [HttpGet("myPendingCount/{role}/{empCode}")]
        public object GetMyPending(string role, string empCode)
        {
            try
            {
                string qry = "";

                if (role == "MPO")
                {
                    qry = " select * from InvestmentInit " +
                        " where Id in (select InvestmentInitId from InvestmentTargetedGroup " +
                        " where MarketCode in (select MarketCode from Employee " +
                        " where Id = "+ empCode +" ) and CompletionStatus = 0) and Confirmation = 1 ";
                }
                else if (role == "TM")
                {
                    qry = " select * from InvestmentInit " +
                        " where Id in (select InvestmentInitId from InvestmentRecComment " +
                        " where TerritoryCode in (select TerritoryCode from Employee " +
                        " where Id = "+empCode+" ) and CompletionStatus = 0 and [Priority] = 2) and Confirmation = 1 ";
                }

                else if (role == "RSM")
                {
                    qry = " select * from InvestmentInit  " +
                        " where Id in (select InvestmentInitId from InvestmentRecComment " +
                        " where RegionCode in (select RegionCode from Employee " +
                        " where Id = "+empCode+"  ) and CompletionStatus = 0 and [Priority] = 3) ";
                }
                else
            {
                return 0;
            }
   
                var result = _dbContext.InvestmentInit.FromSqlRaw(qry);
                return result.Count().ToString();
            }
            catch (System.Exception e)
            {

                throw e;
            }
        }


         [HttpGet("approvalPending/{role}/{empCode}")]
        public object GetApprovalPending(string role, string empCode)
        {
            try
            {
                DateTime fd = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime td = DateTime.Today;

            string empQry = "SELECT * FROM Employee WHERE EmployeeSAPCode= '" + empCode +"' ";
            var empData = _dbContext.Employee.FromSqlRaw(empQry).ToList();
   
            //string qry = " select CAST(a.Id AS INT) as Id ,1 AS DataStatus, SYSDATETIMEOFFSET() AS SetOn, SYSDATETIMEOFFSET() AS ModifiedOn, a.ReferenceNo, d.DonationTypeName, a.DonationTo, b.ProposedAmount, b.FromDate, b.ToDate, dbo.fnGetInvestmentStatus(a.Id) InvStatus, e.EmployeeName,dbo.fnGetInvestmentApprovedBy(a.Id) ApprovedBy,e.MarketName, ISNULL(rcv.ReceiveStatus, 'Not Completed') ReceiveStatus, ISNULL(rcvBy.EmployeeName, 'N/A') ReceiveBy " +
              string qry = " select COUNT(a.Id) " +
                " from InvestmentInit a " +
                " left join InvestmentDetail b on a.Id = b.InvestmentInitId " +
                " left join InvestmentRecv rcv on a.Id = rcv.InvestmentInitId " +
                " inner join Donation d on d.Id = a.DonationId " +
                " left join Employee e on e.Id = a.EmployeeId " +
                " left join Employee rcvBy on rcvBy.Id = rcv.EmployeeId " +
                " Where 1 = 1 AND dbo.fnGetInvestmentStatus(a.Id) = 'Pending' AND a.Confirmation = 1 AND b.ProposedAmount is NOT NULL ";
                //" AND(CONVERT(date, b.FromDate) >= CAST('" + fd + "' as Date) AND CAST('" + td + "' as Date) >= CONVERT(date, b.ToDate)) ";
            if (role != "Administrator")
            {
                qry = qry + " AND (" +
                            " e.MarketGroupCode = COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketGroupCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.MarketCode = COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].MarketCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.TerritoryCode = COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].TerritoryCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.RegionCode = COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].RegionCode + "',''), 'All') = 'All'" +
                            " )" +
                            " AND (" +
                            " e.ZoneCode = COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All')" +
                            " OR COALESCE(NULLIF('" + empData[0].ZoneCode + "',''), 'All') = 'All'" +
                            " )";
            }
        

            //var results = _dbContext.Set.FromSqlRaw(qry).ToList();
            var results = _dbContext.CountInt.FromSqlRaw(qry);
            return results.ToString();

            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
