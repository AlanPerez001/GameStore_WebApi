using GameStore_WebApi.ActionFilters;
using GameStore_WebApi.Authentications;
using GameStore_WebApi.Models;
using GameStore_WebApi.Models.Autenticacion;
using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Controllers
{
    [Produces("application/json")]//Esta es para que la documentacion swagger indique que devuelve json
    [Route("v1/[controller]")]//el v1 es para que se puedan manejar versiones si es que se necesitan
    [ApiController]
    [ServiceFilter(typeof(ModelValidatorFilter))]//Esta es para que de acuerdo a ese filter se validen o no los modelos que se reciban

    public class AutenticacionController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _enviroment;
        private readonly IJWTAuthenticationManager _jwtAuthenticationManager;
        private readonly ITokenRefresher _tokenRefresher;
        private readonly ILogService log;
        private readonly IAutenticacionService autenticacionService;

        public AutenticacionController(IOptions<AppSettings> appSettings,
            IWebHostEnvironment enviroment, IJWTAuthenticationManager jwtAuthenticationManager,
            ITokenRefresher tokenRefresher, ILogService log, IAutenticacionService autenticacionService)
        {
            _appSettings = appSettings.Value;
            _enviroment = enviroment;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            this._tokenRefresher = tokenRefresher;
            this.log = log;
            this.autenticacionService = autenticacionService;
        }


        /// <summary>
        /// Metodo para iniciar sesion y obtejer el JWT y el RefreshToken. El JWT sirve para autentificarse en los controladores que requieran autentificacion
        /// y el RefreshToken sirve para obtener un nuevo JWT en caso de que ya haya vencido. Regularmente dura 15 minutos el JWT
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(Api200Response<RespuestaAutenticacionToken>), 200)]
        public IActionResult IniciarSesion([FromBody] IniciarSesion modelo)
        {
            try
            {
                Login respDB = autenticacionService.iniciaSesion(modelo);
                if (respDB.IdUsuario > 0)
                {
                    var claimsParaJwt = new ClaimsParaJwt()
                    {
                        Id = respDB.IdUsuario.ToString(),
                        Usuario = modelo.Usuario,
                        App = modelo.App,
                        Plataforma = modelo.Plataforma,
                        TokenApp = modelo.TokenAPP,
                        Version = modelo.Version,
                        GuidString = Guid.NewGuid().ToString()
                    };
                    var token = _jwtAuthenticationManager.Authenticate(claimsParaJwt);
                    if (token == null)
                        return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));
                    RespuestaAutenticacionToken resModel = new RespuestaAutenticacionToken(1, respDB.Accion, token);
                    return Ok(new Api200Response<RespuestaAutenticacionToken>(resModel));
                }
                else
                {
                    return new ObjectResult(new ApiResponse(403, respDB.Accion));
                }
            }
            catch (MiExcepcion ex)
            {
                return new ObjectResult(new ApiResponse(500, ex.Message));
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ApiResponse(500, _appSettings.MensajeErrorExcepcion));
            }
        }


        /// <summary>
        /// Metodo para obtener un nuevo JWT en caso de que ya haya vencido.
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(Api200Response<RespuestaAutenticacionToken>), 200)]
        public IActionResult RefreshToken([FromBody] RefreshTokenJwt modelo)
        {
            try
            {
                var token = _tokenRefresher.Refresh(modelo);
                if (token == null)
                    return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));
                RespuestaAutenticacionToken resModel = new RespuestaAutenticacionToken(1, _appSettings.Mensaje200, token);
                return Ok(new Api200Response<RespuestaAutenticacionToken>(resModel));
            }
            catch (MiExcepcion ex)
            {
                return new ObjectResult(new ApiResponse(500, ex.Message));
            }
            catch (Exception)
            {
                return new ObjectResult(new ApiResponse(500, _appSettings.MensajeErrorExcepcion));
            }
        }






    }
}
