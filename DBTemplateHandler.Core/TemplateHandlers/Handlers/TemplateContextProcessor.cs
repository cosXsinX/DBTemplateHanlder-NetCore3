using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class DatabaseContext
    {
        public IDatabaseModel Database { get; set; }
        public ITableModel Table { get; set; }
        public IColumnModel Column { get; set; }
        public IForeignKeyConstraintModel ForeignKeyConstraint { get; set; }
    }

    public class TemplateContextProcessor
    {
        private readonly TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler> templateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler> databaseTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler> tableTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler> columnTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractConstraintTemplateContextHandler> constraintTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler> functionTemplateContextHandlerProvider;


        private readonly List<ITypeMapping> typeMappingsField;
        public IList<ITypeMapping> TypeMappings => typeMappingsField;

        public TemplateContextProcessor(TemplateHandlerNew templateHandlerNew, IList<ITypeMapping> typeMappings)
        {

            typeMappingsField = typeMappings?.ToList() ?? new List<ITypeMapping>();
            templateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>(templateHandlerNew, typeMappings);
            databaseTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>(templateHandlerNew, typeMappings);
            tableTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>(templateHandlerNew, typeMappings);
            columnTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>(templateHandlerNew, typeMappings);
            functionTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>(templateHandlerNew, typeMappings);
        }

        public string ProcessTemplateContextComposite(TemplateContextComposite processed, DatabaseContext databaseContext)
        {
            if (processed.current.InnerContent == null) throw new ArgumentException($"{processed}.{processed.current}.{processed.current.InnerContent} cannot be null");
            var result = processed.current.InnerContent;
            if (processed.childs.Any())
            {
                var minContextDepth = processed.childs.Min(m => m.current.ContextDepth);
                var childs = processed.childs.Where(m => m.current.ContextDepth == minContextDepth).OrderBy(m => m.current.StartIndex).ToList();
                var childAndProcessedContexts = childs.Select(m => Tuple.Create(m, ProcessTemplateContextComposite(m, databaseContext), ToStartAndEndIndexPair(m))).ToList();
                var splittedProcessedInnerContent = Split(processed.current.InnerContent, childAndProcessedContexts.Select(m => m.Item3).ToList());
                var processedChildContext = childAndProcessedContexts.Select(m => m.Item2).ToList();
                IEnumerable<string> chainedResults = childs.First().current.StartIndex == 0 ? 
                    Chain(processedChildContext, splittedProcessedInnerContent) : 
                    Chain(splittedProcessedInnerContent, processedChildContext);
                result = String.Join(string.Empty, chainedResults);
            }

            if (processed.current is DatabaseTemplateContext) return ProcessDatabaseTemplateContext((DatabaseTemplateContext)processed.current, databaseContext);
            if (processed.current is TableTemplateContext) return ProcessTableTemplateContext((TableTemplateContext)processed.current, databaseContext);
            if (processed.current is ColumnTemplateContext) return ProcessColumnTemplateContext((ColumnTemplateContext)processed.current, databaseContext);
            if (processed.current is ConstraintTemplateContext) return ProcessConstraintTemplateContext((ConstraintTemplateContext)processed.current, databaseContext);
            if (processed.current is FunctionTemplateContext) return ProcessFunctionTemplateContext((FunctionTemplateContext)processed.current, databaseContext);
            return processed.current.InnerContent;
        }

        public StartAndEndIndexPair ToStartAndEndIndexPair(TemplateContextComposite templateContextComposite)
        {
            return new StartAndEndIndexPair()
            {
                StartIndex = templateContextComposite.current.StartIndex,
                EndIndex = templateContextComposite.current.StartIndex + templateContextComposite.current.Content.Length,
            };
        }

        public struct StartAndEndIndexPair
        {
            public int StartIndex { get; set; }
            public int EndIndex { get; set; }
        }

        public IEnumerable<string> Chain(IEnumerable<string> ChainBefore, IEnumerable<string> ChainAfter)
        {
            var chainBeforeEnumerator = ChainBefore.GetEnumerator();

            var chainAfterEnumerator = ChainAfter.GetEnumerator();

            if(chainBeforeEnumerator.Current != null)
            {
                yield return chainBeforeEnumerator.Current;
            }

            if(chainAfterEnumerator.Current != null)
            {
                yield return chainAfterEnumerator.Current;
            }

            while(chainBeforeEnumerator.MoveNext() || chainAfterEnumerator.MoveNext())
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

        public IEnumerable<string> Split(string splitted, IList<StartAndEndIndexPair> splitters)
        {
            var excludedIndexes = new HashSet<int>(Flatten(splitters));
            var resultBuilder = new StringBuilder();
            foreach(var current in splitted.Select((value,index)=> new { index,value}))
            {
                if(excludedIndexes.Contains(current.index) && resultBuilder.Length>0)
                {
                    yield return resultBuilder.ToString();
                    resultBuilder.Clear();
                }
                resultBuilder.Append(current.value);
            }
        }


        private IEnumerable<int> Flatten(IList<StartAndEndIndexPair> flatteneds)
        {
            return flatteneds.SelectMany(m => Flatten(m)).Distinct().OrderBy(m => m);
        }

        private IEnumerable<int> Flatten(StartAndEndIndexPair flattened)
        {
            for (int currentIndex = flattened.StartIndex; currentIndex < flattened.EndIndex; currentIndex++) yield return currentIndex;
        }

        public string ProcessDatabaseTemplateContext(DatabaseTemplateContext processed, DatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(processed.StartContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} cannot be null or white space"); 
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Database == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Database} cannot be null");
            
            var databaseTemplateContextHandlerByStartContext = databaseTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!databaseTemplateContextHandlerByStartContext.TryGetValue(processed.StartContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} is not contained in the database template start delimiter context register");
            handler.DatabaseModel = databaseContext.Database;
            var processedContext = handler.processContext(processed.InnerContent);
            return processedContext;
        }

        public string ProcessTableTemplateContext(TableTemplateContext processed, DatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(processed.StartContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Database == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Database} cannot be null");
            if (databaseContext.Table == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Table} cannot be null");

            var databaseTemplateContextHandlerByStartContext = tableTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!databaseTemplateContextHandlerByStartContext.TryGetValue(processed.StartContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} is not contained in the table template start delimiter context register");
            handler.TableModel = databaseContext.Table;
            var processedContext = handler.processContext(processed.InnerContent);
            return processedContext;
        }

        public string ProcessColumnTemplateContext(ColumnTemplateContext processed, DatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(processed.StartContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Database == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Database} cannot be null");
            if (databaseContext.Table == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Table} cannot be null");
            if (databaseContext.Column == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Column} cannot be null");

            var templateContextHandlerByStartContext = columnTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!templateContextHandlerByStartContext.TryGetValue(processed.StartContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} is not contained in the column template start delimiter context register");
            handler.ColumnModel = databaseContext.Column;
            var processedContext = handler.processContext(processed.InnerContent);
            return processedContext;
        }

        public string ProcessConstraintTemplateContext(ConstraintTemplateContext processed, DatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(processed.StartContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Database == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Database} cannot be null");
            if (databaseContext.Table == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Table} cannot be null");
            if (databaseContext.Column == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Column} cannot be null");

            var templateContextHandlerByStartContext = constraintTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!templateContextHandlerByStartContext.TryGetValue(processed.StartContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} is not contained in the constraint template start delimiter context register");
            handler.ForeignKeyConstraintModel = databaseContext.ForeignKeyConstraint;
            var processedContext = handler.processContext(processed.InnerContent);
            return processedContext;
        }

        public string ProcessFunctionTemplateContext(FunctionTemplateContext processed, DatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(processed.StartContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Database == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Database} cannot be null");
            if (databaseContext.Table == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Table} cannot be null");
            if (databaseContext.Column == null) throw new ArgumentException($"{nameof(databaseContext)}.{databaseContext.Column} cannot be null");

            var templateContextHandlerByStartContext = functionTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!templateContextHandlerByStartContext.TryGetValue(processed.StartContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.StartContextDelimiter)} is not contained in the function template start delimiter context register");
            handler.DatabaseModel = databaseContext.Database;
            handler.TableModel = databaseContext.Table;
            handler.ColumnModel = databaseContext.Column;
            handler.ConstraintModel = databaseContext.ForeignKeyConstraint;
            var processedContext = handler.processContext(processed.InnerContent);
            return processedContext;
        }

    }
}
