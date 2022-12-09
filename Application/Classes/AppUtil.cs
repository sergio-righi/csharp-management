using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Extensions;
using Tool.Utilities;

namespace Application.Classes
{
    public static class AppUtil
    {
        public static string Message(EFeedback type, string content, bool removable = true)
        {
            return $"<div class=\"alert {type.ToString().ToLower()}\">" +
                $"<div class=\"alert-content\">" +
                    $"<div class=\"alert-description\">{content}</div>" +
                $"</div>" +
                (removable ? "<div class=\"alert-close\" data-dismiss=\"alert\"></div>" : string.Empty) +
            "</div>";
        }
    }
}
