using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class TemplateHandlerNew
    {
        private static readonly TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler> 
            templateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>();

        private static readonly TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>
            databaseTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>();

        private static readonly TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>
            tableTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>();
        
        private static readonly TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>
            columnTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>();
        
        private static readonly TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>
            functionTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>();





        public static string HandleTemplate(string templateString,
            IDatabaseModel databaseModel,
            ITableModel tableModel,
            IColumnModel columnModel)
        {
            if (templateString == null) return null;
            string handlerStartContext =
                templateContextHandlerProvider.
                    GetHandlerStartContextWordAtEarliestPosition(templateString);
            if (handlerStartContext == null) return templateString;

            AbstractTemplateContextHandler handler = templateContextHandlerProvider
                    .GetStartContextCorrespondingContextHandler(handlerStartContext);


            if (handler is AbstractFunctionTemplateContextHandler)
            {
                return HandleFunctionTemplate(templateString, databaseModel, tableModel, columnModel);
            }
            else if (handler is AbstractDatabaseTemplateContextHandler)
            {
                if (databaseModel == null)
                    return templateString;
                return HandleDatabaseTemplate(templateString, databaseModel);
            }
            else if (handler is AbstractTableTemplateContextHandler)
            {
                if (tableModel == null)
                    return templateString;
                return HandleTableTemplate(templateString, tableModel);
            }
            else if (handler is AbstractColumnTemplateContextHandler)
            {
                if (columnModel == null)
                    return templateString;
                return HandleTableColumnTemplate(templateString, columnModel);
            }
            return null;
        }


        public static string HandleDatabaseTemplate(
                string templateString, IDatabaseModel databaseModel)
        {
            if (databaseModel == null)
                return templateString;
            if (!databaseTemplateContextHandlerProvider.ContainsAHandlerStartContextOfType(templateString))
                return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;
            UpdateContainedTables(databaseModel);
            string currentHandledTemplateString = templateString;
            Stack<string> StartContextWordStack = new Stack<string>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            string earliestStartContextWord = templateContextHandlerProvider.
                GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
            string earliestEndContextWord = templateContextHandlerProvider.
                    GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            while (earliestStartContextWord != null || earliestEndContextWord != null)
            {
                if (earliestStartContextWord != null && earliestEndContextWord != null)
                {
                    int earliestStartContextWordIndex = currentHandledTemplateString.IndexOf(earliestStartContextWord);
                    int earliestEndContextWordIndex = currentHandledTemplateString.IndexOf(earliestEndContextWord);

                    if (earliestStartContextWordIndex < earliestEndContextWordIndex)
                    {
                        StartContextWordStack.Push(earliestStartContextWord);

                        currentHandledContextBufferStringBuilder.
                            Append(StringUtilities.getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestStartContextWord));
                        HanldedContextStringBuilderStakce.Push(currentHandledContextBufferStringBuilder);
                        currentHandledContextBufferStringBuilder = new StringBuilder();

                        currentHandledTemplateString = StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                                        (currentHandledTemplateString, earliestStartContextWord);

                    }
                    else if (earliestStartContextWordIndex > earliestEndContextWordIndex)
                    {

                        String lastStartContextWord = StartContextWordStack.Pop();
                        String lastStartContextWordAssociatedEndContextWord = templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractDatabaseTemplateContextHandler templateContextHandler
                                = databaseTemplateContextHandlerProvider.
                                    GetStartContextCorrespondingContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.DatabaseModel =databaseModel;
                                String processContextResult = templateContextHandler.processContext(
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord);
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                            else
                            {
                                String processContextResult =
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord;
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                        }

                        currentHandledTemplateString =
                                StringUtilities.
                                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                        (currentHandledTemplateString, earliestEndContextWord);
                    }
                }
                else if (earliestEndContextWord != null)
                {
                    String lastStartContextWord = StartContextWordStack.Pop();
                    String lastStartContextWordAssociatedEndContextWord = templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractDatabaseTemplateContextHandler templateContextHandler
                            = databaseTemplateContextHandlerProvider.
                                GetStartContextCorrespondingContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.DatabaseModel = databaseModel;
                            String processContextResult = templateContextHandler.processContext(
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord);
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                        else
                        {
                            String processContextResult =
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord;
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                    }

                    currentHandledTemplateString =
                            StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                    (currentHandledTemplateString, earliestEndContextWord);
                }
                earliestStartContextWord = templateContextHandlerProvider.
                    GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
                earliestEndContextWord = templateContextHandlerProvider.
                        GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }

        public static string HandleTableTemplate(string templateString, ITableModel tableModel)
        {
            if (tableModel == null)
                return templateString;
            if (!tableTemplateContextHandlerProvider.
                    ContainsAHandlerStartContextOfType(templateString)) return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;
            UpdateContainedColumns(tableModel);

            string currentHandledTemplateString = templateString;
            Stack<string> StartContextWordStack = new Stack<string>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            string earliestStartContextWord = templateContextHandlerProvider.
                    GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
            string earliestEndContextWord = templateContextHandlerProvider.
                    GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            while (earliestStartContextWord != null || earliestEndContextWord != null)
            {
                if (earliestStartContextWord != null && earliestEndContextWord != null)
                {
                    int earliestStartContextWordIndex = currentHandledTemplateString.IndexOf(earliestStartContextWord);
                    int earliestEndContextWordIndex = currentHandledTemplateString.IndexOf(earliestEndContextWord);

                    if (earliestStartContextWordIndex < earliestEndContextWordIndex)
                    {
                        StartContextWordStack.Push(earliestStartContextWord);

                        currentHandledContextBufferStringBuilder.
                            Append(StringUtilities.getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestStartContextWord));
                        HanldedContextStringBuilderStakce.Push(currentHandledContextBufferStringBuilder);
                        currentHandledContextBufferStringBuilder = new StringBuilder();

                        currentHandledTemplateString = StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                                        (currentHandledTemplateString, earliestStartContextWord);

                    }
                    else if (earliestStartContextWordIndex > earliestEndContextWordIndex)
                    {

                        String lastStartContextWord = StartContextWordStack.Pop();
                        String lastStartContextWordAssociatedEndContextWord = templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractTableTemplateContextHandler templateContextHandler
                                = tableTemplateContextHandlerProvider.
                                    GetStartContextCorrespondingContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.TableModel =tableModel;
                                String processContextResult = templateContextHandler.processContext(
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord);
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                            else
                            {
                                String processContextResult =
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord;
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                        }

                        currentHandledTemplateString =
                                StringUtilities.
                                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                        (currentHandledTemplateString, earliestEndContextWord);
                    }
                }
                else if (earliestEndContextWord != null)
                {
                    String lastStartContextWord = StartContextWordStack.Pop();
                    String lastStartContextWordAssociatedEndContextWord = templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractTableTemplateContextHandler templateContextHandler
                            = tableTemplateContextHandlerProvider.
                                GetStartContextCorrespondingContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.TableModel = tableModel;
                            String processContextResult = templateContextHandler.processContext(
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord);
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                        else
                        {
                            String processContextResult =
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord;
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                    }

                    currentHandledTemplateString =
                            StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                    (currentHandledTemplateString, earliestEndContextWord);
                }
                earliestStartContextWord = templateContextHandlerProvider.
                    GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
                earliestEndContextWord = templateContextHandlerProvider.
                        GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }

        public static string HandleTableColumnTemplate(string templateString, IColumnModel descriptionPOJO)
        {
            if (descriptionPOJO == null) return templateString;
            if (!columnTemplateContextHandlerProvider.
                    ContainsAHandlerStartContextOfType(templateString)) return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;

            string currentHandledTemplateString = templateString;
            Stack<string> StartContextWordStack = new Stack<string>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            string earliestStartContextWord = templateContextHandlerProvider.
                GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
            string earliestEndContextWord = templateContextHandlerProvider.
                    GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            while (earliestStartContextWord != null || earliestEndContextWord != null)
            {
                if (earliestStartContextWord != null && earliestEndContextWord != null)
                {
                    int earliestStartContextWordIndex = currentHandledTemplateString.IndexOf(earliestStartContextWord);
                    int earliestEndContextWordIndex = currentHandledTemplateString.IndexOf(earliestEndContextWord);

                    if (earliestStartContextWordIndex < earliestEndContextWordIndex)
                    {
                        StartContextWordStack.Push(earliestStartContextWord);

                        currentHandledContextBufferStringBuilder.
                            Append(StringUtilities.getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestStartContextWord));
                        HanldedContextStringBuilderStakce.Push(currentHandledContextBufferStringBuilder);
                        currentHandledContextBufferStringBuilder = new StringBuilder();

                        currentHandledTemplateString = StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                                        (currentHandledTemplateString, earliestStartContextWord);

                    }
                    else if (earliestStartContextWordIndex > earliestEndContextWordIndex)
                    {

                        String lastStartContextWord = StartContextWordStack.Pop();
                        String lastStartContextWordAssociatedEndContextWord = templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractColumnTemplateContextHandler templateContextHandler
                                = columnTemplateContextHandlerProvider.
                                    GetStartContextCorrespondingContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.ColumnModel = descriptionPOJO;
                                String processContextResult = templateContextHandler.processContext(
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord);
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                            else
                            {
                                String processContextResult =
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord;
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                        }

                        currentHandledTemplateString =
                                StringUtilities.
                                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                        (currentHandledTemplateString, earliestEndContextWord);
                    }
                }
                else if (earliestEndContextWord != null)
                {
                    String lastStartContextWord = StartContextWordStack.Pop();
                    String lastStartContextWordAssociatedEndContextWord = templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractColumnTemplateContextHandler templateContextHandler
                            = columnTemplateContextHandlerProvider.
                                GetStartContextCorrespondingContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.ColumnModel= descriptionPOJO;
                            String processContextResult = templateContextHandler.processContext(
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord);
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                        else
                        {
                            String processContextResult =
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord;
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                    }

                    currentHandledTemplateString =
                            StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                    (currentHandledTemplateString, earliestEndContextWord);
                }
                earliestStartContextWord = templateContextHandlerProvider.
                    GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
                earliestEndContextWord = templateContextHandlerProvider.
                        GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }

        private static void UpdateContainedTables(IDatabaseModel databaseModel)
        {
            var tables = databaseModel.Tables.ToList();
            tables.ForEach(table => table.ParentDatabase = databaseModel);
            tables.ForEach(table => UpdateContainedColumns(table));
        }

        private static void UpdateContainedColumns(ITableModel tableModel)
        {
            var columns = tableModel.Columns.ToList();
            columns.ForEach(m => m.ParentTable = tableModel);
        }

        public static string HandleFunctionTemplate(
                string templateString, IDatabaseModel databaseModel,
                ITableModel tableModel, IColumnModel columnDescriptionPojo)
        {
            if (!functionTemplateContextHandlerProvider.
                    ContainsAHandlerStartContextOfType(templateString)) return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;
            if (databaseModel != null) UpdateContainedTables(databaseModel);
            if (tableModel != null) UpdateContainedColumns(tableModel);

            string currentHandledTemplateString = templateString;
            Stack<string> StartContextWordStack = new Stack<string>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            string earliestStartContextWord = templateContextHandlerProvider.
                GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
            string earliestEndContextWord = templateContextHandlerProvider.
                    GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            while (earliestStartContextWord != null || earliestEndContextWord != null)
            {
                if (earliestStartContextWord != null && earliestEndContextWord != null)
                {
                    int earliestStartContextWordIndex = currentHandledTemplateString.IndexOf(earliestStartContextWord);
                    int earliestEndContextWordIndex = currentHandledTemplateString.IndexOf(earliestEndContextWord);

                    if (earliestStartContextWordIndex < earliestEndContextWordIndex)
                    {
                        StartContextWordStack.Push(earliestStartContextWord);

                        currentHandledContextBufferStringBuilder.
                            Append(StringUtilities.getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestStartContextWord));
                        HanldedContextStringBuilderStakce.Push(currentHandledContextBufferStringBuilder);
                        currentHandledContextBufferStringBuilder = new StringBuilder();

                        currentHandledTemplateString = StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                                        (currentHandledTemplateString, earliestStartContextWord);

                    }
                    else if (earliestStartContextWordIndex > earliestEndContextWordIndex)
                    {

                        String lastStartContextWord = StartContextWordStack.Pop();
                        String lastStartContextWordAssociatedEndContextWord = templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractFunctionTemplateContextHandler templateContextHandler
                                = functionTemplateContextHandlerProvider.
                                    GetStartContextCorrespondingContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.DatabaseModel = databaseModel;
                                templateContextHandler.TableModel= tableModel;
                                templateContextHandler.ColumnModel =columnDescriptionPojo;
                                String processContextResult = templateContextHandler.processContext(
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord);
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                            else
                            {
                                String processContextResult =
                                        lastStartContextWord +
                                            currentHandledContextBufferStringBuilder.ToString() +
                                                earliestEndContextWord;
                                currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                                currentHandledContextBufferStringBuilder.Append(processContextResult);
                            }
                        }

                        currentHandledTemplateString =
                                StringUtilities.
                                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                        (currentHandledTemplateString, earliestEndContextWord);
                    }
                }
                else if (earliestEndContextWord != null)
                {
                    String lastStartContextWord = StartContextWordStack.Pop();
                    String lastStartContextWordAssociatedEndContextWord =
                        templateContextHandlerProvider.GetStartContextCorrespondingEndContext(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractFunctionTemplateContextHandler templateContextHandler
                            = functionTemplateContextHandlerProvider.
                                GetStartContextCorrespondingContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.DatabaseModel = databaseModel;
                            templateContextHandler.TableModel = tableModel;
                            templateContextHandler.ColumnModel = columnDescriptionPojo;
                            String processContextResult = templateContextHandler.processContext(
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord);
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                        else
                        {
                            String processContextResult =
                                    lastStartContextWord +
                                        currentHandledContextBufferStringBuilder.ToString() +
                                            earliestEndContextWord;
                            currentHandledContextBufferStringBuilder = HanldedContextStringBuilderStakce.Pop();
                            currentHandledContextBufferStringBuilder.Append(processContextResult);
                        }
                    }

                    currentHandledTemplateString =
                            StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                    (currentHandledTemplateString, earliestEndContextWord);
                }
                earliestStartContextWord = templateContextHandlerProvider.
                    GetHandlerStartContextWordAtEarliestPosition(currentHandledTemplateString);
                earliestEndContextWord = templateContextHandlerProvider.
                        GetHandlerEndContextWordAtEarliestPosition(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }
    }
}
