using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using Infrastructure.Data;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<ProductInfo> _productRepo;
        private readonly IGenericRepository<MedicineProduct> _medicineProductRepo;
        private readonly IMapper _mapper;
        private readonly StoreContext _dbContext;
        private readonly IConfiguration _configuration;
        public ProductController(IGenericRepository<ProductInfo> productRepo, IGenericRepository<MedicineProduct> medicineProductRepo,
        StoreContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _medicineProductRepo = medicineProductRepo;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpGet("getBrand/{sbu}")]
        public async Task<IReadOnlyList<BrandDto>> GetBrand(string sbu)
        {
            try
            {
                var data = await _productRepo.ListAllAsync();
                var products = data.Where(p => p.SBU == sbu && p.Status == "Active" && p.BrandCode != null && p.BrandName != null).OrderBy(x => x.BrandName).GroupBy(g => new { g.BrandCode, g.BrandName })
                                 .Select(g => g.First())
                                 .ToList();
                var brand = (from r in products
                                 // where r.Status=="Active" && r.SBU==sbu && r.BrandCode!=null
                                 // orderby r.BrandName
                             select new BrandDto
                             {
                                 BrandName = r.BrandName,
                                 BrandCode = r.BrandCode
                             }
                              ).Distinct().ToList();
                return brand;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getProduct")]
        public async Task<IReadOnlyList<ProductDto>> GetProduct()
        {
            try
            {
                var products = await _productRepo.ListAllAsync();
                var data = _mapper
                    .Map<IReadOnlyList<ProductInfo>, IReadOnlyList<ProductDto>>(products);
                return data;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getProduct/{brandCode}/{sbu}")]
        public async Task<List<ProductDto>> GetProduct(string brandCode, string sbu)
        {
            try
            {
                var data = await _productRepo.ListAllAsync();
                var productList = data.Where(p => p.BrandCode == brandCode && p.SBU == sbu && p.Status == "Active" && p.BrandCode != null && p.BrandName != null).OrderBy(x => x.ProductName).GroupBy(g => new { g.ProductCode, g.ProductName })
                                 .Select(g => g.First())
                                 .ToList();
                var products = (from r in productList
                                    //where r.Status == "Active" && r.BrandCode == brandCode
                                    //orderby r.BrandName
                                select new ProductDto
                                {
                                    Id = r.Id,
                                    ProductCode = r.ProductCode,
                                    ProductName = r.ProductName
                                }
                            ).Distinct().ToList();
                //var dataProd = _mapper.Map<IReadOnlyList<ProductInfo>, IReadOnlyList<ProductDto>>(products);
                return products;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getMedicineProductForInvestment")]
        public async Task<IEnumerable<MedicineProduct>> GetMedicineProductForInvestment()
        {
            try
            {
                var products = await _medicineProductRepo.ListAllAsync();
                var medProds = (from r in products
                                where r.Status == "A"
                                orderby r.ProductName
                                select new MedicineProduct
                                {
                                    Id = r.Id,
                                    ProductCode = r.ProductCode,
                                    ProductName = r.ProductName,
                                    UnitTp = r.UnitTp,
                                    UnitVat = r.UnitVat,
                                    Status = r.Status,
                                    SorgaCode = r.SorgaCode,
                                    PackSize = r.PackSize
                                }
                              ).Distinct().ToList();
                //return products.Where(x=>x.Status=="A").OrderBy(x => x.ProductName);
                return medProds;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getProductForInvestment")]
        public async Task<IEnumerable<ProductDto>> GetProductForInvestment()
        {
            try
            {
                var products = await _productRepo.ListAllAsync();
                var data = _mapper
                    .Map<IReadOnlyList<ProductInfo>, IReadOnlyList<ProductDto>>(products);
                return data.OrderBy(x => x.ProductName);

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("getProductForInvestment/{sbu}")]
        public async Task<IEnumerable<ProductDto>> GetProductForInvestment(string sbu)
        {
            try
            {
                var spec = new ProductSpecification(sbu);
                var products = await _productRepo.ListAsync(spec);
                var data = _mapper
                    .Map<IReadOnlyList<ProductInfo>, IReadOnlyList<ProductDto>>(products);
                return data.OrderBy(x => x.ProductName);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("getAllBrand")]
        public async Task<IReadOnlyList<BrandDto>> GetAllBrand()
        {
            try
            {
                var data = await _productRepo.ListAllAsync();
                var brand = (from r in data
                             where r.Status == "Active"
                             orderby r.BrandName
                             select new BrandDto
                             {
                                 BrandName = r.BrandName,
                                 BrandCode = r.BrandCode,
                             }
                              ).Distinct().ToList();
                return brand;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("insertNewProdFromESO")]
        public void InsertNewProdFromESO()
        {
            try
            {
                DataTable dt = new DataTable();
                //string ConnStr = @"Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.189.11)(PORT = 1522)) (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.189.12)(PORT = 1522)) (ADDRESS = (PROTOCOL = TCP)(HOST = 172.16.148.11)(PORT = 1522)))(CONNECT_DATA = (SERVICE_NAME = ESODB.SQUAREGROUP.COM)));;User Id=DIDS_INFO;Password=dids2202";
                using (var _db = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.189.11)(PORT = 1522)))(CONNECT_DATA =(SERVICE_NAME=ESODB.SQUAREGROUP.COM)(SERVER = DEDICATED)));User Id=DIDS_INFO;Password=dids2202"))
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        objCmd.CommandText = "SELECT PRODUCT_CODE, PRODUCT_NAME,SORGA_CODE FROM ESOS_PRD.VW_DIDS_PRODUCTS WHERE to_char(ENTRY_DATE,'MM') = '03' AND PRODUCT_STATUS='A' ";
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
                var result = _dbContext.Database.ExecuteSqlRaw("Delete FROM dbo.ProductInfoTemp");
                var conString = this._configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.ProductInfoTemp";

                        //[OPTIONAL]: Map the Excel columns with that of the database table.
                        sqlBulkCopy.ColumnMappings.Add("PRODUCT_CODE", "ProductCode");
                        sqlBulkCopy.ColumnMappings.Add("PRODUCT_NAME", "ProductName");
                        sqlBulkCopy.ColumnMappings.Add("SORGA_CODE", "SBU");
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                //var resultSp = _dbContext.Database.ExecuteSqlRaw("EXECUTE SP_InsertNewInstitution");

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("insertNewMedProdFromESO")]
        //[RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public void InsertNewMedProdFromESO()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var _db = new OracleConnection("Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.189.11)(PORT = 1521))(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.189.12)(PORT = 1521))(ADDRESS = (PROTOCOL = TCP)(HOST =172.16.148.11)(PORT = 1521)))(CONNECT_DATA =(SID = ESODB)(SERVER = DEDICATED)));User Id=DIDS_INFO;Password=dids2202"))
                {
                    using (OracleCommand objCmd = new OracleCommand())
                    {
                        objCmd.CommandText = "SELECT PRODUCT_CODE, PRODUCT_NAME,ENTRY_DATE FROM ESOS_PRD.VW_DIDS_PRODUCTS WHERE to_char(ENTRY_DATE,'MM') = '03'";
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
                var result = _dbContext.Database.ExecuteSqlRaw("Delete FROM dbo.MedicineProductTemp");
                var conString = this._configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name.
                        sqlBulkCopy.DestinationTableName = "dbo.MedicineProductTemp";

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
