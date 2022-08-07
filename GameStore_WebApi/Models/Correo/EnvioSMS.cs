using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.Correo
{
    public class EnvioSMS
    {
        public string api_key { get; set; }
        public string api_secret { get; set; }
        public string CodigoPais { get; set; }
        public string Text { get; set; }
        public string to { get; set; }
        public string type { get; set; }
    }
}
