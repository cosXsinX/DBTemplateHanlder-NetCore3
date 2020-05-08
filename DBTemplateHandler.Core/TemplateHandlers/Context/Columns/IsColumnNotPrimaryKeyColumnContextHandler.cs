using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsColumnNotPrimaryKeyColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsColumnNotPrimaryKeyColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:FOREACH:CURRENT:IS:KEY:NOT:PRIMARY(";
        public override string EndContext => ")KEY:NOT:PRIMARY:::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current iteration column is not a primary key column";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel columnModel = databaseContext.Column;
            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (!columnModel.IsPrimaryKey)
            {
                return HandleTrimedContext(TrimedStringContext,databaseContext);
            }
            else return "";
        }
    }
}
