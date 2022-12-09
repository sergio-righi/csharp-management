using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class AppContextHelper
    {
        private static IHttpContextAccessor ContextAccessor;
        public static HttpContext Current => ContextAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor context)
        {
            ContextAccessor = context;
        }
    }
}
