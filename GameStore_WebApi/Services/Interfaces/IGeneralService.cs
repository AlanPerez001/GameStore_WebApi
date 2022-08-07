using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services.Interfaces
{
    public interface IGeneralService
    {
        string getDato(string tabla, string campo, string condicion, List<SqlParameter> parametros, int idUsuario);
    }
}
