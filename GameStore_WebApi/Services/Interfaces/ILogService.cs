using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services.Interfaces
{
    public interface ILogService
    {
        public int guardaLog(string nombre, string datos, int idUsuario, Exception exParameter);
        public int guardaLog(string nombre, string datos, string adicionales, int idUsuario);

    }
}
