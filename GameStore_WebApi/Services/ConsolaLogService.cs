using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services
{
    public class ConsolaLogService : ILogService
    {
        public int guardaLog(string nombre, string datos, int idUsuario, Exception exParameter)
        {
            var mensajeExcepcion = Generales.exceptionToString(exParameter);
            Debug.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - {Activity.Current.RootId} - {idUsuario} - {nombre} - {datos} - { mensajeExcepcion}");
            return 1;
        }
        public int guardaLog(string nombre, string datos, string adicionales, int idUsuario)
        {
            Debug.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - {Activity.Current.RootId} - {idUsuario} - {nombre} - {datos} - { adicionales}");
            return 1;
        }
    }
}
