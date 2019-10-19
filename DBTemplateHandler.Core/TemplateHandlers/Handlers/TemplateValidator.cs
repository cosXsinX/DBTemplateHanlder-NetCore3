using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class TemplateValidator
    {

        public static bool TemplateStringValidation(String ValidatedTemplateString)
        {
            if (!ContextOpeningAndClosureTemplateStringValidation(ValidatedTemplateString))
                return false;
            if (!ContextDepthTemplateStringValidation(ValidatedTemplateString))
                return false;
            return true;
        }

        private static bool ContextOpeningAndClosureTemplateStringValidation
            (String ValidatedTemplateString)
        {
            int startContextCount = TemplateContextHandlerPackageProvider.countStartContextWordInSubmittedString(ValidatedTemplateString);
            int endContextCount = TemplateContextHandlerPackageProvider.countEndContextWordInSubmittedString(ValidatedTemplateString);
            return startContextCount == endContextCount;
        }

        private static bool ContextDepthTemplateStringValidation
            (String ValidatedTemplateString)
        {
            String currentHandledTemplateString = ValidatedTemplateString;
            Stack<String> StartContextWordStack = new Stack<String>();

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
                        currentHandledTemplateString = StringUtilities.
                                getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                    (currentHandledTemplateString, earliestStartContextWord);
                    }
                    else if (earliestStartContextWordIndex > earliestEndContextWordIndex)
                    {
                        if (!StartContextWordStack.Any()) return false;
                        String lastStartContextWord = StartContextWordStack.Pop();
                        String associatedEndContextWord =
                                TemplateContextHandlerPackageProvider.
                                    getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                        if (!associatedEndContextWord.Equals(earliestEndContextWord))
                        {
                            return false;
                        }
                        currentHandledTemplateString =
                                StringUtilities.
                                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
                                        (currentHandledTemplateString, earliestEndContextWord);
                    }
                }
                else if (earliestEndContextWord != null)
                {
                    if (!StartContextWordStack.Any()) return false;
                    String lastStartContextWord = StartContextWordStack.Pop();
                    String associatedEndContextWord =
                            TemplateContextHandlerPackageProvider.
                                getStartContextCorrespondingEndContextWrapper(lastStartContextWord);
                    if (!associatedEndContextWord.Equals(earliestEndContextWord))
                    {
                        return false;
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
            if (StartContextWordStack.Any()) return false;
            return true;
        }

    }
}
