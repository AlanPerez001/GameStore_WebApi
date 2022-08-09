using GameStore_WebApi.ActionFilters;
using GameStore_WebApi.Authentications;
using GameStore_WebApi.Models;
using GameStore_WebApi.Models.Catalogo;
using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;

namespace GameStore_WebApi.Controllers
{
    [Produces("application/json")]//Esta es para que la documentacion swagger indique que devuelve json
    [Route("v1/[controller]")]//el v1 es para que se puedan manejar versiones si es que se necesitan
    [ApiController]
    [ServiceFilter(typeof(ModelValidatorFilter))]//Esta es para que de acuerdo a ese filter se validen o no los modelos que se reciban
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CatalogoController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _enviroment;
        private readonly IJWTAuthenticationManager _jwtAuthenticationManager;
        private readonly ITokenRefresher _tokenRefresher;
        private readonly ILogService log;
        private readonly ICatalogoService catalogoService;

        public CatalogoController(IOptions<AppSettings> appSettings,
            IWebHostEnvironment enviroment, IJWTAuthenticationManager jwtAuthenticationManager,
            ITokenRefresher tokenRefresher, ILogService log, ICatalogoService catalogoService)
        {
            _appSettings = appSettings.Value;
            _enviroment = enviroment;
            _jwtAuthenticationManager = jwtAuthenticationManager;
            this._tokenRefresher = tokenRefresher;
            this.log = log;
            this.catalogoService = catalogoService;
        }


        /// <summary>
        /// Metodo para obtener el catalogo completo de videojuegos 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ObtenerVidejuegos")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(Api200Response<RespuestaObtenerVideoJuegos>), 200)]
        public IActionResult ObtenerVidejuegos()
        {
            try
            {
                var claims = User.Claims.ToList();
                var idU = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (idU.Value == null)
                    return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));

                RespuestaObtenerVideoJuegos res = catalogoService.ObtenerCatalogoVideojuegos();
                return Ok(new Api200Response<RespuestaObtenerVideoJuegos>(res));
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
        /// Metodo para obtener el catalogo completo de videojuegos y filtrar por Genero o consola
        /// <paramref name="idFiltro"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ObtenerFiltros/{idFiltro}")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(Api200Response<RespuestaGetfiltros>), 200)]
        public IActionResult ObtenerFiltros(int idFiltro)
        {
            try
            {
                var claims = User.Claims.ToList();
                var idU = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (idU.Value == null)
                    return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));

                RespuestaGetfiltros res = catalogoService.ObtenerFiltros(idFiltro);
                return Ok(new Api200Response<RespuestaGetfiltros>(res));
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
        /// Metodo para obtener el catalogo filtrado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ObtenerjuegosFiltro/{idGenero}/{idConsola}")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(Api200Response<RespuestaObtenerVideoJuegos>), 200)]
        public IActionResult ObtenerjuegosFiltro(int idGenero, int idConsola)
        {
            try
            {
                var claims = User.Claims.ToList();
                var idU = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (idU.Value == null)
                    return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));

                RespuestaObtenerVideoJuegos res = catalogoService.ObtenerCatalogoFiltradoVideojuegos(idGenero, idConsola);
                return Ok(new Api200Response<RespuestaObtenerVideoJuegos>(res));
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
        /// Metodo para obtener el detalle de un registro
        /// </summary>
        /// <param name="idJuego"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ObtenerDetalleJuego/{idJuego}")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(Api200Response<RespuestaObtenerDetalleVideoJuego>), 200)]
        public IActionResult ObtenerDetalleJuego(int idJuego)
        {
            try
            {
                var claims = User.Claims.ToList();
                var idU = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (idU.Value == null)
                    return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));

                RespuestaObtenerDetalleVideoJuego res = catalogoService.ObtenerDetalleJuego(idJuego);
                return Ok(new Api200Response<RespuestaObtenerDetalleVideoJuego>(res));
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
        /// Metodo para crear o actualizar un videojuego en el catalogo
        /// <paramref name="model"/>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GuardaRegistro")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(Api200Response<RespuestaGeneral>), 200)]
        public IActionResult GuardaRegistro([FromBody] PeticionNuevoRegistro model)
        {
            try
            {
                var claims = User.Claims.ToList();
                var idU = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (idU.Value == null)
                    return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));

                RespuestaGeneral res = catalogoService.GuardaRegistro(model);
                return Ok(new Api200Response<RespuestaGeneral>(res));
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
        /// Metodo para eliminar un registro del catalogo
        /// <paramref name="model"/>
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("EliminaRegistro")]
        [ProducesResponseType(typeof(ApiResponse), 500)]
        [ProducesResponseType(typeof(ApiResponse), 401)]
        [ProducesResponseType(typeof(ApiResponse), 403)]
        [ProducesResponseType(typeof(Api200Response<RespuestaGeneral>), 200)]
        public IActionResult EliminaRegistro([FromBody] PeticionEliminar model)
        {
            try
            {
                var claims = User.Claims.ToList();
                var idU = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                if (idU.Value == null)
                    return new ObjectResult(new ApiResponse(401, _appSettings.Mensaje401));

                RespuestaGeneral res = catalogoService.EliminaRegistro(model);
                return Ok(new Api200Response<RespuestaGeneral>(res));
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

    }
}
