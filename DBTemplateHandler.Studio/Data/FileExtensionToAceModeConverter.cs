using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBTemplateHandler.Studio.Data
{
    public class FileExtensionToAceModeConverter
    {
        private static readonly Dictionary<string, string> FileExtensionToAceModeMapping = new Dictionary<string, string>()
        {
            {".sql","sql" },
            {".htm","html" },
            {".html","html" },
            {".cs", "csharp" },
        };

        private const string DefaultMode = "html";

        public static string ToAceMode(string filePath)
        {
            if (filePath == null) return DefaultMode;
            if (!filePath.Contains(".")) return DefaultMode;
            var lastIndexOf = filePath.LastIndexOf('.');
            var extension = filePath.Substring(lastIndexOf);
            if (String.IsNullOrEmpty(extension)) return DefaultMode;
            if (FileExtensionToAceModeMapping.TryGetValue(extension, out var result)) return result;
            return DefaultMode;
        }
    }
}
