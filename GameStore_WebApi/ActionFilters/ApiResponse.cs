using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Clase para manejar las respuestas del servidor. Todas las respuestas incluiran el status Code donde esta el codigo http de respuesta y un message donde esta la descripcion de la respuesta
    /// </summary>
    public class ApiResponse
    {
        public int statusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            this.statusCode = statusCode;
            this.message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                //Agregar mas codigos de error
                case 200:
                    return Startup.respuestasApi.Mensaje200;
                case 400:
                    return Startup.respuestasApi.Mensaje400;
                case 401:
                    return Startup.respuestasApi.Mensaje401;
                case 403:
                    return Startup.respuestasApi.Mensaje403;
                case 404:
                    return Startup.respuestasApi.Mensaje404;
                case 418:
                    return Startup.respuestasApi.Mensaje418;
                case 500:
                    return Startup.respuestasApi.Mensaje500;
                default:
                    return $"Error http {statusCode}";
            }
        }
    }
}
