using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.Template;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.PreprocessorDeclarations;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.IO;
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

        public string ToPreprocessorMappingDeclarationString(IList<ITypeMapping> typeMappings)
        {
            if (typeMappings == null) return string.Empty;
            if (!typeMappings.Any()) return string.Empty;
            var templateHandler = TemplateHandlerBuilder.Build(typeMappings);
            var preprocessor = new MappingDeclarePreprcessorContextHandler(templateHandler);
            return string.Join(Environment.NewLine, typeMappings.Select(preprocessor.ToContextString));
        }

        public IList<ITemplateContextHandlerIdentity> GetAllItemplateContextHandlerIdentity()
        {
            var TemplateHandlerNew = TemplateHandlerBuilder.Build(null);
            return TemplateHandlerNew.GetAllItemplateContextHandlerIdentity();
        }



        public IList<IHandledTemplateResultModel> Process(IDatabaseTemplateHandlerInputModel input)
        {
            var typeMappings = input.typeMappings ?? new List<ITypeMapping>();
            var TemplateHandlerNew = TemplateHandlerBuilder.Build(typeMappings);
            var templatePreprocessor = new TemplatePreprocessor(TemplateHandlerNew, typeMappings);
            var templateModels = input.TemplateModels;
            templatePreprocessor.PreProcess(templateModels);
            var result = templateModels.SelectMany(templateModel =>
                GenerateDatabaseTemplateFiles(templateModel, input.DatabaseModel, TemplateHandlerNew)).Cast<IHandledTemplateResultModel>().ToList();
            return result;
        }

        private string ToTemplateFilePathToOSDependantPath(string filePath)
        {
            return filePath.Replace('\\', Path.DirectorySeparatorChar);
        }

        private IEnumerable<HandledTemplateResultModel> GenerateDatabaseTemplateFiles
            (ITemplateModel templateModel, IDatabaseModel databaseModel,ITemplateHandler templateHandlerNew)
        {
            if (databaseModel == null) yield break;
            if (templateModel == null) yield break;
            string databaseFilePathTemplateWord = DatabaseFilePathTemplateWord;
            string tableFilePathTemplateWord = TableFilePathTemplateWord;
            string columnTemplateFileNameWord = ColumnTemplateFileNameWord;
            string templateFileContent = templateModel.TemplatedFileContent ?? string.Empty;
            string templateFilePath = templateModel.TemplatedFilePath ?? string.Empty;
            templateFilePath = ToTemplateFilePathToOSDependantPath(templateFilePath);
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
                foreach (ITableModel currentTable in databaseModel.Tables)
                {
                    string currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(tableFilePathTemplateWord,
                                        currentTable.Name);
                    foreach (IColumnModel currentColumn in currentTable.Columns)
                    {
                        string currentColumnReplacedDestinationRelativePath =
                                currentTableReplacedDestinationRelativePath.
                                    Replace(columnTemplateFileNameWord, currentColumn.Name);
                        string handlerOutput = templateHandlerNew.HandleTemplate(
                                templateFileContent,
                                    databaseModel, currentTable, currentColumn,null);
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
                foreach (ITableModel currentTable in databaseModel.Tables)
                {
                    string currentTableReplacedDestinationRelativePath =
                            currentDatabaseReplacedDestinationRelativePath.
                                Replace(tableFilePathTemplateWord,
                                        currentTable.Name);
                    string handlerOutput = templateHandlerNew.HandleTemplate(
                                templateFileContent,
                                    databaseModel, currentTable, null,null);
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

                string handlerOutput = templateHandlerNew.HandleTemplate(
                            templateFileContent,
                                databaseModel, null, null,null);
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
