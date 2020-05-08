using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotPrimaryColumnALastAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsNotPrimaryColumnALastAutoColumnContextHandler(ITemplateHandler templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:LAST:COLUMN(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the last column from the iterated not primary key column collection";

        public override string ProcessContext(string StringContext, IDatabaseContext databaseContext)
        {
            ControlContext(StringContext, databaseContext);
            IColumnModel column = databaseContext.Column;
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
            if (!currentLastPrimaryColumn.Equals(column)) return "";
            return HandleTrimedContext(TrimedStringContext,databaseContext);
        }
    }
}
