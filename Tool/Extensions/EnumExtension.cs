using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;
using Tool.Resources;
using Tool.Utilities;

namespace Tool.Extensions
{
    public static class EnumExtesion
    {
        public static string GetDescription(this Enum value)
        {
            return Label.ResourceManager.GetString(value.ToString()) ?? string.Empty;
        }

        public static string GetValue(this Enum value)
        {
            return ((int)(object)value).ToString();
        }

        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>()
                    .Where(x => !((Enum)(object)x).IsHidden())
                    .OrderBy(y => ((Enum)(object)y).GetDescription());
        }

        public static IEnumerable<SelectListItem> GetSelectList<T>(this Enum value, bool show = false)
        {
            return Enum.GetValues(value.GetType()).Cast<T>()
                    .Where(y => !show ? !((Enum)(object)y).IsHidden() : true)
                    .Select(
                        x =>
                        new SelectListItem
                        {
                            Text = ((Enum)(object)x).GetDescription(),
                            Value = ((int)(object)x).ToString()
                        });
        }

        public static bool IsHidden(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attributes = field.GetCustomAttributes(typeof(HiddenValueAttribute), false);

            dynamic hiddenAttribute = null;

            if (attributes.Any())
            {
                hiddenAttribute = attributes.ElementAt(0);
            }

            return hiddenAttribute?.hidden ?? false;
        }
    }
}