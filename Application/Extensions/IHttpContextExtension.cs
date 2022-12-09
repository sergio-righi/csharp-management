using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class IHttpContextExtension
    {
        public static T GetCookie<T>(this HttpContext httpContext, string key)
        {
            if (httpContext.Request.Cookies.ContainsKey(key))
            {
                return JsonConvert.DeserializeObject<T>(httpContext.Request.Cookies[key]);
            }

            return (T)default;
        }

        public static void SetCookie<T>(this HttpContext httpContext, T value, string key, long day = 30)
        {
            httpContext.Response.Cookies.Append(key, JsonConvert.SerializeObject(value), new CookieOptions()
            {
                Path = "/",
                Secure = false,
                HttpOnly = false,
                //IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddDays(day)
            });
        }

        public static void RemoveCookie(this HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }
    }
}
