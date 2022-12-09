using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tool.Extensions
{
    public static class IFormCollectionExtension
    {
        public static string ToString(this IFormCollection form, string key, string param)
        {
            if (form.IsValid(key))
            {
                return form[key].ToString();
            }

            return param;
        }

        public static int? ToInt(this IFormCollection form, string key, int? param)
        {
            if (form.IsValid(key))
            {
                return int.Parse(form[key].ToString());
            }

            return param;
        }

        public static T ToEnum<T>(this IFormCollection form, string key, T param)
        {
            if (form.IsValid(key))
            {
                try
                {
                    return (T)Enum.ToObject(typeof(T), int.Parse(form[key].ToString())) ?? param;
                }
                catch
                {
                    return param;
                }
            }

            return param;
        }

        private static bool IsValid(this IFormCollection form, string key)
        {
            return !string.IsNullOrWhiteSpace(form[key].ToString());
        }
    }
}
