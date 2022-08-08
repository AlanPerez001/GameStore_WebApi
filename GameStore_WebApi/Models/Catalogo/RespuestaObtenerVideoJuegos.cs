using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.Catalogo
{
    public class RespuestaObtenerVideoJuegos
    {
        public int idResponse { get; set; }
        public string Response { get; set; }
        public List<VideoJuegos> Datos { get; set; }
    }

    public class VideoJuegos
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string AnioPublicacion { get; set; }
        public int Calificacion { get; set; }
        public string Consolas { get; set; }
        public string Genero { get; set; }
    }
}
