using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class DoctorController : BaseApiController
    {
        private readonly IGenericRepository<DoctorInfo> _doctorRepo;
        private readonly IGenericRepository<DoctorMarket> _doctorMarketRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private IConfiguration Configuration;
        public DoctorController(IGenericRepository<DoctorInfo> doctorRepo, IGenericRepository<DoctorMarket> doctorMarketRepo, StoreContext dbContext,
        IMapper mapper, IConfiguration _configuration)
        {
            _mapper = mapper;
            _doctorRepo = doctorRepo;
            _doctorMarketRepo = doctorMarketRepo;
            _dbContext = dbContext;
            Configuration = _configuration;
        }


        [HttpGet("doctorsForInvestment/{marketCode}")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult<IEnumerable<DoctorInfo>> GetDoctorsForInvestment(string marketCode)
        {
            try
            {
                //var doctorMarketSpec = new DoctorMarketSpecification(marketCode);

                //var doctorMarketData = await _doctorMarketRepo.ListAsync(doctorMarketSpec);
                //var doctors = await _doctorRepo.ListAllAsync();
                //var data = await _productRepo.ListAllAsync();
                var doctors = (from d in _dbContext.DoctorInfo
                               join dm in _dbContext.DoctorMarket on d.Id equals dm.DoctorCode
                               where dm.MarketCode == marketCode
                               orderby d.DoctorName
                               select new DoctorInfo
                               {
                                   DoctorName = d.DoctorName,
                                   DoctorCode = d.DoctorCode,
                                   Degree = d.Degree,
                                   Designation = d.Designation,
                                   Id = d.Id
                               }
                              ).Union(from d in _dbContext.DoctorInfo
                                      where d.Id == 900000
                                      orderby d.DoctorName
                                      select new DoctorInfo
                                      {
                                          DoctorName = d.DoctorName,
                                          DoctorCode = d.DoctorCode,
                                          Degree = d.Degree,
                                          Designation = d.Designation,
                                          Id = d.Id
                                      }).Distinct().ToList();
                //return doctors.OrderBy(x=>x.DoctorName);
                return doctors;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("doctorsForReport")]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public ActionResult<IEnumerable<DoctorInfo>> GetDoctorsForReport()
        {
            try
            {

                var doctors = (from d in _dbContext.DoctorInfo
                               join dm in _dbContext.DoctorMarket on d.Id equals dm.DoctorCode
                               // where dm.MarketCode == marketCode 
                               orderby d.DoctorName
                               select new DoctorInfo
                               {
                                   DoctorName = d.DoctorName,
                                   DoctorCode = d.DoctorCode,
                                   Degree = d.Degree,
                                   Designation = d.Designation,
                                   Id = d.Id
                               }
                              ).Union(from d in _dbContext.DoctorInfo
                                      where d.Id == 900000
                                      orderby d.DoctorName
                                      select new DoctorInfo
                                      {
                                          DoctorName = d.DoctorName,
                                          DoctorCode = d.DoctorCode,
                                          Degree = d.Degree,
                                          Designation = d.Designation,
                                          Id = d.Id
                                      }).Distinct().ToList();
                return doctors;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("insertNewDoctorsFromMRS")]
        //[RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public void InsertNewDoctorsFromMRS()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var _db = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.189.40)(PORT = 1521)))(CONNECT_DATA =(SID = mrsdb1)(SERVER = DEDICATED)));User Id=MRS;Password=mrs"))
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        objCmd.CommandText = "SELECT DOCTOR_ID ,DOCTOR_NAME ,NVL(PATIENT_PER_DAY, 0) PATIENT_PER_DAY  ,NVL(AVG_PRESC_VALUE, 0) AVG_PRESC_VALUE ,DEGREE || ',' || SPECIALIZATION DEGREES ,DESIGNATION ,ADDRESS1 ,'Active' STATUS FROM DOCTOR A INNER JOIN DOCTOR_SPECIALIZATION B ON A.SPECIA_1ST_CODE = B.SPECIALIZATION_CODE WHERE A.ENTERED_DATE >= sysdate - 7";
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
                var result = _dbContext.Database.ExecuteSqlRaw("Delete FROM dbo.DocTemp");
                var conString = this.Configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.DocTemp";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("DOCTOR_ID", "Id");
                        sqlBulkCopy.ColumnMappings.Add("DOCTOR_ID", "DoctorCode");
                        sqlBulkCopy.ColumnMappings.Add("DOCTOR_NAME", "DoctorName");
                        sqlBulkCopy.ColumnMappings.Add("PATIENT_PER_DAY", "PatientPerDay");
                        sqlBulkCopy.ColumnMappings.Add("AVG_PRESC_VALUE", "AvgPrescValue");
                        sqlBulkCopy.ColumnMappings.Add("DEGREES", "Degree");
                        sqlBulkCopy.ColumnMappings.Add("DESIGNATION", "Designation");
                        sqlBulkCopy.ColumnMappings.Add("ADDRESS1", "Address");
                        sqlBulkCopy.ColumnMappings.Add("STATUS", "Status");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                var resultSp = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InsertNewDoc");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("insertNewDoctorsMktFromMRS")]
        //[RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public void InsertNewDoctorsMktFromMRS()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var _db = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.189.40)(PORT = 1521)))(CONNECT_DATA =(SID = mrsdb1)(SERVER = DEDICATED)));User Id=MRS;Password=mrs"))
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        objCmd.CommandText = "SELECT DOCTOR_ID ,PRAC_MKT_CODE ,MARKET_NAME ,'Active' STATUS ,B.SBU_UNIT FROM DOC_MKT_MAS A INNER JOIN DOC_MKT_DTL B ON A.DOC_MKT_MAS_SLNO = B.DOC_MKT_MAS_SLNO INNER JOIN MARKET C ON B.PRAC_MKT_CODE = C.MARKET_CODE WHERE B.ENTRY_DATE >= sysdate - 7";
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
                var result = _dbContext.Database.ExecuteSqlRaw("Delete FROM dbo.DoctorMarketTemp");
                var conString = this.Configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.DoctorMarketTemp";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("DOCTOR_ID", "DoctorCode");
                        sqlBulkCopy.ColumnMappings.Add("PRAC_MKT_CODE", "MarketCode");
                        sqlBulkCopy.ColumnMappings.Add("MARKET_NAME", "MarketName");
                        sqlBulkCopy.ColumnMappings.Add("STATUS", "Status");
                        sqlBulkCopy.ColumnMappings.Add("SBU_UNIT", "SBU");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                var resultSp = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InsertNewDocMark");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
