using GameStore_WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Este middleware personaliza las respuesta de error para que se devuelvan en formato json
    /// y con esto todas las respuestas del controlador serian en ese formato.
    /// Referencia https://www.devtrends.co.uk/blog/handling-errors-in-asp.net-core-web-api
    /// </summary>
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogService iLog;

        public ErrorWrappingMiddleware(RequestDelegate next, ILogService iLog)
        {
            _next = next;
            this.iLog = iLog;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                iLog.guardaLog($"{GetType().Name} - Invoke", $"", 0, ex);
                context.Response.StatusCode = 500;
            }

            try
            {
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";

                    var response = new ApiResponse(context.Response.StatusCode);

                    var json = JsonConvert.SerializeObject(response);
                    iLog.guardaLog($"{GetType().Name} - Invoke", $"{json}", "", 0);

                    await context.Response.WriteAsync(json);
                }
            }
            catch (Exception ex)
            {
                iLog.guardaLog($"{GetType().Name} - Invoke", $"", 0, ex);
                context.Response.StatusCode = 500;
            }


        }
    }
}
