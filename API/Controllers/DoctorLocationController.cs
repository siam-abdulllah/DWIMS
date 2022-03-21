using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using API.Dtos;
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

namespace API.Controllers
{
    public class DoctorLocationController : BaseApiController
    {
        private readonly StoreContext _db;

        public DoctorLocationController(StoreContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("getDocLocMapInd/{doctorName}/{doctorCode}/{marketName}/{marketCode}")]
        public ActionResult<IReadOnlyList<DocLocMap>> GetDocLocMapSingle(string doctorName, int doctorCode, string marketName, string marketCode)
        {
            string qry = " SELECT distinct a.Id, a.[DataStatus], A.[SetOn],A.[ModifiedOn], a.DoctorCode, d.DoctorName, d.Designation, d.Address, m.MarketCode, m.MarketName, a.SBU " +
                " From DoctorMarket a " +
                " inner join DoctorInfo d on a.DoctorCode = d.DoctorCode " +
                " inner join Employee m on m.MarketCode = a.MarketCode " +
                " WHERE 1=1 ";

            if (!string.IsNullOrEmpty(doctorName) && doctorName != "undefined")
            {
                qry = qry + " AND d.DoctorName Like '%" + doctorName + "%'";
            }
            if (doctorCode != 0)
            {
                qry = qry + " AND a.DoctorCode = " + doctorCode + " ";
            }
            if (!string.IsNullOrEmpty(marketName) && marketName != "undefined")
            {
                qry = qry + " AND m.marketName Like '%" + marketName + "%'";
            }
            if (!string.IsNullOrEmpty(marketCode) && marketCode != "undefined")
            {
                qry = qry + " AND m.MarketCode = '" + marketCode + "' ";
            }
            var results = _db.DocLocMap.FromSqlRaw(qry).ToList();

            return results;
        }

    }
}
