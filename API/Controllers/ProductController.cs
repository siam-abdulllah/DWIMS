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

        [HttpGet("getBrand")]
        public async Task<IReadOnlyList<BrandDto>> GetBrand()
        {
            try
            {
                var data = await _productRepo.ListAllAsync();
                var brand= (from r in data
                            where r.Status=="Active"
                           orderby r.BrandName
                           select new BrandDto
                           {
                               BrandName = r.BrandName.Trim(),
                               BrandCode = r.BrandCode.Trim()
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
        [HttpGet("getProductForInvestment")]
        public async Task<IReadOnlyList<ProductDto>> GetProductForInvestment()
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
        } [HttpGet("getProductForInvestment/{sbu}")]
        public async Task<IReadOnlyList<ProductDto>> GetProductForInvestment(string sbu)
        {
            try
            {
                var spec = new ProductSpecification(sbu);
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
    }
}
