using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using static DBTemplateHandler.Core.TemplateHandlers.Utilities.StringUtilities;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{

    public class TemplateContextProcessor
    {
        private readonly TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler> databaseTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler> tableTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler> columnTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractConstraintTemplateContextHandler> constraintTemplateContextHandlerProvider;
        private readonly TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler> functionTemplateContextHandlerProvider;
        private readonly DatabaseContextCopier databaseContextCopier;


        private readonly List<ITypeMapping> typeMappingsField;
        public IList<ITypeMapping> TypeMappings => typeMappingsField;

        public TemplateContextProcessor(ITemplateHandler templateHandlerNew, IList<ITypeMapping> typeMappings)
        {

            typeMappingsField = typeMappings?.ToList() ?? new List<ITypeMapping>();
            databaseTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>(templateHandlerNew, typeMappings);
            tableTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>(templateHandlerNew, typeMappings);
            columnTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>(templateHandlerNew, typeMappings);
            constraintTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractConstraintTemplateContextHandler>(templateHandlerNew, typeMappings);
            functionTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>(templateHandlerNew, typeMappings);
            databaseContextCopier = new DatabaseContextCopier();
        }

        public string ProcessTemplateContextComposite(TemplateContextComposite processed, IDatabaseContext databaseContext)
        {
            if (processed.current.InnerContent == null)
                throw new ArgumentException($"{nameof(processed)}.{nameof(processed.current)}.{nameof(processed.current.InnerContent)} cannot be null");
            var result = processed.current.Content;
            if (processed.childs?.Any()??false)
            {
                var minContextDepth = processed.childs.Min(m => m.current.ContextDepth);
                var childs = processed.childs.Where(m => m.current.ContextDepth == minContextDepth).OrderBy(m => m.current.StartIndex).ToList();
                var childAndProcessedContexts = childs.Select(m => Tuple.Create(m, ProcessTemplateContextComposite(m, databaseContext), ToStartAndEndIndexPair(m))).ToList();
                var splittedProcessedInnerContent = Split(processed.current.InnerContent, childAndProcessedContexts.Select(m => m.Item3).ToList());
                var processedChildContext = childAndProcessedContexts.Select(m => m.Item2).ToList();
                IEnumerable<string> chainedResults = childs.First().current.StartIndex == 0 ? 
                    Chain(processedChildContext, splittedProcessedInnerContent) :
                    Chain(splittedProcessedInnerContent, processedChildContext);
                var newInnerContent = string.Join(string.Empty, chainedResults);
                result = string.Join(processed.current.StartContextDelimiter, newInnerContent,processed.current.EndContextDelimiter);
            }

            if (processed.current is DatabaseTemplateContext) result = ProcessDatabaseTemplateContext(result,processed.current.StartContextDelimiter, databaseContext);
            if (processed.current is TableTemplateContext) result = ProcessTableTemplateContext(result, processed.current.StartContextDelimiter, databaseContext);
            if (processed.current is ColumnTemplateContext) result = ProcessColumnTemplateContext(result, processed.current.StartContextDelimiter, databaseContext);
            if (processed.current is ConstraintTemplateContext) result = ProcessConstraintTemplateContext(result, processed.current.StartContextDelimiter, databaseContext);
            if (processed.current is FunctionTemplateContext) result = ProcessFunctionTemplateContext(result, processed.current.StartContextDelimiter, databaseContext);
            return result;
        }

        public StartAndEndIndexSplitter ToStartAndEndIndexPair(TemplateContextComposite templateContextComposite)
        {
            return new StartAndEndIndexSplitter()
            {
                StartIndex = templateContextComposite.current.StartIndex,
                EndIndex = templateContextComposite.current.StartIndex + templateContextComposite.current.Content.Length,
            };
        }

        public string ProcessDatabaseTemplateContext(string processed,string startContextDelimiter, IDatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(startContextDelimiter))
                    throw new ArgumentException($"{nameof(startContextDelimiter)} cannot be null or white space"); 
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Database == null) throw new ArgumentException($"{nameof(databaseContext)}.{nameof(databaseContext.Database)} cannot be null");
            
            var databaseTemplateContextHandlerByStartContext = databaseTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!databaseTemplateContextHandlerByStartContext.TryGetValue(startContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(startContextDelimiter)} is not contained in the database template start delimiter context register");
            var processedContext = handler.ProcessContext(processed, databaseContextCopier.CopyWithOverride(databaseContext,databaseContext.Database));
            return processedContext;
        }

        public string ProcessTableTemplateContext(string processed, string startContextDelimiter, IDatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(startContextDelimiter))
                throw new ArgumentException($"{nameof(startContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Table == null) throw new ArgumentException($"{nameof(databaseContext)}.{nameof(databaseContext.Table)} cannot be null");

            var databaseTemplateContextHandlerByStartContext = tableTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!databaseTemplateContextHandlerByStartContext.TryGetValue(startContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(startContextDelimiter)} is not contained in the table template start delimiter context register");
            var processedContext = handler.ProcessContext(processed, databaseContextCopier.CopyWithOverride(databaseContext, databaseContext.Table));
            return processedContext;
        }

        public string ProcessColumnTemplateContext(string processed, string startContextDelimiter, IDatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(startContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(startContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (databaseContext.Column == null) throw new ArgumentException($"{nameof(databaseContext)}.{nameof(databaseContext.Column)} cannot be null");

            var templateContextHandlerByStartContext = columnTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!templateContextHandlerByStartContext.TryGetValue(startContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(startContextDelimiter)} is not contained in the column template start delimiter context register");
            var processedContext = handler.ProcessContext(processed, databaseContextCopier.CopyWithOverride(databaseContext, databaseContext.Column));
            return processedContext;
        }

        public string ProcessConstraintTemplateContext(string processed, string startContextDelimiter, IDatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(startContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(startContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));

            var templateContextHandlerByStartContext = constraintTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!templateContextHandlerByStartContext.TryGetValue(startContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(processed)}.{nameof(startContextDelimiter)} is not contained in the constraint template start delimiter context register");
            var processedContext = handler.ProcessContext(processed, databaseContextCopier.CopyWithOverride(databaseContext, databaseContext.ForeignKeyConstraint));
            return processedContext;
        }

        public string ProcessFunctionTemplateContext(string processed, string startContextDelimiter, IDatabaseContext databaseContext)
        {
            if (processed == null) throw new ArgumentNullException(nameof(processed));
            if (string.IsNullOrWhiteSpace(startContextDelimiter))
                throw new ArgumentException($"{nameof(processed)}.{nameof(startContextDelimiter)} cannot be null or white space");
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));

            var templateContextHandlerByStartContext = functionTemplateContextHandlerProvider.GetContextHandlerByStartContextSignature();
            if (!templateContextHandlerByStartContext.TryGetValue(startContextDelimiter, out var handler))
                throw new ArgumentException($"{nameof(startContextDelimiter)} is not contained in the function template start delimiter context register");
            var processedContext = handler.ProcessContext(processed, databaseContextCopier.Copy(databaseContext));
            return processedContext;
        }

    }
}
