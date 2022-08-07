using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Extension para que sea inyectado el middleware
    /// </summary>
    public static class LogRequestMiddlewareExtension
    {
        public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<LogRequestMiddleware>();
        }
    }
}
