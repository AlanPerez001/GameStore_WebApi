using GameStore_WebApi.Utility;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Clase para personalizar la respuesta de modelos invalidos
    /// </summary>
    public class ApiBadRequestResponse : ApiResponse
    {
        private readonly AppSettings appSettings;

        public IEnumerable<ValidationError> Errors { get; }



        public ApiBadRequestResponse(ModelStateDictionary modelState)
            : base(400)
        {
            if (modelState.IsValid)
            {
                throw new ArgumentException(Startup.respuestasApi.Mensaje400, nameof(modelState));
            }

            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}
