﻿using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Service.Contracts.TypeMapping;
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


        public IList<ITemplateContextHandlerIdentity> GetAllItemplateContextHandlerIdentity()
        {
            var TemplateHandlerNew = new TemplateHandlerNew(null);
            return TemplateHandlerNew.GetAllItemplateContextHandlerIdentity();
        }

        public IList<IHandledTemplateResultModel> Process(IDatabaseTemplateHandlerInputModel input)
        {
            var result = input.TemplateModels.SelectMany(templateModel =>
                GenerateDatabaseTemplateFiles(templateModel, input.DatabaseModel, input.typeMappings)).Cast<IHandledTemplateResultModel>().ToList();
            return result;
        }

        public IEnumerable<HandledTemplateResultModel> GenerateDatabaseTemplateFiles
            (ITemplateModel templateModel, IDatabaseModel databaseModel,IList<ITypeMapping> typeMappings)
        {

            var TemplateHandlerNew = new TemplateHandlerNew(typeMappings);
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
    }
}
