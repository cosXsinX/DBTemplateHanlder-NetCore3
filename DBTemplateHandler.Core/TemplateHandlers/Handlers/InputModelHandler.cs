using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class InputModelHandler
    {

        private const string DatabaseFilePathTemplateWordValue = "%databaseName%";
        private const string TableFilePathTemplateWordValue = "%tableName%";
        private const string ColumnTemplateFileNameWordValue = "%columnName%";

        public string DatabaseFilePathTemplateWord
        {
            get => DatabaseFilePathTemplateWordValue;
        }

        public string TableFilePathTemplateWord
        {
            get => TableFilePathTemplateWordValue;
        }

        public string ColumnTemplateFileNameWord
        {
            get => ColumnTemplateFileNameWordValue;
        }
        
        public IList<string> AllFilePathTemplateWords
        {
            get
            {
                return new List<string> 
                {
                    DatabaseFilePathTemplateWord,
                    TableFilePathTemplateWord,
                    ColumnTemplateFileNameWord
                };
            }
        }

        public List<string> AllContentStartContextWord
        {
            get
            {
                Template
            }
        }

        public IList<IHandledTemplateResultModel> Process(IDatabaseTemplateHandlerInputModel input)
        {
            var result = input.TemplateModels.SelectMany(templateModel =>
                GenerateDatabaseTemplateFiles(templateModel, input.DatabaseModel)).Cast<IHandledTemplateResultModel>().ToList();
            return result;
        }

        public IEnumerable<HandledTemplateResultModel> GenerateDatabaseTemplateFiles
            (ITemplateModel templateModel, IDatabaseModel databaseModel)
        {
            if (databaseModel == null) yield break;
            if (templateModel == null) yield break;
            string databaseFilePathTemplateWord = DatabaseFilePathTemplateWord;
            string tableFilePathTemplateWord = TableFilePathTemplateWord;
            string columnTemplateFileNameWord = ColumnTemplateFileNameWord;
            string templateFileContent = templateModel.TemplatedFileContent ?? string.Empty;
            string templateFilePath = templateModel.TemplatedFilePath ?? string.Empty;
            bool containsTblWord =
                    templateFilePath.
                        Contains(tableFilePathTemplateWord);
            bool containsColWord =
                    templateFilePath.
                        Contains(columnTemplateFileNameWord);
            if (containsColWord)
            {
                string currentDatabaseReplacedDestinationRelativePath =
                        templateFilePath.Replace(
                                databaseFilePathTemplateWord,
                                    databaseModel.Name);
                foreach (TableModel currentTable in databaseModel.Tables)
                {
                    string currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(tableFilePathTemplateWord,
                                        currentTable.Name);
                    foreach (ColumnModel currentColumn in currentTable.Columns)
                    {
                        string currentColumnReplacedDestinationRelativePath =
                                currentTableReplacedDestinationRelativePath.
                                    Replace(columnTemplateFileNameWord, currentColumn.Name);
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
                                databaseFilePathTemplateWord,
                                    databaseModel.Name);
                foreach (TableModel currentTable in databaseModel.Tables)
                {
                    string currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(tableFilePathTemplateWord,
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
                                databaseFilePathTemplateWord,
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

            string databaseFilePathTemplateWord = DatabaseFilePathTemplateWord;
            string tableFilePathTemplateWord = TableFilePathTemplateWord;
            string columnTemplateFileNameWord = ColumnTemplateFileNameWord;
            string templateFileContent = templateModel.TemplatedFileContent ?? string.Empty;
            string templateFilePath = templateModel.TemplatedFilePath ?? string.Empty;
            bool containsColWord =
                        templateFilePath.
                            Contains(columnTemplateFileNameWord);
            if (containsColWord)

            {
                string currentDatabaseReplacedDestinationRelativePath =
                        templateFilePath.Replace(
                                databaseFilePathTemplateWord,
                                    tableModel.ParentDatabase.Name);

                string currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(tableFilePathTemplateWord,
                                    tableModel.Name);
                foreach (ColumnModel currentColumn in tableModel.Columns)
                {
                    string currentColumnReplacedDestinationRelativePath =
                            currentTableReplacedDestinationRelativePath.
                                Replace(columnTemplateFileNameWord, currentColumn.Name);
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
                                databaseFilePathTemplateWord,
                                    tableModel.ParentDatabase.Name);

                string currentTableReplacedDestinationRelativePath =
                        currentDatabaseReplacedDestinationRelativePath.
                            Replace(tableFilePathTemplateWord,
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
