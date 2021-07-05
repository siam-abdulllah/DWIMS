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
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PostController: BaseApiController
    {
         private readonly IGenericRepository<Post> _postRepo;
        private readonly IMapper _mapper;
        public PostController(IGenericRepository<Post> postRepo,       
        IMapper mapper)
        {
            _mapper = mapper;
            _postRepo = postRepo;
        }

          
        [HttpGet]
        public async Task<ActionResult<Pagination<PostToReturnDto>>> GetPosts(
            [FromQuery]PostSpecParams postParrams)
        {
            var spec = new PostWithCommentsSpecification(postParrams);

            var countSpec = new PostWithFiltersForCountSpecificication(postParrams);

            var totalItems = await _postRepo.CountAsync(countSpec);

            var posts = await _postRepo.ListAsync(spec);

            var data = _mapper
                .Map<IReadOnlyList<Post>, IReadOnlyList<PostToReturnDto>>(posts);

            return Ok(new Pagination<PostToReturnDto>(postParrams.PageIndex, postParrams.PageSize, totalItems, data));
        }
        
        
    }
}