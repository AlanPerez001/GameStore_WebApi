using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Clase que engloba los diferentes modelos de respuestas que se tienen.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Api200Response<T> : ApiResponse
    {
        public T respuesta { get; set; }

        public Api200Response(T respuesta) : base(200)
        {
            this.respuesta = respuesta;
        }
    }
}
