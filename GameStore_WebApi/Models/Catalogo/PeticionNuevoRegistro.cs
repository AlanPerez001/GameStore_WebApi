using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.Catalogo
{
    public class PeticionNuevoRegistro
    {
        public int IdJuego { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string AnioPublicacion { get; set; }
        public int Calificacion { get; set; }
        public int idConsola { get; set; }
        public int idGenero { get; set; }
    }
    public class Consola
    {
        public int idConsola { get; set; }
    }
}