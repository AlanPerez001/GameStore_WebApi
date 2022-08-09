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

        public RespuestaObtenerVideoJuegos ObtenerCatalogoVideojuegos()
        {
            RespuestaObtenerVideoJuegos res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_GetVideoJuegos", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
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
                                    idJuego = Convert.ToInt32(reader[0]),
                                    Titulo = reader[1].ToString(),
                                    Descripcion = reader[2].ToString(),
                                    AnioPublicacion = reader[3].ToString(),
                                    Calificacion = Convert.ToInt32(reader[4]),
                                    Consolas = reader[5].ToString(),
                                    Genero = reader[6].ToString()
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

        public RespuestaGetfiltros ObtenerFiltros(int idFiltro)
        {
            RespuestaGetfiltros res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_GetFiltros", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idFiltro", idFiltro);
                        comm.CommandTimeout = timeoutCommand;
                        con.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            res = new RespuestaGetfiltros()
                            { IdResponse = 1,
                            Response = "Solicitud Completada"

                            };
                            res.Datos = new List<filtros>();
                            while (reader.Read())
                            {
                                res.Datos.Add(new filtros()
                                {
                                    Id = Convert.ToInt32(reader[0]),
                                    Descripcion = reader[1].ToString(),

                                });
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

        public RespuestaObtenerVideoJuegos ObtenerCatalogoFiltradoVideojuegos(int idgenero, int idConsola)
        {
            RespuestaObtenerVideoJuegos res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_GetVideoJuegosFiltrado", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idgenero", idgenero);
                        comm.Parameters.AddWithValue("@idConsola", idConsola);
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
                                    idJuego = Convert.ToInt32(reader[0]),
                                    Titulo = reader[1].ToString(),
                                    Descripcion = reader[2].ToString(),
                                    AnioPublicacion = reader[3].ToString(),
                                    Calificacion = Convert.ToInt32(reader[4]),
                                    Consolas = reader[5].ToString(),
                                    Genero = reader[6].ToString()
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

        public RespuestaObtenerDetalleVideoJuego ObtenerDetalleJuego(int idJuego)
        {
            RespuestaObtenerDetalleVideoJuego res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_GetDetalleVideoJuegos", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idJuego", idJuego);
                        comm.CommandTimeout = timeoutCommand;
                        con.Open();
                        using (var reader = comm.ExecuteReader())
                        {
                            res = new RespuestaObtenerDetalleVideoJuego();
                            while (reader.Read())
                            {
                                res.Datos = new VideoJuegos()
                                {
                                    idJuego = Convert.ToInt32(reader[0]),
                                    Titulo = reader[1].ToString(),
                                    Descripcion = reader[2].ToString(),
                                    AnioPublicacion = reader[3].ToString(),
                                    Calificacion = Convert.ToInt32(reader[4]),
                                    Consolas = reader[5].ToString(),
                                    Genero = reader[6].ToString()
                                };
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

        public RespuestaGeneral GuardaRegistro(PeticionNuevoRegistro model)
        {
            RespuestaGeneral res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_GuardaRegistro", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idJuego", model.IdJuego);
                        comm.Parameters.AddWithValue("@Titulo", model.Titulo);
                        comm.Parameters.AddWithValue("@Descripcion", model.Descripcion);
                        comm.Parameters.AddWithValue("@AnioPublicacion", model.AnioPublicacion);
                        comm.Parameters.AddWithValue("@Calificacion", model.Calificacion);
                        comm.Parameters.AddWithValue("@idConsola", model.idConsola);
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

        public RespuestaGeneral EliminaRegistro(PeticionEliminar model)
        {
            RespuestaGeneral res = null;
            try
            {
                using (var con = new SqlConnection(conectionStrings.Str))
                {
                    using (var comm = new SqlCommand("pa_EliminaRegistro", con))
                    {
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@idJuego", model.IdJuego);
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
