using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Core.TemplateHandlers.Utilities;

namespace DBTemplateHandler.Core.TemplateHandlers.Context
{
    public class TemplateContextHandlerPackageProvider
    {
        private const string NEW_LINE_CHAR = "\t\n";

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

        public static string GetContextHandlerSignatures<T>(string header) where T:ITemplateContextHandler
        {
            var handlers = GetHandlers<T>();
            var splittedResult =
                Enumerable.Repeat(header, 1)
                .Concat(handlers.Select(m => m.Signature()));
            var result = string.Join(NEW_LINE_CHAR, splittedResult);
            return result;
        }

        public static bool ContainsAHandlerStartContextOfType<T>(string submittedString) where T: ITemplateContextHandler
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetHandlers<T>();
            return templateContextHandlers.Any(handler =>
                submittedString.Contains(handler.StartContext));
        }

        public static bool ContainsAHandlerEndContextOfType<T>(string submittedString) where T:ITemplateContextHandler
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

        //Column part
        public static IDictionary<string, AbstractColumnTemplateContextHandler> GetColumnContextHandlerByStartContextSignature()
        {
            return GetContextHandlerByStartContextSignature<AbstractColumnTemplateContextHandler>();
        }
        public static string GetColumnHandlerStartContextWordAtEarliestPosition(string SubmittedString)
        {
            return GetHandlerStartContextWordAtEarliestPosition
                <AbstractColumnTemplateContextHandler>(SubmittedString);
        }

        public static bool ContainsAColumnHandlerStartContext(string submittedString)
        {
            return ContainsAHandlerStartContextOfType
                <AbstractColumnTemplateContextHandler>(submittedString);
        }

        public static AbstractColumnTemplateContextHandler GetStartContextCorrespondingColumnContextHandler(String StartContextWrapper)
        {
            return GetStartContextCorrespondingContextHandler
                <AbstractColumnTemplateContextHandler>(StartContextWrapper);
        }
        //Column Part End


        //Table Part
        public static IDictionary<string, AbstractTableTemplateContextHandler> GetTableContextHandlerByStartContextWord()
        {
            return GetContextHandlerByStartContextSignature<AbstractTableTemplateContextHandler>();
        }

        public static bool IsSubmittedStringContainsATableHandlerStartContextWord(string submittedString)
        {
            return ContainsAHandlerStartContextOfType<AbstractTableTemplateContextHandler>(submittedString);
        }

        public static AbstractTableTemplateContextHandler GetStartContextCorrespondingTableContextHandler(string StartContextWrapper)
        {
            return GetStartContextCorrespondingContextHandler
                <AbstractTableTemplateContextHandler>(StartContextWrapper);

        }
        //End Table context


        //Database context Handler Part

        public static IDictionary<string, AbstractDatabaseTemplateContextHandler> getStartContextWrapperStringIndexedDatabaseContextHandlerMap()
        {
            return GetContextHandlerByStartContextSignature<AbstractDatabaseTemplateContextHandler>();
        }

        public static bool IsSubmittedStringContainsADatabaseHandlerStartContextWord(string submittedString)
        {
            return ContainsAHandlerStartContextOfType<AbstractDatabaseTemplateContextHandler>(submittedString);
        }

        public static AbstractDatabaseTemplateContextHandler getStartContextCorrespondingDatabaseContextHandler(String StartContextWrapper)
        {
            return GetStartContextCorrespondingContextHandler
                <AbstractDatabaseTemplateContextHandler>(StartContextWrapper);
        }
        //Database end context

        //Function start context
        private static IEnumerable<AbstractFunctionTemplateContextHandler> DefaultLoadFunctionContextHandlerDefault()
        {
            return GetHandlers<AbstractFunctionTemplateContextHandler>();
        }

        public static IEnumerable<AbstractFunctionTemplateContextHandler> getAllFunctionContextHandler()
        {
            return DefaultLoadFunctionContextHandlerDefault();
        }

        public static IDictionary<string, AbstractFunctionTemplateContextHandler> getStartContextWrapperStringIndexedFunctionContextHandlerMap()
        {
            return GetContextHandlerByStartContextSignature<AbstractFunctionTemplateContextHandler>();
        }

