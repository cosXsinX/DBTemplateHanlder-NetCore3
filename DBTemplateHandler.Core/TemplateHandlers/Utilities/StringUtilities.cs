using System;
namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public static class StringUtilities
    {

        public static String getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence
        (String submittedString, String SearchedWord)
        {
            if (submittedString == null) return null;
            if (SearchedWord == null) return null;
            if (SearchedWord.Equals("")) return submittedString;
            int FirstOccurenceIndex = submittedString.IndexOf(SearchedWord, StringComparison.Ordinal);
            if (FirstOccurenceIndex == -1) return submittedString;
            return submittedString.Substring(0, FirstOccurenceIndex);
        }

        public static String getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
        (String submittedString, String SearchedWord)
        {
            if (submittedString == null) return null;
            if (SearchedWord == null) return null;
            if (SearchedWord.Equals("")) return submittedString;
            int FirstOccurenceIndex = submittedString.IndexOf(SearchedWord, StringComparison.Ordinal);
            if (FirstOccurenceIndex == -1) return submittedString;
            return submittedString.Substring(FirstOccurenceIndex + SearchedWord.Length, submittedString.Length - (FirstOccurenceIndex + SearchedWord.Length));
        }
    }
}
