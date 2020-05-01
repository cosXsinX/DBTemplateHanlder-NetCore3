using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Utilities
{
    public static class StringUtilities
    {

        public static string getLeftPartOfSubmittedStringBeforeFirstSearchedWordOccurence
        (string submittedString, string SearchedWord)
        {
            if (submittedString == null) return null;
            if (SearchedWord == null) return null;
            if (SearchedWord.Equals("")) return submittedString;
            int FirstOccurenceIndex = submittedString.IndexOf(SearchedWord, StringComparison.Ordinal);
            if (FirstOccurenceIndex == -1) return submittedString;
            return submittedString.Substring(0, FirstOccurenceIndex);
        }

        public static string getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence
        (string submittedString, string SearchedWord)
        {
            if (submittedString == null) return null;
            if (SearchedWord == null) return null;
            if (SearchedWord.Equals("")) return submittedString;
            int FirstOccurenceIndex = submittedString.IndexOf(SearchedWord, StringComparison.Ordinal);
            if (FirstOccurenceIndex == -1) return submittedString;
            return submittedString.Substring(FirstOccurenceIndex + SearchedWord.Length, submittedString.Length - (FirstOccurenceIndex + SearchedWord.Length));
        }

        public static IEnumerable<string> Chain(IEnumerable<string> ChainBefore, IEnumerable<string> ChainAfter)
        {
            var chainBeforeEnumerator = ChainBefore.GetEnumerator();
            var chainAfterEnumerator = ChainAfter.GetEnumerator();
            while (chainBeforeEnumerator.MoveNext() | chainAfterEnumerator.MoveNext())
            {
                if (chainBeforeEnumerator.Current != null)
                {
                    yield return chainBeforeEnumerator.Current;
                }

                if (chainAfterEnumerator.Current != null)
                {
                    yield return chainAfterEnumerator.Current;
                }
            }
        }

        public struct StartAndEndIndexSplitter
        {
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
        }

        public static IEnumerable<string> Split(string splitted, IEnumerable<StartAndEndIndexSplitter> splitters)
        {
            var excludedIndexes = new HashSet<int>(splitters.SelectMany(flattened => {return Flatten(flattened); }));
            var resultBuilder = new StringBuilder();
            foreach (var current in splitted.Select((value, index) => new { index, value }))
            {
                if (excludedIndexes.Contains(current.index) )
                {
                    if (resultBuilder.Length > 0)
                    {
                        yield return resultBuilder.ToString();
                        resultBuilder.Clear();
                    }
                    continue;
                }
                resultBuilder.Append(current.value);
            }
            if (resultBuilder.Length > 0)
            {
                yield return resultBuilder.ToString();
                resultBuilder.Clear();
            }
        }

        private static IEnumerable<int> Flatten(StartAndEndIndexSplitter flattened)
        {
            for (int currentIndex = flattened.StartIndex; currentIndex < flattened.EndIndex; currentIndex++) yield return currentIndex;
        }

    }
}
