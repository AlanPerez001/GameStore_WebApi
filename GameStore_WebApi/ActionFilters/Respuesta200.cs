using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Clase para devolver las respuestas exitosas HTTP 200.
    /// OJO en un login puede tener un error de credenciales no validas pero aun asi se mandaria es una respuesta HTTP 200
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Respuesta200<T>
    {
        public int idRespuesta { get; set; }
        public string mensaje { get; set; }
        public T datos { get; set; }

        public Respuesta200(int idRespuesta, string mensaje, T datos)
        {
            this.idRespuesta = idRespuesta;
            this.mensaje = mensaje;
            this.datos = datos;
        }
    }
}
