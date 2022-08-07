using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.ZhortMe
{
    public class ZhortMe_Response
    {
        public int idResponse { get; set; }
        public string description { get; set; }
        public string ligaOriginal { get; set; }
        public string ligaAcortada { get; set; }
        public string fechaCaducidad { get; set; }
    }
}
