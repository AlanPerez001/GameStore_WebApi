using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameStore_WebApi.Utility
{
    public class Generales
    {

        public static string exceptionToString(Exception ex)
        {
            if (ex != null)
            {
                return $"{ex.Message},{ex.StackTrace},{(ex.InnerException == null ? "" : ex.InnerException.Message)}";

            }
            else
            {
                return "";
            }
        }
        public static string reemplazaCaracteresEspeciales(string cadena)
        {
            string res = "";
            Regex reg = new Regex("[^a-zA-Z0-9 ]");
            string textoNormalizado = cadena.Normalize(NormalizationForm.FormD);
            res = reg.Replace(textoNormalizado, "");
            return res;
        }
        public static string genericoToString<T>(T objeto)
        {
            string res = "";
            if (objeto != null)
            {
                var typeObjeto = objeto.GetType();
                if (objeto.GetType().IsGenericType && objeto is IEnumerable)
                {
                    foreach (var item in objeto as IEnumerable)
                    {
                        var typeItem = item.GetType();
                        if (puedeConvertirseToString(typeItem))
                        {
                            res += $"{Convert.ToString(item)},";
                        }
                        else
                        {
                            var props = typeItem.GetProperties();
                            foreach (var prop in props)
                            {
                                res += $"{prop.Name}:{ prop.GetValue(item)},";
                            }
                        }
                    }
                }
                else
                {
                    if (puedeConvertirseToString(typeObjeto))
                    {
                        res += $"{Convert.ToString(objeto)}";
                    }
                    else
                    {
                        var props = typeObjeto.GetProperties();

                        foreach (var prop in props)
                        {
                            res += $"{prop.Name}:{ prop.GetValue(objeto)},";
                        }
                    }
                }
            }
            return res;
        }
        public static bool puedeConvertirseToString(Type type)
        {

            if (type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(int) ||
                type == typeof(decimal) ||
                type == typeof(float) ||
                type == typeof(bool) ||
                type == typeof(string) ||
                type == typeof(byte) ||
                type == typeof(char) ||
                type == typeof(decimal) ||
                type == typeof(DateTime) ||
                type == typeof(DateTimeOffset) ||
                type == typeof(TimeSpan) ||
                type == typeof(Guid) ||
                IsNullableSimpleType(type))
                return true;
            else
                return false;

            static bool IsNullableSimpleType(Type t)
            {
                var underlyingType = Nullable.GetUnderlyingType(t);
                return underlyingType != null && puedeConvertirseToString(underlyingType);
            }
        }
        public static void mapeaPropiedades<T, A>(ref T modeloOrigen, ref A modeloDestino)
        {
            PropertyInfo[] propertyInfo = modeloOrigen.GetType().GetProperties();
            foreach (var propiedad in propertyInfo)
            {
                PropertyInfo info = modeloDestino.GetType().GetProperty(propiedad.Name);
                if (info != null)
                {
                    info.SetValue(modeloDestino, propiedad.GetValue(modeloOrigen));
                }
            }
        }

    }
}
