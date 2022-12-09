
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Resources;
using Tool.Utilities;

namespace Application.Helpers
{
    public static class StringFormatHelper
    {
        public static string Money = @"{0:C0}";
        public static string Float = @"{0:0.00}";
        public static string Date = @"{0:dd/MM/yyyy}";
        public static string HideZero = @"{0:#\\%;0:#;#}";
    }
}
