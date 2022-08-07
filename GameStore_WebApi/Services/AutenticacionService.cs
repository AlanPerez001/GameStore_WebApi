using GameStore_WebApi.Models;
using GameStore_WebApi.Models.Autenticacion;
using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services
{
    public class AutenticacionService : IAutenticacionService
    {
        private readonly ConnectionStrings conectionStrings;
        private readonly ILogService log;
        private const int timeoutCommand = 300;

        public AutenticacionService(IOptions<ConnectionStrings> conectionStrings, ILogService log)
        {
            this.conectionStrings = conectionStrings.Value;
            this.log = log;
        }

        public Login iniciaSesion(IniciarSesion modelo)
        {
            Login res = null;
            try
            {
                Random ran = new Random();
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_IniciaSesion", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@correo", modelo.Usuario);
                        comm.Parameters.AddWithValue("@contrasena", modelo.Password);
                        comm.Parameters.AddWithValue("@plataforma", modelo.Plataforma);
                        comm.Parameters.AddWithValue("@version", modelo.Version);
                        comm.CommandTimeout = timeoutCommand;
                        con.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                res = new Login(Convert.ToInt32(reader[0]), reader[1].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.guardaLog($"{GetType().Name} - {MethodBase.GetCurrentMethod().Name}", $"", 0, ex);
                throw new MiExcepcion($"{Startup.respuestasApi.MensajeErrorExcepcionDB}");
            }
            return res;
        }


        public RespuestaGeneral guardaRefreshToken(int idUsuario, string refreshToken, string jwt, string refreshTokenAnterior)
        {
            RespuestaGeneral res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_guardaRefreshToken", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idUsuario", idUsuario);
                        comm.Parameters.AddWithValue("@refreshToken", refreshToken);
                        comm.Parameters.AddWithValue("@jwt", jwt);
                        comm.Parameters.AddWithValue("@refreshTokenAnterior", refreshTokenAnterior);
                        SqlParameter idRespuesta = new SqlParameter("@idRespuesta", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        SqlParameter respuesta = new SqlParameter("@respuesta", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                        comm.Parameters.Add(idRespuesta);
                        comm.Parameters.Add(respuesta);
                        comm.CommandTimeout = timeoutCommand;
                        con.Open();
                        comm.ExecuteNonQuery();
                        res = new RespuestaGeneral(Convert.ToInt32(idRespuesta.Value), respuesta.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.guardaLog($"{GetType().Name} - {MethodBase.GetCurrentMethod().Name}",
                  $"{refreshToken} - {jwt} - {refreshTokenAnterior}", idUsuario, ex);
                throw new MiExcepcion($"{Startup.respuestasApi.MensajeErrorExcepcionDB}");
            }
            return res;
        }

        public RespuestaGeneral validaRefreshToken(int idUsuario, string refreshToken)
        {
            RespuestaGeneral res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_validaRefreshToken", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idUsuario", idUsuario);
                        comm.Parameters.AddWithValue("@refreshToken", refreshToken);
                        SqlParameter idRespuesta = new SqlParameter("@idRespuesta", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        SqlParameter respuesta = new SqlParameter("@respuesta", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                        comm.Parameters.Add(idRespuesta);
                        comm.Parameters.Add(respuesta);
                        comm.CommandTimeout = timeoutCommand;
                        con.Open();
                        comm.ExecuteNonQuery();
                        res = new RespuestaGeneral(Convert.ToInt32(idRespuesta.Value), respuesta.Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.guardaLog($"{GetType().Name} - {MethodBase.GetCurrentMethod().Name}", $"{refreshToken}", idUsuario, ex);
                throw new MiExcepcion($"{Startup.respuestasApi.MensajeErrorExcepcionDB}");
            }
            return res;
        }
    }
}