        public static IDictionary<string, AbstractFunctionTemplateContextHandler> getEndContextWrapperStringIndexedFunctionContextHandlerMap()
        {
            return GetContextHandlerByEndContextSignature<AbstractFunctionTemplateContextHandler>();
        }

        public static string getFunctionHandlerStartContextWordAtEarliestPositionInSubmittedString(string SubmittedString)
        {
            return GetHandlerStartContextWordAtEarliestPosition<AbstractFunctionTemplateContextHandler>(SubmittedString);
        }

        public static string getFunctionHandlerEndContextWordAtLatestPositionInSubmittedString(string submittedString)
        {
            return GetHandlerEndContextWordAtLatestPosition<AbstractFunctionTemplateContextHandler>(submittedString);
        }

        public static bool isSubmittedStringContainsAFunctionHandlerStartContextWord(string submittedString)
        {
            return ContainsAHandlerStartContextOfType<AbstractFunctionTemplateContextHandler>(submittedString);
        }

        public static bool isSubmittedStringContainsAnFunctionHandlerEndContextWord(string submittedString)
        {
            return ContainsAHandlerEndContextOfType<AbstractFunctionTemplateContextHandler>(submittedString);
        }

        public static AbstractFunctionTemplateContextHandler getStartContextCorrespondingFunctionContextHandler(string StartContextWrapper)
        {
            return GetStartContextCorrespondingContextHandler<AbstractFunctionTemplateContextHandler>(StartContextWrapper);
        }
        //Function end context

        public static IEnumerable<AbstractTemplateContextHandler> getAllContextHandler()
        {
            return GetHandlers< AbstractTemplateContextHandler>();
        }

