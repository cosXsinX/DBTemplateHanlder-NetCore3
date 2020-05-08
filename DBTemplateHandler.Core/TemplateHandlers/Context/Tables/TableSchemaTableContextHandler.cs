using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Tables
{
    public class TableSchemaTableContextHandler : AbstractTableTemplateContextHandler
    {
        public TableSchemaTableContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:CURRENT:SCHEMA"; }
        public override string EndContext { get => "::}"; }

        public override bool isStartContextAndEndContextAnEntireWord => true;
        public override string ContextActionDescription => "Is replaced by the current table schema name";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            ITableModel table = databaseContext.Table;

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!TrimedStringContext.Equals(""))
                throw new Exception($"There is a problem with the provided {nameof(StringContext)} :'{StringContext}' to the suited word '{Signature}'");
            return table.Schema;
        }
    }
}
