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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ReportInvestmentController : BaseApiController
    {
        private readonly StoreContext _db;

        private readonly IGenericRepository<ReportConfig> _rptConfitRepo;
        private readonly IGenericRepository<ReportInvestmentInfo> _investRepo;
        private readonly IMapper _mapper;



        public ReportInvestmentController(IGenericRepository<ReportInvestmentInfo> investRepo, IGenericRepository<ReportConfig> rptConfitRepo, IMapper mapper, StoreContext db)
        {
            _mapper = mapper;
            _investRepo = investRepo;
            _rptConfitRepo = rptConfitRepo;
            _db = db;
        }

        [HttpPost("GetInsSocietyBCDSWiseInvestment")]
        public ActionResult<IReadOnlyList<InstSocDocInvestmentDto>> GetInstituteInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams,ReportSearchDto search)
        {

            List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter("@UserId", search.UserId),
                        new SqlParameter("@FromDate", search.FromDate),
                        new SqlParameter("@ToDate", search.ToDate),
                        new SqlParameter("@SBU", search.SBU),
                        new SqlParameter("@DonationType", search.DonationType),
                        new SqlParameter("@InvestType", search.InvestType),
                        new SqlParameter("@InstitutionId", search.InstitutionId),
                        new SqlParameter("@SocietyId", search.SocietyId),
                        new SqlParameter("@BcdsId", search.BcdsId),
                        new SqlParameter("@LocationType", search.LocationType),
                        new SqlParameter("@TerritoryCode", search.TerritoryCode),
                        new SqlParameter("@MarketCode", search.MarketCode),
                        new SqlParameter("@regionCode", search.RegionCode),
                        new SqlParameter("@ZoneCode", search.ZoneCode),
                        new SqlParameter("@DivisionCode", search.DivisionCode),
                    };

            var results = _db.ReportInvestmentInfo.FromSqlRaw("EXECUTE SP_InvestmentReport @UserId,@FromDate,@ToDate, @SBU, @DonationType, @InvestType, " +
                " @InstitutionId, @SocietyId, @BcdsId, @LocationType, @TerritoryCode, @MarketCode, @regionCode, @ZoneCode,  @DivisionCode", parms.ToArray()).ToList();

            var data = _mapper.Map<IReadOnlyList<ReportInvestmentInfo>, IReadOnlyList<InstSocDocInvestmentDto>>(results);

            return Ok(new Pagination<InstSocDocInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, 10, data));
        }

        [HttpPost("GetDoctorCampaingWiseInvestment")]
        public ActionResult<IReadOnlyList<RptDocCampWiseInvestment>> GetDoctorCampaingWiseInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        {

            //List<SqlParameter> parms = new List<SqlParameter>
            //        {
            //            new SqlParameter("@UserId", search.UserId),
            //            new SqlParameter("@FromDate", search.FromDate),
            //            new SqlParameter("@ToDate", search.ToDate),
            //            new SqlParameter("@SBU", search.SBU),
            //            new SqlParameter("@DonationType", search.DonationType),
            //            new SqlParameter("@InvestType", search.InvestType),
            //            new SqlParameter("@InstitutionId", search.InstitutionId),
            //            new SqlParameter("@SocietyId", search.SocietyId),
            //            new SqlParameter("@BcdsId", search.BcdsId),
            //            new SqlParameter("@LocationType", search.LocationType),
            //            new SqlParameter("@TerritoryCode", search.TerritoryCode),
            //            new SqlParameter("@MarketCode", search.MarketCode),
            //            new SqlParameter("@regionCode", search.RegionCode),
            //            new SqlParameter("@ZoneCode", search.ZoneCode),
            //            new SqlParameter("@DivisionCode", search.DivisionCode),
            //        };

            var results = _db.RptDocCampWiseInvestment.FromSqlRaw("SELECT * FROM RptDocCampWiseInvestment").ToList();

            // var data = _mapper.Map<IReadOnlyList<RptDocCampWiseInvestment>, IReadOnlyList<RptDocCampWiseInvestmentDto>>(results);

            // return Ok(new Pagination<RptDocCampWiseInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, 10, data));
            return results;
        }

        [HttpPost("GetDoctorLocationWiseInvestment")]
        public ActionResult<IReadOnlyList<RptDocLocWiseInvestment>> GetDoctorLocationWiseInvestment([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        {

            //List<SqlParameter> parms = new List<SqlParameter>
            //        {
            //            new SqlParameter("@UserId", search.UserId),
            //            new SqlParameter("@FromDate", search.FromDate),
            //            new SqlParameter("@ToDate", search.ToDate),
            //            new SqlParameter("@SBU", search.SBU),
            //            new SqlParameter("@DonationType", search.DonationType),
            //            new SqlParameter("@InvestType", search.InvestType),
            //            new SqlParameter("@InstitutionId", search.InstitutionId),
            //            new SqlParameter("@SocietyId", search.SocietyId),
            //            new SqlParameter("@BcdsId", search.BcdsId),
            //            new SqlParameter("@LocationType", search.LocationType),
            //            new SqlParameter("@TerritoryCode", search.TerritoryCode),
            //            new SqlParameter("@MarketCode", search.MarketCode),
            //            new SqlParameter("@regionCode", search.RegionCode),
            //            new SqlParameter("@ZoneCode", search.ZoneCode),
            //            new SqlParameter("@DivisionCode", search.DivisionCode),
            //        };

            //var results = _db.RptDocLocWiseInvestment.FromSqlRaw("EXECUTE SP_InvestmentReport @UserId,@FromDate,@ToDate, @SBU, @DonationType, @InvestType, " +
            //    " @InstitutionId, @SocietyId, @BcdsId, @LocationType, @TerritoryCode, @MarketCode, @regionCode, @ZoneCode,  @DivisionCode", parms.ToArray()).ToList();

            var results = _db.RptDocLocWiseInvestment.FromSqlRaw("SELECT * FROM RptDocLocWiseInvestment").ToList();

            //var data = _mapper.Map<IReadOnlyList<RptDocLocWiseInvestment>, IReadOnlyList<RptDocLocWiseInvestmentDto>>(results);

            //return Ok(new Pagination<RptDocLocWiseInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, 10, data));

            return results;
        }


        //[HttpPost("GetComparativeStudy")]
        //public async ActionResult<IReadOnlyList<RptDocLocWiseInvestmentDto>> GetComparativeStudy([FromQuery] ReportInvestmentInfoSpecParams rptParrams, ReportSearchDto search)
        //{

        //    List<SqlParameter> parms = new List<SqlParameter>
        //            {
        //                new SqlParameter("@UserId", search.UserId),
        //                new SqlParameter("@FromDate", search.FromDate),
        //                new SqlParameter("@ToDate", search.ToDate),
        //                new SqlParameter("@SBU", search.SBU),
        //                new SqlParameter("@DonationType", search.DonationType),
        //                new SqlParameter("@InvestType", search.InvestType),
        //                new SqlParameter("@InstitutionId", search.InstitutionId),
        //                new SqlParameter("@SocietyId", search.SocietyId),
        //                new SqlParameter("@BcdsId", search.BcdsId),
        //                new SqlParameter("@LocationType", search.LocationType),
        //                new SqlParameter("@TerritoryCode", search.TerritoryCode),
        //                new SqlParameter("@MarketCode", search.MarketCode),
        //                new SqlParameter("@regionCode", search.RegionCode),
        //                new SqlParameter("@ZoneCode", search.ZoneCode),
        //                new SqlParameter("@DivisionCode", search.DivisionCode),
        //            };

        //    //var results = _db.Database.ExecuteSqlRaw($"EXECUTE SP_InvestmentReport @UserId,@FromDate,@ToDate, @SBU, @DonationType, @InvestType, @InstitutionId, @SocietyId, @BcdsId, @LocationType, @TerritoryCode, @MarketCode, @regionCode, @ZoneCode,  @DivisionCode");

        //    //var data = _mapper.Map<IReadOnlyList<RptDocLocWiseInvestment>, IReadOnlyList<RptDocLocWiseInvestmentDto>>(results);

        //    return Ok(new Pagination<RptDocLocWiseInvestmentDto>(rptParrams.PageIndex, rptParrams.PageSize, 10, data));
        //}

        [HttpGet("getReportList")]
        public async Task<ActionResult<IReadOnlyList<ReportConfigDto>>> getReportList([FromQuery] ReportConfigSpecParams rptParrams)
        {

            var spec = new ReportConfigSpecification(rptParrams);

            var countSpec = new ReportConfigWithFiltersForCountSpecificication(rptParrams);

            var totalItems = await _rptConfitRepo.CountAsync(countSpec);

            var posts = await _rptConfitRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<ReportConfig>, IReadOnlyList<ReportConfigDto>>(posts);

            return Ok(new Pagination<ReportConfigDto>(rptParrams.PageIndex, rptParrams.PageSize, totalItems, data));
        }
    }
}
