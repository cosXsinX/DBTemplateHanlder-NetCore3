using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class ColumnValueTypeColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        private ColumnValueTypeSemanticDefinition semanticDefinition = new ColumnValueTypeSemanticDefinition();
        public ColumnValueTypeColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => semanticDefinition.StartContext;
        public override string EndContext => semanticDefinition.EndContext;
        public override string ContextActionDescription => "Is replaced by the current column database model type";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            if (databaseContext == null) throw new ArgumentNullException(nameof(databaseContext));
            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel columnModel = databaseContext.Column;
            if (columnModel == null)
                throw new Exception($"The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '" + (Signature) + "'");
            return columnModel.Type;
        }

        public override bool isStartContextAndEndContextAnEntireWord => true;
    }

    public class ColumnValueTypeSemanticDefinition
    {
        public string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:TYPE";
        public string EndContext => "::}";
        public string Signature => $"{StartContext}{EndContext}";
    }
}
