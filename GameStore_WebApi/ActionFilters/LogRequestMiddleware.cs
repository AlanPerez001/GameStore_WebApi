using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Middleware para poder obtener las peticiones y las respuestas del web api y poder logearlas.
    /// </summary>
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogService iLog;
        private readonly AppSettings appSettings;

        public LogRequestMiddleware(RequestDelegate next, ILogService iLog, IOptions<AppSettings> appSettings)
        {
            _next = next;
            this.iLog = iLog;
            this.appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var xx = Activity.Current;
            Debug.WriteLine($"LogRequestMiddleware TraceId{xx.TraceId} - Id{xx.Id} - RootId{xx.RootId}");
            string contenido = context.Request.ContentType;
            var metodo = context.Request.Method;
            var autorizacion = string.IsNullOrEmpty(context.Request.Headers["Authorization"]) ? "" : context.Request.Headers["Authorization"].ToString();
            var idUsuario = 0;
            var response = "";
            var datosBody = "";
            ////Se valida si es una peticion con autorizacion para poder sacar el idusuario y poder identificar de manera mas facil quien manda las peticiones
            if (!string.IsNullOrEmpty(autorizacion))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                try
                {
                    var principal = tokenHandler.ValidateToken(autorizacion.Replace("Bearer ", ""), new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.KEYJWT)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero,

                    }, out validatedToken);
                    var jwtToken = validatedToken as JwtSecurityToken;
                    if (jwtToken != null || jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var conversionValida = int.TryParse(principal.Identity.Name, out idUsuario);
                    }
                }
                catch (Exception ex)
                {
                    iLog.guardaLog($"{GetType().Name} - Invoke", $"{ex.Message}", idUsuario, ex);
                }
            }

            var url = UriHelper.GetDisplayUrl(context.Request);
            var ipRemote = context.Connection.RemoteIpAddress;
            var ipLocal = context.Connection.LocalIpAddress;
            var puerto = context.Connection.RemotePort;
            var puero2 = context.Connection.LocalPort;
            ////Se obtienen los datos de la peticion para guardarlos como log
            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;
            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            datosBody = new StreamReader(requestBodyStream).ReadToEnd();
            requestBodyStream.Seek(0, SeekOrigin.Begin);
            context.Request.Body = requestBodyStream;
            if (!string.IsNullOrEmpty(contenido) && contenido.Contains("multipart/form-data"))
            {
                ////Si la peticion es multipart , solo guardamos lo que contenga datos tipo json
                var datosGuardadoMultipart = "";
                foreach (var formData in datosBody.Split("Upload"))
                {
                    if (formData.Contains("application/json"))
                    {
                        foreach (var salto in formData.Split("\r\n"))
                        {
                            if (!salto.Contains("--") && !string.IsNullOrEmpty(salto) && !salto.Contains("Content-"))
                            {
                                datosGuardadoMultipart += salto;
                            }
                        }
                    }
                }
                datosBody = datosGuardadoMultipart;
            }
            var guardarLog = true;
            var IgnorarMetodosLog = appSettings.IgnorarMetodosLog.Split(",");
            foreach (string x in IgnorarMetodosLog)
            {
                if (url.Contains(x))
                {
                    guardarLog = false;
                }
            }
            string[] palabrasRechazadas = appSettings.PalabrasSQLInjection.Split(",");
            foreach (string x in palabrasRechazadas)
            {
                if (datosBody.Contains(x))
                {
                    context.Response.ContentType = "application/json";
                    var apiResponse = new ApiResponse(418);
                    var json = JsonConvert.SerializeObject(apiResponse);
                    iLog.guardaLog($"{this.GetType().Name} - Invoke", $"respuesta sql injection {json}", idUsuario, null);
                    await context.Response.WriteAsync(json);
                    return;
                }
            }
            try
            {
                //Copy a pointer to the original response body stream
                var originalBodyStream = context.Response.Body;
                //Create a new memory stream...
                using (var responseBody = new MemoryStream())
                {
                    ////...and use that for the temporary response body
                    context.Response.Body = responseBody;
                    //Continue down the Middleware pipeline, eventually returning to this class
                    await _next(context);
                    //Format the response from the server
                    response = await FormatResponse(context.Response);
                    if (!url.Contains("swagger"))
                    {
                        iLog.guardaLog($"{this.GetType().Name} - Invoke", $"respuesta {response}",
                            $"url: {url} | PETICION:{(guardarLog ? datosBody : "")} | metodo:{metodo} | contenido:{contenido} | ipremote:{ipRemote} | ip:local{ipLocal}", idUsuario);
                    }
                    ////Copy the contents of the new memory stream(which contains the response) to the original stream, which is then returned to the client.
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            catch (Exception ex)
            {
                iLog.guardaLog($"{this.GetType().Name} - Invoke", $"respuesta {response}",
                            $"url: {url} | PETICION:{(guardarLog ? datosBody : "")} | metodo:{metodo} | contenido:{contenido} | ipremote:{ipRemote} | ip:local{ipLocal}", idUsuario);
                iLog.guardaLog($"{GetType().Name} - Invoke", $"{ex.Message} - {ex.StackTrace}", idUsuario, ex);
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);
            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);
            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return text;
        }


    }
}
