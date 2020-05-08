using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotPrimaryColumnNotALastAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {

        public IsNotPrimaryColumnNotALastAutoColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext { get => "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:NOT:LAST:COLUMN("; }
        public override string EndContext { get => "):::}"; }
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is not the last column from the iterated not primary key column collection";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel descriptionPojo = databaseContext.Column;

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (databaseContext.Table == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = databaseContext.Table.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            IColumnModel currentLastPrimaryColumn = null;
            foreach (IColumnModel currentColumn in columnList)
            {
                if (!currentColumn.IsPrimaryKey)
                {
                    currentLastPrimaryColumn = currentColumn;
                }
            }
            if (currentLastPrimaryColumn == null) return "";
            if (currentLastPrimaryColumn.Equals(descriptionPojo)) return "";
            return HandleTrimedContext(TrimedStringContext,databaseContext);
        }
    }
}
