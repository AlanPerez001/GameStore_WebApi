using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.Autenticacion
{
    public class Login
    {
        public Login()
        {

        }
        public Login(int idUsuario, string accion)
        {
            IdUsuario = idUsuario;
            Accion = accion;
        }

        public int IdUsuario { get; }
        public string Accion { get; }
    }
}
