using System;
using System.Collections.Generic;
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
        public static string HandleTemplate(string templateString,
            DatabaseDescriptor databaseDescriptionPOJO,
            TableDescriptor tableDescriptionPOJO,
            ColumnDescriptor columnDescriptionPOJO)
        {
            if (templateString == null) return null;
            string handlerStartContext =
                TemplateContextHandlerPackageProvider.
                    getHandlerStartContextWordAtEarliestPositionInSubmittedString
                        (templateString);
            if (handlerStartContext == null) return templateString;

            AbstractTemplateContextHandler handler = TemplateContextHandlerPackageProvider
                    .getStartContextCorrespondingContextHandler(handlerStartContext);


            if (handler is AbstractFunctionTemplateContextHandler)
            {
                return TemplateHandlerNew.HandleFunctionTemplate(templateString, databaseDescriptionPOJO, tableDescriptionPOJO, columnDescriptionPOJO);
            }
            else if (handler is AbstractDatabaseTemplateContextHandler)
            {
                if (databaseDescriptionPOJO == null)
                    return templateString;
                return TemplateHandlerNew.HandleDatabaseTemplate(templateString, databaseDescriptionPOJO);
            }
            else if (handler is AbstractTableTemplateContextHandler)
            {
                if (tableDescriptionPOJO == null)
                    return templateString;
                return TemplateHandlerNew.HandleTableTemplate(templateString, tableDescriptionPOJO);
            }
            else if (handler is AbstractColumnTemplateContextHandler)
            {
                if (columnDescriptionPOJO == null)
                    return templateString;
                return TemplateHandlerNew.HandleTableColumnTemplate(templateString, columnDescriptionPOJO);
            }
            return null;
        }


        public static String HandleDatabaseTemplate(
                String templateString, DatabaseDescriptor descriptionPOJO)
        {
            if (descriptionPOJO == null)
                return templateString;
            if (!TemplateContextHandlerPackageProvider.
                    IsSubmittedStringContainsADatabaseHandlerStartContextWord(templateString)) return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;
            descriptionPOJO.UpdateContainedTablesParentReference();
            String currentHandledTemplateString = templateString;
            Stack<String> StartContextWordStack = new Stack<String>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            String earliestStartContextWord = TemplateContextHandlerPackageProvider.
                getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            String earliestEndContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
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
                        String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractDatabaseTemplateContextHandler templateContextHandler
                                = TemplateContextHandlerPackageProvider.
                                    getStartContextCorrespondingDatabaseContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.setAssociatedDatabaseDescriptorPOJO(descriptionPOJO);
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
                    String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractDatabaseTemplateContextHandler templateContextHandler
                            = TemplateContextHandlerPackageProvider.
                                getStartContextCorrespondingDatabaseContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.setAssociatedDatabaseDescriptorPOJO(descriptionPOJO);
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
                earliestStartContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
                earliestEndContextWord = TemplateContextHandlerPackageProvider.
                        getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }

        public static String HandleTableTemplate(String templateString, TableDescriptor descriptionPOJO)
        {
            if (descriptionPOJO == null)
                return templateString;
            if (!TemplateContextHandlerPackageProvider.
                    IsSubmittedStringContainsATableHandlerStartContextWord(templateString)) return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;
            descriptionPOJO.UpdateContainedColumnsParentReference();

            String currentHandledTemplateString = templateString;
            Stack<String> StartContextWordStack = new Stack<String>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            String earliestStartContextWord = TemplateContextHandlerPackageProvider.
                getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            String earliestEndContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
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
                        String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractTableTemplateContextHandler templateContextHandler
                                = TemplateContextHandlerPackageProvider.
                                    GetStartContextCorrespondingTableContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.setAssociatedTableDescriptorPOJO(descriptionPOJO);
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
                    String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractTableTemplateContextHandler templateContextHandler
                            = TemplateContextHandlerPackageProvider.
                                GetStartContextCorrespondingTableContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.setAssociatedTableDescriptorPOJO(descriptionPOJO);
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
                earliestStartContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
                earliestEndContextWord = TemplateContextHandlerPackageProvider.
                        getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }

        public static String HandleTableColumnTemplate(String templateString, ColumnDescriptor descriptionPOJO)
        {
            if (descriptionPOJO == null)
                return templateString;
            if (!TemplateContextHandlerPackageProvider.
                    IsSubmittedStringContainsAColumnHandlerStartContextWord(templateString)) return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;

            String currentHandledTemplateString = templateString;
            Stack<String> StartContextWordStack = new Stack<String>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            String earliestStartContextWord = TemplateContextHandlerPackageProvider.
                getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            String earliestEndContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
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
                        String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractColumnTemplateContextHandler templateContextHandler
                                = TemplateContextHandlerPackageProvider.
                                    GetStartContextCorrespondingColumnContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.setAssociatedColumnDescriptorPOJO(descriptionPOJO);
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
                    String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractColumnTemplateContextHandler templateContextHandler
                            = TemplateContextHandlerPackageProvider.
                                GetStartContextCorrespondingColumnContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.setAssociatedColumnDescriptorPOJO(descriptionPOJO);
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
                earliestStartContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
                earliestEndContextWord = TemplateContextHandlerPackageProvider.
                        getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }

        public static String HandleFunctionTemplate(
                String templateString, DatabaseDescriptor databaseDescriptionPOJO,
                TableDescriptor tableDescriptionPOJO, ColumnDescriptor columnDescriptionPojo)
        {
            if (!TemplateContextHandlerPackageProvider.
                    isSubmittedStringContainsAFunctionHandlerStartContextWord(templateString)) return templateString;
            if (!TemplateValidator.TemplateStringValidation(templateString)) return templateString;
            if (databaseDescriptionPOJO != null) databaseDescriptionPOJO.UpdateContainedTablesParentReference();
            if (tableDescriptionPOJO != null) tableDescriptionPOJO.UpdateContainedColumnsParentReference();

            String currentHandledTemplateString = templateString;
            Stack<String> StartContextWordStack = new Stack<String>();

            Stack<StringBuilder> HanldedContextStringBuilderStakce = new Stack<StringBuilder>();
            StringBuilder currentHandledContextBufferStringBuilder = new StringBuilder();

            String earliestStartContextWord = TemplateContextHandlerPackageProvider.
                getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            String earliestEndContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
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
                        String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                        if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                    getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                            AbstractFunctionTemplateContextHandler templateContextHandler
                                = TemplateContextHandlerPackageProvider.
                                    getStartContextCorrespondingFunctionContextHandler(lastStartContextWord);
                            if (templateContextHandler != null)
                            {
                                templateContextHandler.setAssociatedDatabaseDescriptionPOJO(databaseDescriptionPOJO);
                                templateContextHandler.setAssociatedTableDescriptorPOJO(tableDescriptionPOJO);
                                templateContextHandler.setAssociatedColumnDescriptionPOJO(columnDescriptionPojo);
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
                    String lastStartContextWordAssociatedEndContextWord = TemplateContextHandlerPackageProvider.getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                    if (lastStartContextWordAssociatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        currentHandledContextBufferStringBuilder.Append(StringUtilities.
                                getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence(currentHandledTemplateString, earliestEndContextWord));

                        AbstractFunctionTemplateContextHandler templateContextHandler
                            = TemplateContextHandlerPackageProvider.
                                getStartContextCorrespondingFunctionContextHandler(lastStartContextWord);
                        if (templateContextHandler != null)
                        {
                            templateContextHandler.setAssociatedDatabaseDescriptionPOJO(databaseDescriptionPOJO);
                            templateContextHandler.setAssociatedTableDescriptorPOJO(tableDescriptionPOJO);
                            templateContextHandler.setAssociatedColumnDescriptionPOJO(columnDescriptionPojo);
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
                earliestStartContextWord = TemplateContextHandlerPackageProvider.
                    getHandlerStartContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
                earliestEndContextWord = TemplateContextHandlerPackageProvider.
                        getHandlerEndContextWordAtEarliestPositionInSubmittedString(currentHandledTemplateString);
            }
            if (!currentHandledTemplateString.Equals("")) currentHandledContextBufferStringBuilder.Append(currentHandledTemplateString);
            return currentHandledContextBufferStringBuilder.ToString();
        }

        public static bool IsEarliestPositionStartContextWrapperADatabaseStartContextWrapper(String templateString)
        {
            if (templateString == null) return false;
            if (templateString.Equals("")) return false;
            String earliestStartContextWrapperString = TemplateContextHandlerPackageProvider.
                GetColumnHandlerStartContextWordAtEarliestPosition(templateString);
            IDictionary<String, AbstractDatabaseTemplateContextHandler> map =
                    TemplateContextHandlerPackageProvider.
                        getStartContextWrapperStringIndexedDatabaseContextHandlerMap();
            return map.ContainsKey(earliestStartContextWrapperString);
        }

        public static bool IsEarliestPositionStartContextWrapperATableStartContextWrapper(String templateString)
        {
            if (templateString == null) return false;
            if (templateString.Equals("")) return false;
            String earliestStartContextWrapperString = TemplateContextHandlerPackageProvider.
                GetColumnHandlerStartContextWordAtEarliestPosition(templateString);
            IDictionary<String, AbstractTableTemplateContextHandler> map =
                    TemplateContextHandlerPackageProvider.
                        GetTableContextHandlerByStartContextWord();
            return map.ContainsKey(earliestStartContextWrapperString);
        }

        public static bool IsEarliestPositionStartContextWrapperAColumnStartContextWrapper(String templateString)
        {
            if (templateString == null) return false;
            if (templateString.Equals("")) return false;
            String earliestStartContextWrapperString = TemplateContextHandlerPackageProvider.
                GetColumnHandlerStartContextWordAtEarliestPosition(templateString);
            IDictionary<String, AbstractColumnTemplateContextHandler> map =
                    TemplateContextHandlerPackageProvider.
                        GetColumnContextHandlerByStartContextSignature();
            return map.ContainsKey(earliestStartContextWrapperString);
        }
    }
}
