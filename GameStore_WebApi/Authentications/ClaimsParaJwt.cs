using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Authentications
{
    /// <summary>
    /// Clase para tener un modelo con los Claims que contendra el Token de JWT
    /// </summary>
    public class ClaimsParaJwt
    {
        public string Id { get; set; }
        public string Usuario { get; set; } = "";
        public string Version { get; set; } = "";
        public string Plataforma { get; set; } = "";
        public string App { get; set; } = "";
        public string TokenApp { get; set; } = "";
        public string GuidString { get; set; } = "";
        public string IpCliente { get; set; } = "";
    }
}
