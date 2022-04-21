using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class DepotPrintTrackController : BaseApiController
    {

        private readonly IGenericRepository<DepotPrintTrack> _trackRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _db;

        public DepotPrintTrackController(IGenericRepository<DepotPrintTrack> trackRepo,
       IMapper mapper, StoreContext db)
        {
            _mapper = mapper;
            _trackRepo = trackRepo;
            _db = db;
        }

        [HttpPost("createTrackRecord")]
        public ActionResult<DepotPrintTrackDto> InsertDepoTracker(DepotPrintTrack trackDto)
        {
            var check = _db.DepotPrintTrack.FromSqlRaw("select * from DepotPrintTrack where PayRefNo = '" + trackDto.PayRefNo + "'").ToList(); ;

            string dptCode = "";
            string dptName = "";

            var depoCode = _db.InvestmentRecDepot.Where(x => x.InvestmentInitId == trackDto.InvestmentInitId).FirstOrDefault();

            if (depoCode != null)
            {
                dptCode = depoCode.DepotCode;
                dptName = depoCode.DepotName;
            }
            else
            {
                dptCode = "";
                dptName = "";
            }

            if (check.Count() == 0)
            {
                var bcds = new DepotPrintTrack
                {
                    InvestmentInitId = trackDto.InvestmentInitId,
                    PaymentRefNo = trackDto.PaymentRefNo,
                    SAPRefNo = trackDto.PaymentRefNo,
                    PaymentDate = trackDto.PaymentDate,
                    PayRefNo = trackDto.PayRefNo,
                    DepotId = dptCode,
                    DepotName = dptName,
                    Remarks = trackDto.Remarks,
                    SetOn = DateTimeOffset.Now,
                    EmployeeId = trackDto.EmployeeId,
                    LastPrintTime = DateTimeOffset.Now,
                    BankName = trackDto.BankName,
                    ChequeNo = trackDto.ChequeNo,
                    PrintCount = 1,
                };

                _trackRepo.Add(bcds);
                _trackRepo.Savechange();

                return new DepotPrintTrackDto
                {
                    Id = bcds.Id,
                    InvestmentInitId = trackDto.InvestmentInitId,
                    PaymentRefNo = trackDto.PaymentRefNo,
                    SAPRefNo = trackDto.PaymentRefNo,
                    PaymentDate = trackDto.PaymentDate,
                    PayRefNo = trackDto.PayRefNo,
                    DepotId = trackDto.DepotId,
                    DepotName = trackDto.DepotName,
                    Remarks = trackDto.Remarks,
                    EmployeeId = trackDto.EmployeeId,
                    LastPrintTime = DateTimeOffset.Now,
                    BankName = trackDto.BankName,
                    ChequeNo = trackDto.ChequeNo,
                    PrintCount = 1,
                };
            }
            else
            {
                return Ok("Data Exists");
            }
        }

    }
}