using Common.Contracts;
using Common.Contracts.Behavior.Search;
using Common.Contracts.Queries;
using Common.Infrastructure.Entities;
using Common.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTO;
using Product.Domain.Filters.Querie;

namespace Product.Application.Behaviors.Category
{
    public class CategorySearchQuerieBulkBehavior : ISearchQuerieBulkBehavior<Domain.Entities.Category, CategoryDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public int Order => 1;
        public CategorySearchQuerieBulkBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> OnAfterAsync(BaseResponse response, CancellationToken cancellationToken)
        {
            if(response is Response<IPagedList<CategoryDTO>> res)
            {
                foreach (var c in res.Data.SelectMany(x => x.Children).Where(x => x.Children == null))
                {
                    var childrens = await _unitOfWork.SearchAsync<Domain.Entities.Category>(new CategoryQuerieFilter { 
                        OnlyParents = true,
                    }, cancellationToken);
                }
                response = res;
            }
            return response;
        }

        public async Task<IQuerieFilter> OnBeforeAsync(IQuerieFilter filter, CancellationToken cancellationToken)
        {
            return filter;
        }
    }
}
