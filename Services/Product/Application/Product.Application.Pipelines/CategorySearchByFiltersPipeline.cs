
using Common.Contracts.Behavior.Search;
using Common.Contracts.Queries;
using Common.Infrastructure.Entities;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTO;
using Product.Domain.Entities;

namespace Product.Application.Pipelines
{
    public class CategorySearchByFiltersPipeline : ISearchQuerieBulkBehavior<Category, CategoryDTO>
    {
        private readonly DbContext _ctx;
        private readonly IMapper _mapper;

        public int Order => 1;

        public CategorySearchByFiltersPipeline(DbContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken)
        {
            var categories = _ctx.Set<Category>().Where(x => !x.ParentId.HasValue).ToList();
            var categoriesDTO = _mapper.Map<List<CategoryDTO>>(categories);
            return response;
        }

        public async Task<IQuerieFilter> OnBeforeAsync(IQuerieFilter filter, CancellationToken cancellationToken)
        {
            return filter;
        }
    }
}
