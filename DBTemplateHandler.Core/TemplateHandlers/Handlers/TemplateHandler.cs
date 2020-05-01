using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Core.TemplateHandlers.Context.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context.Functions;
using DBTemplateHandler.Core.TemplateHandlers.Context.Tables;
using DBTemplateHandler.Service.Contracts.TypeMapping;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class TemplateHandler : ITemplateHandler
    {
        private readonly TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>
            templateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>
            databaseTemplateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>
            tableTemplateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>
            columnTemplateContextHandlerProvider;

        private readonly TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>
            functionTemplateContextHandlerProvider;

        private readonly TemplateValidator templateValidator;
        private readonly ContextVisitor<AbstractTemplateContextHandler> contextVisitor;
        private readonly TemplateContextProcessor templateContextProcessor;

        private readonly ToQualifiedTemplateContextConverter toQualifiedTemplateContextConverter;


        private readonly List<ITypeMapping> typeMappingsField;
        public IList<ITypeMapping> TypeMappings => typeMappingsField;

        public TemplateHandler(IList<ITypeMapping> typeMappings)
        {
            typeMappingsField = typeMappings?.ToList() ?? new List<ITypeMapping>();
            templateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>(this, typeMappings);
            databaseTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractDatabaseTemplateContextHandler>(this, typeMappings);
            tableTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractTableTemplateContextHandler>(this, typeMappings);
            columnTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractColumnTemplateContextHandler>(this, typeMappings);
            functionTemplateContextHandlerProvider = new TemplateContextHandlerPackageProvider<AbstractFunctionTemplateContextHandler>(this, typeMappings);
            templateValidator = new TemplateValidator(this, typeMappings);
            contextVisitor = new ContextVisitor<AbstractTemplateContextHandler>(templateContextHandlerProvider);
            templateContextProcessor = new TemplateContextProcessor(this, typeMappings);
            toQualifiedTemplateContextConverter = new ToQualifiedTemplateContextConverter(this, typeMappings);
        }

        public IList<ITemplateContextHandlerIdentity> GetAllItemplateContextHandlerIdentity()
        {
            return templateContextHandlerProvider.GetHandlers().Cast<ITemplateContextHandlerIdentity>().ToList();
        }

        public string HandleDatabaseTemplate(string templateString, IDatabaseModel databaseModel)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;

            UpdateContainedTables(databaseModel);

            var contextes = contextVisitor.ExtractAllContextUntilDepth(templateString, 0);
            var qualifiedContextes = toQualifiedTemplateContextConverter.ToQualifiedTemplateContextes(contextes);
            var result = templateString;
            var charDiffSum = 0;
            foreach (var context in qualifiedContextes)
            {
                var contextContent = context.current.Content;
                var processorDatabaseContext = new ProcessorDatabaseContext()
                {
                    Database = databaseModel,
                };
                var contextContentProcessed = templateContextProcessor.ProcessTemplateContextComposite(context, processorDatabaseContext);
                result = $"{result.Substring(0, context.current.StartIndex + charDiffSum)}{contextContentProcessed}{result.Substring(context.current.StartIndex + contextContent.Length + charDiffSum)}";
                charDiffSum = charDiffSum + contextContentProcessed.Length - contextContent.Length;
            }
            return result;
        }

        public string HandleFunctionTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnModel)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;

            if (databaseModel != null) UpdateContainedTables(databaseModel);
            if (tableModel != null) UpdateContainedColumns(tableModel);

            var contextes = contextVisitor.ExtractAllContextUntilDepth(templateString, 0);
            var qualifiedContextes = toQualifiedTemplateContextConverter.ToQualifiedTemplateContextes(contextes);
            var result = templateString;
            var charDiffSum = 0;
            foreach (var context in qualifiedContextes)
            {
                var contextContent = context.current.Content;
                var processorDatabaseContext = new ProcessorDatabaseContext()
                {
                    Column = columnModel,
                    Table = tableModel,
                    Database = databaseModel,
                };
                var contextContentProcessed = templateContextProcessor.ProcessTemplateContextComposite(context, processorDatabaseContext);
                result = $"{result.Substring(0, context.current.StartIndex + charDiffSum)}{contextContentProcessed}{result.Substring(context.current.StartIndex + contextContent.Length + charDiffSum)}";
                charDiffSum = charDiffSum + contextContentProcessed.Length - contextContent.Length;
            }
            return result;
        }

        public string HandleTableColumnTemplate(string templateString, IColumnModel column)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;

            var contextes = contextVisitor.ExtractAllContextUntilDepth(templateString, 0);
            var qualifiedContextes = toQualifiedTemplateContextConverter.ToQualifiedTemplateContextes(contextes);
            var result = templateString;
            var charDiffSum = 0;
            foreach (var context in qualifiedContextes)
            {
                var contextContent = context.current.Content;
                var processorDatabaseContext = new ProcessorDatabaseContext()
                {
                    Column = column,
                };
                var contextContentProcessed = templateContextProcessor.ProcessTemplateContextComposite(context, processorDatabaseContext);
                result = $"{result.Substring(0, context.current.StartIndex + charDiffSum)}{contextContentProcessed}{result.Substring(context.current.StartIndex + contextContent.Length + charDiffSum)}";
                charDiffSum = charDiffSum + contextContentProcessed.Length - contextContent.Length;
            }
            return result;
        }

        public string HandleTableTemplate(string templateString, ITableModel tableModel)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;

            UpdateContainedColumns(tableModel);

            var contextes = contextVisitor.ExtractAllContextUntilDepth(templateString, 0);
            var qualifiedContextes = toQualifiedTemplateContextConverter.ToQualifiedTemplateContextes(contextes);
            var result = templateString;
            var charDiffSum = 0;
            foreach (var context in qualifiedContextes)
            {
                var contextContent = context.current.Content;
                var processorDatabaseContext = new ProcessorDatabaseContext()
                {
                    Table = tableModel,
                };
                var contextContentProcessed = templateContextProcessor.ProcessTemplateContextComposite(context, processorDatabaseContext);
                result = $"{result.Substring(0, context.current.StartIndex + charDiffSum)}{contextContentProcessed}{result.Substring(context.current.StartIndex + contextContent.Length + charDiffSum)}";
                charDiffSum = charDiffSum + contextContentProcessed.Length - contextContent.Length;
            }
            return result;
        }

        public string HandleTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnModel)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;

            var contextes = contextVisitor.ExtractAllContextUntilDepth(templateString, 0);
            var qualifiedContextes = toQualifiedTemplateContextConverter.ToQualifiedTemplateContextes(contextes);
            var result = templateString;
            var charDiffSum = 0;
            foreach (var context in qualifiedContextes)
            {
                var contextContent = context.current.Content;
                var processorDatabaseContext = new ProcessorDatabaseContext()
                {
                    Column = columnModel,
                    Table = tableModel,
                    Database = databaseModel,
                };
                var contextContentProcessed = templateContextProcessor.ProcessTemplateContextComposite(context, processorDatabaseContext);
                result = $"{result.Substring(0, context.current.StartIndex + charDiffSum)}{contextContentProcessed}{result.Substring(context.current.StartIndex + contextContent.Length + charDiffSum)}";
                charDiffSum = charDiffSum + contextContentProcessed.Length - contextContent.Length;
            }
            return result;
        }

        private static void UpdateContainedTables(IDatabaseModel databaseModel)
        {
            var tables = databaseModel.Tables.ToList();
            tables.ForEach(table => table.ParentDatabase = databaseModel);
            tables.ForEach(table => UpdateContainedColumns(table));
        }

        private static void UpdateContainedColumns(ITableModel tableModel)
        {
            var columns = tableModel.Columns.ToList();
            columns.ForEach(m => m.ParentTable = tableModel);
        }

        //TODO Bad architecture problem symptom => template handler should not manage type mapping
        public void OverwriteTypeMapping(IList<ITypeMapping> typeMappings)
        {
            var newTypeMappings = TypeMappings.MergeAndAttachTypeMappingItems(typeMappings).ToList();
            typeMappingsField.Clear();
            typeMappingsField.AddRange(newTypeMappings);
        }
    }
}
