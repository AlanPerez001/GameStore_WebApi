using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.ActionFilters
{
    /// <summary>
    /// Clase auxiliar para definir un error sobre los campos invalidos dentro de un model invalido
    /// </summary>
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Campo { get; }

        public string Mensaje { get; }

        public ValidationError(string field, string message)
        {
            Campo = field != string.Empty ? field : null;
            Mensaje = message;
        }
    }
}
