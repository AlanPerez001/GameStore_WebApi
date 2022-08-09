using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.Catalogo
{
    public class RespuestaGetfiltros
    {
        public int IdResponse { get; set; }
        public string Response { get; set; }
        public List<filtros> Datos { get; set; }
    }
    public class filtros
    {

        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}
