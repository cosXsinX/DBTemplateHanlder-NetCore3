using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Service.Contracts.TypeMapping;
using System.Collections.Generic;
using System.Linq;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class ToQualifiedTemplateContextConverter
    {
        private readonly TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>
            databaseTemplateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>
            tableTemplateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>
            columnTemplateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractConstraintTemplateContextHandler>
            constraintTemplateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>
            functionTemplateContextHandlerProvider;

        public ToQualifiedTemplateContextConverter(ITemplateHandler templateHandler, IList<ITypeMapping> typeMappings)
        {
            databaseTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>(templateHandler, typeMappings);
            tableTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>(templateHandler, typeMappings);
            columnTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>(templateHandler, typeMappings);
            constraintTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractConstraintTemplateContextHandler>(templateHandler, typeMappings);
            functionTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>(templateHandler, typeMappings);
        }

        public IList<TemplateContextComposite> ToQualifiedTemplateContextes(IEnumerable<TemplateContextComposite> converteds)
        {
            return converteds.Select(ToQualifiedTemplateContext).ToList();
        }

        public TemplateContextComposite ToQualifiedTemplateContext(TemplateContextComposite converted)
        {
            var result = new TemplateContextComposite();
            result.current = ToQualifiedTemplateContext(converted.current);
            result.childs = converted?.childs?.Select(ToQualifiedTemplateContext)?.ToList()??new List<TemplateContextComposite>();
            return result;
        }

        public TemplateContext ToQualifiedTemplateContext(TemplateContext templateContext)
        {
            TemplateContext result = null;
            if (databaseTemplateContextHandlerProvider.ContainsAHandlerStartContextOfType(templateContext.StartContextDelimiter)) result = new DatabaseTemplateContext();
            if (tableTemplateContextHandlerProvider.ContainsAHandlerStartContextOfType(templateContext.StartContextDelimiter)) result = new TableTemplateContext();
            if (columnTemplateContextHandlerProvider.ContainsAHandlerStartContextOfType(templateContext.StartContextDelimiter)) result = new ColumnTemplateContext();
            if (constraintTemplateContextHandlerProvider.ContainsAHandlerStartContextOfType(templateContext.StartContextDelimiter)) result = new ConstraintTemplateContext();
            if (functionTemplateContextHandlerProvider.ContainsAHandlerStartContextOfType(templateContext.StartContextDelimiter)) result = new FunctionTemplateContext();
            if (result == null) return templateContext;
            Map(templateContext, result);
            return result;
        }

        public void Map(TemplateContext source, TemplateContext destination)
        {
            destination.InnerContent = source.InnerContent;
            destination.StartContextDelimiter = source.StartContextDelimiter;
            destination.ContextDepth = source.ContextDepth;
            destination.EndContextDelimiter = source.EndContextDelimiter;
            destination.StartIndex = source.StartIndex;
        }
    }
}
