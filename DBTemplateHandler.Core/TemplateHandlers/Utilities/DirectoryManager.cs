using System;
using System.Collections.Generic;
using System.IO;

namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public class DirectoryManager
    {
        public static bool CreateDirectoryPath(String directoryPathStr)
        {
            Stack<String> stack = new Stack<String>();

            String currentPathDirectory = directoryPathStr;
            while (!FileManager.DoesFolderExists(currentPathDirectory))
            {
                stack.Push(currentPathDirectory);

                if ((currentPathDirectory = PathManager.
                        getParentDirectoryPathFromStringPath(currentPathDirectory)) == null) break;
            }
            if (currentPathDirectory == null) return false;

            while (stack.Count > 0)
            {
                Directory.CreateDirectory(stack.Pop());
            }
            return true;
        }
    }
}
