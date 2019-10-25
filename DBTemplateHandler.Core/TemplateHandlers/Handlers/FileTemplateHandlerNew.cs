﻿using System;
using System.IO;
using System.Text;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class FileTemplateHandlerNew
    {

        private const string DATABASE_TEMPLATE_FILE_NAME_WORD = "%databaseName%";
        private const string TABLE_TEMPLATE_FILE_NAME_WORD = "%tableName%";
        private const string COLUMN_TEMPLATE_FILE_NAME_WORD = "%columnName%";

        private String _outpuFolderPath;
        public String get_outpuFolderPath()
        {
            return _outpuFolderPath;
        }
        public void set_outpuFolderPath(String value)
        {
            this._outpuFolderPath = value;
        }


        public bool GenerateDatabaseTemplateFiles
            (String handledTemplateFilePath,
                    String specifiedDestinationRelativePath,
                        DatabaseDescriptionPOJO databaseDescriptionPOJO)
        {
            if (databaseDescriptionPOJO == null) return false;
            if (specifiedDestinationRelativePath == null) return false;
            String handledTemplateStringContent = getHandledTemplateStringContent(handledTemplateFilePath);
            if (handledTemplateStringContent == null) return false;
            bool containsTblWord =
                    specifiedDestinationRelativePath.
                        Contains(TABLE_TEMPLATE_FILE_NAME_WORD);
            bool containsColWord =
                    handledTemplateFilePath.
                        Contains(COLUMN_TEMPLATE_FILE_NAME_WORD);
            if (containsColWord)
            {
                String currentDatabaseReplacedDestinationRelativePath =
                        specifiedDestinationRelativePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    databaseDescriptionPOJO.getDatabaseNameStr());
                foreach (TableDescriptionPOJO currentTable in databaseDescriptionPOJO.getTableList())
                {
                    String currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                        currentTable.get_NameStr());
                    foreach (TableColumnDescriptionPOJO currentColumn in currentTable.get_ColumnsList())
                    {
                        String currentColumnReplacedDestinationRelativePath =
                                currentTableReplacedDestinationRelativePath.
                                    Replace(COLUMN_TEMPLATE_FILE_NAME_WORD, currentColumn.get_NameStr());
                        String handlerOutput = TemplateHandlerNew.HandleTemplate(
                                handledTemplateStringContent,
                                    databaseDescriptionPOJO, currentTable, currentColumn);
                        String destinationFilePath = PathManager.AppendAtEndFileSeparatorIfNeeded(get_outpuFolderPath()) + currentColumnReplacedDestinationRelativePath;
                        CreateOrReplaceFileWithContent(destinationFilePath, handlerOutput);
                    }
                }
            }
            else if (containsTblWord)
            {
                String currentDatabaseReplacedDestinationRelativePath =
                        specifiedDestinationRelativePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    databaseDescriptionPOJO.getDatabaseNameStr());
                foreach (TableDescriptionPOJO currentTable in databaseDescriptionPOJO.getTableList())
                {
                    String currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                        currentTable.get_NameStr());
                    String handlerOutput = TemplateHandlerNew.HandleTemplate(
                                handledTemplateStringContent,
                                    databaseDescriptionPOJO, currentTable, null);
                    String destinationFilePath = PathManager.AppendAtEndFileSeparatorIfNeeded(get_outpuFolderPath()) + currentTableReplacedDestinationRelativePath;
                    CreateOrReplaceFileWithContent(destinationFilePath, handlerOutput);
                }
            }
            else
            {
                String currentDatabaseReplacedDestinationRelativePath =
                        specifiedDestinationRelativePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    databaseDescriptionPOJO.getDatabaseNameStr());

                String handlerOutput = TemplateHandlerNew.HandleTemplate(
                            handledTemplateStringContent,
                                databaseDescriptionPOJO, null, null);
                String destinationFilePath = PathManager.AppendAtEndFileSeparatorIfNeeded(get_outpuFolderPath()) + currentDatabaseReplacedDestinationRelativePath;
                CreateOrReplaceFileWithContent(destinationFilePath, handlerOutput);

            }
            return true;
        }

        public bool GenerateTableTemplateFiles
        (String handledTemplateFilePath,
                String specifiedDestinationRelativePath,
                        TableDescriptionPOJO tableDescriptionPOJO)
        {
            if (tableDescriptionPOJO == null) return false;
            if (specifiedDestinationRelativePath == null) return false;
            String handledTemplateStringContent = getHandledTemplateStringContent(handledTemplateFilePath);
            if (handledTemplateStringContent == null) return false;
            bool containsColWord =
                        specifiedDestinationRelativePath.
                            Contains(COLUMN_TEMPLATE_FILE_NAME_WORD);
            if (containsColWord)

            {
                String currentDatabaseReplacedDestinationRelativePath =
                        specifiedDestinationRelativePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    tableDescriptionPOJO.ParentDatabase.getDatabaseNameStr());

                String currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                    tableDescriptionPOJO.get_NameStr());
                foreach (TableColumnDescriptionPOJO currentColumn in tableDescriptionPOJO.get_ColumnsList())
                {
                    String currentColumnReplacedDestinationRelativePath =
                            currentTableReplacedDestinationRelativePath.
                                Replace(COLUMN_TEMPLATE_FILE_NAME_WORD, currentColumn.get_NameStr());
                    String handlerOutput = TemplateHandlerNew.HandleTemplate(
                            handledTemplateStringContent,
                                tableDescriptionPOJO.ParentDatabase, tableDescriptionPOJO, currentColumn);
                    String destinationFilePath = PathManager.AppendAtEndFileSeparatorIfNeeded(get_outpuFolderPath()) + currentColumnReplacedDestinationRelativePath;

                    CreateOrReplaceFileWithContent(destinationFilePath, handlerOutput);
                }
            }
            else

            {
                String currentDatabaseReplacedDestinationRelativePath =
                        specifiedDestinationRelativePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    tableDescriptionPOJO.ParentDatabase.getDatabaseNameStr());

                String currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                    tableDescriptionPOJO.get_NameStr());
                String handlerOutput = TemplateHandlerNew.HandleTemplate(
                            handledTemplateStringContent,
                                tableDescriptionPOJO.ParentDatabase, tableDescriptionPOJO, null);
                String destinationFilePath = PathManager.AppendAtEndFileSeparatorIfNeeded(get_outpuFolderPath()) + currentTableReplacedDestinationRelativePath;
                CreateOrReplaceFileWithContent(destinationFilePath, handlerOutput);
            }
            return true;
        }

        private String getHandledTemplateStringContent(String handledTemplateFilePath)
        {
            if (handledTemplateFilePath == null) return null;
            if (!File.Exists(handledTemplateFilePath)) return null;
            using (FileStream fs = new FileStream(handledTemplateFilePath,
                                            FileMode.Open,
                                            FileAccess.Read,
                                            FileShare.ReadWrite))
            {
                if (!fs.CanRead) return null;
                return readFile(fs);
            }
        }

        private String readFile(FileStream file)
        {
            using (StreamReader reader = new StreamReader(file))
            {
                String line = null;
                StringBuilder stringBuilder = new StringBuilder();
                String ls = Environment.NewLine;

                while ((line = reader.ReadLine()) != null)
                {
                    stringBuilder.Append(line);
                    stringBuilder.Append(ls);
                }

                return stringBuilder.ToString();
            }
        }

        private bool CreateOrReplaceFileWithContent(String filePath, String fileContent)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            if (Directory.GetParent(filePath) != null)
            {
                var ParentFolderFile = Directory.GetParent(filePath);
                if (!ParentFolderFile.Exists) ParentFolderFile.Create();
            }
            File.WriteAllText(filePath, fileContent);
            return true;
        }
    }
}
