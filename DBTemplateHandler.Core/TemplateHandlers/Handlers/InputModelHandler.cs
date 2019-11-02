using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class InputModelHandler
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
            (string handledTemplateFilePath,
                    string specifiedDestinationRelativePath,
                        DatabaseModel databaseDescriptionPOJO)
        {
            if (databaseDescriptionPOJO == null) return false;
            if (specifiedDestinationRelativePath == null) return false;
            string handledTemplateStringContent = getHandledTemplateStringContent(handledTemplateFilePath);
            if (handledTemplateStringContent == null) return false;
            bool containsTblWord =
                    specifiedDestinationRelativePath.
                        Contains(TABLE_TEMPLATE_FILE_NAME_WORD);
            bool containsColWord =
                    handledTemplateFilePath.
                        Contains(COLUMN_TEMPLATE_FILE_NAME_WORD);
            if (containsColWord)
            {
                string currentDatabaseReplacedDestinationRelativePath =
                        specifiedDestinationRelativePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    databaseDescriptionPOJO.Name);
                foreach (TableModel currentTable in databaseDescriptionPOJO.Tables)
                {
                    String currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                        currentTable.Name);
                    foreach (ColumnModel currentColumn in currentTable.Columns)
                    {
                        String currentColumnReplacedDestinationRelativePath =
                                currentTableReplacedDestinationRelativePath.
                                    Replace(COLUMN_TEMPLATE_FILE_NAME_WORD, currentColumn.Name);
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
                                    databaseDescriptionPOJO.Name);
                foreach (TableModel currentTable in databaseDescriptionPOJO.Tables)
                {
                    String currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                        currentTable.Name);
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
                                    databaseDescriptionPOJO.Name);

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
                        TableModel tableDescriptionPOJO)
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
                                    tableDescriptionPOJO.ParentDatabase.Name);

                String currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                    tableDescriptionPOJO.Name);
                foreach (ColumnModel currentColumn in tableDescriptionPOJO.Columns)
                {
                    String currentColumnReplacedDestinationRelativePath =
                            currentTableReplacedDestinationRelativePath.
                                Replace(COLUMN_TEMPLATE_FILE_NAME_WORD, currentColumn.Name);
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
                                    tableDescriptionPOJO.ParentDatabase.Name);

                String currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                    tableDescriptionPOJO.Name);
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
