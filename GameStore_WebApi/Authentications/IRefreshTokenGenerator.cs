using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Authentications
{
    /// <summary>
    /// Interfaz que para el RefreshTokenGenerator 
    /// </summary>
    public interface IRefreshTokenGenerator
    {
        string GenerateToken();
    }
}
