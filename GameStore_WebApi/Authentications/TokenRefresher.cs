using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GameStore_WebApi.Authentications
{
    /// <summary>
    /// Clase para actualizar un JWT en base a un refresh token. Aqui se haran todas las reglas sobre el refresh token
    /// </summary>
    public class TokenRefresher : ITokenRefresher
    {
        private readonly byte[] keyJwt;
        private readonly IJWTAuthenticationManager jwtAuthenticationManager;
        private readonly ILogService iLog;
        private readonly AppSettings appSettings;
        private readonly IAutenticacionService autenticationService;

        public TokenRefresher(byte[] keyJwt, IJWTAuthenticationManager jwtAuthenticationManager, ILogService iLog,
            AppSettings appSettings, IAutenticacionService autenticationService)
        {
            this.keyJwt = keyJwt;
            this.jwtAuthenticationManager = jwtAuthenticationManager;
            this.iLog = iLog;
            this.appSettings = appSettings;
            this.autenticationService = autenticationService;
        }
        public AuthenticationResponse Refresh(RefreshTokenJwt refreshTojenJwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            var idUsuario = 0;
            try
            {
                var principal = tokenHandler.ValidateToken(refreshTojenJwt.JwtToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyJwt),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,

                }, out validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException(appSettings.MensajeErrorRefreshToken);
                }
                var idUser = principal.Identity.Name;
                Int32.TryParse(idUser, out idUsuario);
                var validaToken = autenticationService.validaRefreshToken(idUsuario, refreshTojenJwt.RefreshToken);
                if (validaToken.IdResponse <= 0)
                {
                    throw new SecurityTokenException(validaToken.Response);
                }
                return jwtAuthenticationManager.Authenticate(idUser, principal.Claims.ToArray(), refreshTojenJwt.RefreshToken);
            }
            catch (Exception ex)
            {
                iLog.guardaLog($"{GetType().Name} - {MethodBase.GetCurrentMethod().Name}", "", idUsuario, ex);
                throw new MiExcepcion(appSettings.MensajeErrorRefreshToken);
            }
        }
    }
}
