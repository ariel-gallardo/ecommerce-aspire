using Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Common.Api.Filters.Controllers
{
    public class ControllerPaginationFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is not null)
            {
                var responseValue = objectResult.Value;
                var responseType = responseValue.GetType();

                // obtener propiedad Data dentro del Response<T>
                var dataProp = responseType.GetProperty("Data");
                if (dataProp == null)
                {
                    await next();
                    return;
                }

                var dataValue = dataProp.GetValue(responseValue);
                if (dataValue == null)
                {
                    await next();
                    return;
                }

                var dataType = dataValue.GetType();

                // detectar si Data implementa IPagedList<>
                var pagedInterface = dataType
                    .GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IPagedList<>));

                if (pagedInterface != null)
                {
                    var currentPage = (int)dataType.GetProperty("CurrentPage")!.GetValue(dataValue)!;
                    var totalPages = (int)dataType.GetProperty("TotalPages")!.GetValue(dataValue)!;
                    var pageSize = (int)dataType.GetProperty("PageSize")!.GetValue(dataValue)!;
                    var totalCount = (int)dataType.GetProperty("TotalCount")!.GetValue(dataValue)!;

                    var headers = context.HttpContext.Response.Headers;

                    headers["CurrentPage"] = currentPage.ToString();
                    headers["TotalPages"] = totalPages.ToString();
                    headers["PageSize"] = pageSize.ToString();
                    headers["TotalCount"] = totalCount.ToString();
                }
            }

            await next();
        }
    }
}
