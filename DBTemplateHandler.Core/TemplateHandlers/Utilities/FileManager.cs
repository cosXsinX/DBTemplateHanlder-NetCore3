using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public static class FileManager
    {
	    public static bool DoesFileExists(String FilePathStr)
        {
            return File.Exists(FilePathStr) ;
        }

        public static bool DoesFolderExists(String FolderPathStr)
        {
            return !File.Exists(FolderPathStr) && Directory.Exists(FolderPathStr);
        }

        public static bool CreateTextFile(String FilePathStr, String FileContentString)
        {
            return CreateTextFile(FilePathStr,
                    FileContentString, false);
        }

        public static bool CreateTextFile(String FilePathStr,
                String FileContentString, bool DoCreateAssociatedDirectoryPath)
        {
            if (DoesFileExists(FilePathStr)) return false;
            String parentFolderDirectoryPath = PathManager.getParentDirectoryPathFromStringPath(FilePathStr);
            if (parentFolderDirectoryPath == null) return false; //Wrong path provided for file creation
            if (!DoesFolderExists(parentFolderDirectoryPath))
                if (DoCreateAssociatedDirectoryPath) DirectoryManager.CreateDirectoryPath(parentFolderDirectoryPath);
                else return false; // if the parent directory path do not exist and cannot be created then there is a problem in the provided path, thus no file creation is possible
            File.WriteAllText(FilePathStr, FileContentString);
            return true;
        }

        public static bool AppendTextFile(String FilePathStr, String AppendedFileContentString)
        {
            if (!DoesFileExists(FilePathStr)) return false;
            File.AppendText(AppendedFileContentString);
            return true;
        }

        public static bool CreateOrAppendTextFile(String FilePathStr, String TextContentStr)
        {
            if (DoesFileExists(FilePathStr)) return AppendTextFile(FilePathStr, TextContentStr);
            else return CreateTextFile(FilePathStr, TextContentStr);
        }


        public static bool DeleteFile(String FilePathStr)
        {
            if (!DoesFileExists(FilePathStr)) return true;
            File.Delete(FilePathStr);
            return true;
        }

        public static bool RenameFile(String RenamedFilePathStr, String NewFileName)
        {
            return RenameFile(RenamedFilePathStr, NewFileName, false);
        }

        public static bool RenameFile(String RenamedFilePathStr, String NewFileName, bool ReplaceFileWithSameNewFileName)
        {
            if (RenamedFilePathStr == null) return false;
            if (NewFileName == null) return false;
            if (!DoesFileExists(RenamedFilePathStr)) return false;
            String NewFilePath = PathManager.AppendAtEndFileSeparatorIfNeeded(
                    PathManager.getParentDirectoryPathFromStringPath(RenamedFilePathStr)) + NewFileName;
            
            
            if (DoesFileExists(NewFilePath))
            {
                if (ReplaceFileWithSameNewFileName)
                {
                    File.Delete(NewFilePath);
                }
                else return false;
            }
            File.Move(RenamedFilePathStr, NewFilePath);
            return true;
        }
    }
}
