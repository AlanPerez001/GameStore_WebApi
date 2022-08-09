using GameStore_WebApi.Models;
using GameStore_WebApi.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services.Interfaces
{
    /// <summary>
    ///  Interfaz para inplementar las conexiones a la base de datos
    /// </summary>
    public interface ICatalogoService
    {
        public RespuestaObtenerVideoJuegos ObtenerCatalogoVideojuegos();
        public RespuestaObtenerDetalleVideoJuego ObtenerDetalleJuego(int idJuego);
        public RespuestaObtenerVideoJuegos ObtenerCatalogoFiltradoVideojuegos(int idgenero, int idConsola);
        public RespuestaGetfiltros ObtenerFiltros(int idFiltro);
        public RespuestaGeneral GuardaRegistro(PeticionNuevoRegistro model);
        public RespuestaGeneral EliminaRegistro(PeticionEliminar model);
    }
}
