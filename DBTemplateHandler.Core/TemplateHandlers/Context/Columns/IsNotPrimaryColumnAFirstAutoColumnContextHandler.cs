using DBTemplateHandler.Core.Database;
using DBTemplateHandler.Core.TemplateHandlers.Columns;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBTemplateHandler.Core.TemplateHandlers.Context.Columns
{
    public class IsNotPrimaryColumnAFirstAutoColumnContextHandler : AbstractColumnTemplateContextHandler
    {


        public const String START_CONTEXT_WORD = "{:TDB:TABLE:COLUMN:NOT:PRIMARY:FOREACH:CURRENT:IS:FIRST:COLUMN(";
        public const String END_CONTEXT_WORD = "):::}";
        public override string StartContext { get => START_CONTEXT_WORD; }
        public override string EndContext { get => END_CONTEXT_WORD; }

        public override String processContext(String StringContext)
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

            foreach(IColumnModel currentColumn in columnList)
            {
                if (!currentColumn.IsPrimaryKey)
                {
                    if (currentColumn.Equals(column))
                    {
                        return HandleTrimedContext(TrimedStringContext);
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            return "";
        }

        public override bool isStartContextAndEndContextAnEntireWord => false;
    }
}
