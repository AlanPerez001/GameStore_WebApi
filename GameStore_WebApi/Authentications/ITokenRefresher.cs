using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Authentications
{
    /// <summary>
    /// Interfaz para el TokenRefresher
    /// </summary>
    public interface ITokenRefresher
    {
        AuthenticationResponse Refresh(RefreshTokenJwt refreshTojenJwt);
    }
}
