using Common.Infrastructure.Contracts;
using Common.Infrastructure.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Common.Infrastructure.Persistence.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyModifiers<T>(this IQueryable<T> query, IServiceProvider sp) where T : class
        {
            var modifiers = sp.GetServices<IQueryModifier<T>>();
            foreach (var modifier in modifiers)
            {
                query = modifier.Apply(query);
            }
            return query;
        }
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
            var equals = typeof(DomainEntity).Equals(typeof(ResultType));
            var count = await source.CountAsync();
            if (!equals)
            {
                var items = filters.TakeAll.HasValue && filters.TakeAll.Value
                ? await source.ProjectToType<ResultType>().ToListAsync()
                : await source.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).ProjectToType<ResultType>(mapper.Config).ToListAsync();
                return new PagedList<ResultType>(items, count, filters.Page, filters.PageSize, filters.TakeAll.HasValue ? filters.TakeAll.Value : false);
            }
            else
            {
                var items = filters.TakeAll.HasValue && filters.TakeAll.Value
                ? await source.ToListAsync()
                : await source.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).ToListAsync();
                return new PagedList<DomainEntity>(items, count, filters.Page, filters.PageSize, filters.TakeAll.HasValue ? filters.TakeAll.Value : false) as IPagedList<ResultType>;
            }

        }

        public static async Task<IPagedList<ResultType>> PaginateAsync<DomainEntity, ResultType>(this IQueryable<DomainEntity> source, IMapper mapper, int page, int pageSize)
        where DomainEntity : class, IEntity
        where ResultType : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ProjectToType<ResultType>(mapper.Config).ToListAsync();
            return new PagedList<ResultType>(items, count, page, pageSize);
        }

        public static async Task<IPagedList<ResultType>> PaginateAsync<ResultType>(this IQueryable<IIdentifiable> source, IMapper mapper, IQuerieFilter filters)
        where ResultType : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).ProjectToType<ResultType>(mapper.Config).ToListAsync();
            return new PagedList<ResultType>(items, count, filters.Page, filters.PageSize);
        }

        public static async Task<IPagedList<ResultType>> PaginateAsync<ResultType>(this IQueryable<IIdentifiable> source, IMapper mapper, int page, int pageSize)
        where ResultType : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ProjectToType<ResultType>(mapper.Config).ToListAsync();
            return new PagedList<ResultType>(items, count, page, pageSize);
        }

        public static async Task<IPagedList<ResultType>> PaginateAsync<ResultType>(this IQueryable<IIdentifiableGuid> source, IMapper mapper, IQuerieFilter filters)
        where ResultType : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((filters.Page - 1) * filters.PageSize).Take(filters.PageSize).ProjectToType<ResultType>(mapper.Config).ToListAsync();
            return new PagedList<ResultType>(items, count, filters.Page, filters.PageSize);
        }

        public static async Task<IPagedList<ResultType>> PaginateAsync<ResultType>(this IQueryable<IIdentifiableGuid> source, IMapper mapper, int page, int pageSize)
        where ResultType : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ProjectToType<ResultType>(mapper.Config).ToListAsync();
            return new PagedList<ResultType>(items, count, page, pageSize);
        }
    }
}