        public static string getAllContextHandlerSignature()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(GetContextHandlerSignatures<AbstractDatabaseTemplateContextHandler>("Database context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetContextHandlerSignatures<AbstractTableTemplateContextHandler>("Table context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetContextHandlerSignatures<AbstractColumnTemplateContextHandler>("Column context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetContextHandlerSignatures<AbstractFunctionTemplateContextHandler>("Function context handler signature"));
            stringBuilder.Append(NEW_LINE_CHAR);
            return stringBuilder.ToString();
        }

        private static IEnumerable<AbstractTemplateContextHandler> _allContextHandlerIterable = null;
        public static IEnumerable<AbstractTemplateContextHandler> getAllContextHandlerIterable()
        {
            if (_allContextHandlerIterable == null)
                _allContextHandlerIterable = GetHandlers<AbstractTemplateContextHandler>();
            return _allContextHandlerIterable;
        }

        private static IDictionary<string, AbstractTemplateContextHandler> _startContextWrapperStringIndexedAllContextHandlerMap;
        public static IDictionary<string, AbstractTemplateContextHandler> getStartContextWrapperStringIndexedAllContextHandlerMap()
        {
            if (_startContextWrapperStringIndexedAllContextHandlerMap == null)
                _startContextWrapperStringIndexedAllContextHandlerMap = 
                    GetContextHandlerByStartContextSignature<AbstractTemplateContextHandler>();
            return _startContextWrapperStringIndexedAllContextHandlerMap;
        }

        private static IDictionary<string, AbstractTemplateContextHandler> _endContextWrapperStringIndexedAllContextHandlerMap;
        public IDictionary<string, AbstractTemplateContextHandler> getEndContextWrapperStringIndexedAllContextHandlerMap()
        {
            if (_endContextWrapperStringIndexedAllContextHandlerMap == null)
                _endContextWrapperStringIndexedAllContextHandlerMap = 
                    GetContextHandlerByEndContextSignature<AbstractTemplateContextHandler>();
            return _endContextWrapperStringIndexedAllContextHandlerMap;
        }



        public static string getHandlerStartContextWordAtEarliestPositionInSubmittedString(string SubmittedString)
        {
            return GetHandlerStartContextWordAtEarliestPosition<AbstractTemplateContextHandler>(SubmittedString);
        }

        public static string getHandlerStartContextWordAtLattestPositionInSubmittedString(string SubmittedString)
        {
            string result = null;
            if (!isSubmittedStringContainsAHandlerStartContextWord(SubmittedString)) return result;
            int EarliestPosition = -1;
            if (SubmittedString == null) return null;
            var abatractTemplateContextHandlers = getAllContextHandlerIterable();
            foreach (AbstractTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
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

        public static string getHandlerEndContextWordAtEarliestPositionInSubmittedString(string submittedString)
        {
            String result = null;
            int EarliestPosition = -1;
            if (submittedString == null) return null;
            if (!isSubmittedStringContainsAnHandlerEndContextWord(submittedString)) return result;
            var abatractTemplateContextHandlers = getAllContextHandlerIterable();
            foreach (AbstractTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
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

        public static string getHandlerEndContextWordAtLatestPositionInSubmittedString(string submittedString)
        {
            return GetHandlerEndContextWordAtLatestPosition<AbstractTemplateContextHandler>(submittedString);
        }

        public static bool isSubmittedStringContainsAHandlerStartContextWord(string submittedString)
        {
            if (submittedString == null) return false;
            IEnumerable<AbstractTemplateContextHandler> abatractTemplateContextHandlers = getAllContextHandlerIterable();
            foreach (AbstractTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                if (submittedString.Contains(currentHandler.StartContext)) return true;
            }
            return false;
        }

        public static bool isSubmittedStringContainsAnHandlerEndContextWord(String submittedString)
        {
            if (submittedString == null) return false;

            IEnumerable<AbstractTemplateContextHandler> abatractTemplateContextHandlers =
                    TemplateContextHandlerPackageProvider.getAllContextHandlerIterable();

            foreach (AbstractTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                if (submittedString.Contains(currentHandler.EndContext)) return true;
            }
            return false;
        }

        public static int countStartContextWordInSubmittedString(String submittedString)
        {
            int result = 0;
            if (submittedString == null) return result;
            if (!submittedString.Any()) return result;
            String FirstStartContextWordTrimmedString = submittedString;
            String startContextWord = getHandlerStartContextWordAtEarliestPositionInSubmittedString(FirstStartContextWordTrimmedString);
            while (startContextWord != null)
            {
                FirstStartContextWordTrimmedString = StringUtilities.
                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(
                            FirstStartContextWordTrimmedString, startContextWord);
                startContextWord =
                        getHandlerStartContextWordAtEarliestPositionInSubmittedString(
                                FirstStartContextWordTrimmedString);
                result++;
            }
            return result;
        }

        public static int countEndContextWordInSubmittedString(String submittedString)
        {
            int result = 0;
            if (submittedString == null) return result;
            if (!submittedString.Any()) return result;
            String FirstEndContextWordTrimmedString = submittedString;
            String endContextWord = getHandlerEndContextWordAtEarliestPositionInSubmittedString(submittedString);
            while (endContextWord != null)
            {
                FirstEndContextWordTrimmedString = StringUtilities.
                    getRightPartOfSubmittedStringAfterFirstSearchedWordOccurence(
                            FirstEndContextWordTrimmedString, endContextWord);
                endContextWord =
                        getHandlerEndContextWordAtEarliestPositionInSubmittedString(
                                FirstEndContextWordTrimmedString);
                result++;
            }
            return result;
        }

        public static string getStartContextCorrespondingEndContextWrapper(String StartContextWrapper)
        {
            if (StartContextWrapper == null) return null;
            if (StartContextWrapper.Equals("")) return null;
            IDictionary<String, AbstractTemplateContextHandler> contextHandlerMap =
                    getStartContextWrapperStringIndexedAllContextHandlerMap();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return null;
            return contextHandlerMap[StartContextWrapper].EndContext;
        }

        public static AbstractTemplateContextHandler getStartContextCorrespondingContextHandler(string StartContextWrapper)
        {
            if (StartContextWrapper == null) return null;
            if (StartContextWrapper.Equals("")) return null;
            IDictionary<string, AbstractTemplateContextHandler> contextHandlerMap =
                    getStartContextWrapperStringIndexedAllContextHandlerMap();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return null;
            return contextHandlerMap[StartContextWrapper];
        }
    }
}
