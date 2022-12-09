using System.Collections.Generic;
using System.Linq;
using Tool.Resources;

namespace Tool.Utilities
{
    public static class FileManagement
    {
        public static string Read { get; } = @"/files/";
        public static string Write { get; } = @"C:\Users\Sergio\source\repos\Management\Application\wwwroot\files\";

        public static DirectoryManagement Avatar
        {
            get
            {
                return new DirectoryManagement() { Read = $"{Read}image/avatar/", Write = $"{Write}image\\avatar\\" };
            }
        }

        public static DirectoryManagement Product
        {
            get
            {
                return new DirectoryManagement() { Read = $"{Read}image/product/", Write = $"{Write}image\\product\\" };
            }
        }

        public static string GetExtensionDirectory(string extension)
        {
            foreach (var type in GetExtensions())
            {
                if (type.Value.Contains(extension))
                {
                    return GetDirectories()[type.Key];
                }
            }

            return null;
        }

        public static EExtension GetExtensionCategory(string extension)
        {
            foreach (var category in GetExtensions())
            {
                if (category.Value.Contains(extension))
                {
                    return category.Key;
                }
            }

            return EExtension.Undefined;
        }

        public static bool IsImage(string extension)
        {
            var extensions = GetExtensions()[EExtension.Image];

            if (extensions.Contains(extension))
            {
                return true;
            }

            return false;
        }

        private static Dictionary<EExtension, string[]> GetExtensions()
        {
            return new Dictionary<EExtension, string[]>
            {
                { EExtension.Audio, new string[] { ".mp3" }},
                { EExtension.Image, new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" }},
                { EExtension.Document, new string[] { ".doc", ".docx", ".txt" }},
                { EExtension.Spreadsheet, new string[] { ".xls", ".xlsx" }},
                { EExtension.Pdf, new string[] { ".pdf" }},
                { EExtension.Compressed, new string[] { ".rar", ".zip", ".7z" }},
                { EExtension.Presentation, new string[] { ".ppt", ".pptx", ".pps", ".ppsx" }},
                { EExtension.Video, new string[] { ".mp4" }}
            };
        }

        private static Dictionary<EExtension, string> GetDirectories()
        {
            return new Dictionary<EExtension, string>
            {
                { EExtension.Audio, "audio\\" },
                { EExtension.Image, "image\\" },
                { EExtension.Document, "document\\" },
                { EExtension.Spreadsheet, "spreadsheet\\" },
                { EExtension.Pdf, "pdf\\" },
                { EExtension.Compressed, "compressed\\" },
                { EExtension.Presentation, "presentation\\" },
                { EExtension.Video, "video\\" }
            };
        }
    }
}
