using GameStore_WebApi.Models;
using GameStore_WebApi.Models.Catalogo;
using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services
{
    public class CatalogoService : ICatalogoService
    {
        private readonly ConnectionStrings conectionStrings;
        private readonly ILogService log;
        private const int timeoutCommand = 300;

        public CatalogoService(IOptions<ConnectionStrings> conectionStrings, ILogService log)
        {
            this.conectionStrings = conectionStrings.Value;
            this.log = log;
        }

        public RespuestaObtenerVideoJuegos ObtenerCatalogoVideojuegos(int idGenero, int idConsola)
        {
            RespuestaObtenerVideoJuegos res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_GetVideoJuegos", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idConsla", idConsola);
                        comm.Parameters.AddWithValue("@idGenero", idGenero);
                        comm.CommandTimeout = timeoutCommand;
                        con.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            res = new RespuestaObtenerVideoJuegos();
                            res.Datos = new List<VideoJuegos>();
                            while (reader.Read())
                            {
                                res.Datos.Add(new VideoJuegos()
                                {
                                    Titulo = reader[0].ToString(),
                                    Descripcion = reader[1].ToString(),
                                    AnioPublicacion = reader[2].ToString(),
                                    Calificacion = Convert.ToInt32(reader[3]),
                                    Consolas = reader[4].ToString(),
                                    Genero = reader[5].ToString()
                                });
                            }
                            if (res.Datos.Count > 0)
                            {
                                res.idResponse = 1;
                                res.Response = "Solicitud completada";
                            }
                            else
                            {
                                res.idResponse = 0;
                                res.Response = "No se encontraron resultados";
                                res.Datos = null;
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

        public RespuestaGeneral CreaNuevoRegistro(PeticionNuevoRegistro model, string CadenaConsola)
        {
            RespuestaGeneral res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_InsertaNuevoRegistro", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@Titulo", model.Titulo);
                        comm.Parameters.AddWithValue("@Descripcion", model.Descripcion);
                        comm.Parameters.AddWithValue("@AnioPublicacion", model.AnioPublicacion);
                        comm.Parameters.AddWithValue("@Calificacion", model.Calificacion);
                        comm.Parameters.AddWithValue("@idConsola", JsonConvert.SerializeObject(model.idConsola));
                        comm.Parameters.AddWithValue("@idGenero", model.idGenero);
                        comm.CommandTimeout = timeoutCommand;
                        con.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            res = new RespuestaGeneral();
                            while (reader.Read())
                            {
                                res.IdResponse = Convert.ToInt32(reader[0]);
                                res.Response = reader[1].ToString();
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
    }
}
