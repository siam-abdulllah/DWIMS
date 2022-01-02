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

namespace API.Controllers
{
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<ProductInfo> _productRepo;
        private readonly IMapper _mapper;
        public ProductController(IGenericRepository<ProductInfo> productRepo,
        IMapper mapper)
        {
            _mapper = mapper;
            _productRepo = productRepo;
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
        public async Task<List<ProductDto>> GetProduct(string brandCode,string sbu)
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
    }
}
