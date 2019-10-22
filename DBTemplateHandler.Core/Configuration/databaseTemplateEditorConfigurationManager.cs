using System;
using System.IO;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;

namespace DBTemplateHandler.Core.Configuration
{
    public class databaseTemplateEditorConfigurationManager
    {

        private const String RESSOURCE_FOLDER_NAME = "ressources";
        private const String TEMPLATE_FOLDER_NAME = "template";
        private const String TEMPLATE_BUNDLE_FOLDER_NAME = "template_bundles";
        private const String SAVED_DATABASE_MODELS_TEMPLATE_FOLDER_NAME = "saved_database_models";
        private const String GENERATED_FILE_FOLDER_NAME = "generated-file";

        private static String _currentApplicationDirectory = CurrentDLLDirectory();
        private static String _currentApplicationTemplateDirectoryPathStr;
        private static String _currentApplicationTemplateBundelDirectoryPathStr;
        private static String _currentApplicationSavedModelDirectoryPathStr;
        private static String _currentApplicationGeneratedFileFromTemplateDirectoryPathStr;


        private static string CurrentDLLDirectory()
        {
            string fullPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string theDirectory = Path.GetDirectoryName(fullPath);
            return theDirectory;
        }

        private static String AppendSubVolumeElementToPath(String PathStr, String SubElementName)
        {
            return PathManager.AppendAtEndFileSeparatorIfNeeded(PathStr) + SubElementName;
        }

        public static String get_templateDirectoryRootFolderPathStr()
        {
            if (_currentApplicationTemplateDirectoryPathStr == null)
            {
                _currentApplicationTemplateDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationDirectory, RESSOURCE_FOLDER_NAME);
                _currentApplicationTemplateDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationTemplateDirectoryPathStr, TEMPLATE_FOLDER_NAME);
                _currentApplicationTemplateDirectoryPathStr = PathManager.AppendAtEndFileSeparatorIfNeeded(_currentApplicationTemplateDirectoryPathStr);
            }
            return _currentApplicationTemplateDirectoryPathStr;
        }

        public static String get_templateBundlesDirectoryRootFolderPathStr()
        {
            if (_currentApplicationTemplateBundelDirectoryPathStr == null)
            {
                _currentApplicationTemplateBundelDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationDirectory, RESSOURCE_FOLDER_NAME);
                _currentApplicationTemplateBundelDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationTemplateDirectoryPathStr, TEMPLATE_BUNDLE_FOLDER_NAME);
                _currentApplicationTemplateBundelDirectoryPathStr = PathManager.AppendAtEndFileSeparatorIfNeeded(_currentApplicationTemplateDirectoryPathStr);
            }
            return _currentApplicationTemplateBundelDirectoryPathStr;
        }

        public static String get_savedModelDirectoryPathStr()
        {
            if (_currentApplicationSavedModelDirectoryPathStr == null)
            {
                _currentApplicationSavedModelDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationDirectory, RESSOURCE_FOLDER_NAME);
                _currentApplicationSavedModelDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationSavedModelDirectoryPathStr, SAVED_DATABASE_MODELS_TEMPLATE_FOLDER_NAME);
                _currentApplicationSavedModelDirectoryPathStr = PathManager.AppendAtEndFileSeparatorIfNeeded(_currentApplicationSavedModelDirectoryPathStr);
            }
            return _currentApplicationSavedModelDirectoryPathStr;
        }

        public static String get_generatedFileFromTemplateDirectoryPathStr()
        {
            if (_currentApplicationGeneratedFileFromTemplateDirectoryPathStr == null)
            {
                _currentApplicationGeneratedFileFromTemplateDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationDirectory, RESSOURCE_FOLDER_NAME);
                _currentApplicationGeneratedFileFromTemplateDirectoryPathStr = AppendSubVolumeElementToPath(_currentApplicationGeneratedFileFromTemplateDirectoryPathStr, GENERATED_FILE_FOLDER_NAME);
                _currentApplicationGeneratedFileFromTemplateDirectoryPathStr = PathManager.AppendAtEndFileSeparatorIfNeeded(_currentApplicationGeneratedFileFromTemplateDirectoryPathStr);
            }
            return _currentApplicationGeneratedFileFromTemplateDirectoryPathStr;
        }


        public static void InitApplicationConfigurationFileSystem()
        {
            String templateDirectoryStr = get_templateDirectoryRootFolderPathStr();
            if (!FileManager.DoesFolderExists(templateDirectoryStr))
                DirectoryManager.CreateDirectoryPath(templateDirectoryStr);
            String templateBundleDirectoryStr = get_templateBundlesDirectoryRootFolderPathStr();
            if (!FileManager.DoesFolderExists(templateBundleDirectoryStr))
                DirectoryManager.CreateDirectoryPath(templateBundleDirectoryStr);
            String savedModelDatabaseDirectoryStr = get_savedModelDirectoryPathStr();
            if (!FileManager.DoesFolderExists(savedModelDatabaseDirectoryStr))
                DirectoryManager.CreateDirectoryPath(savedModelDatabaseDirectoryStr);
            String GeneratedFileFromTemplateDirectoryStr = get_generatedFileFromTemplateDirectoryPathStr();
            if (!FileManager.DoesFolderExists(GeneratedFileFromTemplateDirectoryStr))
                DirectoryManager.CreateDirectoryPath(GeneratedFileFromTemplateDirectoryStr);
        }

    }
}
