using System;
using System.Collections.Generic;
using System.Linq;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public class TemplateContextHandlerPackageProvider<U> where U: ITemplateContextHandler
    {
        private readonly TemplateContextHandlerRegister _register = new TemplateContextHandlerRegister();

        public TemplateContextHandlerPackageProvider()
        {
        }

        public IList<U> GetHandlers()
        {
            return register.GetHanlders<U>();
        }

        public IDictionary<string, U> GetContextHandlerByStartContextSignature()
        {
            var handlers = GetHandlers();
            return handlers.
                ToDictionary(hanlder => hanlder.StartContext,
                handler => handler);
        }

        public IDictionary<string, U> GetContextHandlerByEndContextSignature()
        {
            var handlers = GetHandlers();
            return handlers.
                ToDictionary(hanlder => hanlder.EndContext,
                handler => handler);
        }

        public bool ContainsAHandlerStartContextOfType(string submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetHandlers();
            return templateContextHandlers.Any(handler =>
                submittedString.Contains(handler.StartContext));
        }

        public bool ContainsAHandlerEndContextOfType(string submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetHandlers();
            return templateContextHandlers.Any(handler =>
                submittedString.Contains(handler.EndContext));
        }

        public string GetHandlerStartContextWordAtEarliestPosition(string SubmittedString)
        {
            string result = null;
            if (!ContainsAHandlerStartContextOfType(SubmittedString)) return result;

            int EarliestPosition = -1;
            if (SubmittedString == null) return null;
            var templateContextHandlers = GetHandlers();
            foreach (var currentHandler in templateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.IndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf < EarliestPosition)))
                {
                    EarliestPosition = currentIndexOf;
                    result = currentStartWord;
                }
            }
            return result;
        }

        public string GetHandlerStartContextWordAtLattestPosition(string SubmittedString)
        {
            string result = null;
            if (!ContainsAHandlerStartContextOfType(SubmittedString)) return result;
            int EarliestPosition = -1;
            if (SubmittedString == null) return null;
            var abatractTemplateContextHandlers = GetHandlers();
            foreach (var currentHandler in abatractTemplateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.LastIndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf > EarliestPosition)))
                {
                    EarliestPosition = currentIndexOf;
                    result = currentStartWord;
                }
            }
            return result;
        }

        public string GetHandlerEndContextWordAtEarliestPosition(string submittedString)
        {
            string result = null;
            int EarliestPosition = -1;
            if (submittedString == null) return null;
            if (!this.ContainsAHandlerEndContextOfType(submittedString)) return result;
            var abatractTemplateContextHandlers = this.GetHandlers();
            foreach (var currentHandler in abatractTemplateContextHandlers)
            {
                var currentEndWord = currentHandler.EndContext;
                var currentlastIndexOf = submittedString.IndexOf(currentEndWord, StringComparison.Ordinal);
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf < EarliestPosition)))
                {
                    EarliestPosition = currentlastIndexOf;
                    result = currentEndWord;
                }
            }
            return result;
        }

        public string GetHandlerEndContextWordAtLatestPosition(string submittedString)
        {
            String result = null;
            if (submittedString == null) return null;
            if (!ContainsAHandlerEndContextOfType(submittedString)) return result;

            var templateContextHandlers = this.GetHandlers().OrderBy(m => m.EndContext.Length);
            int EarliestPosition = -1;
            foreach (var currentHandler in templateContextHandlers)
            {
                var currentEndWord = currentHandler.EndContext;
                var currentlastIndexOf = submittedString.LastIndexOf(currentEndWord, StringComparison.Ordinal);
                var currentEndWordEndsWithResult = result == null? false :currentEndWord.EndsWith(result);
                var ResultEndsWithcurrentEndWord = result == null ? false : result.EndsWith(currentEndWord);
                string currentWordLengthWithoutResult = null;
                if(currentEndWordEndsWithResult && (currentEndWord.Length - result.Length <= currentEndWord.Length))
                    currentWordLengthWithoutResult = currentEndWord.Substring(0, currentEndWord.Length - result.Length);
                var currentWordLengthWithoutResultLength = currentWordLengthWithoutResult == null ? 0: currentWordLengthWithoutResult.Length;
                if (currentlastIndexOf >= 0 && 
                    (
                        (EarliestPosition == -1) 
                        || (currentlastIndexOf > EarliestPosition)
                        || (currentlastIndexOf < EarliestPosition && currentWordLengthWithoutResultLength == (EarliestPosition - currentlastIndexOf) && currentEndWordEndsWithResult && !ResultEndsWithcurrentEndWord)))
                {
                    EarliestPosition = currentlastIndexOf;
                    result = currentEndWord;
                }
            }
            return result;
        }

        public U GetStartContextCorrespondingContextHandler(string StartContextWrapper)
        {
            if (StartContextWrapper == null) return default(U);
            if (StartContextWrapper.Equals("")) return default(U);
            IDictionary<string, U> contextHandlerMap =
                    this.GetContextHandlerByStartContextSignature();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return default(U);
            return contextHandlerMap[StartContextWrapper];
        }

        public int CountStartContextWordIn(string submittedString)
        {
            int result = 0;
            if (submittedString == null) return result;
            if (!submittedString.Any()) return result;
            string FirstStartContextWordTrimmedString = submittedString;
            string startContextWord = GetHandlerStartContextWordAtEarliestPosition(FirstStartContextWordTrimmedString);
            while (startContextWord != null)
            {
                FirstStartContextWordTrimmedString = StringUtilities.
                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(
                            FirstStartContextWordTrimmedString, startContextWord);
                startContextWord =
                        GetHandlerStartContextWordAtEarliestPosition(FirstStartContextWordTrimmedString);
                result++;
            }
            return result;
        }

        public int CountEndContextWordIn(string submittedString)
        {
            int result = 0;
            if (submittedString == null) return result;
            if (!submittedString.Any()) return result;
            string FirstEndContextWordTrimmedString = submittedString;
            string endContextWord = GetHandlerEndContextWordAtEarliestPosition(submittedString);
            while (endContextWord != null)
            {
                FirstEndContextWordTrimmedString = StringUtilities.
                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(
                            FirstEndContextWordTrimmedString, endContextWord);
                endContextWord =
                        this.GetHandlerEndContextWordAtEarliestPosition(FirstEndContextWordTrimmedString);
                result++;
            }
            return result;
        }

        public string GetStartContextCorrespondingEndContext(string StartContextWrapper)
        {
            var handler = this.GetStartContextCorrespondingContextHandler(StartContextWrapper);
            return handler?.EndContext;
        }



        private static readonly TemplateContextHandlerRegister register =
            new TemplateContextHandlerRegister();

        public static IList<T> GetHandlers<T>() where T : ITemplateContextHandler
        {
            return register.GetHanlders<T>();
        }

        public static IDictionary<string, T> GetContextHandlerByStartContextSignature<T>() where T : ITemplateContextHandler
        {
            var handlers = GetHandlers<T>();
            return handlers.
                ToDictionary(hanlder => hanlder.StartContext,
                handler => handler);
        }

        public static IDictionary<string, T> GetContextHandlerByEndContextSignature<T>() where T : ITemplateContextHandler
        {
            var handlers = GetHandlers<T>();
            return handlers.
                ToDictionary(hanlder => hanlder.EndContext,
                handler => handler);
        }

        public static bool ContainsAHandlerStartContextOfType<T>(string submittedString) where T : ITemplateContextHandler
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetHandlers<T>();
            return templateContextHandlers.Any(handler =>
                submittedString.Contains(handler.StartContext));
        }

        public static bool ContainsAHandlerEndContextOfType<T>(string submittedString) where T : ITemplateContextHandler
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetHandlers<T>();
            return templateContextHandlers.Any(handler =>
                submittedString.Contains(handler.EndContext));
        }

        public static string GetHandlerStartContextWordAtEarliestPosition<T>(string SubmittedString) where T : ITemplateContextHandler
        {
            string result = null;
            if (!ContainsAHandlerStartContextOfType<T>(SubmittedString)) return result;

            int EarliestPosition = -1;
            if (SubmittedString == null) return null;
            var templateContextHandlers = GetHandlers<T>();
            foreach (var currentHandler in templateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.IndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf < EarliestPosition))) result = currentStartWord;
            }
            return result;
        }

        public static string GetHandlerStartContextWordAtLattestPosition<T>(string SubmittedString) where T : ITemplateContextHandler
        {
            string result = null;
            if (!ContainsAHandlerStartContextOfType<T>(SubmittedString)) return result;
            int EarliestPosition = -1;
            if (SubmittedString == null) return null;
            var abatractTemplateContextHandlers = GetHandlers<T>();
            foreach (T currentHandler in abatractTemplateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.LastIndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf > EarliestPosition)))
                {
                    EarliestPosition = currentIndexOf;
                    result = currentStartWord;
                }
            }
            return result;
        }

        public static string GetHandlerEndContextWordAtEarliestPosition<T>(string submittedString) where T : ITemplateContextHandler
        {
            string result = null;
            int EarliestPosition = -1;
            if (submittedString == null) return null;
            if (!ContainsAHandlerEndContextOfType<T>(submittedString)) return result;
            var abatractTemplateContextHandlers = GetHandlers<T>();
            foreach (T currentHandler in abatractTemplateContextHandlers)
            {
                var currentEndWord = currentHandler.EndContext;
                var currentlastIndexOf = submittedString.IndexOf(currentEndWord, StringComparison.Ordinal);
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf < EarliestPosition)))
                {
                    EarliestPosition = currentlastIndexOf;
                    result = currentEndWord;
                }
            }
            return result;
        }

        public static string GetHandlerEndContextWordAtLatestPosition<T>(string submittedString) where T : ITemplateContextHandler
        {
            String result = null;
            if (submittedString == null) return null;
            if (!ContainsAHandlerEndContextOfType<T>(submittedString)) return result;

            var templateContextHandlers = GetHandlers<T>();
            int EarliestPosition = -1;
            foreach (var currentHandler in templateContextHandlers)
            {
                var currentEndWord = currentHandler.EndContext;
                var currentlastIndexOf = submittedString.LastIndexOf(currentEndWord, StringComparison.Ordinal);
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf > EarliestPosition))) result = currentEndWord;
            }
            return result;
        }

        public static T GetStartContextCorrespondingContextHandler<T>(string StartContextWrapper) where T : ITemplateContextHandler
        {
            if (StartContextWrapper == null) return default(T);
            if (StartContextWrapper.Equals("")) return default(T);
            IDictionary<string, T> contextHandlerMap =
                    GetContextHandlerByStartContextSignature<T>();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return default(T);
            return contextHandlerMap[StartContextWrapper];
        }

        public static int CountStartContextWordIn<T>(string submittedString) where T : ITemplateContextHandler
        {
            int result = 0;
            if (submittedString == null) return result;
            if (!submittedString.Any()) return result;
            string FirstStartContextWordTrimmedString = submittedString;
            string startContextWord = GetHandlerStartContextWordAtEarliestPosition<T>(FirstStartContextWordTrimmedString);
            while (startContextWord != null)
            {
                FirstStartContextWordTrimmedString = StringUtilities.
                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(
                            FirstStartContextWordTrimmedString, startContextWord);
                startContextWord =
                        GetHandlerStartContextWordAtEarliestPosition<T>(FirstStartContextWordTrimmedString);
                result++;
            }
            return result;
        }

        public static int CountEndContextWordIn<T>(string submittedString) where T : ITemplateContextHandler
        {
            int result = 0;
            if (submittedString == null) return result;
            if (!submittedString.Any()) return result;
            string FirstEndContextWordTrimmedString = submittedString;
            string endContextWord = GetHandlerEndContextWordAtEarliestPosition<T>(submittedString);
            while (endContextWord != null)
            {
                FirstEndContextWordTrimmedString = StringUtilities.
                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(
                            FirstEndContextWordTrimmedString, endContextWord);
                endContextWord =
                        GetHandlerEndContextWordAtEarliestPosition<T>(FirstEndContextWordTrimmedString);
                result++;
            }
            return result;
        }

        public static string GetStartContextCorrespondingEndContext<T>(string StartContextWrapper) where T : ITemplateContextHandler
        {
            var handler = GetStartContextCorrespondingContextHandler<T>(StartContextWrapper);
            return handler?.EndContext;
        }

    }
}
