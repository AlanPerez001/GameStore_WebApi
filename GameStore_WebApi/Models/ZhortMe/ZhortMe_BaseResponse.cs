using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Models.ZhortMe
{
    public class ZhortMe_BaseResponse
    {
        public int statusCode { get; set; }
        public string message { get; set; }
        public ZhortMe_Response respuesta { get; set; }
    }
}
