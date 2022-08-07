using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Utility
{
    /// <summary>
    /// Clase auxiliar para representar los datos parametrizados que se guardan en el AppSettings de appsettings.json
    /// </summary>
    public class AppSettings
    {
        public string DirLogTxt { get; set; }
        public string NameLogTxt { get; set; }
        public string KEYJWT { get; set; }
        public string IgnorarMetodosLog { get; set; }


        public string PalabrasSQLInjection { get; set; }
        public string MensajeErrorExcepcion { get; set; }
        public string MensajeErrorExcepcionDB { get; set; }
        public string MensajeErrorAutenticacionJWT { get; set; }
        public string MensajeErrorRefreshToken { get; set; }

        public string Mensaje200 { get; set; }
        public string Mensaje400 { get; set; }
        public string Mensaje401 { get; set; }
        public string Mensaje403 { get; set; }
        public string Mensaje404 { get; set; }
        public string Mensaje418 { get; set; }
        public string Mensaje500 { get; set; }

        public string MensajeCorreoNoEnviado { get; set; }
        public string MensajeCorreoEnviado { get; set; }



        public string BaseUrlActiva { get; set; }

        public string UrlWeb { get; set; }


        public string CryptoServiceKey { get; set; }




    }
}
