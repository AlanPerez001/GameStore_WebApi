using GameStore_WebApi.Models;
using GameStore_WebApi.Models.Autenticacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services.Interfaces
{
    /// <summary>
    ///  Interfaz para inplementar las conexiones a la base de datos para la Autenticacion
    /// </summary>
    public interface IAutenticacionService
    {
        public Login iniciaSesion(IniciarSesion modelo);
        public RespuestaGeneral guardaRefreshToken(int idUsuario, string refreshToken, string jwt, string refreshTokenAnterior);
        public RespuestaGeneral validaRefreshToken(int idUsuario, string refreshToken);
    }
}
