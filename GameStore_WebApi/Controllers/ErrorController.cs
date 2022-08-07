using GameStore_WebApi.ActionFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Controllers
{
    /// <summary>
    /// Controlador que sirve para que se manejen los errores que se generen en el web api como un 404
    /// </summary>
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [Route("error/{code}")]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
