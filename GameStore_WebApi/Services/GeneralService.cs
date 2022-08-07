using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services
{
    public class GeneralService : IGeneralService
    {
        private readonly ConnectionStrings connectionStrings;
        private readonly ILogService iLogDbService;
        private int timeoutCommand = 300;
        public GeneralService(IOptions<ConnectionStrings> connectionStrings, ILogService logService)
        {
            this.connectionStrings = connectionStrings.Value;
            this.iLogDbService = logService;
        }

        public string getDato(string tabla, string campo, string condicion, List<SqlParameter> parametros, int idUsuario)
        {
            string value = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStrings.Str))
                {
                    string query = string.Format(@"Select {0} from {1} with(nolock) {2}", campo, tabla, condicion);
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        foreach (var parametro in parametros)
                        {
                            comm.Parameters.Add(parametro);
                        }
                        conn.Open();
                        using (SqlDataReader rdr = comm.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                value = rdr[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                iLogDbService.guardaLog($"{this.GetType().Name} - {MethodBase.GetCurrentMethod().Name}", "", idUsuario, ex);
            }
            return value;
        }
    }
}
