using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.Autenticacion
{
    /// <summary>
    /// Modelo para peticiones de inicio de sesion
    /// </summary>
    public class IniciarSesion
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        //[RegularExpression(@"^(?=.*[a-z-])(?=.*[A-Z-])(?=.*\d)(?=.*[$@$!%*?&#.$($)$-$_-])[A-Za-z\d$@$!%*?&#.$($)$-$_-]{8,15}$", ErrorMessage = "La contraseña debe contener entre 8 y 12 caracteres, incluir un número, una letra mayúscula y un carácter especial.No debe incluir espacios.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre del APP es requerida")]
        public string App { get; set; }

        [Required(ErrorMessage = "El número de versión es requerido")]
        public string Version { get; set; }

        [Required(ErrorMessage = "La plataforma es requerida")]
        public string Plataforma { get; set; }
        public string IpCliente { get; set; }

        public string TokenAPP { get; set; }
    }
}
