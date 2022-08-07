using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services
{
    public class DbLogService : ILogService
    {
        private int timeoutCommand = 300;
        private readonly ConnectionStrings connectionStrings;

        public DbLogService(IOptions<ConnectionStrings> connectionStrings)
        {
            this.connectionStrings = connectionStrings.Value;
        }

        public int guardaLog(string nombre, string datos, int idUsuario, Exception exParameter)
        {
            int res = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStrings.StrLogs))
                using (SqlCommand comm = new SqlCommand("pa_guardaLogWebApiAplicaciones", conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@nombre", nombre);
                    comm.Parameters.AddWithValue("@datos", datos);
                    comm.Parameters.AddWithValue("@idUsuario", idUsuario);
                    comm.Parameters.AddWithValue("@adicionales", Generales.exceptionToString(exParameter));
                    comm.Parameters.AddWithValue("@identifier", Activity.Current.RootId);
                    comm.CommandTimeout = timeoutCommand;
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{this.GetType().Name} - {MethodBase.GetCurrentMethod().Name} - {ex.Message}");
                res = -1;
            }
            return res;
        }

        public int guardaLog(string nombre, string datos, string adicionales, int idUsuario)
        {
            int res = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStrings.StrLogs))
                using (SqlCommand comm = new SqlCommand("pa_guardaLogWebApiAplicaciones", conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@nombre", nombre);
                    comm.Parameters.AddWithValue("@datos", datos);
                    comm.Parameters.AddWithValue("@idUsuario", idUsuario);
                    comm.Parameters.AddWithValue("@adicionales", adicionales);
                    comm.Parameters.AddWithValue("@identifier", Activity.Current.RootId);
                    comm.CommandTimeout = timeoutCommand;
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{this.GetType().Name} - {MethodBase.GetCurrentMethod().Name} - {ex.Message}");
                res = -1;
            }
            return res;
        }

    }
}
