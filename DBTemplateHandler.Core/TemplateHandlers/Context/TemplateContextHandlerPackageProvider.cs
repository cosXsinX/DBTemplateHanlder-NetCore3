using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context.Columns;
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

        //Column part
        public static IEnumerable<AbstractColumnTemplateContextHandler> GetAllColumnContextHandlers()
        {
            return register.GetColumnHandlers();
        }

        public static IDictionary<string, AbstractColumnTemplateContextHandler> GetColumnContextHandlerByStartContextSignature()
        {
            var handlers = GetAllColumnContextHandlers();
            return handlers.
                ToDictionary(hanlder => hanlder.StartContext,
                handler => handler);
        }

        public static IDictionary<string, AbstractColumnTemplateContextHandler> GetColumnContextHandlerByEndContextSignature()
        {
            var handlers = GetAllColumnContextHandlers();
            return handlers.ToDictionary(handler => handler.EndContext, 
                handler => handler);
        }

        public static string GetAllColumnContextHandlerSignature()
        {
            var handlers = GetAllColumnContextHandlers();
            var splittedResult = 
                Enumerable.Repeat("Column context handler signature", 1)
                .Concat(handlers.Select(m => m.Signature()));
            var result = string.Join(NEW_LINE_CHAR, splittedResult);
            return result;
        }

        public static string GetColumnHandlerStartContextWordAtEarliestPosition(string SubmittedString)
        {
            string result = null;
            if (!IsSubmittedStringContainsAColumnHandlerStartContextWord(SubmittedString)) return result;

            int EarliestPosition = -1;
            if (SubmittedString == null) return null;            
            var templateContextHandlers = GetAllColumnContextHandlers();
            foreach (var currentHandler in templateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.IndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf < EarliestPosition))) result = currentStartWord;
            }
            return result;
        }

        public static string GetColumnHandlerEndContextWordAtLatestPosition(string submittedString)
        {
            String result = null;
            if (submittedString == null) return null;
            if (!IsSubmittedStringContainsAnColumnHandlerEndContextWord(submittedString)) return result;

            var templateContextHandlers = GetAllColumnContextHandlers();
            int EarliestPosition = -1;
            foreach (var currentHandler in templateContextHandlers)
            {
                var currentEndWord = currentHandler.EndContext;
                var currentlastIndexOf = submittedString.LastIndexOf(currentEndWord, StringComparison.Ordinal);
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf > EarliestPosition))) result = currentEndWord;
            }
            return result;
        }

        public static bool IsSubmittedStringContainsAColumnHandlerStartContextWord(String submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetAllColumnContextHandlers();
            return templateContextHandlers.Any(handler => 
                submittedString.Contains(handler.StartContext));
        }

        public static bool IsSubmittedStringContainsAnColumnHandlerEndContextWord(String submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetAllColumnContextHandlers();
            return templateContextHandlers.Any(handler =>
                submittedString.Contains(handler.EndContext));
        }

        public static AbstractColumnTemplateContextHandler GetStartContextCorrespondingColumnContextHandler(String StartContextWrapper)
        {
            if (StartContextWrapper == null) return null;
            if (StartContextWrapper.Equals("")) return null;
            IDictionary<string, AbstractColumnTemplateContextHandler> contextHandlerMap =
                    GetColumnContextHandlerByStartContextSignature();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return null;
            return contextHandlerMap[StartContextWrapper];

        }
        //Column Part End


        //Table Part
        public static IEnumerable<AbstractTableTemplateContextHandler> GetAllTableContextHandlers()
        {
            return register.GetTableHandlers();
        }

        public static IDictionary<string, AbstractTableTemplateContextHandler> GetTableContextHandlerByStartContextWord()
        {
            var handlers = GetAllTableContextHandlers();
            return handlers.ToDictionary(m => m.StartContext, m => m);
        }

        public static IDictionary<string, AbstractTableTemplateContextHandler> GetTableContextHandlerByEndContextWord()
        {
            var handlers = GetAllTableContextHandlers();
            return handlers.ToDictionary(m => m.EndContext, m => m);
        }

        public static string GetAllTableContextHandlerSignature()
        {
            var handlers = GetAllTableContextHandlers();
            var splittedResult =
                Enumerable.Repeat("Table context handler signature", 1)
                .Concat(handlers.Select(m => m.Signature()));
            var result = string.Join(NEW_LINE_CHAR, splittedResult);
            return result;
        }

        public static string GetTableHandlerStartContextWordAtEarliestPosition(string SubmittedString)
        {
            string result = null;
            if (!IsSubmittedStringContainsATableHandlerStartContextWord(SubmittedString)) return result;
            if (SubmittedString == null) return null;
            
            var abatractTemplateContextHandlers = GetAllTableContextHandlers();
            int EarliestPosition = -1;
            foreach (AbstractTableTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.IndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf < EarliestPosition))) result = currentStartWord;
            }
            return result;
        }

        public static string GetTableHandlerEndContextWordAtLatestPosition(string submittedString)
        {
            string result = null;
            if (submittedString == null) return null;
            if (!IsSubmittedStringContainsAnTableHandlerEndContextWord(submittedString)) return result;

            var abatractTemplateContextHandlers = GetAllTableContextHandlers();
            int EarliestPosition = -1;
            foreach (AbstractTableTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                string currentEndWord = currentHandler.EndContext;
                int currentlastIndexOf = submittedString.LastIndexOf(currentEndWord, StringComparison.Ordinal);
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf > EarliestPosition))) result = currentEndWord;
            }
            return result;
        }

        public static bool IsSubmittedStringContainsATableHandlerStartContextWord(string submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers =GetAllTableContextHandlers();
            return templateContextHandlers
                .Any(currentHandler => submittedString.Contains(currentHandler.StartContext));
        }

        public static bool IsSubmittedStringContainsAnTableHandlerEndContextWord(string submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers = GetAllTableContextHandlers();
            return templateContextHandlers
                .Any(currentHandler => submittedString.Contains(currentHandler.EndContext));
        }

        public static AbstractTableTemplateContextHandler GetStartContextCorrespondingTableContextHandler(string StartContextWrapper)
        {
            if (StartContextWrapper == null) return null;
            if (StartContextWrapper.Equals("")) return null;
            IDictionary<string, AbstractTableTemplateContextHandler> contextHandlerMap =
                    GetTableContextHandlerByStartContextWord();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return null;
            return contextHandlerMap[StartContextWrapper];

        }
        //End Table context


        //Database context Handler Part
        public static IEnumerable<AbstractDatabaseTemplateContextHandler> getAllDatabaseContextHandler()
        {
            return register.GetDatabaseHandlers();
        }

        public static IDictionary<string, AbstractDatabaseTemplateContextHandler> getStartContextWrapperStringIndexedDatabaseContextHandlerMap()
        {
            IEnumerable<AbstractDatabaseTemplateContextHandler> handlers = getAllDatabaseContextHandler();
            return handlers.ToDictionary(m => m.StartContext, m => m);
        }

        public static IDictionary<string, AbstractDatabaseTemplateContextHandler> getEndContextWrapperStringIndexedDatabaseContextHandlerMap()
        {
            IEnumerable<AbstractDatabaseTemplateContextHandler> handlers = getAllDatabaseContextHandler();
            return handlers.ToDictionary(m => m.EndContext, m => m);
        }

        public static string GetAllDatabaseContextHandlerSignature()
        {
            var handlers = getAllDatabaseContextHandler();
            var splittedResult =
                Enumerable.Repeat("Database context handler signature", 1)
                .Concat(handlers.Select(m => m.Signature()));
            var result = string.Join(NEW_LINE_CHAR, splittedResult);
            return result;
        }

        public static string GetDatabaseHandlerStartContextWordAtEarliestPosition(string SubmittedString)
        {
            string result = null;
            if (!IsSubmittedStringContainsADatabaseHandlerStartContextWord(SubmittedString)) return result;
            if (SubmittedString == null) return null;
            var abatractTemplateContextHandlers = getAllDatabaseContextHandler();
            int EarliestPosition = -1;
            foreach (AbstractDatabaseTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.IndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf < EarliestPosition))) result = currentStartWord;
            }
            return result;
        }

        public static string GetDatabaseHandlerEndContextWordAtLatestPosition(string submittedString)
        {
            string result = null;
            if (submittedString == null) return null;
            if (!isSubmittedStringContainsAnDatabaseHandlerEndContextWord(submittedString)) return result;
            var abatractTemplateContextHandlers = getAllDatabaseContextHandler();
            int EarliestPosition = -1;
            foreach (AbstractDatabaseTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                var currentEndWord = currentHandler.EndContext;
                var currentlastIndexOf = submittedString.LastIndexOf(currentEndWord, StringComparison.Ordinal);
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf > EarliestPosition))) result = currentEndWord;
            }

            return result;
        }

        public static bool IsSubmittedStringContainsADatabaseHandlerStartContextWord(string submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers = getAllDatabaseContextHandler();
            return templateContextHandlers.Any(currentHandler => 
                submittedString.Contains(currentHandler.StartContext));
        }

        public static bool isSubmittedStringContainsAnDatabaseHandlerEndContextWord(String submittedString)
        {
            if (submittedString == null) return false;
            var abstractTemplateContextHandlers = getAllDatabaseContextHandler();
            return abstractTemplateContextHandlers.Any(current => 
                submittedString.Contains(current.EndContext));
        }

        public static AbstractDatabaseTemplateContextHandler getStartContextCorrespondingDatabaseContextHandler(String StartContextWrapper)
        {
            if (StartContextWrapper == null) return null;
            if (StartContextWrapper.Equals("")) return null;
            var contextHandlerMap = getStartContextWrapperStringIndexedDatabaseContextHandlerMap();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return null;
            return contextHandlerMap[StartContextWrapper];
        }
        //Database end context

        //Function start context
        private static IEnumerable<AbstractFunctionTemplateContextHandler> DefaultLoadFunctionContextHandlerDefault()
        {
            return register.GetFunctionHandlers();
        }

        public static IEnumerable<AbstractFunctionTemplateContextHandler> getAllFunctionContextHandler()
        {
            return DefaultLoadFunctionContextHandlerDefault();
        }

        public static IDictionary<String, AbstractFunctionTemplateContextHandler> getStartContextWrapperStringIndexedFunctionContextHandlerMap()
        {
            IEnumerable<AbstractFunctionTemplateContextHandler> handlers = getAllFunctionContextHandler();
            Dictionary<String, AbstractFunctionTemplateContextHandler> result = new Dictionary<String, AbstractFunctionTemplateContextHandler>();
            foreach (AbstractFunctionTemplateContextHandler currentHandler in handlers)
            {
                result.Add(currentHandler.StartContext, currentHandler);
            }
            return result;
        }

        public static IDictionary<String, AbstractFunctionTemplateContextHandler> getEndContextWrapperStringIndexedFunctionContextHandlerMap()
        {
            IEnumerable<AbstractFunctionTemplateContextHandler> handlers = getAllFunctionContextHandler();
            Dictionary<String, AbstractFunctionTemplateContextHandler> result = new Dictionary<String, AbstractFunctionTemplateContextHandler>();
            foreach (AbstractFunctionTemplateContextHandler currentHandler in handlers)
            {
                result.Add(currentHandler.EndContext, currentHandler);
            }
            return result;
        }

        public static String getAllFunctionContextHandlerSignature()
        {
            IEnumerable<AbstractFunctionTemplateContextHandler> handlers =
                    getAllFunctionContextHandler();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Function context handler signature");
            stringBuilder.Append(NEW_LINE_CHAR);

            foreach (AbstractFunctionTemplateContextHandler currentHandler in handlers)
            {
                stringBuilder.Append(currentHandler.Signature());
                stringBuilder.Append(NEW_LINE_CHAR);
            }
            return stringBuilder.ToString();
        }

        public static string getFunctionHandlerStartContextWordAtEarliestPositionInSubmittedString(string SubmittedString)
        {
            string result = null;
            if (!isSubmittedStringContainsAFunctionHandlerStartContextWord(SubmittedString)) return result;

            int EarliestPosition = -1;
            if (SubmittedString == null) return null;
            var abatractTemplateContextHandlers =getAllFunctionContextHandler();
            foreach (AbstractFunctionTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                var currentStartWord = currentHandler.StartContext;
                var currentIndexOf = SubmittedString.IndexOf(currentStartWord, StringComparison.Ordinal);
                if (currentIndexOf >= 0 && ((EarliestPosition == -1) || (currentIndexOf < EarliestPosition))) 
                    result = currentStartWord;
            }

            return result;
        }

        public static string getFunctionHandlerEndContextWordAtLatestPositionInSubmittedString(string submittedString)
        {
            String result = null;
            int EarliestPosition = -1;
            if (submittedString == null) return null;
            if (!isSubmittedStringContainsAnFunctionHandlerEndContextWord(submittedString)) return result;

            var abatractTemplateContextHandlers = getAllFunctionContextHandler();
            foreach (AbstractFunctionTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                var currentEndWord = currentHandler.EndContext;
                var currentlastIndexOf = submittedString.LastIndexOf(currentEndWord, StringComparison.Ordinal);
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf > EarliestPosition))) result = currentEndWord;
            }

            return result;
        }

        public static bool isSubmittedStringContainsAFunctionHandlerStartContextWord(string submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers =getAllFunctionContextHandler();
            return templateContextHandlers.Any(current => 
                submittedString.Contains(current.StartContext));
        }

        public static bool isSubmittedStringContainsAnFunctionHandlerEndContextWord(string submittedString)
        {
            if (submittedString == null) return false;
            var templateContextHandlers =getAllFunctionContextHandler();
            return templateContextHandlers.Any(current => submittedString.Contains(current.EndContext));
        }

        public static AbstractFunctionTemplateContextHandler getStartContextCorrespondingFunctionContextHandler(String StartContextWrapper)
        {
            if (StartContextWrapper == null) return null;
            if (StartContextWrapper.Equals("")) return null;
            IDictionary<String, AbstractFunctionTemplateContextHandler> contextHandlerMap =
                    getStartContextWrapperStringIndexedFunctionContextHandlerMap();
            if (!contextHandlerMap.ContainsKey(StartContextWrapper)) return null;
            return contextHandlerMap[StartContextWrapper];

        }
        //Function end context

        public static IEnumerable<AbstractTemplateContextHandler> getAllContextHandler()
        {
            List<AbstractTemplateContextHandler> result = new List<AbstractTemplateContextHandler>();
            result.AddRange(GetAllColumnContextHandlers());
            result.AddRange(GetAllTableContextHandlers());
            result.AddRange(getAllDatabaseContextHandler());
            return result;
        }

        public static string getAllContextHandlerSignature()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(GetAllDatabaseContextHandlerSignature());
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetAllTableContextHandlerSignature());
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(GetAllColumnContextHandlerSignature());
            stringBuilder.Append(NEW_LINE_CHAR);
            stringBuilder.Append(getAllFunctionContextHandlerSignature());
            stringBuilder.Append(NEW_LINE_CHAR);
            return stringBuilder.ToString();
        }

        private static IEnumerable<AbstractTemplateContextHandler> _allContextHandlerIterable = null;
        public static IEnumerable<AbstractTemplateContextHandler> getAllContextHandlerIterable()
        {
            if (_allContextHandlerIterable == null)
            {
                List<AbstractTemplateContextHandler> result = new List<AbstractTemplateContextHandler>();
                IEnumerable<AbstractDatabaseTemplateContextHandler> DatabaseHandlers = getAllDatabaseContextHandler();
                foreach (AbstractTemplateContextHandler currentHandler in DatabaseHandlers)
                {
                    result.Add(currentHandler);
                }
                IEnumerable<AbstractTableTemplateContextHandler> TableHandlers = GetAllTableContextHandlers();
                foreach (AbstractTemplateContextHandler currentHandler in TableHandlers)
                {
                    result.Add(currentHandler);
                }
                IEnumerable<AbstractColumnTemplateContextHandler> ColumnHandlers = GetAllColumnContextHandlers();
                foreach (AbstractTemplateContextHandler currentHandler in ColumnHandlers)
                {
                    result.Add(currentHandler);
                }
                IEnumerable<AbstractFunctionTemplateContextHandler> functionHandlers = getAllFunctionContextHandler();
                foreach (AbstractTemplateContextHandler currentHandler in functionHandlers)
                {
                    result.Add(currentHandler);
                }

                _allContextHandlerIterable = result;
            }
            return _allContextHandlerIterable;
        }

        private static IDictionary<string, AbstractTemplateContextHandler> _startContextWrapperStringIndexedAllContextHandlerMap;
        public static IDictionary<string, AbstractTemplateContextHandler> getStartContextWrapperStringIndexedAllContextHandlerMap()
        {
            if (_startContextWrapperStringIndexedAllContextHandlerMap == null)
            {
                Dictionary<string, AbstractTemplateContextHandler> result = new Dictionary<string, AbstractTemplateContextHandler>();
                IEnumerable<AbstractTemplateContextHandler> DatabaseHandlers = getAllContextHandlerIterable();
                foreach (AbstractTemplateContextHandler currentHandler in DatabaseHandlers)
                {
                    result.Add(currentHandler.StartContext, currentHandler);
                }
                _startContextWrapperStringIndexedAllContextHandlerMap = result;
            }
            return _startContextWrapperStringIndexedAllContextHandlerMap;
        }

        private static IDictionary<string, AbstractTemplateContextHandler> _endContextWrapperStringIndexedAllContextHandlerMap;
        public IDictionary<string, AbstractTemplateContextHandler> getEndContextWrapperStringIndexedAllContextHandlerMap()
        {
            if (_endContextWrapperStringIndexedAllContextHandlerMap == null)
            {
                Dictionary<string, AbstractTemplateContextHandler> result = new Dictionary<string, AbstractTemplateContextHandler>();
                IEnumerable<AbstractTemplateContextHandler> DatabaseHandlers = getAllContextHandlerIterable();
                foreach (AbstractTemplateContextHandler currentHandler in DatabaseHandlers)
                {
                    result.Add(currentHandler.EndContext, currentHandler);
                }
                _endContextWrapperStringIndexedAllContextHandlerMap = result;
            }
            return _endContextWrapperStringIndexedAllContextHandlerMap;
        }



        public static string getHandlerStartContextWordAtEarliestPositionInSubmittedString(string SubmittedString)
        {
            String result = null;
            if (!isSubmittedStringContainsAHandlerStartContextWord(SubmittedString)) return result;
            int EarliestPosition = -1;
            if (SubmittedString == null) return null;
            var abatractTemplateContextHandlers =getAllContextHandlerIterable();

            foreach (AbstractTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
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
            string result = null;
            int EarliestPosition = -1;
            if (submittedString == null) return null;
            if (!isSubmittedStringContainsAnHandlerEndContextWord(submittedString)) return result;
            var abatractTemplateContextHandlers =getAllContextHandlerIterable();
            int currentOffset = 0;
            string submittedStringCopy = submittedString;
            foreach (AbstractTemplateContextHandler currentHandler in abatractTemplateContextHandlers)
            {
                string currentEndWord = currentHandler.EndContext;
                int currentSubmittedCopyEarliestIndexOf = submittedStringCopy.IndexOf(currentEndWord, StringComparison.Ordinal);
                var currentlastIndexOf = currentSubmittedCopyEarliestIndexOf + currentOffset;
                if (currentlastIndexOf >= 0 && ((EarliestPosition == -1) || (currentlastIndexOf > EarliestPosition)))
                {
                    EarliestPosition = currentlastIndexOf;
                    result = currentEndWord;
                    submittedStringCopy.Substring(currentSubmittedCopyEarliestIndexOf + currentEndWord.Length);
                }
            }
            return result;
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
