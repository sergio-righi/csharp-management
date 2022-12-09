using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Linq.Expressions;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.IO;
using Tool.Utilities;
using Tool.Resources;
using System.Collections.Generic;
using Tool.Extensions;
using System.Linq;

namespace Application.Helpers
{
    public static class FormHelper
    {
        public static string GetSelectFormat(this EStringGender gender, string message, EStringFormat format = EStringFormat.One)
        {
            return gender switch
            {
                EStringGender.Male => string.Format(Message.SelectOneMale, message.ToLower()),
                EStringGender.Female => string.Format(Message.SelectOneFemale, message.ToLower()),
                _ => string.Empty,
            };
        }

        public static IEnumerable<SelectListItem> AsSelectList<T>(this ICollection<T> list, string value, string text, string label = null, object selected = null)
        {
            if (list.IsNullOrEmpty())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            if (selected != null)
            {
                return new SelectList(list, value, text, selected, label);
            }

            return new SelectList(list, value, text, label);
        }
    }
}