using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameStore_WebApi.Authentications
{
    /// <summary>
    /// Interfaz para el JWT authentication manager
    /// </summary>
    public interface IJWTAuthenticationManager
    {
        AuthenticationResponse Authenticate(ClaimsParaJwt datosParaJwt);
        AuthenticationResponse Authenticate(string idUser, Claim[] claims, string refreshTokenAnterior);
    }
}
