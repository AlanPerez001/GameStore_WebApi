using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models
{
    public class RespuestaGeneral
    {
        public RespuestaGeneral()
        {

        }
        public RespuestaGeneral(int idResponse, string response)
        {
            IdResponse = idResponse;
            Response = response;
        }
        public int IdResponse { get; set; }
        public string Response { get; set; }
    }
}
