using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Contracts;
using Common.Contracts.Entities;
using Common.Contracts.Queries;
using Common.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Common.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> query, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return query;

            var props = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(p => p.Trim())
                               .ToArray();

            var parameter = Expression.Parameter(typeof(T), "x");
            bool first = true;

            foreach (var prop in props)
            {
                var parts = prop.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var propertyName = parts[0];
                bool descending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

                Expression propertyAccess = parameter;
                foreach (var member in propertyName.Split('.'))
                {
                    propertyAccess = Expression.PropertyOrField(propertyAccess, member);
                }

                var keySelector = Expression.Lambda(propertyAccess, parameter);

                string methodName;
                if (first)
                {
                    methodName = descending ? "OrderByDescending" : "OrderBy";
                    first = false;
                }
                else
                {
                    methodName = descending ? "ThenByDescending" : "ThenBy";
                }
                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName
                                && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), propertyAccess.Type);
                query = (IQueryable<T>)method.Invoke(null, new object[] { query, keySelector })!;
            }

            return query;
        }

        public static async Task<IPagedList<ResultType>> PaginateAsync<DomainEntity,ResultType>(this IQueryable<DomainEntity> source, IMapper mapper, IQuerieFilter filters)
            where DomainEntity : class, IEntity
            where ResultType : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).ProjectTo<ResultType>(mapper.ConfigurationProvider).ToListAsync();
            return new PagedList<ResultType>(items, count, filters.Page, filters.PageSize);
        }

        public static async Task<IPagedList<ResultType>> PaginateAsync<DomainEntity, ResultType>(this IQueryable<DomainEntity> source, IMapper mapper, int page, int pageSize)
        where DomainEntity : class, IEntity
        where ResultType : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<ResultType>(mapper.ConfigurationProvider).ToListAsync();
            return new PagedList<ResultType>(items, count, page, pageSize);
        }
    }
}
