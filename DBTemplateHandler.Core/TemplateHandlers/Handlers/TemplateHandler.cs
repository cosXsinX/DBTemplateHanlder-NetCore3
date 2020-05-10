using System.Collections.Generic;
using System.Linq;
using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Context;
using DBTemplateHandler.Service.Contracts.TypeMapping;

namespace DBTemplateHandler.Core.TemplateHandlers.Handlers
{
    public class TemplateHandler : ITemplateHandler
    {
        private readonly TemplateContextHandlerPackageProvider<AbstractTemplateContextHandler>
            templateContextHandlerProvider;

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
            return HandleDatabaseTemplate(templateString, new ProcessorDatabaseContext()
            {
                Database = databaseModel,
            });
        }

        public string HandleFunctionTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnModel,IForeignKeyConstraintModel foreignKeyConstraintModel)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;
            return HandleFunctionTemplate(templateString, new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = tableModel,
                Database = databaseModel,
                ForeignKeyConstraint = foreignKeyConstraintModel,
            });
        }

        public string HandleTableColumnTemplate(string templateString, IColumnModel column)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;

            return HandleTableColumnTemplate(templateString, new ProcessorDatabaseContext()
            {
                Column = column,
                //Table = tableModel,
                //Database = databaseModel,
                //ForeignKeyConstraint = foreignKeyConstraintModel,
            });
        }

        public string HandleTableTemplate(string templateString, ITableModel tableModel)
        {
            return HandleTableTemplate(templateString, new ProcessorDatabaseContext()
            {
                Table = tableModel,
                //Database = databaseModel,
                //ForeignKeyConstraint = foreignKeyConstraintModel,
            });
        }

        public string HandleTemplate(string templateString, IDatabaseModel databaseModel, ITableModel tableModel, IColumnModel columnModel,IForeignKeyConstraintModel foreignKeyConstraintModel)
        {
            return HandleTableTemplate(templateString, new ProcessorDatabaseContext()
            {
                Column = columnModel,
                Table = tableModel,
                Database = databaseModel,
                ForeignKeyConstraint = foreignKeyConstraintModel,
            });
        }

        

        //TODO Bad architecture problem symptom => template handler should not manage type mapping
        public void OverwriteTypeMapping(IList<ITypeMapping> typeMappings)
        {
            var newTypeMappings = TypeMappings.MergeAndAttachTypeMappingItems(typeMappings).ToList();
            typeMappingsField.Clear();
            typeMappingsField.AddRange(newTypeMappings);
        }

        public string HandleDatabaseTemplate(string templateString, IDatabaseContext databaseContext)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;
            return HandleTemplate(templateString, databaseContext);
        }

        public string HandleFunctionTemplate(string templateString, IDatabaseContext databaseContext)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;
            return HandleTemplate(templateString, databaseContext);
        }

        public string HandleTableColumnTemplate(string templateString, IDatabaseContext databaseContext)
        {   
            return HandleTemplate(templateString, databaseContext);
        }

        public string HandleTableTemplate(string templateString, IDatabaseContext databaseContext)
        {
            return HandleTemplate(templateString, databaseContext);
        }

        public string HandleTemplate(string templateString, IDatabaseContext databaseContext)
        {
            if (!templateValidator.TemplateStringValidation(templateString)) return templateString;

            var contextes = contextVisitor.ExtractAllContextUntilDepth(templateString, 0);
            var qualifiedContextes = toQualifiedTemplateContextConverter.ToQualifiedTemplateContextes(contextes);
            var result = templateString;
            var charDiffSum = 0;
            foreach (var context in qualifiedContextes)
            {
                var contextContent = context.current.Content;
                var processorDatabaseContext = databaseContext;
                var contextContentProcessed = templateContextProcessor.ProcessTemplateContextComposite(context, processorDatabaseContext);
                result = $"{result.Substring(0, context.current.StartIndex + charDiffSum)}{contextContentProcessed}{result.Substring(context.current.StartIndex + contextContent.Length + charDiffSum)}";
                charDiffSum = charDiffSum + contextContentProcessed.Length - contextContent.Length;
            }
            return result;
        }
    }
}
