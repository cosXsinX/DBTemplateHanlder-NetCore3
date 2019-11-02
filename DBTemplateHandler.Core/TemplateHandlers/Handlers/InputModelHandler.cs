using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class InputModelHandler
    {

        private const string DATABASE_TEMPLATE_FILE_NAME_WORD = "%databaseName%";
        private const string TABLE_TEMPLATE_FILE_NAME_WORD = "%tableName%";
        private const string COLUMN_TEMPLATE_FILE_NAME_WORD = "%columnName%";

        public IList<HandledTemplateResultModel> Process(DatabaseTemplateHandlerInputModel input)
        {
            var result = input.TemplateModels.SelectMany(templateModel =>
                GenerateDatabaseTemplateFiles(templateModel, input.DatabaseModel)).ToList();
            return result;
        }

        public IEnumerable<HandledTemplateResultModel> GenerateDatabaseTemplateFiles
            (TemplateModel templateModel, DatabaseModel databaseModel)
        {
            if (databaseModel == null) yield break;
            if (templateModel == null) yield break;
            string templateFileContent = templateModel.TemplatedFileContent ?? string.Empty;
            string templateFilePath = templateModel.TemplatedFilePath ?? string.Empty;
            bool containsTblWord =
                    templateFilePath.
                        Contains(TABLE_TEMPLATE_FILE_NAME_WORD);
            bool containsColWord =
                    templateFilePath.
                        Contains(COLUMN_TEMPLATE_FILE_NAME_WORD);
            if (containsColWord)
            {
                string currentDatabaseReplacedDestinationRelativePath =
                        templateFilePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    databaseModel.Name);
                foreach (TableModel currentTable in databaseModel.Tables)
                {
                    string currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                        currentTable.Name);
                    foreach (ColumnModel currentColumn in currentTable.Columns)
                    {
                        string currentColumnReplacedDestinationRelativePath =
                                currentTableReplacedDestinationRelativePath.
                                    Replace(COLUMN_TEMPLATE_FILE_NAME_WORD, currentColumn.Name);
                        string handlerOutput = TemplateHandlerNew.HandleTemplate(
                                templateFileContent,
                                    databaseModel, currentTable, currentColumn);
                        string destinationFilePath = currentColumnReplacedDestinationRelativePath;
                        yield return new HandledTemplateResultModel()
                        {
                            Path = destinationFilePath,
                            Content = handlerOutput
                        };
                    }
                }
            }
            else if (containsTblWord)
            {
                string currentDatabaseReplacedDestinationRelativePath =
                        templateFilePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    databaseModel.Name);
                foreach (TableModel currentTable in databaseModel.Tables)
                {
                    string currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                        currentTable.Name);
                    string handlerOutput = TemplateHandlerNew.HandleTemplate(
                                templateFileContent,
                                    databaseModel, currentTable, null);
                    string destinationFilePath = currentTableReplacedDestinationRelativePath;
                    yield return new HandledTemplateResultModel()
                    {
                        Path = destinationFilePath,
                        Content = handlerOutput
                    };
                }
            }
            else
            {
                string currentDatabaseReplacedDestinationRelativePath =
                        templateFilePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    databaseModel.Name);

                string handlerOutput = TemplateHandlerNew.HandleTemplate(
                            templateFileContent,
                                databaseModel, null, null);
                string destinationFilePath = currentDatabaseReplacedDestinationRelativePath;
                yield return new HandledTemplateResultModel()
                {
                    Path = destinationFilePath,
                    Content = handlerOutput
                };
            }
        }

        public IEnumerable<HandledTemplateResultModel> GenerateTableTemplateFiles
        (TemplateModel templateModel, TableModel tableModel)
        {
            if (tableModel == null) yield break;


            string templateFileContent = templateModel.TemplatedFileContent ?? string.Empty;
            string templateFilePath = templateModel.TemplatedFilePath ?? string.Empty;
            bool containsColWord =
                        templateFilePath.
                            Contains(COLUMN_TEMPLATE_FILE_NAME_WORD);
            if (containsColWord)

            {
                string currentDatabaseReplacedDestinationRelativePath =
                        templateFilePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    tableModel.ParentDatabase.Name);

                string currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                    tableModel.Name);
                foreach (ColumnModel currentColumn in tableModel.Columns)
                {
                    string currentColumnReplacedDestinationRelativePath =
                            currentTableReplacedDestinationRelativePath.
                                Replace(COLUMN_TEMPLATE_FILE_NAME_WORD, currentColumn.Name);
                    string handlerOutput = TemplateHandlerNew.HandleTemplate(
                            templateFileContent,
                                tableModel.ParentDatabase, tableModel, currentColumn);
                    string destinationFilePath = currentColumnReplacedDestinationRelativePath;

                    yield return new HandledTemplateResultModel()
                    {
                        Path = destinationFilePath,
                        Content = handlerOutput
                    };
                }
            }
            else

            {
                string currentDatabaseReplacedDestinationRelativePath =
                        templateFilePath.Replace(
                                DATABASE_TEMPLATE_FILE_NAME_WORD,
                                    tableModel.ParentDatabase.Name);

                string currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(TABLE_TEMPLATE_FILE_NAME_WORD,
                                    tableModel.Name);
                string handlerOutput = TemplateHandlerNew.HandleTemplate(
                            templateFileContent,
                                tableModel.ParentDatabase, tableModel, null);
                string destinationFilePath = currentTableReplacedDestinationRelativePath;
                yield return new HandledTemplateResultModel()
                {
                    Path = destinationFilePath,
                    Content = handlerOutput
                };
            }
        }
    }
}
