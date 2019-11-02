using System;
using System.Collections.Generic;
using System.IO;

namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public class DirectoryManager
    {
        public static bool CreateDirectoryPath(string directoryPathStr)
        {
            try
            {
                var directoryInfo = Directory.CreateDirectory(directoryPathStr);
                return true;
            }
            catch(IOException e)
            {
                throw e;
            }
        }
    }
}
