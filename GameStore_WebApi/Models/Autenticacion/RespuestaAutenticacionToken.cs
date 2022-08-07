using GameStore_WebApi.ActionFilters;
using GameStore_WebApi.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.Autenticacion
{
    public class RespuestaAutenticacionToken : Respuesta200General
    {
        public AuthenticationResponse datos { get; set; }

        public RespuestaAutenticacionToken(int idRespuesta, string mensaje, AuthenticationResponse datos)
        {
            this.datos = datos;
            this.idRespuesta = idRespuesta;
            this.mensaje = mensaje;
        }
    }
}
