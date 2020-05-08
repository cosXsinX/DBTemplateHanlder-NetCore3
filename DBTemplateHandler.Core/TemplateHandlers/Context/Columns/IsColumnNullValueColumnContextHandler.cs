using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnNullValueColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsColumnNullValueColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:NULL(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current iteration column is a nullable value column";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            var column = databaseContext.Column;
            if (!column.IsNotNull)
            {
                return HandleTrimedContext(TrimedStringContext,databaseContext);
            }
            else return "";
        }
    }
}
