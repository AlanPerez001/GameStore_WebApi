using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Utility
{
    /// <summary>
    /// Clase auxiliar para representar las cadenas de conexion de ConnectionStrings de appsettings.json
    /// </summary>
    public class ConnectionStrings
    {
        public string Str { get; set; }
        public string StrLogs { get; set; }
    }
}
