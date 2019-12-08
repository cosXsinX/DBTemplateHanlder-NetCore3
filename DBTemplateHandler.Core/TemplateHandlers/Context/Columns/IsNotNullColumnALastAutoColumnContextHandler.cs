using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using DBTemplateHandler.Core.TemplateHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotNullColumnALastAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {
        public IsNotNullColumnALastAutoColumnContextHandler(TemplateHandlerNew templateHandlerNew) : base(templateHandlerNew) { }
        public override string StartContext => "{:TDB:TABLE:COLUMN:NOT:NULL:FOREACH:CURRENT:IS:LAST:COLUMN(";
        public override string EndContext => "):::}";
        public override bool isStartContextAndEndContextAnEntireWord => false;
        public override string ContextActionDescription => "Is replaced by the inner context when the current column is the last column from the iterated not nullable value column collection";

        public override string processContext(string StringContext)
        {

            if (StringContext == null)
                throw new Exception($"The provided {nameof(StringContext)} is null");
            IColumnModel column = ColumnModel;
            if (column == null)
                throw new Exception($" The {nameof(ColumnModel)} is not set");

            string TrimedStringContext = TrimContextFromContextWrapper(StringContext);
            if (column.ParentTable == null)
                throw new Exception("The provided column has no parent table");
            IList<IColumnModel> columnList = column.ParentTable.Columns;
            if (columnList == null || !(columnList.Count > 0))
                throw new Exception("The provided column's parent table has no column associated to");
            IColumnModel currentLastAutoColumn = null;
            foreach (IColumnModel currentColumn in columnList)
            {
                if (currentColumn.IsNotNull)
                {
                    currentLastAutoColumn = currentColumn;
                }
            }
            if (currentLastAutoColumn == null) return "";
            if (!currentLastAutoColumn.Equals(column)) return "";
            return HandleTrimedContext(TrimedStringContext);
        }


    }
}
