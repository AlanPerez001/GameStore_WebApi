using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Authentications
{
    /// <summary>
    /// Clase modelo para generar un nuevo JWT en base a un refresh Token
    /// </summary>
    public class RefreshTokenJwt
    {
        [Required(ErrorMessage = "El JwtToken es requerido")]
        public string JwtToken { get; set; }
        [Required(ErrorMessage = "El RefreshToken es requerido")]
        public string RefreshToken { get; set; }
        [Required(ErrorMessage = "El User es requerido")]
        public string User { get; set; }
    }
}
