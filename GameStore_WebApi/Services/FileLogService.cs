using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore_WebApi.Services
{
    public class FileLogService : ILogService
    {
        private readonly AppSettings appSettings;
        private readonly IWebHostEnvironment environment;

        public FileLogService(IOptions<AppSettings> appSettings, IWebHostEnvironment environment)
        {
            this.appSettings = appSettings.Value;
            this.environment = environment;
        }

        public int guardaLog(string nombre, string datos, int idUsuario, Exception exParameter)
        {
            var mensajeExcepcion = Generales.exceptionToString(exParameter);
            try
            {
                if (ValidaArchivo())
                {
                    var pathLogTxt = environment.ContentRootPath + appSettings.DirLogTxt;
                    var nameTxt = pathLogTxt + appSettings.NameLogTxt + ".txt";
                    using (StreamWriter writer = System.IO.File.AppendText(nameTxt))
                    {
                        writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - {Activity.Current.RootId} - {idUsuario} - {nombre} - {datos} - { mensajeExcepcion}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Error al crar archivo log {idUsuario} - {nombre} - {datos} - { mensajeExcepcion}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message} {idUsuario} - {nombre} - {datos} - {mensajeExcepcion}");

            }
            return 1;
        }

        public int guardaLog(string nombre, string datos, string adicionales, int idUsuario)
        {
            try
            {
                if (ValidaArchivo())
                {
                    var pathLogTxt = environment.ContentRootPath + appSettings.DirLogTxt;
                    var nameTxt = pathLogTxt + appSettings.NameLogTxt + ".txt";
                    using (StreamWriter writer = System.IO.File.AppendText(nameTxt))
                    {
                        writer.WriteLine($"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - {Activity.Current.RootId} - {idUsuario} - {nombre} - {datos} - { adicionales}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Error al crar archivo log {idUsuario} - {nombre} - {datos} - { adicionales}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message} {idUsuario} - {nombre} - {datos} - {adicionales}");

            }
            return 1;
        }

        public bool ValidaArchivo()
        {
            var res = false;
            try
            {
                var pathLogTxt = environment.WebRootPath + appSettings.DirLogTxt;
                var nameTxt = pathLogTxt + appSettings.NameLogTxt;
                if (!new DirectoryInfo(pathLogTxt).Exists)
                {
                    Directory.CreateDirectory(pathLogTxt);
                }
                var PathArchivo = nameTxt + ".txt";
                if (System.IO.File.Exists(PathArchivo))
                {
                    System.IO.FileInfo file = new FileInfo(PathArchivo);
                    var lenght = file.Length;
                    string size = FileSizeFormatter.FormatSize(lenght);
                    if (lenght > 1000000)
                    {
                        var PathArchivoRespaldo = nameTxt + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".txt";
                        file.MoveTo(PathArchivoRespaldo);
                        using (FileStream fs = System.IO.File.Create(PathArchivo))
                        {
                        }
                    }
                }
                else
                {
                    using (FileStream fs = File.Create(PathArchivo))
                    {
                    }
                }
                res = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message} {ex.StackTrace} {(ex.InnerException == null ? "" : ex.InnerException.Message)}");
            }

            return res;
        }

        public static class FileSizeFormatter
        {
            // Load all suffixes in an array  
            static readonly string[] suffixes =
            { "Bytes", "KB", "MB", "GB", "TB", "PB" };
            public static string FormatSize(Int64 bytes)
            {
                int counter = 0;
                decimal number = (decimal)bytes;
                while (Math.Round(number / 1024) >= 1)
                {
                    number = number / 1024;
                    counter++;
                }
                return string.Format("{0:n1}{1}", number, suffixes[counter]);
            }
        }
    }
}
