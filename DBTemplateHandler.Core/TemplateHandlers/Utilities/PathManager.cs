﻿using System;
using System.IO;

namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public class PathManager
    {

        public static String AppendAtEndFileSeparatorIfNeeded(String pathStr)
        {
            String result = pathStr;
            if (!pathStr.EndsWith(Path.DirectorySeparatorChar)) result = result + Path.DirectorySeparatorChar;
            return result;
        }

        public static String getParentDirectoryPathFromStringPath(String PathStr)
        {
            String result = PathStr;
            if (result == null) return result;
            if (StringManager.countStringInString(PathStr, Path.DirectorySeparatorChar.ToString()) <= 1) return null;
            if (result.EndsWith(Path.DirectorySeparatorChar)) result = result.Substring(0, result.Length - 1);
            int lastIndexOfPathSeparator = result.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastIndexOfPathSeparator < 1) return null;
            result = result.Substring(0, lastIndexOfPathSeparator);
            return result;
        }


        public static String getFileOrFolderNameFromStringPath(String PathStr)
        {
            String result = PathStr;
            if (result == null) return result;
            if (result.EndsWith(Path.DirectorySeparatorChar)) result = result.Substring(0, result.Length - 1);
            int lastIndexOfPathSeparator = result.LastIndexOf(Path.DirectorySeparatorChar);
            if (lastIndexOfPathSeparator < 1) return result;
            result = result.Substring(lastIndexOfPathSeparator, result.Length);
            return result;
        }

        public static String getFileBaseNameFromString(String PathStr)
        {
            String result = getFileOrFolderNameFromStringPath(PathStr);
            int lastIndexOfFileExtensionSeparation = result.LastIndexOf('.');
            if (lastIndexOfFileExtensionSeparation < 1) return result;
            result = result.Substring(0, lastIndexOfFileExtensionSeparation);
            return result;
        }
    }
}
