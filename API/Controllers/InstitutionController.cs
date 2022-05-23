using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class InstitutionController : BaseApiController
    {
        private readonly IGenericRepository<InstitutionInfo> _institutionRepo;
        private readonly IGenericRepository<InstitutionMarket> _InstitutionMarketRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private IConfiguration Configuration;
        public InstitutionController(IGenericRepository<InstitutionInfo> institutionRepo, IGenericRepository<InstitutionMarket> InstitutionMarketRepo, 
            StoreContext dbContext,IMapper mapper, IConfiguration _configuration)
        {
            _mapper = mapper;
            _institutionRepo = institutionRepo;
            _InstitutionMarketRepo = InstitutionMarketRepo;
            _dbContext = dbContext;
            Configuration = _configuration;
        }



        [HttpGet("institutionsForInvestment/{marketCode}")]
        public ActionResult<IEnumerable<InstitutionInfo>> GetInstitutionsForInvestment(string marketCode)
        {
            try
            {
                //  var institutions = await _institutionRepo.ListAllAsync();
                var institutions = (from d in _dbContext.InstitutionInfo
                                    join dm in _dbContext.InstitutionMarket on d.Id equals dm.InstitutionCode
                                    where dm.MarketCode == marketCode
                                    orderby d.InstitutionName
                                    select new InstitutionInfo
                                    {
                                        InstitutionName = d.InstitutionName,
                                        InstitutionCode = d.InstitutionCode,
                                        InstitutionType = d.InstitutionType,
                                        Address = d.Address,
                                        Id = d.Id
                                    }).Union(from d in _dbContext.InstitutionInfo
                                             where d.Id == 99999
                                             select new InstitutionInfo
                                             {
                                                 InstitutionName = d.InstitutionName,
                                                 InstitutionCode = d.InstitutionCode,
                                                 InstitutionType = d.InstitutionType,
                                                 Address = d.Address,
                                                 Id = d.Id
                                             }
                             ).Distinct().ToList();
                return institutions;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("insertNewInstitutionsFromMRS")]
        public void InsertNewInstitutionsFromMRS()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var _db = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.189.40)(PORT = 1521)))(CONNECT_DATA =(SID = mrsdb1)(SERVER = DEDICATED)));User Id=MRS;Password=mrs"))
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        objCmd.CommandText = "SELECT  INSTI_CODE , INSTI_NAME , INSTI_TYPE_NAME , ADDRESS1 ,'Active' STATUS ,NVL(NO_OF_BEDS, 0) NO_OF_BEDS ,NVL(AVG_NO_ADMT_PATI, 0) AVG_NO_ADMT_PATI FROM INSTITUTION A INNER JOIN INSTITUTION_TYPE B ON A.INSTI_TYPE_CODE = B.INSTI_TYPE_CODE WHERE to_char(A.ENTERED_DATE,'MM') = '03' ";
                        objCmd.Connection = _db;
                        _db.Open();
                        objCmd.ExecuteNonQuery();
                        using (OracleDataReader rdr = objCmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                dt.Load(rdr);
                            }
                        }
                    }
                }
                var result = _dbContext.Database.ExecuteSqlRaw("Delete FROM dbo.InstitutionInfoTemp");
                var conString = this.Configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.InstitutionInfoTemp";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("INSTI_CODE", "Id");
                        sqlBulkCopy.ColumnMappings.Add("INSTI_CODE", "InstitutionCode");
                        sqlBulkCopy.ColumnMappings.Add("INSTI_NAME", "InstitutionName");
                        sqlBulkCopy.ColumnMappings.Add("INSTI_TYPE_NAME", "InstitutionType");
                        sqlBulkCopy.ColumnMappings.Add("ADDRESS1", "Address");
                        sqlBulkCopy.ColumnMappings.Add("NO_OF_BEDS", "NoOfBeds");
                        sqlBulkCopy.ColumnMappings.Add("AVG_NO_ADMT_PATI", "AvgNoAdmtPati");
                        sqlBulkCopy.ColumnMappings.Add("STATUS", "Status");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                var resultSp = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InsertNewInstitution");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("insertNewInstitutionsMktFromMRS")]
        //[RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public void InsertNewInstitutionsMktFromMRS()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var _db = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.189.40)(PORT = 1521)))(CONNECT_DATA =(SID = mrsdb1)(SERVER = DEDICATED)));User Id=MRS;Password=mrs"))
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        //objCmd.CommandText = "SELECT    INSTI_CODE, B.MARKET_CODE ,MARKET_NAME,'Active' STATUS , B.SBU_UNIT FROM INSTI_MKT_MAS A INNER JOIN INSTI_MKT_DTL B ON A.INSTI_MKT_MAS_SLNO = B.INSTI_MKT_MAS_SLNO INNER JOIN MARKET C ON B.MARKET_CODE = C.MARKET_CODE WHERE to_char(B.ENTERED_DATE,'MM') = '03'";
                        objCmd.CommandText = "SELECT    INSTI_CODE, B.MARKET_CODE ,MARKET_NAME,'Active' STATUS , B.SBU_UNIT FROM INSTI_MKT_MAS A INNER JOIN INSTI_MKT_DTL B ON A.INSTI_MKT_MAS_SLNO = B.INSTI_MKT_MAS_SLNO INNER JOIN MARKET C ON B.MARKET_CODE = C.MARKET_CODE ";
                        objCmd.Connection = _db;
                        _db.Open();
                        objCmd.ExecuteNonQuery();
                        using (OracleDataReader rdr = objCmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                dt.Load(rdr);
                            }
                        }
                    }
                }
                var result = _dbContext.Database.ExecuteSqlRaw("Delete FROM dbo.InstitutionMarketTemp");
                var conString = this.Configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.InstitutionMarketTemp";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("INSTI_CODE", "InstitutionCode");
                        sqlBulkCopy.ColumnMappings.Add("MARKET_CODE", "MarketCode");
                        sqlBulkCopy.ColumnMappings.Add("MARKET_NAME", "MarketName");
                        sqlBulkCopy.ColumnMappings.Add("STATUS", "Status");
                        sqlBulkCopy.ColumnMappings.Add("SBU_UNIT", "SBU");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                var resultSp = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InsertNewInstitutionMarket");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
