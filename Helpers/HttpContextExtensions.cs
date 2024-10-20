using Microsoft.EntityFrameworkCore;

namespace MeterReaderAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParametersPagintionInHelper<T>(this HttpContext httpContext, IQueryable<T> queryable)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            double count = await queryable.CountAsync();
            httpContext.Response.Headers.Add("totalAmountOfRcords", count.ToString());
        }
    }
}
