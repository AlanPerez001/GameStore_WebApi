using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GameStore_WebApi.Authentications
{
    /// <summary>
    /// Clase para manejar la generacion de los JWT
    /// </summary>
    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {

        private readonly string tokenKey;
        private readonly IRefreshTokenGenerator refreshTokenGenerator;
        private readonly ILogService iLog;
        private readonly AppSettings appSettings;
        private readonly IAutenticacionService autenticationService;

        public JWTAuthenticationManager(string tokenKey, IRefreshTokenGenerator refreshTokenGenerator, ILogService iLog,
            AppSettings appSettings, IAutenticacionService autenticationService)
        {
            this.tokenKey = tokenKey;
            this.refreshTokenGenerator = refreshTokenGenerator;
            this.iLog = iLog;
            this.appSettings = appSettings;
            this.autenticationService = autenticationService;
        }

        /// <summary>
        /// Metodo para generar el token de acuerdo a un refresh token
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public AuthenticationResponse Authenticate(string idUser, Claim[] claims, string refreshTokenAnterior)
        {
            AuthenticationResponse res = null;
            var idUsuario = 0;
            try
            {
                Int32.TryParse(idUser, out idUsuario);
                var token = GenerateTokenString(new ClaimsParaJwt() { Id = idUser }, DateTime.UtcNow, claims);
                if (!string.IsNullOrEmpty(token))
                {
                    var refreshToken = refreshTokenGenerator.GenerateToken();
                    var guardaRefreshToken = autenticationService.guardaRefreshToken(idUsuario, refreshToken, token, refreshTokenAnterior);
                    if (guardaRefreshToken.IdResponse > 0)
                    {
                        res = new AuthenticationResponse { JwtToken = token, RefreshToken = refreshToken };
                    }
                    else
                    {
                        throw new MiExcepcion(guardaRefreshToken.Response);
                    }
                }
            }
            catch (Exception ex)
            {
                iLog.guardaLog($"{GetType().Name} - {MethodBase.GetCurrentMethod().Name}", $"", idUsuario, ex);
                throw new MiExcepcion(appSettings.MensajeErrorAutenticacionJWT);
            }
            return res;
        }

        /// <summary>
        /// Metodo para generar el JWT completo por ejemplo desde un login o un registro
        /// </summary>
        /// <param name="datosParaJwt"></param>
        /// <returns></returns>
        public AuthenticationResponse Authenticate(ClaimsParaJwt datosParaJwt)
        {
            AuthenticationResponse res = null;
            var idUsuario = 0;
            try
            {
                Int32.TryParse(datosParaJwt.Id, out idUsuario);
                var token = GenerateTokenString(datosParaJwt, DateTime.UtcNow);
                if (!string.IsNullOrEmpty(token))
                {
                    var refreshToken = refreshTokenGenerator.GenerateToken();
                    var guardaRefreshToken = autenticationService.guardaRefreshToken(idUsuario, refreshToken, token, "");
                    if (guardaRefreshToken.IdResponse > 0)
                    {
                        res = new AuthenticationResponse { JwtToken = token, RefreshToken = refreshToken };
                    }
                    else
                    {
                        throw new MiExcepcion(guardaRefreshToken.Response);
                    }
                }
            }
            catch (Exception ex)
            {
                iLog.guardaLog($"{GetType().Name} - {MethodBase.GetCurrentMethod().Name}", $"", idUsuario, ex);
                throw new MiExcepcion(appSettings.MensajeErrorAutenticacionJWT);
            }
            return res;
        }

        private string GenerateTokenString(ClaimsParaJwt datosParaJwt, DateTime expires, Claim[] claims = null)
        {
            var res = "";
            var idUsuario = 0;
            try
            {
                Int32.TryParse(datosParaJwt.Id, out idUsuario);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(tokenKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims ?? new Claim[] {
                new Claim(ClaimTypes.Name, datosParaJwt.Id.ToString()),
                new Claim(ClaimTypes.Email, datosParaJwt.Usuario),
                new Claim(ClaimTypes.Version, datosParaJwt.Version),
                new Claim(ClaimTypes.System, datosParaJwt.Plataforma),
                new Claim("App", datosParaJwt.App),
                new Claim("TokenAPP", datosParaJwt.TokenApp),
                new Claim("IpCliente", datosParaJwt.IpCliente),
                new Claim(JwtRegisteredClaimNames.Jti, datosParaJwt.GuidString)
                }),
                    //NotBefore = expires,
                    Expires = expires.AddMinutes(15),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                res = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
            }
            catch (Exception ex)
            {
                iLog.guardaLog($"{GetType().Name} - {MethodBase.GetCurrentMethod().Name}", $"", idUsuario, ex);
            }
            return res;
        }
    }
}
